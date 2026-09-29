# LoadFileView — Writer List "Load List" User Guide

The **Load List** screen is the Writer List preparation step. It loads a writer list file,
checks every writer against the rules the database expects, compares each writer with the
**WriterList** and **Composit** tables, lets you settle the differences in either direction,
and writes the list into the database.

Where to find it: **preparation → Load List** (in the left hand menu), or
`/View/writers/LoadFileView.xaml`.

---

## 1. What Load List is for

A writer list file is produced by the web registration system. Before those writers can be
batched and scored they have to exist in the **WriterList** table with correct biographical
details.

Load List does four things:

1. **Reads** the file and shows one row per writer.
2. **Validates** the fields that matter (section 5) and colours the failures red.
3. **Compares** every row with the WriterList and Composit records already in the database,
   and shows the differences on the right-click menu (section 7).
4. **Writes** the list into the database (**Save to Database**), after adding the writers that
   are not there yet.

---

## 2. Before you start

- The **database must be available**. The comparison values, the duplicate checks, the new NBT
  numbers and the save all read or write the database. If it is not reachable the screen still
  opens, but those commands do nothing useful.
- Have the writer list **file** to hand. It is a comma separated file with a header row,
  in Unicode, with the columns in the order listed in section 10.
- The file must not be open in Excel (a locked file is refused).
- Load List is about **writer lists**. Candidate QA files are handled by the separate
  **QA** module.

---

## 3. Opening the module and screen layout

The screen has three bands:

| Area | What is there |
|---|---|
| Top | The **Select file** button, the name of the file that is loaded, and **Clean File** |
| Middle | The **writer grid** - one row per writer, worst first (most errors at the top) |
| Bottom | **File Summary Stats**, the **Duplicates** list (left) and the **DB Duplicates** list (right), and the command buttons |

While a file is being read or saved, a progress ring spins at the bottom right and the row
counts stay as they were until the read finishes.

---

## 4. The writer grid

Each row is one writer. The columns are:

| Column | What it shows |
|---|---|
| Errors | How many fields on that row failed validation |
| NBT Reference | The 14 digit NBT number |
| Surname | Family name |
| First Name | Given name |
| Initials | Initials as registered |
| South African ID | 13 digit SA identity number, when the writer has one |
| Foreign ID | Passport or foreign identity number, when the writer has one |
| Date of Birth | Taken from the file |
| Gender | Male or Female |
| Classification | Classification as registered |
| AQL and Maths | The test combination the writer is registered for |
| Test Language | English or Afrikaans |
| Venue | Venue the writer is registered at |
| Date of Test | Date of the test |
| Mobile No. | Mobile number |
| Home Telephone | Home telephone number |
| E-Mail | E-mail address |
| Registration Date | Date the writer registered |
| EasyPay | Amount paid |
| Account Creation Date | Date the web account was created |

Columns can be sorted and resized, and a cell can be edited in place. Editing a cell
re-runs the validation for that row immediately, and the **Errors** column follows.

---

## 5. How errors are shown, and the rules behind them

A field that fails validation is shown in **red** with the message as a tooltip. The
**Errors** column counts the failed fields on that row, and the grid is sorted worst first so
the rows that need attention come to the top.

| Field | Rules | Message shown |
|---|---|---|
| **NBT Reference** | Cannot be empty | *NBT number cannot be empty* |
| | Must be exactly 14 characters | *Not proper length for NBT number* |
| | The last digit must be a valid check digit | *Not a Valid NBT number* |
| | Must agree with WriterList or Composit | *Reference differs from database* |
| **First Name** | Cannot be empty | *FirstName cannot be empty* |
| | Cannot contain digits | *First name cannot have digits* |
| | Cannot start or end with a space and cannot contain punctuation | *cannot start/end with space or have funny characters* |
| | At most 18 characters | *To many characters for Name (max is 18)* |
| | Must agree with WriterList or Composit | *Name differs from database* |
| **Surname** | Cannot be empty | *Surname cannot be empty* |
| | Cannot contain digits | *Surname cannot have digits* |
| | Cannot start or end with a space and cannot contain punctuation | *cannot start/end with space or have funny characters* |
| | At most 30 characters | *Too many characters for Surname* |
| | Must agree with WriterList or Composit | *Surname differs from database* |
| **South African ID** | Optional - a writer with only a foreign ID is fine | |
| | Digits only | *SA Id must contain only digits* |
| | Exactly 13 digits | *SA Id must be 13 digits* |
| | The first six digits must be a real date of birth | *SA Id date of birth is not valid* |
| | The 11th digit (citizenship) must be 0 or 1 | *SA Id citizenship digit (11th) must be 0 or 1* |
| | The 13th digit must be a valid check digit | *SA Id check digit is not valid* |
| | Must agree with WriterList or Composit | *SA ID differs from database* |
| **Foreign ID** | Optional | |
| | At most 15 characters | *ForeignID has too many characters* |
| | Must agree with WriterList or Composit | *Foreign ID differs from database* |
| **Date of Birth** | The writer must be between 10 and 99 years old | *Wrong age for Matric* |
| | Must agree with WriterList or Composit | *Date of birth differs from database* |

