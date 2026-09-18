using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CETAP_LOB.Model.QA;
using CETAP_LOB.ViewModel.processing;

namespace CETAP_LOB.View.processing
{
    /// <summary>
    /// Interaction logic for QAView.xaml
    /// </summary>
    public partial class QAView : UserControl
    {
        private readonly List<object> _dynamicMenuItems = new List<object>();
        private MenuItem _writerValueMenuItem;
        private MenuItem _writerRecordMenuItem;
        private MenuItem _compositValueMenuItem;
        private MenuItem _compositRecordMenuItem;
        private MenuItem _allocateWalkInMenuItem;
        private Separator _dynamicMenuSeparator;
        private QADatRecord _menuRecord;

        public QAView()
        {
            InitializeComponent();
        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Adds the WriterList and Composit comparison entries to the top of the
        /// grid's existing context menu for the record that was right clicked.
        /// </summary>
        private void QAGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid == null || grid.ContextMenu == null)
                return;

            RemoveDynamicMenuItems(grid);

            DataGridCell cell = FindAncestor<DataGridCell>(e.OriginalSource as DependencyObject);
            QADatRecord record = cell == null ? null : cell.DataContext as QADatRecord;
            if (record == null)
                record = grid.SelectedItem as QADatRecord;
            if (record == null)
                return;

            EnsureMenuItems();

            string field = FieldForColumn(cell == null || cell.Column == null ? null : cell.Column.Header as string);
            _menuRecord = record;

            List<object> entries = new List<object>();

            if (record.HasWriterRecord)
            {
                if (field != null)
                {
                    _writerValueMenuItem.Header = "Use WriterList value: " + WriterValueFor(record, field);
                    _writerValueMenuItem.Tag = field;
                    _writerValueMenuItem.ToolTip = "Copy the value recorded in the WriterList for this column";
                    _writerValueMenuItem.IsEnabled = record.CanApplyWriterValue(field);
                    entries.Add(_writerValueMenuItem);
                }

                _writerRecordMenuItem.Header = BuildRecordHeader(record.GetWriterRecordLines());
                _writerRecordMenuItem.ToolTip = "The matching WriterList record";
                entries.Add(_writerRecordMenuItem);
            }

            if (record.HasCompositRecord)
            {
                _compositValueMenuItem.Header = "Use Composit reference: " + record.CompositReference;
                _compositValueMenuItem.ToolTip = "Change the walk-in reference to the reference held in Composit";
                _compositValueMenuItem.IsEnabled = record.CanApplyCompositReference;
                entries.Add(_compositValueMenuItem);

                _compositRecordMenuItem.Header = BuildRecordHeader(record.GetCompositRecordLines());
                _compositRecordMenuItem.ToolTip = "The matching Composit record";
                entries.Add(_compositRecordMenuItem);
            }

            if (CanAllocateWalkInReference(record))
            {
                _allocateWalkInMenuItem.ToolTip = "Allocate an unused walk-in reference and record the replaced reference";
                entries.Add(_allocateWalkInMenuItem);
            }

            if (entries.Count == 0)
            {
                _menuRecord = null;
                return;
            }

            entries.Add(_dynamicMenuSeparator);
            for (int i = entries.Count - 1; i >= 0; i--)
                grid.ContextMenu.Items.Insert(0, entries[i]);
            _dynamicMenuItems.AddRange(entries);
        }

        private void EnsureMenuItems()
        {
            if (_writerValueMenuItem != null)
                return;

            _writerValueMenuItem = new MenuItem();
            _writerValueMenuItem.Click += UseWriterListValue_Click;
            _writerRecordMenuItem = CreateDisplayItem();

            _compositValueMenuItem = new MenuItem();
            _compositValueMenuItem.Click += UseCompositReference_Click;
            _compositRecordMenuItem = CreateDisplayItem();

            _allocateWalkInMenuItem = new MenuItem();
            _allocateWalkInMenuItem.Header = "Allocate new walk-in reference";
            _allocateWalkInMenuItem.Click += AllocateWalkInReference_Click;

            _dynamicMenuSeparator = new Separator();
        }

        /// <summary>A read only menu item used to show a record block.</summary>
        private static MenuItem CreateDisplayItem()
        {
            MenuItem item = new MenuItem();
            item.IsHitTestVisible = false;
            item.Focusable = false;
            return item;
        }

        private static TextBlock BuildRecordHeader(List<string> lines)
        {
            return new TextBlock { Text = string.Join(Environment.NewLine, lines) };
        }

        /// <summary>
        /// A new walk-in reference is only offered for a proper reference whose
        /// name, surname, SA ID or foreign ID differs from the WriterList.
        /// </summary>
        private static bool CanAllocateWalkInReference(QADatRecord record)
        {
            return record.HasWriterRecord
                && !QADatRecord.IsWalkInReference(record.Reference)
                && (record.NameMismatch || record.SurnameMismatch || record.SAIDMismatch || record.ForeignIDMismatch);
        }

        private void RemoveDynamicMenuItems(DataGrid grid)
        {
            foreach (object item in _dynamicMenuItems)
                grid.ContextMenu.Items.Remove(item);
            _dynamicMenuItems.Clear();
            _menuRecord = null;
        }

        /// <summary>
        /// Corrects the field that was right clicked using the value from the
        /// matching WriterList record. The MenuItem Tag identifies the field.
        /// </summary>
        private void UseWriterListValue_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _menuRecord == null)
                return;
            _menuRecord.ApplyWriterValue(item.Tag as string);
        }

        /// <summary>Replaces the walk-in reference with the reference held in Composit.</summary>
        private void UseCompositReference_Click(object sender, RoutedEventArgs e)
        {
            if (_menuRecord == null)
                return;
            _menuRecord.ApplyCompositReference();
        }

        /// <summary>Allocates an unused walk-in reference for the right clicked record.</summary>
        private void AllocateWalkInReference_Click(object sender, RoutedEventArgs e)
        {
            if (_menuRecord == null)
                return;
            QAViewModel viewModel = QAGrid == null ? null : QAGrid.DataContext as QAViewModel;
            if (viewModel == null)
                return;
            viewModel.AllocateWalkInReference(_menuRecord);
        }

        /// <summary>Maps the grid column header to the field name used by ApplyWriterValue.</summary>
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

        private static string WriterValueFor(QADatRecord record, string field)
        {
            switch (field)
            {
                case "Reference":
                    return record.WriterReference;
                case "Surname":
                    return record.WriterSurname;
                case "Name":
                    return record.WriterName;
                case "SAID":
                    return record.WriterSAID;
                case "ForeignID":
                    return record.WriterForeignID;
                case "DOB":
                    return record.WriterDOB;
                case "Gender":
                    return record.WriterGender;
                default:
                    return "";
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                T match = current as T;
                if (match != null)
                    return match;
                if (current is Visual || current is System.Windows.Media.Media3D.Visual3D)
                    current = VisualTreeHelper.GetParent(current);
                else
                    current = LogicalTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}