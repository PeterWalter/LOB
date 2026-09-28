using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CETAP_LOB.BDO;
using CETAP_LOB.Database;
using CETAP_LOB.Model.venueprep;

namespace CETAP_LOB.View.writers
{
    /// <summary>
    /// Interaction logic for LoadFileView.xaml
    /// </summary>
    public partial class LoadFileView : UserControl
    {
        private readonly List<object> _dynamicMenuItems = new List<object>();
        private MenuItem _writerValueMenuItem;
        private MenuItem _writerRecordMenuItem;
        private MenuItem _compositValueMenuItem;
        private MenuItem _writerKeepFileMenuItem;
        private MenuItem _compositKeepFileMenuItem;
        private MenuItem _compositRecordMenuItem;
        private MenuItem _columnValuesMenuItem;
        private Separator _dynamicMenuSeparator;
        private WebWriters _menuRecord;

        public LoadFileView()
        {
            InitializeComponent();
        }

        private void LoadGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid == null || grid.ContextMenu == null)
                return;

            RemoveDynamicMenuItems(grid);

            DataGridCell cell = FindAncestor<DataGridCell>(e.OriginalSource as DependencyObject);
            if (cell == null)
            {
                DependencyObject hit = grid.InputHitTest(Mouse.GetPosition(grid)) as DependencyObject;
                cell = FindAncestor<DataGridCell>(hit);
            }

            WebWriters record = cell == null ? null : cell.DataContext as WebWriters;
            if (record == null)
                record = grid.CurrentItem as WebWriters;
            if (record == null)
                record = grid.SelectedItem as WebWriters;
            if (record == null)
                return;

            try
            {
                AttachDatabaseSnapshotsIfMissing(record);
            }
            catch
            {
                // If DB is unavailable or a lookup fails, still show the context
                // menu and any already attached values instead of dropping back to
                // only the static items.
            }
            EnsureMenuItems();

            string field = FieldForColumn(cell == null || cell.Column == null ? null : cell.Column.Header as string);
            if (field == null && grid.CurrentCell.Column != null)
                field = FieldForColumn(grid.CurrentCell.Column.Header as string);
            _menuRecord = record;

            List<object> infoEntries = new List<object>();
            List<object> actionEntries = new List<object>();
            infoEntries.Add(BuildColumnValuesMenu(record, field));

            if (record.HasWriterRecord)
            {
                if (field != null)
                {
                    SetActionHeader(_writerValueMenuItem, "Use WriterList value: " + DisplayValue(record.GetWriterValue(field)));
                    _writerValueMenuItem.Tag = field;
                    _writerValueMenuItem.ToolTip = "Copy the value recorded in WriterList for this column";
                    bool writerDiff = record.IsWriterValueDifferent(field);
                    _writerValueMenuItem.IsEnabled = writerDiff && record.CanApplyWriterValue(field);
                    if (writerDiff)
                    {
                        actionEntries.Add(CreateSectionHeaderMenuItem("WriterList value"));
                        actionEntries.Add(_writerValueMenuItem);
                    }
                }

                _writerRecordMenuItem.Header = BuildRecordHeader(record.GetWriterRecordLines());
                _writerRecordMenuItem.ToolTip = "The matching WriterList record";
                infoEntries.Add(_writerRecordMenuItem);
            }

