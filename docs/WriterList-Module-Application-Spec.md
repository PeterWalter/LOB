# WriterList Module - Application Specification

**Document type:** Product requirements + build specification
**Subject:** An application that reproduces the behaviour of the CETAP LOB **WriterList** module
**Baseline:** CETAP_LOB (WPF, .NET Framework 4.8) on Microsoft SQL Server (NBT_Production)
**Status:** Draft for engineering review

---

## 0. How to read this document

This document is written so that a separate team can build a **new application with the same
functionality** as the existing WriterList module, without needing the original source. It is
split into:

- **Part A - Product requirements** (why, who, what) - sections 1 to 5.
- **Part B - Functional specification** (exact behaviour and rules) - sections 6 to 12.
- **Part C - Build guidance** (architecture, data, testing, acceptance) - sections 13 to 18.
- **Appendices** - file format, worked packing example, source map.

Something described here as "the module" is the existing implementation; when this document says
"the new application" it means the replacement to be built.

---

# Part A - Product requirements

## 1. Problem statement

The NBT programme registers candidates (called **writers**) for a test session through a
web registration system. That system exports a list of writers, but the list:

- is produced per registration state and is not trustworthy as stored:
  names contain punctuation, accents and stray symbols;
  South African ID numbers are malformed or swapped into the wrong column;
  dates of birth are missing or unreadable; the same NBT reference can appear twice;
- must agree with the biographical details the scoring pipeline already holds
  (the **WriterList** and **Composit** tables), and the two sides drift;
- must be converted, before test day, into the physical working documents used to run a venue:
  a check-in sheet per venue, a master writer list, and a packing list that says how many test
  booklets to send to each venue.

Doing this by hand is slow, error-prone, and produces documents that do not reconcile with the
database. A wrong NBT number or a stale name propagates into scanning, QA and scoring.

## 2. Solution

An application that:

1. **Imports** a web-registration writer list file (Unicode CSV, fixed 19-column layout).
2. **Validates** every row against the identity, name, ID-number, date-of-birth and contact rules,
   showing each failure on the field that caused it, and count and sort worst-first.
3. **Compares** each row with the WriterList and Composit records already in the database,
   and lets a user settle each difference in either direction, field by field, without leaving
   the grid.
4. **Repairs** the rows that are in error with a reviewable clean-up pass.
5. **Detects duplicates** inside the file and against the database, and can re-issue a clean NBT
   reference from a pool of reserved numbers.
6. **Saves** the list to the database: adds writers that are not there yet, and brings existing
   writers up to date without ever wiping a stored value because the file left it blank.
7. **Processes** the loaded list into per-venue outputs: a master writer-list workbook, one
   printable check-in sheet per venue, and a packing list with test-booklet quantities computed
   from the registered counts.
8. **Maintains venue master data** (venue code, names, province, capacity, availability) that
   the import and the outputs depend on.

## 3. Scope

**In scope**

- Writer list file import, validation, cleaning, duplicate handling, NBT reallocation.
- Comparison against and synchronisation with the WriterList and Composit data.
- Writer list persistence.
- Writer list processing and all exports listed above.
- Venue master data maintenance used by the import and exports.
- Intake-year scoping of writer data.

**Out of scope** (see also section 17)

- The web registration system that *produces* the writer list file.
- Candidate answer-sheet scanning, QA and scoring (other modules own these), except that this
  application must remain a faithful source and target for the WriterList values those modules use.
- Trial/APS score calculation.
- Payment (Easy Pay) file handling; only the "amount paid" field carried on the writer row is in scope.

## 4. Actors

| Actor | Description | Primary goals |
|---|---|---|
| Data preparation officer | Loads the web file, fixes errors, reconciles with the database | A clean, saved writer list |
| Preparations/logistics officer | Produces the venue check-in sheets and packing list | Correct documents for test day |
| Database administrator | Owns the schema, the NBT number pool, and the intake calendar | Availability and integrity |
| Downstream systems | QA, Composit/scoring, batching | Read WriterList values; QA can write an accepted value back |

## 5. Domain glossary

| Term | Meaning |
|---|---|
| **Writer** | A registered test candidate |
| **WriterList** | The database table holding one row per writer for an intake |
| **Composit** | The scored composite record for a barcode/reference; used here only as a comparison source |
| **NBT Reference** | The 14-digit NBT number identifying a writer's test registration; last 13 digits carry a Luhn check digit, the leading digit is a series prefix |
| **Intake** | A registration cycle with a start and end date (IntakeYears row); "current intake" is the one containing today |
| **Writer list file** | The Unicode CSV exported by web registration (Appendix A) |
| **Duplicates** | Two or more rows in the loaded file sharing a Reference |
| **DB Duplicates** | A loaded row whose Reference already exists in WriterList for the intake but with a different SA ID and Foreign ID |
| **Clean File** | The tidy-up pass that strips punctuation/accents and repairs the permitted identity problems |
| **New NBT Number** | A replacement reference taken from the unused pool, for a writer whose reference is duplicated |
| **Check-in sheet** | The per-venue printable sheet listing registered writers with a signature column |
| **Packing list** | The workbook that drives how many AQL/Mathematics test booklets are sent per venue |
| **Round amount** | Round a count up to the next 10 |
| **Walk-in buffer** | An extra estimate of on-the-day registrations, added to the ordered quantity |

---

# Part B - Functional specification

## 6. Module map and system context

The module is two cooperating halves plus shared reference data:

    +---------------------+        +---------------------------------+
    | Web registration    |        |  Reference data                 |
    | (external)          |        |  TestVenue, Province, IntakeYear|
    |  CSV export         |        |  ProfileAllocation, test names  |
    +----------+----------+        +----------------+----------------+
               |                                    |
               v                                    |
    +---------------------+   compare/persist   +----v----------------+
    | 1. LOAD LIST        |<------------------->|  Database           |
    |  (import, validate, |                    |  WriterList         |
    |   reconcile, save)  |                    |  Composit           |
    +----------+----------+                    |  NewNBTNumbers      |
               | in-memory list                |  Logs               |
               v                               +----+----------------+
    | 2. WRITER LIST      |                          ^
    |  (group by venue,   |                          | QA writes back
    |   stats, exports)   |--------------------> documents
    +---------------------+        |                  |
                             check-in sheets,        |
                             master workbook,        |
                             packing list            |
                                                    |
    Downstream consumers: QA / Composit scoring / batching --> WriterList

The new application MUST keep the WriterList table as the contract with downstream modules:
whatever else changes, the row shape and the reconciliation semantics in section 10 must match.

## 7. User stories

**Import and display**

