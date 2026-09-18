# QAView — QA Processing User Guide

Applies to the CETAP LOB desktop application (WPF, .NET Framework 4.8).
This guide describes the **QA** screen under **Data Processing** and the end-to-end
quality-assurance workflow for scanned NBT answer-sheet batches (`.dat` files).

---

## 1. What QAView is for

After answer sheets are scanned they are written as `.dat` batch files. Before those
batches can be scored, a QA operator must confirm that the scanned candidate details
are complete and correct.

QAView is that workstation. For each batch file it:

1. Reads every candidate record and validates it.
2. Shows each candidate with a count of validation errors.
3. Lets the operator correct records in place, or pull missing data from the database.
4. Checks the batch for duplicate barcodes / NBT numbers / IDs.
5. Writes the corrected file to a dated sub-folder and records it on the scan tracker.
6. Produces an Excel summary for scoring.

---

## 2. Before you start

| Requirement | Where it is set |
|---|---|
| **QA folder** — the folder QAView reads `.dat` files from and writes results to | **Scan Settings** (`QAFolder`) |
| **Intake year** — selects the candidate/test period used for database look-ups | **Scan Settings** (`IntakeYear`) |
| **Database availability** — enables the database look-up features | Set automatically at start-up (`DBAvailable`) |

When the database is unavailable, QAView still validates and saves files, but the
database-dependent actions (Get NBT / Get names / Get ID / Correct DOB / AutoClean
database fixes / WriterList comparison) are skipped or do nothing.

---

## 3. Opening QAView

1. Start **CETAP LOB**.
2. In the top navigation bar choose the **Data Processing** group.
3. Click **QA**.

The group also contains *Batching*, *Tracker*, *Error Logs* and *Bio QA*.

---

## 4. Screen layout

```
┌───────────────┬──────────┬───────────────────────────────────────────────────────┐
│               │          │                                                       │
│  QA Files     │ Toolbar  │   Candidate grid (one row per candidate)              │
│  (batch list) │ (7 icons)│   Errors | ScanNo | Edited | Barcode | NBT Reference… │
│               │          │                                                       │
├───────────────┴──────────┴───────────────────────────────────────────────────────┤
│  ◀──────────── drag bar (GridSplitter) ────────────▶                              │
├──────────────────────────────────────────────────────────────────────────────────┤
│  AQL answer sections:   1 ▭▭▭▭  2 ▭▭▭▭  3 ▭▭▭▭  4 ▭▭▭▭  5 ▭▭▭▭  6 ▭▭▭▭  7 ▭▭▭▭    │
│  MAT SECTION:           ▭▭▭▭▭▭▭▭                                                 │
└──────────────────────────────────────────────────────────────────────────────────┘
```

- **Left panel — QA Files** — every `*.dat` file in the QA folder.
- **Middle — toolbar** — the batch-level actions (section 8).
- **Right — candidate grid** — the records of the selected file.
- **Bottom — answer sections** — the AQL sections 1–7 and the MAT section of the
  selected candidate, one box per answer.
- The horizontal **drag bar** resizes the grid against the answer sections.

---

## 5. The QA Files list

Each entry is a batch file. The list is sorted by **error count, highest first**, so
the worst files are at the top.

The file name is coloured by its total error count:

| Errors | Colour |
|---|---|
| 0 | Black (clean) |
| 1 – 5 | Chocolate |
| 6 – 10 | Violet |
| 11 – 20 | Orange |
| 21 – 30 | Orange Red |
| above 30 | Red |

Clicking a file loads its candidates into the grid. Loading a file reads the `.dat`
file, parses the fixed-width records and runs the full validation set (section 7).

---

## 6. The candidate grid

One row per candidate. Rows are sorted by error count, highest first.

| Column | Meaning |
|---|---|
| **Errors** | Number of validation errors on the record (see section 7) |
| **ScanNo** | Scan sequence number within the batch |
| **Edited** | Edit marker carried on the record |
| **Barcode** | 12-character session/barcode |
| **NBT Reference** | 14-character NBT number |
| **Surname** | Candidate surname |
| **First Name** | Candidate first name |
| **Initials** | Candidate initials |
| **South African ID** | 13-digit SA ID |
| **Foreign ID** | Passport / foreign ID (when there is no SA ID) |
| **ID Type** | ID type code |
| **Date of Birth** | Candidate date of birth |
| **Gender** | Gender code |
| **Citizenship** | Citizenship code |
| **Venue Code** | Test venue (file-level constant) |
| **Date of Test** | Test date (file-level constant) |
| **Home Language** | Home language code |
| **School Language** | School / grade-12 language code |
| **Classification** | Classification code |
| **AQL Language** | AQL test language (file-level constant) |
| **AQL Code** | AQL test code (file-level constant) |
| **Mat Language** | Maths test language (file-level constant) |
| **Mat Code** | Maths test code (file-level constant) |
| **Faculty1 / 2 / 3** | Faculty preference codes |