            if (record.HasCompositRecord)
            {
                if (field != null)
                {
                    SetActionHeader(_compositValueMenuItem, "Use Composit value: " + DisplayValue(record.GetCompositValue(field)));
                    _compositValueMenuItem.Tag = field;
                    _compositValueMenuItem.ToolTip = "Copy the value recorded in Composit for this column";
                    bool compositDiff = record.IsCompositValueDifferent(field);
                    _compositValueMenuItem.IsEnabled = compositDiff && record.CanApplyCompositValue(field);
                    if (compositDiff)
                    {
                        actionEntries.Add(CreateSectionHeaderMenuItem("Composit value"));
                        actionEntries.Add(_compositValueMenuItem);
                        SetActionHeader(_compositKeepFileMenuItem, "Keep file value (update Composit)");
                        _compositKeepFileMenuItem.Tag = field;
                        _compositKeepFileMenuItem.ToolTip = "Keep the current file value and write it into Composit for this column";
                        actionEntries.Add(_compositKeepFileMenuItem);
                    }
                }

                _compositRecordMenuItem.Header = BuildRecordHeader(record.GetCompositRecordLines());
                _compositRecordMenuItem.ToolTip = "The matching Composit record";
                infoEntries.Add(_compositRecordMenuItem);
            }

            if (infoEntries.Count == 0 && actionEntries.Count == 0)
            {
                _menuRecord = null;
                return;
            }

            List<object> entries = new List<object>();
            entries.AddRange(infoEntries);
            if (actionEntries.Count > 0)
            {
                entries.Add(_dynamicMenuSeparator);
                entries.AddRange(actionEntries);
            }