1. As a data preparation officer, I want to choose a writer list file, so that its writers become
   available for preparation.
2. As a data preparation officer, I want the file read in the background with a progress indicator,
   so that the window stays responsive while a large list loads.
3. As a data preparation officer, I want one grid row per writer, so that I can see the whole list.
4. As a data preparation officer, I want the file name shown, so that I know which list is loaded.
5. As a data preparation officer, I want the grid sorted worst-first by error count, so that the
   rows needing attention are at the top.
6. As a data preparation officer, I want to see total writers, distinct venues, male/female and
   English/Afrikaans counts, so that I can sanity-check the file against expectations.

**Validation**

7. As a data preparation officer, I want each failing field marked in red with the reason as a
   tooltip, so that I can correct it in place.
8. As a data preparation officer, I want the error count per row to update as I edit, so that I can
   see the row become clean.
9. As a data preparation officer, I want the NBT reference checked for length and check digit, so
   that mistyped references are caught.
10. As a data preparation officer, I want names checked for digits, punctuation and length, so that
    scanner-unfriendly characters never reach the database.
11. As a data preparation officer, I want South African ID numbers fully validated (digits, length,
    embedded date, citizenship digit, check digit), so that invalid IDs are caught.
12. As a data preparation officer, I want the date of birth checked for a plausible matric age, so
    that obviously wrong dates are caught.
13. As a data preparation officer, I want contact details length-checked and e-mail validated, so
    that unusable contact data is visible.

**Database comparison and reconciliation**

14. As a data preparation officer, I want every row compared with the matching WriterList record,
    so that database drift is visible.
15. As a data preparation officer, I want every row also compared with the matching Composit record,
    so that both stored views of the writer are considered.
16. As a data preparation officer, I want a "differs from database" flag only when the file agrees
    with neither the WriterList nor the Composit value, so that a single stored blank does not
    raise a false alarm.
17. As a data preparation officer, I want a right-click submenu showing the file, WriterList and
    Composit values side by side for the column I clicked, so that I can decide which side is right.
18. As a data preparation officer, I want to copy the WriterList value into the grid, so that the
    database's value wins for that row without changing the database.
19. As a data preparation officer, I want to copy the Composit value into the grid, so that the
    composite copy wins without changing the database.
20. As a data preparation officer, I want to write the file value into WriterList immediately for one
    field, so that I can correct a single stale field without a full save.
21. As a data preparation officer, I want to write the file value into Composit immediately for one
    field, so that the scored record can be corrected too.
22. As a data preparation officer, I want a read-only listing of the whole matching WriterList record
    and Composit record in the context menu, so that I can inspect the rest of what is stored.
23. As a data preparation officer, I want a row that found no match at load time to be looked up
    again when I right-click it, so that correcting a reference can bring a comparison to life.
24. As a data preparation officer, I want a clear message when a per-field write cannot be applied,
    so that I know nothing was changed and why.

**Cleaning and duplicates**

25. As a data preparation officer, I want a Clean File command that only touches rows currently in
    error, so that clean rows are never altered.
26. As a data preparation officer, I want punctuation, accent and HTML-escape artefacts stripped
    from names, so that they can be stored.
27. As a data preparation officer, I want an over-long first name shortened by dropping the last
    word, so that the name fits the database.
28. As a data preparation officer, I want an invalid SA ID moved to the Foreign ID column when that
    is empty, and cleared when it is not, so that an ID that is really a passport number is treated
    as one.
29. As a data preparation officer, I want a date of birth derived from a valid SA ID, but never
    invented from an invalid one, so that bad data stays flagged.
30. As a data preparation officer, I want the home telephone left untouched by the clean pass, so
    that the number the writer gave is not destroyed.
31. As a data preparation officer, I want the file's internal duplicates listed, so that I can see
    repeated references immediately.
32. As a data preparation officer, I want a DB Duplicates check that finds references already used
    by a different writer, so that collisions are caught before saving.
33. As a data preparation officer, I want a New NBT Number command that issues the next unused
    reserved number, records the number it replaced and marks the replacement used, so that a
    duplicated writer can be given a clean reference.
34. As a data preparation officer, I want to be told when the number pool is empty, so that I can
    ask for a new batch.

**Saving**

35. As a data preparation officer, I want Save to Database to add writers not yet stored, so that the
    list is created.
36. As a data preparation officer, I want Save to Database to update writers already stored, so that
    stale details are corrected.
37. As a data preparation officer, I want a field that is blank in the file to leave the stored value
    alone, so that a save never silently wipes data.
38. As a data preparation officer, I want saving refused while DB Duplicates remain, so that a
    collision is never written.
39. As a data preparation officer, I want a confirmation or a reason when saving finishes, so that I
    know whether it worked.
40. As a data preparation officer, I want to delete a row from the loaded list without touching the
    database, so that I can remove a writer from this run.
41. As a data preparation officer, I want to save the corrected list back to a CSV, so that I can
    keep or hand on a clean copy.
42. As a data preparation officer, I want to refresh the grid, so that validation and duplicate
    checks re-run after edits.

**Writer list processing and outputs**

43. As a logistics officer, I want the loaded writers joined to venue master data, so that each row
    carries the venue short name and centre code.
44. As a logistics officer, I want the list grouped by venue with collapsible groups, so that I can
    work one venue at a time.
45. As a logistics officer, I want each venue's writers numbered sequentially (REG #), so that the
    printed sheet has a count.
46. As a logistics officer, I want the writer's name shown both as surname/name and as "Surname, Name"
    full name, so that the printed documents are readable.
47. As a logistics officer, I want the ID column to show the SA ID when present, otherwise the foreign
    ID, so that every writer has an identity on the sheet.
48. As a logistics officer, I want AQL and Mathematics language letters (E/A), so that each writer's
    test combination is on the sheet.
49. As a logistics officer, I want per-venue test counts by language and by test, so that I can plan.
50. As a logistics officer, I want a master writer-list Excel workbook with one sheet per venue, so
    that I have the full list in one file.
51. As a logistics officer, I want one printable check-in sheet per venue, landscape-independent A4,
    with a signature column and repeating header, so that reception can sign writers in.
52. As a logistics officer, I want a packing list workbook driven by a selected test profile, so that
    the correct test names, centre codes and quantities are on it.
53. As a logistics officer, I want ordered booklet quantities computed with the rounding and small-
    count rules, so that no venue is under-supplied.
54. As a logistics officer, I want walk-in buffers included in the ordered quantity, so that on-the-day
    registrations are covered.
55. As a logistics officer, I want batch composition strings (for example "3(50) 1(10)"), so that the
    packers know how to bundle booklets.

