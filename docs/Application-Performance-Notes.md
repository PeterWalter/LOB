# Application performance notes

Measured on the production database (\PRDSQLCLUSTER2\PRD002, NBT_Production) from the
release build. The figures are **UI thread blocking time**: the window cannot repaint, so
clicking a left hand menu group appears to do nothing until the work finishes.

## What was wrong

1. **A left hand menu group also navigates.** ModernUI's `ModernMenu` selects the group's
   first link as soon as the group header is clicked, so the target page is created and its
   view model constructed while the click is still being handled. Any database work in that
   view model constructor froze the whole window, menu included.

2. **A query per row (N+1).** `DataService.GetAllTestAllocations()` resolved the test name by
   calling `getTestName(TestID)` inside the mapping loop, so a 2,944 row table produced 2,944
   round trips to the cluster. Everything built on it inherited the cost.

3. **Whole tables read on the UI thread.** `dbo.Logs` (140,534 rows), the batch list (35,881),
   the tracker list (37,448) and the Easy Pay FTP folder listing were read while the page was
   being constructed.

## Before and after

Service calls:

| Call | Before | After |
|---|---|---|
| `GetAllTestAllocations()` | 16,915 ms | **124 ms** |
| `GetProfileAllocationsByDate(any date)` | 13,642 ms | **87 ms** |
| `GetAllProfileAllocations()` | 9,536 ms | **90 ms** |
| `GetAllErrors()` (140,534 rows) | 22,900 ms | ~19,000 ms (still one big read - now in the background) |

View model construction (what the menu click waits for):

| Menu item | View model | Before | After |
|---|---|---|---|
| Data preparation → The Tests | `TestsViewModel` | 52,350 ms | **1,499 ms** |
| Data Processing → Batching | `BatchViewModel` | 24,123 ms | **155 ms** |
| Easy Pay → FTP files | `EasyPayViewModel` | 21,150 ms | **18 ms** |
| Data Processing → Error Logs | `ErrorViewModel` | 18,974 ms | **1 ms** |
| Data preparation → Writer List | `ProcessViewModel` | 9,242 ms | **581 ms** |
| Data Processing → Tracker | `ScanTrackerViewModel` | 2,005 ms | **2 ms** |
| Composite → Remotes | `RemotesViewModel` | 1,025 ms | **1 ms** |

## What changed

- `DataService.GetAllTestAllocations()` reads the test names once into a dictionary and passes
  it to `TranslateTestAllocationDALToTestAllocationBDO`. The translator keeps its old two
  argument behaviour (a single lookup) for the callers that only read one row.
- `AsNoTracking()` on the large read-only queries (logs, trackers, batches): they are never
  updated through the context that read them.
- View models with slow constructors now build their collections empty, register their
  commands, then load in the background and assign the collections when the data arrives:

  | View model | Loaded in the background |
  |---|---|
  | `ErrorViewModel` | `GetAllErrors()` |
  | `EasyPayViewModel` | `ReadLastFile()` + `ListFTPFiles()` |
  | `BatchViewModel` | `GetAllUsers()`, `GetAllIntakeYears()`, `GetAllbatches()` |
  | `ScanTrackerViewModel` | `GetAllTracks()`, `GetAllIntakeYears()`, `GetAllvenues()` |
  | `RemotesViewModel` | `GetIntakeRecord()`, `GetAllRemoteScoresByIntakeYear()` |
  | `QAViewModel` | the whole QA folder scan |

  Each of them exposes `IsLoading` (or reuses `InProgress`) and `ErrorView.xaml` shows a ring
  while the log is being read.

## Known remaining items

- `TestsViewModel` still blocks for about 1.5 s (its `RefreshTests`, `GetProfiles`,
  `Refresh_alloc` and `Refresh_ProfAllocs` helpers run synchronously and are shared with the
  save/update commands) and `ProcessViewModel` for about 0.6 s.
- `GetAllErrors()` still reads all 140,534 rows. The page is now responsive while it does so,
  but the grid is empty for roughly twenty seconds. Reading only the most recent rows, or
  paging, would fix that - it needs a decision about how much history the page must show.
- `ModerationViewModel` throws `CsvMissingFieldException: Fields 'ProvinceId' do not exist in
  the CSV file` while it is being constructed.
- The menu also links to `BioQAView.xaml`, `SubdomainsView.xaml` and `BarcodesView.xaml`;
  none of those three files exist in the project.