---

## 7. Validation and error rules

Validation runs as each record is loaded and again after every edit. Each failing
field adds an error; the **Errors** column is the number of failing fields.

### 7.1 Field rules

| Field | Checked |
|---|---|
| **NBT Reference** | Must be exactly 14 characters and have a valid NBT check digit |
| **Barcode** | Must be exactly 12 characters and have a valid check digit |
| **Surname** | Not empty; no digits; no special characters; each part at least 2 letters; at most 2 spaces |
| **First Name** | Not empty; no digits; no special characters; each part at least 2 letters; at most 2 spaces |
| **SA ID** | Valid 13-digit SA ID check digit; numeric; no spaces |
| **Foreign ID** | Either an SA ID or a Foreign ID must be present |
| **Date of Birth** | Within the Matric age range; must agree with the SA ID when one is present |
| **ID Type** | Must match the expected type for the file's CSX family (e.g. `1`/`2`, or `S`/`F`) |
| **Gender** | Must agree with the SA ID and the expected code for the file's CSX family |
| **Citizenship** | Must be present and less than 5 |
| **Classification** | Numeric, present, and less than 6 |
| **Venue Code** | Must equal the venue encoded in the file name |
| **Date of Test** | Must equal the batch test date |
| **Home Language** | Must be present; a numeric value must be below 13 |
| **School Language** | Must be present; a numeric value must be 01, 02 or 03 |
| **AQL Language** | Must equal the file's AQL language |
| **AQL Code** | Must equal the AQL test code expected for the file's profile. If the profile yields no AQL code, the value is not checked - except on a Maths-only file, where it must be blank |
| **Mat Language** | Must equal the file's Maths language |
| **Mat Code** | Must equal the Maths test code expected for the file's profile. If the profile yields no Maths code, the value is not checked - except on an AQL-only file, where it must be blank |
| **Sections 1–7** | Every answer must be A, B, C, D, N, X or blank |
| **Maths Section** | Every answer must be a valid answer value |
| **Faculty1 / 2 / 3** | Faculty 1 required; no `*` placeholders |

### 7.2 Subject-specific rules

- AQL-only files (test codes `0105`, `0115`) — the **Maths section must be blank**.
- Maths-only files (test codes `0106`, `0116`) — the **AQL sections must be blank** and
  the AQL language must be empty.

### 7.3 How errors are shown

- The **Errors** column and the file-list colour give the count.
- A field that fails validation is outlined with a red validation border and the
  message appears as a tooltip on the field - **red always means a validation error**.
- Bio-information differences against the WriterList are shown in **purple** on the
  affected fields (section 9), so they are never confused with validation errors.
- Correcting a field clears its marking immediately: a field is only coloured while its
  value still fails validation, or still differs from the matching WriterList record.

---

## 8. Toolbar commands

The seven icons down the left of the grid are, top to bottom:

| # | Icon tooltip | What it does |
|---|---|---|
| 1 | **Save File** | Writes the corrected records back to fixed-width format, moves the file into a dated sub-folder (`<QAFolder>\<yyyyMMdd>\`), deletes the original `.dat`, records the QA count and QA date on the scan tracker, removes the file from the list and clears the grid. |
| 2 | **Refresh Directory** | Re-reads the QA folder and rebuilds the file list. |
| 3 | **AutoClean** | Automatically repairs the fields the application can repair (see below). |
| 4 | **Duplicate Barcodes** | Runs the full duplicate check for the whole QA folder (section 10). Only enabled when every file in the list has zero errors. |
| 5 | **Write Excel Summary data** | Writes `SummaryForScoring.xlsx` into the QA folder (section 11). Only enabled when every file in the list has zero errors. |
| 6 | **Update Dat Tracker** | Reconciles each file's record count with the scan tracker. |
| 7 | **Find Duplicates** | *Not currently available* — see section 15. |

### 8.1 AutoClean — what it repairs

AutoClean walks every record that has errors and fixes the fields it can determine
automatically:

- **NBT Reference** — looked up from the database by SA ID or Foreign ID; if the
  candidate is not in the database, a new NBT number is allocated instead.
- **Surname / First Name** — reloaded from the database by NBT.
- **SA ID / Foreign ID / Date of Birth** — reloaded from the database by NBT.
- **ID Type** — flipped between the two valid codes.
- **Venue Code** — set from the venue encoded in the file name.
- **Date of Test** — set from the batch test date.
- **AQL Language / Mat Language** — set from the file's language.
- **AQL Code / Mat Code** — set from the expected test code.
- **Faculty1 / 2 / 3** — `*` placeholders replaced with `N`.
- **Sections 1–7, Maths Section** — `*` replaced with `N`, and the section emptied
  when the file format says it should be blank.

AutoClean only changes records that failed validation; clean records are untouched.

---

## 9. Bio information mismatch against the WriterList

Every record that can be matched to a candidate in the WriterList is compared with
the WriterList details.

### 9.1 How records are matched

1. First by **NBT Reference** (exact match on the WriterList NBT).
2. If there is no NBT match, by **SA ID**.

A record with no match is left alone. It is **not** flagged by this check.

### 9.2 What is compared

| QA record | WriterList |
|---|---|
| First Name | Name |
| Surname | Surname |
| NBT Reference | NBT |
| SA ID | SA ID |
| Foreign ID | Foreign ID |
| Date of Birth | DOB |
| Gender | Gender |
| Date of Test | DOT |

Text comparisons ignore letter case and surrounding spaces. Dates are compared by
calendar day. Gender is treated as equivalent across encodings (`1` = `M`, `2` = `F`).
A field that is blank on the WriterList side is **not** treated as a mismatch.

### 9.3 How a mismatch is shown

- Only the fields that actually differ are shown in **purple text** - the rest of the
  record keeps its normal appearance, and the normal validation borders are not
  affected. Purple is used so a WriterList difference is never mistaken for the red
  validation errors (section 7.3).
- Hovering a purple field shows which fields differ.
- The record's error count includes the mismatch.

### 9.4 Correcting a mismatch

Right-click the purple field. The grid's context menu opens with a
**Use WriterList value: <value>** entry at the top, for example
`Use WriterList value: MOKOENA`. Clicking it copies the WriterList value into that
field; the field re-validates and the purple marking clears if it now agrees.

The entry is offered only for the eight comparable columns and only when the candidate
has a WriterList match. It is enabled for a field whenever using the WriterList value
would actually change it - including when the scanned value is missing or unreadable
(for example a Reference of `*`). This is how a blank or invalid **NBT Reference** is
filled in from the WriterList. For all other columns the menu is unchanged.

---

## 10. File-level values (constants for the whole file)

Six fields describe the file as a whole and must be **identical on every record**:

- **Venue Code**
- **Date of Test**
- **AQL Language** and **AQL Code**
- **Mat Language** and **Mat Code**

### 10.1 Correcting one corrects all

These fields are handled as file-level values. Typing a new value into any one row
immediately writes that value to **every** record in the loaded file, so the whole
file stays consistent and correcting one record corrects the file.

### 10.2 Detecting the odd ones out

Whenever a record's value does not agree with the file's expected value, the normal
validation flags it (section 7): `Wrong Venue Code`, `Wrong Test date`,
`Wrong Language`, or the AQL/MAT code message. Fix the value once and the whole file
is corrected.

---

## 11. Duplicate detection (Duplicate Barcodes)

The duplicate check examines **every file in the QA folder**, not just the selected
one. It works in stages and stops at the first stage that finds duplicates:

1. **Within the QA batches** — repeated Barcode, NBT Reference, SA ID or Foreign ID
   among the QA records.
2. **Against the scoring queue** — barcodes, SA IDs or foreign IDs already queued for
   scoring.
3. **Against the database** — matches already recorded in the database.

If any duplicates are found, an Excel report is written to the QA folder
(`Duplicate QA records<yyyyMMdd_HHmm>.xlsx`) listing barcode, NBT number, batch name,
SA ID, passport, reason and date; a dialog reports how many were found and asks you
to re-run the option after corrections.

If no duplicates are found the barcodes are placed on the scoring queue and a
confirmation is shown.

---

## 12. Excel summary (Write Excel Summary data)

Produces `SummaryForScoring.xlsx` in the QA folder with two sheets:

| Sheet | Contents |
|---|---|
| **Batches** | One row per batch: batch id, batch name, file combination, test code, client, profile, AQL language, Maths language, record count, venue code |
| **Venues** | Totals per venue: venue code, venue name, total candidates |

The summary is built from all `.dat` files currently in the QA folder, and each file
is also written to the QA database table as it is processed.

---

## 13. A typical session

1. Open **Data Processing → QA**.
2. Pick the batch with the highest error count from **QA Files**.
3. Review the grid; work from the top (worst) rows downwards.
4. For each candidate:
   - correct obvious typing errors directly in the cells;
   - use the right-click database commands to fetch NBT, names, ID or date of birth;
   - use the right-click **Use WriterList value** entry to fix bio differences;
   - check the AQL sections 1–7 and the MAT section at the bottom.
5. Run **AutoClean** to let the application repair everything it can, then review what
   remains.
6. When the file shows 0 errors, click **Save File**.
7. Repeat for the remaining files.
8. When all files are clean, run **Duplicate Barcodes**, fix anything it reports and
   re-run it.
9. Run **Update Dat Tracker**.
10. Run **Write Excel Summary data** for scoring.

---

## 14. The right-click menu

Right-click a candidate row (or any non-bio cell) to reach:

| Menu item | Action |
|---|---|
| **Delete Record** | *Not currently available* — see section 15. |
| **Get NBT Number from DataBase** | Looks up the NBT for the selected candidate by SA ID (or Foreign ID when there is no SA ID). |
| **Get Name and Surname from DataBase** | Fills first name and surname from the database for the record's NBT. |
| **Get ID using NBT from DataBase** | Fills SA ID and Foreign ID from the database for the record's NBT. Only applies when the reference has `9` as its 8th character; otherwise it does nothing. |
| **Correct Date of Birth** | Fills the date of birth from the database for the record's NBT. |
| **Clear Math Area** | *Not currently available* — see section 15. |
| **Add Surname to Database** | Adds the record's surname to the database surname list. |
| **Add First Name to Database** | Adds the record's first name to the database first-name list. |

On the eight bio columns the menu instead shows the **Use WriterList value** entry
(section 9.4) at the top, followed by a separator and the items above.

---

## 15. Known limitations

| Item | Status |
|---|---|
| Toolbar **Find Duplicates** | The button is present but has no action attached. Use **Duplicate Barcodes** instead. |
| Right-click **Delete Record** | The menu item is present but has no action attached. |
| Right-click **Clear Math Area** | The menu item is present but has no action attached. |
| **Bio QA** page | A separate page exists but is not fully implemented; use the **QA** page. |

---

## 16. File naming convention

QAView derives the file's expected values from the 22-character `.dat` file name:

```
TestCode(4) Profile(2) x Venue(5) x Random(5) Client(1) Records(3)
```

| Part | Meaning |
|---|---|
| **Test code** | `0105` AQL English · `0115` AQL Afrikaans · `0106` MAT English · `0116` MAT Afrikaans · `0107` AQL+MAT English · `0117` AQL+MAT Afrikaans · `0127` AQL English + MAT Afrikaans · `0137` AQL Afrikaans + MAT English |
| **Profile** | Test profile number |
| **Venue** | 5-digit venue code |
| **Client** | `R` National · `X` Re-score · `M` Moderated · `W` Remote · `S` Special · `O` Walk-in Bio · `D` Disability · `B` Braille · `L` Large Print |
| **Records** | Number of candidates in the file |

A file name that is not 22 characters, or whose test code or client letter is not
recognised, is rejected with a *Wrong File Name*, *No Such Test Code* or
*No Such Client* error.

For **Walk-in Bio** (`O`) files the venue and random blocks shift one position left
(venue at positions 7–11, random at 12–16).

The scanner format is identified by the CSX number on the first record; supported
formats are `667`, `761`, `886` and `909`.

---

## 17. Troubleshooting

| Symptom | What to check |
|---|---|
| The QA file list is empty | The **QA folder** in Scan Settings; it must contain `.dat` files. |
| A file loads with no records | The file may be malformed or in an unsupported CSX format. The scanner format must be 667, 761, 886 or 909. |
| Many fields show `Wrong Language` / `Wrong AQL Code` | The file name's test code and profile decide the expected language and code - check the file name against section 16. |
| Every row shows the same venue/date/language error | The file-level value is wrong (or the file name is wrong). Correct it once on any row; it applies to the whole file. |
| Database commands do nothing | The database is unavailable. Check connectivity and restart; `DBAvailable` is set at start-up. |
| A purple bio field will not clear | The WriterList value and the scanned value may both be wrong, or the candidate is genuinely different. Use the right-click **Use WriterList value** entry or edit the field; check the database record. |
| **Duplicate Barcodes** / **Write Excel Summary data** are greyed out | They only enable when every file in the QA list has zero errors. |
| **SummaryForScoring.xlsx** cannot be saved | The file may be open in Excel. Close it and re-run. |

---

## 18. Glossary

| Term | Meaning |
|---|---|
| **Batch / `.dat` file** | One scanned answer-sheet file. |
| **NBT Reference** | The candidate's NBT number - the primary candidate identifier. |
| **Barcode / Session ID** | The 12-character barcode printed on the answer sheet. |
| **CSX** | Scanner/format family that determines how the fixed-width file is parsed. |
| **WriterList** | The authoritative candidate register held in the database. |
| **File-level value** | A value that must be identical for every record in the file (venue, test date, languages, test codes). |
| **AutoClean** | The automatic repair pass over all failing records. |
| **Tracker** | The scan tracker that records when a batch was QA'd and sent for scoring. |