**Venue maintenance**

56. As a data preparation officer, I want to create, update and delete test venues, so that imports and
    outputs use current venues.
57. As a data preparation officer, I want each venue to carry code, name, short name, website name,
    room, type, province, capacity and availability, so that all uses of a venue are consistent.
58. As a data preparation officer, I want to select a venue on a map to set its location, so that the
    venue register has coordinates.

**Non-functional**

59. As an operator, I want the module to be usable while the database is slow, so that reading a big
    list does not freeze the application.
60. As an operator, I want meaningful failures written to the log, so that support can diagnose them.
61. As a security owner, I want the application never to write a WriterList value that the file did not
    carry, so that accidental mass data loss is impossible.

## 8. Detailed functional requirements

### 8.1 File ingestion (FR-ING)

- **FR-ING-1** Accept a file chosen by the user with the CSV/Data/Text filter; default extension .csv.
- **FR-ING-2** Refuse a file that is locked by another process (for example open in Excel) and tell the
  user "File is opened by another process or file does not exist".
- **FR-ING-3** Read comma-separated text, Unicode encoding, with a header row that is skipped, and
  without failing on a missing field.
- **FR-ING-4** Map fields positionally exactly as Appendix A.
- **FR-ING-5** An empty Easy Pay field is treated as 0.
- **FR-ING-6** Parse dates with the web formats in Appendix A; an unparseable date must not crash the
  load; it surfaces as an empty date and is caught by the date-of-birth rule when relevant.
- **FR-ING-7** Trim surname and first name on read.
- **FR-ING-8** After a successful read, compute the summary statistics (FR-UI-6) and, when a file
  cannot be read, return null and show "File could not be loaded" with the reason; log the failure.

### 8.2 Validation (FR-VAL)

Validation is a pure function of the row plus the attached WriterList/Composit snapshots. Editing any
validated field re-runs that row's validation. The rules and messages are in section 9.

- **FR-VAL-1** Every row exposes an error count and a per-property error collection with
  change notification.
- **FR-VAL-2** A field in error is shown red with the first error as a tooltip.
- **FR-VAL-3** "Differs from database" follows the mismatch semantics in section 9.2 exactly.
- **FR-VAL-4** Only the columns named in section 9.1 are validated. All other columns are loaded and
  saved but not validated on the Load List screen.

### 8.3 Database comparison and attachment (FR-CMP)

- **FR-CMP-1** On load, collect the distinct keys in the file: Reference (integer), SA ID (integer),
  Foreign ID (string, case-insensitive).
- **FR-CMP-2** Query WriterList and Composit in chunks of 1000 by each key type. Do not issue one
  query per row (the original performance incident in Appendix C, note 1).
- **FR-CMP-3** For each row resolve the WriterList snapshot by Reference, then SA ID, then Foreign ID;
  the first match wins. Resolve the Composit snapshot the same way.
- **FR-CMP-4** Attach the snapshots and re-run validation so mismatches are reported.
- **FR-CMP-5** A row that matched at load time keeps its snapshot for the rest of the session, even if
  the user edits the row.
- **FR-CMP-6** A row that did not match is looked up again when the user right-clicks it, using the
  on-screen values; a successful lookup attaches the snapshot for the rest of the session.
- **FR-CMP-7** The comparison fields are: NBT Reference, Surname, First Name, South African ID,
  Foreign ID, Date of Birth, Gender. Other columns show no value entries.
- **FR-CMP-8** Per-field writes ("Keep file value") act immediately on the one matching database row
  and one field, and report a specific reason on failure (see section 10.4).

### 8.4 Duplicates and NBT reallocation (FR-DUP)

- **FR-DUP-1** On load and refresh, fill the Duplicates list with every row whose Reference appears more
  than once in the loaded list (all members of the group).
- **FR-DUP-2** The DB Duplicates check fills its list with rows whose Reference exists in WriterList for
  the current intake but whose SA ID and Foreign ID both differ from the loaded row. The comparison is
  against the row's SA ID string and Foreign ID as loaded.
- **FR-DUP-3** Save to Database and Check DB Duplicates are enabled only when the loaded list is
  non-empty, has no file duplicates, and has been marked changed.
- **FR-DUP-4** Save to Database is refused while the DB Duplicates list is non-empty; log a warning and
  report "Nothing was saved: settle the records in the DB Duplicates list first."
- **FR-DUP-5** New NBT Number takes the first pool row whose Original NBT is empty; if none exists,
  report "No New NBT numbers available to allocate" and change nothing.
- **FR-DUP-6** A successful allocation sets the row's Reference to the new number, stores the replaced
  number on the pool row, marks the pool row used, timestamps it, and refreshes the screen.

### 8.5 Clean File (FR-CLN)

Clean File touches only rows that currently have errors. The exact transformations are in section 11.

- **FR-CLN-1** Never invent a date of birth; never modify a home telephone number.
- **FR-CLN-2** Report or make visible that Clean File changes real data, not just formatting.

### 8.6 Save to Database (FR-SAVE)

- **FR-SAVE-1** Scope existing data to the current intake (IntakeYears containing today).
- **FR-SAVE-2** A loaded row is a **new** writer when no stored writer in the intake has the same
  (NBT Reference, Date of Test) pair. New writers are inserted with a new RowGuid and DateModified now.
- **FR-SAVE-3** A loaded row matches an existing writer when the (NBT Reference, Date of Test) pair
  matches. The existing writer is **updated** only when at least one file field differs: First Name,
  Surname, SA ID, Foreign ID, Date of Birth, Gender, Test Language, Test Type, E-mail.
- **FR-SAVE-4** On update, only non-blank file values are copied; a blank file field leaves the stored
  value unchanged. A stored value is never deleted by a save.
- **FR-SAVE-5** Date of Birth is compared by date only; SA ID is compared only when the file carries one.
- **FR-SAVE-6** Persist in batches (roughly 200 rows per commit) so a large list does not build one
  enormous transaction.
- **FR-SAVE-7** Return a boolean success; on exception, log and return false. Report clearly to the
  user: success, refused because of DB Duplicates, or failed.
- **FR-SAVE-8** Resolve the venue by matching the file's Venue string to TestVenue.WebsiteName. If no
  venue matches, report "Database has no venue like <value>" and do not save a row with an unresolved
  venue (see risk R-3).

### 8.7 Writer List processing (FR-PROC)

- **FR-PROC-1** Data source is the list loaded by the Load List step in the same session. If nothing is
  loaded, the screen is empty rather than an error.