**"Differs from database"** is only raised when the file value matches **neither** the
WriterList record **nor** the Composit record attached to that row. If either of them is
blank, or agrees with the file, the row is accepted. A difference on its own is therefore not
an error where the database has nothing to compare with.

The remaining columns (initials, classification, test combination, language, venue, date of
test, contact details, EasyPay, account creation date) are loaded and saved but are not
validated on this screen.

---

## 6. File Summary Stats

After a file is read, the **File Summary Stats** panel shows:

| Statistic | Meaning |
|---|---|
| Total Writers | Number of rows read from the file |
| Total Venues | Number of distinct venues in the file |
| Males / Females | Writers by gender |
| English / Afrikaans | Writers by test language |

Use these to sanity-check the file against what you expect before saving it.

---

## 7. Comparing with the database (the right-click menu)

This is the heart of Load List: every row is compared with the WriterList and Composit
records the database holds for that writer, and the differences are offered to you as menu
entries.

### 7.1 Which database records a row is compared with

| Table | Tried in this order |
|---|---|
| WriterList | NBT Reference, then South African ID, then Foreign ID |
| Composit | NBT Reference, then South African ID, then Foreign ID |

The first match found is used. If no record matches, that row simply has nothing to compare
with and the corresponding menu section is not shown.

The lookup is done for the whole file when it is read, so a row you have edited is still
compared against the record it matched at load time. A row that found nothing at load time is
looked up again when you right-click it, using the values on screen at that moment - correcting
a reference can therefore bring a comparison to life, and one that has already matched keeps
the record it found when the file was read.

### 7.2 The menu

Right-click inside a data cell (the row and cell under the pointer are selected for you) and
the menu opens with:

1. **Column values for <column>** - a submenu showing, for the column you right-clicked:
   - `File: <the value in the grid>`
   - `WriterList (DIFFERENT): <value>` or `WriterList: same as file`
   - `Composit (DIFFERENT): <value>` or `Composit: same as file`

   This lets you see all three values at a glance before deciding.

2. **WriterList value** (only when the WriterList value differs from the file)
   - **Use WriterList value: <value>** - copies the WriterList value into the grid. The
     database is not changed by this entry.
   - **Keep file value (update WriterList)** - keeps what the file says and writes it into
     WriterList straight away.

3. **The matching WriterList record** - a read-only listing of the whole WriterList record,
   so you can see the rest of what the database holds for that writer.

4. **Composit value** (only when the Composit value differs from the file)
   - **Use Composit value: <value>** - copies the Composit value into the grid.
   - **Keep file value (update Composit)** - writes the file value into Composit.

5. **The matching Composit record** - a read-only listing of the whole Composit record.

6. The standard entries: **Delete Row** and **Clean Names** (section 8).

The columns that can be compared and corrected this way are **NBT Reference**, **Surname**,
**First Name**, **South African ID**, **Foreign ID**, **Date of Birth** and **Gender**.
Right-clicking any other column still shows the menu, but without value entries for it.

### 7.3 Direction of the correction

| What you want | What to click |
|---|---|
| The database is right, the file is wrong | **Use WriterList value** / **Use Composit value** |
| The file is right, the database is stale | **Keep file value (update WriterList)** / **Keep file value (update Composit)** |