            for (int i = entries.Count - 1; i >= 0; i--)
                grid.ContextMenu.Items.Insert(0, entries[i]);
            _dynamicMenuItems.AddRange(entries);
        }

        private MenuItem BuildColumnValuesMenu(WebWriters record, string field)
        {
            _columnValuesMenuItem.Items.Clear();
            _columnValuesMenuItem.Header = field == null
                ? "Column values (right-click inside a data cell)"
                : "Column values for " + ColumnHeader(field);

            if (field == null)
            {
                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem("File: <select a data column>"));
                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem("WriterList: <select a data column>"));
                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem("Composit: <select a data column>"));
            }
            else
            {
                string fileValue = DisplayValue(record.GetCurrentValue(field));
                bool writerDiff = record.IsWriterValueDifferent(field);
                bool compositDiff = record.IsCompositValueDifferent(field);
                string writerLabel = writerDiff
                    ? "WriterList (DIFFERENT): " + DisplayValue(record.GetWriterValue(field))
                    : "WriterList: same as file";
                string compositLabel = compositDiff
                    ? "Composit (DIFFERENT): " + DisplayValue(record.GetCompositValue(field))
                    : "Composit: same as file";

                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem("File: " + fileValue));
                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem(writerLabel));
                _columnValuesMenuItem.Items.Add(CreateInfoMenuItem(compositLabel));
            }

            return _columnValuesMenuItem;
        }

        private void EnsureMenuItems()
        {
            if (_writerValueMenuItem != null)
                return;

            _writerValueMenuItem = new MenuItem();
            StyleActionMenuItem(_writerValueMenuItem, Colors.LightSteelBlue);
            _writerValueMenuItem.Click += UseWriterListValue_Click;
            _writerKeepFileMenuItem = new MenuItem();
            StyleActionMenuItem(_writerKeepFileMenuItem, Colors.Honeydew);
            _writerKeepFileMenuItem.Click += KeepFileValueForWriter_Click;

            _writerRecordMenuItem = CreateDisplayItem();

            _compositValueMenuItem = new MenuItem();
            StyleActionMenuItem(_compositValueMenuItem, Colors.LightSteelBlue);
            _compositValueMenuItem.Click += UseCompositValue_Click;
            _compositKeepFileMenuItem = new MenuItem();
            StyleActionMenuItem(_compositKeepFileMenuItem, Colors.Honeydew);
            _compositKeepFileMenuItem.Click += KeepFileValueForComposit_Click;

            _compositRecordMenuItem = CreateDisplayItem();
            _columnValuesMenuItem = new MenuItem();
            _columnValuesMenuItem.StaysOpenOnClick = true;

            _dynamicMenuSeparator = new Separator();
        }

        private static MenuItem CreateDisplayItem()
        {
            MenuItem item = new MenuItem();
            item.IsHitTestVisible = false;
            item.Focusable = false;
            return item;
        }

        private static MenuItem CreateInfoMenuItem(string text)
        {
            MenuItem item = new MenuItem();
            item.Header = text;
            item.IsHitTestVisible = false;
            item.Focusable = false;
            return item;
        }

        private static MenuItem CreateSectionHeaderMenuItem(string text)
        {
            MenuItem item = new MenuItem();
            item.IsHitTestVisible = false;
            item.Focusable = false;
            item.Header = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.DodgerBlue),
                Margin = new Thickness(0, 2, 0, 0)
            };
            return item;
        }

        private static void StyleActionMenuItem(MenuItem item, Color background)
        {
            item.FontWeight = FontWeights.Bold;
            item.Padding = new Thickness(8, 4, 8, 4);
            item.Margin = new Thickness(1, 2, 1, 2);
            item.Background = new SolidColorBrush(background);
            item.Foreground = new SolidColorBrush(Colors.DodgerBlue);
        }

        private static void SetActionHeader(MenuItem item, string text)
        {
            item.Header = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.DodgerBlue)
            };
        }

        private static TextBlock BuildRecordHeader(List<string> lines)
        {
            return new TextBlock { Text = string.Join(Environment.NewLine, lines) };
        }

        private void RemoveDynamicMenuItems(DataGrid grid)
        {
            foreach (object item in _dynamicMenuItems)
                grid.ContextMenu.Items.Remove(item);
            _dynamicMenuItems.Clear();
            _menuRecord = null;
        }

        private void UseWriterListValue_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _menuRecord == null)
                return;
            _menuRecord.ApplyWriterValue(item.Tag as string);
        }

        private void UseCompositValue_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _menuRecord == null)
                return;
            _menuRecord.ApplyCompositValue(item.Tag as string);
        }

        private void KeepFileValueForWriter_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _menuRecord == null)
                return;
            _menuRecord.AcceptFileValueForWriter(item.Tag as string);
        }

        private void KeepFileValueForComposit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _menuRecord == null)
                return;
            _menuRecord.AcceptFileValueForComposit(item.Tag as string);
        }

        private static string FieldForColumn(string header)
        {
            switch (header)
            {
                case "NBT Reference":
                    return "Reference";
                case "Surname":
                    return "Surname";
                case "First Name":
                    return "Name";
                case "South African ID":
                    return "SAID";
                case "Foreign ID":
                    return "ForeignID";
                case "Date of Birth":
                    return "DOB";
                case "Gender":
                    return "Gender";
                default:
                    return null;
            }
        }

        private static string ColumnHeader(string field)
        {
            switch (field)
            {
                case "Reference":
                    return "NBT Reference";
                case "Surname":
                    return "Surname";
                case "Name":
                    return "First Name";
                case "SAID":
                    return "South African ID";
                case "ForeignID":
                    return "Foreign ID";
                case "DOB":
                    return "Date of Birth";
                case "Gender":
                    return "Gender";
                default:
                    return field ?? "";
            }
        }

        private static string DisplayValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "<no value>" : value.Trim();
        }

        private void LoadGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid == null)
                return;

            DependencyObject source = e.OriginalSource as DependencyObject;
            DataGridCell cell = FindAncestor<DataGridCell>(source);
            if (cell != null && !cell.IsEditing)
            {
                if (!cell.IsFocused)
                    cell.Focus();
                DataGridRow row = FindAncestor<DataGridRow>(cell);
                if (row != null)
                    row.IsSelected = true;
                grid.CurrentCell = new DataGridCellInfo(cell);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private static void AttachDatabaseSnapshotsIfMissing(WebWriters record)
        {
            if (record == null || (record.HasWriterRecord && record.HasCompositRecord))
                return;

            long parsed;
            long? reference = long.TryParse((record.Reference ?? "").Trim(), out parsed) ? parsed : (long?)null;
            long? said = long.TryParse((record.SAID ?? "").Trim(), out parsed) ? parsed : (long?)null;
            string foreignId = (record.ForeignID ?? "").Trim();

            using (var context = new CETAPEntities())
            {
                if (!record.HasWriterRecord)
                {
                    WriterList writer = null;
                    if (reference.HasValue)
                        writer = context.WriterLists.FirstOrDefault(x => x.NBT == reference.Value);
                    if (writer == null && said.HasValue)
                        writer = context.WriterLists.FirstOrDefault(x => x.SAID.HasValue && x.SAID.Value == said.Value);
                    if (writer == null && foreignId.Length > 0)
                        writer = context.WriterLists.FirstOrDefault(x => x.ForeignID == foreignId);

                    if (writer != null)
                    {
                        record.AttachWriterRecord(new WritersBDO
                        {
                            Id = writer.Id,
                            NBT = writer.NBT,
                            Surname = writer.Surname,
                            Name = writer.Name,
                            Initials = writer.Initials,
                            SAID = writer.SAID,
                            ForeignID = writer.ForeignID,
                            Gender = writer.Gender,
                            DOB = writer.DOB,
                            Classification = writer.Classification,
                            TestLanguage = writer.TestLanguage,
                            TestType = writer.TestType,
                            VenueID = writer.VenueID,
                            DOT = writer.DOT,
                            Mobile = writer.Mobile,
                            HomeTelephone = writer.HomeTelephone,
                            EMail = writer.EMail,
                            Paid = writer.Paid,
                            AccountCreation = writer.AccountCreation,
                            RegistrationDate = writer.RegistrationDate,
                            Wrote = writer.Wrote,
                            RowGuid = writer.RowGuid,
                            DateModified = writer.DateModified,
                            RowVersion = writer.RowVersion
                        });
                    }
                }

                if (!record.HasCompositRecord)
                {
                    Composit composit = null;
                    if (reference.HasValue)
                        composit = context.Composits.FirstOrDefault(x => x.RefNo == reference.Value);
                    if (composit == null && said.HasValue)
                        composit = context.Composits.FirstOrDefault(x => x.SAID.HasValue && x.SAID.Value == said.Value);
                    if (composit == null && foreignId.Length > 0)
                        composit = context.Composits.FirstOrDefault(x => x.ForeignID == foreignId);

                    if (composit != null)
                    {
                        record.AttachCompositRecord(new CompositBDO
                        {
                            RefNo = composit.RefNo,
                            Barcode = composit.Barcode,
                            Surname = composit.Surname,
                            Name = composit.Name,
                            Initials = composit.Initials,
                            SAID = composit.SAID,
                            ForeignID = composit.ForeignID,
                            DOB = composit.DOB,
                            ID_Type = composit.ID_Type,
                            Citizenship = composit.Citizenship,
                            Classification = composit.Classification,
                            Gender = composit.Gender,
                            Faculty = composit.Faculty,
                            DOT = composit.DOT,
                            VenueCode = composit.VenueCode,
                            VenueName = composit.VenueName,
                            GR12Language = composit.GR12Language,
                            AQLLanguage = composit.AQLLanguage,
                            AQLCode = composit.AQLCode,
                            MatLanguage = composit.MatLanguage,
                            MatCode = composit.MatCode,
                            ALScore = composit.ALScore,
                            ALLevel = composit.ALLevel,
                            QLScore = composit.QLScore,
                            QLLevel = composit.QLLevel,
                            MATScore = composit.MATScore,
                            MATLevel = composit.MATLevel,
                            WroteAL = composit.WroteAL,
                            WroteQL = composit.WroteQL,
                            WroteMat = composit.WroteMat,
                            Faculty2 = composit.Faculty2,
                            Faculty3 = composit.Faculty3,
                            Batch = composit.Batch,
                            RowGuid = composit.RowGuid,
                            RowVersion = composit.RowVersion,
                            DateModified = composit.DateModified,
                            ProvinceId = composit.ProvinceID
                        });
                    }
                }
            }
        }
    }
}