- **FR-PROC-2** Derive the test date from the loaded data and load the profile allocations for that date
  into a profile selector.
- **FR-PROC-3** When a profile is selected, resolve the four test names (AQL English, Maths English,
  AQL Afrikaans, Maths Afrikaans) to their test codes for the packing list.
- **FR-PROC-4** Join each writer to TestVenue on trimmed Venue equals WebsiteName; use the venue short
  name and code; label unmatched writers "Unknown Venue".
- **FR-PROC-5** Build the display/export model per writer: Surname in upper case; Name in title case;
  ID = SA ID when non-blank, otherwise Foreign ID; AQL language letter; Maths language letter (blank
  when the writer's test combination is AQL only); Paid; venue short name; contact details; test date;
  registration date; account creation date.
- **FR-PROC-6** Number writers per venue from 001 upward, resetting at each venue.
- **FR-PROC-7** Order writers by venue, language, test combination, surname, first name.
- **FR-PROC-8** Group the displayed list by venue with collapsible groups.
- **FR-PROC-9** Compute per-venue summary statistics and quantities as specified in section 12.

### 8.8 Exports (FR-EXP)

- **FR-EXP-1 Master workbook** - one worksheet per venue containing Surname, First Name, Reg #,
  Full Name, ID, AQL, Maths, Venue, Reference, Date of Test, Mobile, Home, E-Mail, Date of Registration,
  Creation Date; worksheet name from the venue (max 30 chars, invalid characters removed); saved to the
  user-chosen path.
- **FR-EXP-2 Check-in sheets** - one workbook per venue saved next to the master file, named
  "<long test date> <Venue>.xlsx", portrait A4, print area A1:F51, header set from
  "NBT TEST - <long date>, CHECK-IN SHEET : REGISTERED WRITERS, <VENUE>", first row repeated, columns
  #, Full Name, ID #, AQL, MAT, SIGN, rows numbered with leading zeros, signature column blank,
  formatted with borders, cyan header and a fixed font size.
- **FR-EXP-3 Packing list** - built from the supplied template workbook, using its summary worksheet
  and its venue worksheet; fill centre code (5 digits), test date, client, the four test names/codes,
  registered counts, quantities to send, walk-in totals and totals per venue; save to the user-chosen
  path.
- **FR-EXP-4** All exports must be available only with the prerequisites met (a loaded/joined list, and
  for the packing list a selected profile).
- **FR-EXP-5 Intake writer-list workbook (related output)** - the composite step also produces a
  workbook whose sheet is named "Writerlist" and whose file is named
  "<intake year> Intake Cycle writerlist_<date>". Its 34 columns and order are fixed in Appendix D.
  If the new application reproduces the wider pipeline, it must reproduce this file; if it only
  reproduces the WriterList screens, this file is an interface to keep compatible.

### 8.9 Venue master data (FR-VEN)

- **FR-VEN-1** List, create, update and delete test venues.
- **FR-VEN-2** Fields: venue code (positive integer, key), venue name, short name, website name, room,
  venue type, province, capacity, availability, comments, location, plus row version/guid/date modified.
- **FR-VEN-3** Validate that a venue code is positive; venue name and short name are required.
- **FR-VEN-4** Ask for confirmation before deleting a venue.
- **FR-VEN-5** Provide a map on which a venue's location can be chosen/stored.

### 8.10 UI requirements (FR-UI)

- **FR-UI-1** The Load List screen is a three-band layout: top toolbar (select file, file name, clean),
  middle writer grid, bottom summary/duplicates/commands.
- **FR-UI-2** Writer grid columns and order as in section 13.2; sortable, resizable, editable in place.
- **FR-UI-3** Right-click context menu as specified in section 13.3, with a heading and grouped entries.
- **FR-UI-4** Command availability exactly as in section 13.4.
- **FR-UI-5** A progress indicator is visible while a file is being read or saved.
- **FR-UI-6** Summary panel shows total writers, distinct venues, males, females, English, Afrikaans.
- **FR-UI-7** Two duplicate grids at the bottom, each with a context menu offering Delete Record and
  New NBT Number.
- **FR-UI-8** The Writer List screen shows the grouped venue grid, the per-venue summary grid, a
  profile selector, and buttons for the master workbook and the packing list.

## 9. Validation rules (exact)

### 9.1 Field rules

Error property keys in brackets. Messages are the exact existing wording; a new application may
improve wording but MUST keep the meaning and the "differs from database" semantics.

| Field [key] | Rule | Message |
|---|---|---|
| NBT Reference [Reference] | not empty | NBT number cannot be empty |
| | exactly 14 characters | Not proper length for NBT number |
| | the 13 digits after the first must pass the Luhn check (checksum over those 13 equals 0) | Not a Valid NBT number |
| | must not conflict with attached records (9.2) | Reference differs from database |
| First Name [FirstName] | not empty | FirstName cannot be empty |
| | no digit | First name cannot have digits |
| | must match the allowed-character pattern (9.3) | cannot start/end with space or have funny characters |
| | at most 18 characters | To many characters for Name (max is 18) |
| | must not contain the character e-acute | cannot have funny characters |
| | must not conflict with attached records (9.2) | Name differs from database |
| Surname [Surname] | not empty | Surname cannot be empty |
| | no digit | Surname cannot have digits |
| | allowed-character pattern | cannot start/end with space or have funny characters |
| | at most 30 characters | Too many characters for Surname |
| | no e-acute | cannot have funny characters |
| | must not conflict with attached records (9.2) | Surname differs from database |
| SA ID [SAID] | optional (a foreign-ID-only writer is valid) | - |
| | digits only | SA Id must contain only digits |
| | exactly 13 digits | SA Id must be 13 digits |
| | first 6 digits a real date (yyMMdd) | SA Id date of birth is not valid |
| | 11th digit (citizenship) is 0 or 1 | SA Id citizenship digit (11th) must be 0 or 1 |
| | last digit is a valid Luhn check digit over the first 12 | SA Id check digit is not valid |
| | must not conflict with attached records (9.2) | SA ID differs from database |
| Foreign ID [ForeignID] | optional | - |
| | at most 15 characters | ForeignID has too many characters |
| | must not conflict with attached records (9.2) | Foreign ID differs from database |
| Date of Birth [DOB] | if set, writer must be between 10 and 99 years old | Wrong age for Matric |
| | must not conflict with attached records (9.2) | Date of birth differs from database |
| Mobile [Mobile] | if set, at most 15 characters | Cellphone number has too many characters |
| Home Telephone [HTelephone] | if set, at most 15 characters | Telephone number has too many characters |
| E-Mail [Email] | if set, must parse as an e-mail address | Wrong email address |
| | if set, at most 50 characters | Email is to long |

Newlines: multiple errors on one field are joined with "; " and shown together.

### 9.2 "Differs from database" semantics (critical)

For a text field, compare the trimmed, upper-cased file value against each attached record value:

- A comparison against a record counts as **matching** when either side is blank/absent, or the
  normalised values are equal.
- The field is reported as differing only when **both** the WriterList and the Composit comparisons
  fail (that is, both records are present with a non-blank value that disagrees with the file).

Consequences to preserve:

- A row with no attached record is never flagged as differing.
- A row where only one record is attached and it matches is not flagged.
- A row where only one record is attached and it disagrees **is** flagged (the messages naming the
  WriterList/Composit separately are then shown; in the current implementation the single message
  is generic "Name differs from database" - see open item Q-1).
- Dates are compared by calendar date only; a blank/default date on either side matches.

### 9.3 Allowed name pattern

The allowed-character regular expression used by both names is:

    ^[^\s=!@#](?:[^!@#;'`èëéáàãíìïòôöúüç©]*[^\s!@#])?$

In words: the first and last characters may not be whitespace, "=", "!", "@" or "#"; interior
characters may not be "!", "@", "#", ";", apostrophe or grave accent; the accented characters in the
character class are additionally rejected (the code also separately rejects e-acute). A name must not
start or end with a space.

### 9.4 Checksums

- **SA ID**: standard Luhn on the first 12 digits; the 13th must equal the computed check digit.
- **NBT Reference**: apply the same Luhn computation to the 13 digits after the leading digit and
  require the checksum result to be 0.
- A checksum helper must be shared and unit-tested (see section 15).

## 10. Comparison and reconciliation (detail)

### 10.1 Key resolution order

| Store | Order |
|---|---|
| WriterList | Reference, then SA ID, then Foreign ID |
| Composit | Reference, then SA ID, then Foreign ID |

The first match found in that order wins. Foreign IDs compare case-insensitively after trim.

### 10.2 What a context-menu comparison shows

For the clicked column, show:

- File: the value in the grid.
- WriterList: "same as file", or "DIFFERENT: <value>", or absent when there is no attached record.
- Composit: the same.

### 10.3 Direction of correction

| Intent | Action |
|---|---|
| Database is right, file is wrong | Copy the WriterList or Composit value into the grid (no write) |
| File is right, database is stale | Write the file value into WriterList or Composit immediately |

### 10.4 Per-field write failures (must be reported; nothing changes)

- No matching WriterList record: "No matching WriterList record was found in the database."
- Reference is not numeric: "Reference is not a valid number for WriterList update."
- SA ID is not numeric: "South African ID is not a valid number for WriterList update."
- Unsupported field: "Unsupported WriterList field: <field>."
- No row selected: "No record selected for WriterList update."
- No column selected: "No column selected for WriterList update."
- Database validation failure: a "Database validation failed" message.
- Success: "WriterList updated."

The same is applied for Composit with the analogous messages.

## 11. Clean File transformations (exact)

Applies only to rows with at least one error, for the fields that are in error.

| Field in error | Transformation |
|---|---|
| Surname | Remove ";" "!" "@" and the apostrophe; replace accented characters (a-grave, a-acute, a-tilde, c-cedilla, c-copyright, e-grave, e-acute, e-diaeresis, i-grave, i-acute, i-diaeresis, o-grave, o-circumflex, o-diaeresis, u-grave, u-acute, u-diaeresis) with plain letters; remove the literal sequence ampersand-hash-zero-three-nine-semicolon |
| First Name | The same replacements; additionally, when the result is longer than 18 characters, drop the last word (if there is more than one word) |
| SA ID | If Foreign ID is blank: move the SA ID value into Foreign ID and clear SA ID. Otherwise: clear SA ID. (The invalid "ID" is treated as a passport number.) |
| Date of Birth | If SA ID is non-blank and SA ID is not itself in error, take the date of birth from the SA ID. Otherwise leave the date of birth exactly as loaded. |
| Home Telephone | Leave unchanged; the error remains for manual correction |

The pass must be idempotent-ish in intent: it should not oscillate a value and should re-run validation
afterwards so the grid shows the new error state.

## 12. Packing and ordering arithmetic (exact)

Definitions:

- **Round amount** R(n) = nearest multiple of 10, rounding halves away from zero.
- **Walk-in estimate** W(n) = ceiling(n / 250) * 10, but 0 when n = 0.
- **Batch string** B(n):
  - n = 0 -> empty string
  - n < 50 -> ceil(n/10) followed by "(10)"
  - n >= 50 -> floor(n/50) "(50)" [and, if the remainder rounded up to tens is non-zero,
    " " + ceil((n mod 50)/10) + "(10)"]

Per venue, with E = English and A = Afrikaans:

- Count registered AQL_E, AQL_A, Maths_E, Maths_A (writers whose test combination includes that test).
- W_E = W(AQL_E), W_A = W(AQL_A)
- AQLE_S = R(AQL_E) + W_E
- AQLA_S = R(AQL_A) + W_A
- MATE_S = R(Maths_E) + 10 when Maths_E > 0, else 0
- MATA_S = R(Maths_A) + 10 when Maths_A > 0, else 0

Then apply the small-count overrides:

- If AQL_E = 0 -> W_E = 0 and AQLE_S = 0; else if AQL_E < 5 -> AQLE_S = 10. (Same for A.)
- If Maths_E > 0 and Maths_E < 5 -> MATE_S = 10. (Same for A.)
- Batch strings: AQLE_Batch = B(AQLE_S), AQLA_Batch = B(AQLA_S), MATE_Batch = B(MATE_S),
  MATA_Batch = B(MATA_S).
- Total sent for the packing summary = AQLE_S + AQLA_S.
- Total walk-ins for the packing summary = W_E + W_A.

A worked example is in Appendix B.

---

# Part C - Build guidance

## 13. Data model

The new application may implement these in any store, but the fields and semantics must match.

### 13.1 WriterList (primary entity)

| Field | Type | Notes |
|---|---|---|
| Id | int, PK, identity | Surrogate key |
| NBT | long | The 14-digit reference |
| Surname | string | |
| Name | string | First name |
| Initials | string | |
| SAID | long, nullable | Leading zeros must be preserved by formatting to 13 digits |
| ForeignID | string, nullable | |
| Gender | string | Male/Female |
| DOB | date | |
| Classification | string | |
| TestLanguage | string | English/Afrikaans |
| TestType | string | Test combination (drives AQL-only vs AQL+Maths) |
| VenueID | int | FK to TestVenue.VenueCode |
| DOT | date | Date of test |
| Mobile | string | |
| HomeTelephone | string | |
| EMail | string | |
| Paid | decimal | |
| AccountCreation | date | |
| RegistrationDate | date | |
| Wrote | bool | |
| RowGuid | guid | New guid per inserted row |
| DateModified | datetime | Set on insert and on each update |
| RowVersion | byte[] | Concurrency token |

Relationships: many WriterList rows to one TestVenue.

### 13.2 Supporting entities

| Entity | Key fields | Purpose |
|---|---|---|
| TestVenue | VenueCode PK; VenueName; ShortName; WebsiteName; Room; VenueType; ProvinceID; Capacity; Comments; Available; Place (geography); RowVersion; RowGuid; DateModified | Venue master data; WebsiteName is the join key from the writer file |
| Province | Id, Code, Name | Venue province |
| IntakeYear | yearStart, yearEnd | Defines the current intake window |
| Composit | RefNo; SAID; ForeignID; Name; Surname; Initials; DOB; Gender; Classification; VenueName; DOT | Second comparison source; also the scored record |
| NewNBTNumber | NewNBT PK; OriginalNBT nullable; Used; DateModified; WriterListID nullable | Pool of reserved replacement references |
| DisabledWriter | RefNo; VenueId; DOT; CategoryId; ExtraTime; CreatedBy; DateCreated; ModifiedBy; DateModified | Special-needs writer flag (adjacent module; keep compatible) |
| WritersPayment | EasyPayPayNumber; CurrentNBT; OldNBT nullable; DateModified | Links a payment to a current/old NBT |
| temp_WriterList | Same columns as WriterList (Id PK, no FK) | Staging copy |
| ProfileAllocation / TestProfile / TestAllocation / TestName | Profile; test date; the four test names; test codes | Drives the packing list profile selector |

### 13.3 Intake scoping

- Current intake = the IntakeYears row with yearStart < today < yearEnd.
- "Existing writers" for the save are WriterList rows whose Date of Test falls inside the current
  intake window.
- The active intake year is also an application setting used to name outputs.

### 13.4 Matching keys

- Writer identity for save/update: (NBT, Date of Test) pair.
- Comparison snapshots: Reference, then SA ID, then Foreign ID (section 10.1).
- Venue: file Venue string trimmed equals TestVenue.WebsiteName.

## 14. UI specification

### 14.1 Navigation

Left menu group "preparation" contains: The Tests, Venues, Load List, Writer List. The Welcome group
contains Introduction, Instructions, Map. Menu text and ordering should be preserved for familiarity.

### 14.2 Writer grid columns (Load List), in order

Errors, NBT Reference, Surname, First Name, Initials, South African ID, Foreign ID, Date of Birth,
Gender, Classification, AQL and Maths, Test Language, Venue, Date of Test, Mobile No., Home Telephone,
E-Mail, Registration Date, Easy Pay, Account Creation Date.

Grid behaviour: sortable (Errors sorts on the error count), resizable, editable in place; editing
re-runs validation immediately.

### 14.3 Context menu (Load List)

Opens on right-click inside a data cell; the row and cell are selected for the user. Order:

1. Column values for <column> - a submenu with File, WriterList, Composit values.
2. WriterList value (only when different):
   - Use WriterList value: <value> (disabled when the value cannot be applied)
   - Keep file value (update WriterList)
3. The matching WriterList record (read-only listing of all its fields).
4. Composit value (only when different):
   - Use Composit value: <value>
   - Keep file value (update Composit)
5. The matching Composit record (read-only listing).
6. Delete Row and Clean Names.

### 14.4 Command availability

| Command | Available when |
|---|---|
| Select file | Always |
| Clean File / Clean Names | A file is loaded |
| Save a new CSV file | A file is loaded |
| Save to Database | No file duplicates, list marked changed |
| Delete Selected record | A file is loaded |
| Refresh data in Table | A file is loaded |
| Check DB Duplicates | No file duplicates, list marked changed |
| Delete Row (menu) | A row is under the pointer |
| New NBT Number (menu) | A duplicate row is selected |
| Master workbook export | A processed list exists |
| Packing list export | A profile is selected |

### 14.5 Writer List screen

Grouped venue grid (expander per venue, collapsed by default) with columns Surname, Name, REG #,
Full Name, National ID, Test, AQL, Maths, Paid, Reference, Date of Test, Mobile, Home, E-Mail,
Creation Date, Registration Date; a per-venue summary grid; a profile selector; and the export buttons.

## 15. Non-functional requirements

- **NFR-1 Responsiveness**: file read, save and any whole-table read must run off the UI thread; the
  window must repaint and the progress indicator must animate. The original incident (Appendix C)
  showed view-model constructors blocking the UI for seconds; the new application must not repeat it.
- **NFR-2 Query efficiency**: comparison lookups must be keyed and chunked, never one query per row.
- **NFR-3 Durability**: saves are batched and committed; a failed save must leave the database in a
  consistent state and report the failure.
- **NFR-4 Logging**: log errors, refusals and save outcomes to a durable log (table and/or rolling file).
- **NFR-5 Database availability**: the module degrades explicitly when the database is unavailable;
  it must not silently show every row as differing.
- **NFR-6 Data safety**: blank file values never overwrite stored values; destructive clean-up is
  limited to rows in error.
- **NFR-7 File handling**: locked files are refused with a clear message.
- **NFR-8 Compatibility**: the WriterList row shape and the reconciliation semantics remain the
  contract for downstream QA/Composit consumers.

## 16. Implementation decisions

### 16.1 Recommended deep modules (build these, not one large service)

Each is a small interface that can be tested without the UI or a database.

| Module | Responsibility | Interface sketch |
|---|---|---|
| WriterFileReader | Parse the Unicode CSV, map columns, parse dates | Read(path) -> IReadOnlyList<WriterRow>; throws on locked/unreadable |
| WriterValidator | Pure validation of a row plus optional snapshots | Validate(row, writerSnapshot, compositSnapshot) -> IReadOnlyList<FieldError> |
| ChecksumValidator | Luhn for SA ID and NBT reference | IsValidSaid(s); IsValidReference(s) |
| WriterComparisonService | Resolve keys, batch-fetch, attach snapshots | AttachAsync(rows) -> rows with snapshots |
| WriterDuplicates | In-file and in-database duplicates | FindInFile(rows); FindInDatabaseAsync(rows, intake) |
| NbtNumberAllocator | Issue next unused reference | TryAllocate(row, out newRef) |
| WriterCleaner | The Clean File transformations | Clean(row) -> row plus change report |
| WriterRepository | Upsert with no-wipe semantics, intake scoping | SaveAsync(rows, intake) -> SaveResult(new, updated, skipped, refused) |
| VenueRepository | Venue master data | Get/Create/Update/Delete |
| WriterMasterWorkbookExporter | One worksheet per venue | Export(rowsByVenue, path) |
| CheckInSheetExporter | One workbook per venue | Export(venue, rows, directory, testDate) |
| PackingListExporter | Template-driven packing list | Export(summary, profile, path) |
| WriterListProcessor | Join venues, build rows, number, stats, quantities | Process(rows, venues, profile) -> ProcessedList |

Keep the processing model (per-writer row, per-venue summary) separate from the exporters so the
arithmetic in section 12 can be tested on its own.

### 16.2 Suggested technology

- The original is a Windows WPF desktop application with an MVVM structure, Entity Framework and
  ClosedXML for Excel. The new application can keep a desktop UI or move to a web UI; the behaviours
  in Part B are UI-independent except where a section explicitly describes a screen.
- If a web UI is chosen, replace "right-click context menu" with an equivalent per-cell action panel
  that preserves: select a cell, show the three values, and offer both directions.
- Keep the exports server-side or in a background worker, never on the UI thread.

### 16.3 State to preserve for the two-step flow

The Writer List screen consumes the in-session list loaded by Load List. If the new application is
multi-user or stateless, replace this with an explicit "batch/import run" entity so the processing step
can load a saved run rather than relying on process memory.

### 16.4 Module boundary decision

The cleanest boundary is:

- **Ingest and reconcile** produces a saved, validated batch (and the WriterList rows).
- **Process and export** consumes a saved batch and produces documents.

Do not let the exporters read the file directly; they should read the same processed model that the
screen shows, so the documents and the screen can never disagree.

## 17. Testing decisions

Good tests here check **external behaviour**: given a row and optional snapshots, which errors are
reported; given a set of rows and a fake store, what is inserted/updated and what is left alone; given
counts, what quantities are produced; given a file, what is parsed.

### 17.1 Prior art

The repository already has an xUnit test project with tests that exercise exactly this area:
SA ID validation theories, database-difference reporting, and accepting a file value to clear a
difference. The new application should reuse this style (plain xUnit theories/facts, no UI).

### 17.2 Modules to test (priority order)

1. **WriterValidator** - every rule in section 9.1, including all mismatch combinations of
   section 9.2 (no record, one matching record, one disagreeing record, two disagreeing records,
   one blank one disagreeing).
2. **ChecksumValidator** - valid/invalid SA IDs and references, leading zeros, non-numeric input.
3. **WriterCleaner** - each transformation in section 11, including "invalid SA ID moves to Foreign
   ID", "valid SA ID supplies DOB", "invalid SA ID does not supply DOB", and over-long first name.
4. **WriterDuplicates** - file duplicates include all members; DB duplicates use the SA ID and
   Foreign ID condition.
5. **WriterRepository** - add/update/no-change/blank-does-not-wipe; refusal when duplicates remain;
   intake scoping; batching does not change the outcome.
6. **WriterListProcessor / packing arithmetic** - section 12, including the 0, 1-4, exactly 5 and
   large-count boundaries, and the batch-string function.
7. **WriterFileReader** - column mapping, empty Easy Pay, unparseable date, locked file.
8. **Exporters** - smoke tests that a workbook is produced with the expected sheet names, headers,
   row counts and file names; do not assert formatting pixel by pixel.

### 17.3 Suggested concrete test cases (must-have)

- SA ID 8001015009087 valid; 8001015009088, 8001015009287, 8013015009087, a 11-digit value and a
  value with a letter invalid.
- Reference: a valid 14-digit reference passes; changing any digit fails the check digit.
- Mismatch: file disagrees with WriterList but matches Composit -> no error; disagrees with both -> error.
- Save: file row with blank E-mail against a stored E-mail -> stored value kept; file row with a new
  E-mail -> updated; new (NBT, DOT) -> inserted; same (NBT, DOT) with no differences -> nothing written.
- Duplicates: two rows sharing a reference -> both listed; a DB row with the same reference but the
  same SA ID -> not a DB duplicate.
- Packing: AQL_E = 3 -> AQLE_S = 10; AQL_E = 10 -> R(10)=10 + W(10)=10 = 20; AQL_E = 250 -> 250 + 10 = 260.
- Batch string: 0 -> empty; 30 -> "3(10)"; 50 -> "1(50)"; 60 -> "1(50) 1(10)".

## 18. Acceptance criteria

The new application is accepted when, for a representative writer list file and database:

1. Loading the file shows one row per writer, worst-first, with red fields and tooltips matching
   section 9.1.
2. The summary statistics equal the file's true totals.
3. A row that agrees with WriterList or Composit shows no difference; a row that agrees with neither is
   flagged and offers both directions.
4. "Use WriterList value" changes only the grid; "Keep file value (update WriterList)" changes only the
   one stored field and reports success or a specific reason.
5. Clean File alters only rows that were in error, exactly as in section 11, and never invents a DOB or
   changes a telephone number.
6. File duplicates and DB duplicates are detected per section 8.4, and a New NBT Number allocation
   updates the row and the pool.
7. Save to Database is refused while DB duplicates remain; otherwise it inserts new writers, updates
   only differing non-blank fields, preserves blank-file fields, and reports the outcome.
8. The master workbook, one check-in sheet per venue, and the packing list are produced with the
   contents and names in section 8.8 and the quantities in section 12.
9. No step blocks the UI; a large file loads with a visible progress indicator.
10. Failures are logged and surfaced, not swallowed.

---

# Part D - Appendices

## Appendix A - Writer list file format

Comma-separated, Unicode, one header row that is ignored. Fields are positional:

| # | Field | Notes |
|---|---|---|
| 0 | NBT Reference | 14 digits |
| 1 | Surname | |
| 2 | First Name | |
| 3 | Initials | |
| 4 | South African ID | 13 digits, may be empty |
| 5 | Foreign ID | up to 15 chars, may be empty |
| 6 | Date of Birth | date |
| 7 | Gender | Male or Female |
| 8 | Classification | |
| 9 | AQL and Maths | test combination; equal to "AQL" means AQL only |
| 10 | Test Language | English or Afrikaans |
| 11 | Venue | matched to TestVenue.WebsiteName |
| 12 | Date of Test | date |
| 13 | Mobile No. | |
| 14 | Home Telephone | |
| 15 | E-Mail | |
| 16 | Registration Date | date |
| 17 | Easy Pay | amount; empty treated as 0 |
| 18 | Account Creation Date | date |

Accepted date formats (Registration/Creation Date use the wider set):

- yyyy/MM/dd H:mm:ss tt
- yyyy/MM/dd HH:mm:ss
- yyyy-MM-dd H:mm:ss tt
- yyyy-MM-dd HH:mm:ss
- MM/dd/yyyy h:mm:ss tt
- yyyy-MM-dd HH:mm
- yyyy/MM/dd HH:mm
- MM/dd/yyyy HH:mm

Date of Birth and Date of Test use dd-MM-yyyy or yyyy/MM/dd handling. A value that cannot be read
yields an empty date rather than an exception.

## Appendix B - Packing arithmetic worked example

Venue with AQL_E = 3, AQL_A = 12, Maths_E = 0, Maths_A = 4:

- W_E = W(3) = ceil(3/250)*10 = 10
- AQLE_S = R(3) + 10 = 10 + 10 = 20; then the small-count override (0 < 3 < 5) sets AQLE_S = 10
- W_A = W(12) = ceil(12/250)*10 = 10
- AQLA_S = R(12) + 10 = 20 + 10 = 30
- MATE_S = 0 (Maths_E = 0)
- MATA_S = R(4) + 10 = 10 + 10 = 20; the small-count override (0 < 4 < 5) sets MATA_S = 10
- Batch strings: AQLE_Batch = B(10) = "1(10)"; AQLA_Batch = B(30) = "3(10)";
  MATE_Batch = B(0) = ""; MATA_Batch = B(10) = "1(10)"
- Total sent = AQLE_S + AQLA_S = 10 + 30 = 40
- Total walk-ins = W_E + W_A = 10 + 10 = 20

## Appendix C - Known defects and risks (fix, do not reproduce)

1. **UI-thread blocking.** Whole-table reads in a view-model constructor froze the shell; the Writer
   List view-model took seconds to construct. Move work off the UI thread and show progress.
2. **N+1 queries.** A per-row lookup inside a mapping loop caused thousands of round trips. Chunk and
   key all comparison lookups.
3. **Unresolved venue crash.** When the file's Venue matches no TestVenue.WebsiteName, the code shows a
   message and then still dereferences the null venue, which throws. The new application must stop the
   save for that row and report it.
4. **Generic mismatch messages.** The current implementation reports "Name differs from database"
   rather than naming the WriterList and Composit values separately, and the repository's unit tests
   expect the richer wording. Decide the wording once and make code and tests agree (open item Q-1).
5. **Dead writer-list code paths.** A helper named GenerateWriterList is an empty stub, and the
   composite view-model's GenerateWriterList is commented out. Do not treat either as the source of
   truth; the real outputs are described in section 8.8.
6. **Paid amount and Easy Pay.** Only the amount carried on the writer row is in scope; FTP payment
   handling is a separate module.
7. **SA ID leading zeros.** SA IDs are stored as a long; always format to 13 digits when displaying or
   comparing strings.

## Appendix D - Intake writer-list workbook columns (related output)

The composite step writes a worksheet named "Writerlist" with these columns, in order:

1. Test Session ID
2. NBT Reference
3. Surname
4. First Name
5. Middle Initials
6. South African ID
7. Foreign ID
8. Date of Birth
9. Wrote AL
10. Wrote QL
11. Wrote Maths
12. Student Number
13. Faculty
14. Programme
15. Date of Test
16. Venue
17. Gender
18. Street and Number
19. Street Name
20. Suburb
21. City/Town
22. Province/Region
23. Postal Code
24. e-mail Address
25. Landline Number
26. Mobile Number
27. AL Score
28. AL Performance
29. QL Score
30. QL Performance
31. Maths Score
32. Maths Performance
33. AQL TEST LANGUAGE
34. MATHS TEST LANGUAGE

Output file name pattern: "<intake year> Intake Cycle writerlist_<date>". The sibling workbook
"NBT_Composite" carries the composite columns; other files (UCT, UFS, WEB uploads) are outside this
module.

## Appendix E - Source map (evidence for this specification)

Existing implementation, for reference only (paths relative to the repository root):

- Loading and validation model: CETAP_LOB/Model/venueprep/WebWriters.cs
- Import, save, duplicates, cleaning, NBT allocation, composite writer-list export:
  CETAP_LOB/Model/DataService.cs (Writer prep region, Writerlist to Database region,
  New NBT Numbers region, GenerateExcelComposite)
- Load List view model: CETAP_LOB/ViewModel/writers/LoadWritersViewModel.cs
- Load List view and context menu: CETAP_LOB/View/writers/LoadFileView.xaml(.cs)
- Writer List processing and exports: CETAP_LOB/ViewModel/writers/ProcessViewModel.cs
- Writer List view: CETAP_LOB/View/writers/ProcessView.xaml
- Venue maintenance: CETAP_LOB/ViewModel/writers/VenuesViewModel.cs,
  CETAP_LOB/ViewModel/writers/VenueViewModel.cs, CETAP_LOB/View/writers/VenuesView.xaml(.cs),
  CETAP_LOB/View/writers/VenueMapView.xaml(.cs)
- Entities: CETAP_LOB/Database/WriterList.cs, temp_WriterList.cs, TestVenue.cs, NewNBTNumber.cs,
  DisabledWriter.cs, WritersPayment.cs, Composit.cs
- Packaging/rounding helpers: CETAP_LOB/Helper/HelperUtils.cs
- Checksum and date parsing: CETAP_LOB/Helper/HelperUtils.cs
- Navigation: CETAP_LOB/ViewModel/MainViewModel.cs
- Configuration (database, intake year, folders, logging): CETAP_LOB/App.config
- Existing tests: LOB_Tests/SouthAfricanIdValidationTests.cs
- Existing user guide for the Load List screen: docs/LoadListView-User-Guide.md
- Performance notes: docs/Application-Performance-Notes.md

## Appendix F - Open questions

- **Q-1** Should the database-difference message name the WriterList and Composit values separately
  (as the unit tests expect), or remain generic (as the current code and user guide show)?
- **Q-2** Should the Writer List screen depend on a saved batch rather than process memory, so that it
  can be re-run after a restart?
- **Q-3** Is the venue check-in sheet required to be A4 portrait with the exact print area and header
  wording, or is the content the requirement and the styling guidance?
- **Q-4** Should failed per-row venue resolution abort the whole save or skip only that row? The
  existing code intends a message but then fails.
- **Q-5** Is the intake writer-list workbook (Appendix D) in scope for the new application, or is it
  owned by the composite pipeline and only an interface to keep compatible?