Both *Keep file value* entries write to the database **immediately**, for that one field and
that one writer. They do not save the file: use **Save a new csv file** if you also want the
corrected list on disk. If the write fails - no matching database record, a value that is not
a number for NBT Reference or South African ID, or a database validation rule - a message
explains why and nothing is changed.

---

## 8. Toolbar and menu commands

| Command (tooltip) | What it does | When it is available |
|---|---|---|
| **Select file** | Opens the file browser and reads the chosen writer list file | Always |
| **Clean File** / **Clean Names** | Repairs only the rows that have validation errors (see section 9.1 - it changes real data, so review it) | A file is loaded |
| **Save a new csv file** | Writes the rows you are looking at, with your corrections, to a CSV file you choose | A file is loaded |
| **Save to Database** | Adds the writers that are not in WriterList for the intake year yet, matched on NBT Reference and Date of Test. Writers that are already there are left as they are; a message reports when it finishes | The file has no duplicated NBT references in it, and the list has been marked as changed |
| **Delete Selected record** | Removes the selected row from the loaded list. The database is not touched | A file is loaded |
| **Refresh data in Table** | Re-reads the loaded rows, re-runs validation and the duplicate checks | A file is loaded |
| **Check DB Duplicates** | Looks every NBT Reference in the file up in WriterList for the intake year and fills the **DB Duplicates** list | The file has no duplicated NBT references in it, and the list has been marked as changed |
| **Delete Row** (right-click) | Same as **Delete Selected record**, for the row you right-clicked | A row is under the pointer |
| **New NBT Number** (right-click, duplicate lists) | Allocates a replacement NBT number to the record from the pool of new numbers, records the number it replaced, and refreshes the screen | A duplicate row is selected |

---

## 9. The duplicate lists

| List | What it holds |
|---|---|
| **Duplicates** (bottom left) | Rows in the loaded file that share the same **NBT Reference** with another row in the same file. Filled as soon as the file is read |
| **DB Duplicates** (bottom right) | Rows whose **NBT Reference** already exists in WriterList for the intake year but with a **different South African ID and Foreign ID** - that is, the same NBT number is being used by somebody else. Filled by **Check DB Duplicates** |

While the **DB Duplicates** list has rows in it, **Save to Database** does not write anything;
a warning is logged instead. Settle the duplicates first - either correct the numbers in the
grid, or use **New NBT Number** on the duplicate row - and run **Check DB Duplicates** again
until the list is empty.

**Save to Database** and **Check DB Duplicates** become available once the file has no
duplicated NBT references inside it - at that moment the screen also marks the list as
changed - so a list that is waiting on you to settle its duplicates cannot be saved.

---

### 9.1 What Clean File changes

**Clean File** (and the **Clean Names** menu entry) works through the rows that currently have
validation errors and repairs them:

| Field in error | What it does |
|---|---|
| Surname | Removes `;`, `!`, `@`, `'` and the `&#039;` sequence, and replaces accented characters (`é`, `è`, `ë`, `í`, `ì`, `ï`, `ò`, `ô`, `ö`, `á`, `à`, `ã`, `ú`, `ü`, `ç`, `©`) with plain letters |
| First Name | The same replacements, and when the name is longer than 18 characters the last word is dropped |
| South African ID | When the Foreign ID is empty the SA ID is **moved into the Foreign ID field** and the SA ID cleared; when a Foreign ID is already there the SA ID is **cleared**. (The ID that failed validation is treated as a passport number) |
| Date of Birth | Taken from the South African ID when there still is one; otherwise set to **01/01/1960** |
| Home Telephone | When it is longer than 15 characters it is replaced by the mobile number |

> **Clean File changes data, not just formatting.** It can clear a South African ID, move it to
> the foreign ID column, overwrite a date of birth, or overwrite a telephone number. The rows it
> touched are exactly the rows that were red, so check them - and use **Save a new csv file** to
> keep a copy if you are unsure.

---

## 10. The writer list file layout

Comma separated, Unicode, with a header row (the header is not read). The fields must be in
this order:

| # | Field | Notes |
|---|---|---|
| 1 | NBT Reference | 14 digits |
| 2 | Surname | |
| 3 | First Name | |
| 4 | Initials | |
| 5 | South African ID | 13 digits, may be empty |
| 6 | Foreign ID | Up to 15 characters, may be empty |
| 7 | Date of Birth | Date |
| 8 | Gender | `Male` or `Female` |
| 9 | Classification | |
| 10 | AQL and Maths | Test combination |
| 11 | Test Language | `English` or `Afrikaans` |
| 12 | Venue | |
| 13 | Date of Test | Date |
| 14 | Mobile No. | |
| 15 | Home Telephone | |
| 16 | E-Mail | |
| 17 | Registration Date | Date |
| 18 | EasyPay | Amount; empty is treated as 0 |
| 19 | Account Creation Date | Date |

Dates are read in the formats the web registration system produces; a date that cannot be
read comes through as an empty date and is reported by the Date of Birth rule if it matters.

---

## 11. A typical session

1. **preparation → Load List**.
2. Click **Select file** and choose the writer list file. Wait for the progress ring: the
   stats, the grid and the **Duplicates** list fill in.
3. Check **File Summary Stats** against what you expect (total writers, venues, gender and
   language split).
4. Work through the red fields, worst rows first. Hover a red field to read why it failed.
5. **Clean File** repairs the red rows in one pass (section 9.1). It changes data - it can
   clear or move a South African ID and overwrite a date of birth - so review the affected rows
   afterwards rather than trusting it blindly.
6. For each row that says a value *differs from database*, right-click the field and use
   **Column values for ...** to see the file, WriterList and Composit values together, then
   choose which side is right (section 7.3).
7. Run **Check DB Duplicates**. If the DB Duplicates list fills, deal with each row - correct
   the NBT Reference or allocate a **New NBT Number** - and check again.
8. When the list is clean, click **Save to Database** and wait for the confirmation.
9. Optionally click **Save a new csv file** to keep a corrected copy of the list.

---

## 12. Troubleshooting

| Symptom | What to check |
|---|---|
| "File is opened by another process or file does not exist" | The file is open in Excel, or has been moved. Close Excel and select the file again |
| The grid is empty after selecting a file | The file may have the wrong columns or the wrong encoding - it must be the Unicode comma separated export described in section 10 |
| Every row shows *differs from database* | The database is not available, or the file belongs to a different intake. Check that the database is up and that the NBT numbers are for the current intake year |
| A name is red for *funny characters* | Use **Clean File**, or correct the name by hand. Names may not contain digits or punctuation |
| **Save to Database** stays greyed out | The list still has validation errors, or it has not been changed since it was loaded. Fix the red fields and try again |
| Saving reports success but the difference is still there | **Save to Database** only adds missing writers; it does not change the details of writers already in WriterList. Correct the field with the right-click **Keep file value (update WriterList)** entry |
| A South African ID disappeared after **Clean File** | That is what Clean File does with an ID that fails validation: it moves it to the Foreign ID column when that is empty, otherwise it clears it. Type the correct ID back in |
| **Keep file value** reports "No matching WriterList record was found" | The row has no WriterList match, so there is nothing to update. Check the NBT Reference, or add the writer with **Save to Database** first |
| The DB Duplicates list keeps coming back | The NBT number really is in use by another writer. Correct the number in the grid, or use **New NBT Number**, then check again |
| **New NBT Number** says no numbers are available | Every row of the new number pool has been used. A new batch of numbers has to be loaded before more can be issued |
| The database writes fail with a "Database validation failed" message | The value is not acceptable to the database (for example a number that is too long). Correct it and try the menu entry again |

---

## 13. Glossary

| Term | Meaning |
|---|---|
| **WriterList** | The database table holding the registered writers for the intake, one row per writer |
| **Composit** | The database table holding the scored composite record for a barcode/reference |
| **NBT Reference** | The 14 digit NBT number that identifies a writer's test |
| **Writer list file** | The Unicode CSV exported by the web registration system, loaded by this screen |
| **Duplicates** | Two or more rows in the loaded file sharing the same NBT Reference |
| **DB Duplicates** | A loaded row whose NBT Reference belongs to a different writer in WriterList for the intake year |
| **Clean File** | The tidy-up pass that strips punctuation and accents from names |
| **New NBT Number** | A replacement number taken from the pool of unused numbers, for a writer whose number is duplicated |
