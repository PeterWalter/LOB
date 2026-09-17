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

namespace CETAP_LOB.View.processing
{
    /// <summary>
    /// Interaction logic for QAView.xaml
    /// </summary>
    public partial class QAView : UserControl
    {
        private MenuItem _writerValueMenuItem;
        private Separator _writerValueSeparator;
        private QADatRecord _writerValueRecord;

        public QAView()
        {
            InitializeComponent();
        }

        private void ModernButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Adds the WriterList value for the right clicked column to the grid's
        /// existing context menu. The item is only added for the biography
        /// columns when the record has a matching WriterList entry.
        /// </summary>
        private void QAGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid == null || grid.ContextMenu == null)
                return;

            RemoveWriterValueItems(grid);

            DataGridCell cell = FindAncestor<DataGridCell>(e.OriginalSource as DependencyObject);
            QADatRecord record = cell == null ? null : cell.DataContext as QADatRecord;
            if (record == null)
                record = grid.SelectedItem as QADatRecord;

            string field = FieldForColumn(cell == null || cell.Column == null ? null : cell.Column.Header as string);
            if (record == null || field == null || !record.HasWriterRecord)
                return;

            if (_writerValueMenuItem == null)
            {
                _writerValueMenuItem = new MenuItem();
                _writerValueMenuItem.Click += UseWriterListValue_Click;
                _writerValueSeparator = new Separator();
            }

            _writerValueRecord = record;
            _writerValueMenuItem.Header = "Use WriterList value: " + WriterValueFor(record, field);
            _writerValueMenuItem.Tag = field;
            _writerValueMenuItem.ToolTip = "Copy the value recorded in the WriterList for this column";
            _writerValueMenuItem.IsEnabled = record.CanApplyWriterValue(field);

            grid.ContextMenu.Items.Insert(0, _writerValueSeparator);
            grid.ContextMenu.Items.Insert(0, _writerValueMenuItem);
        }

        private void RemoveWriterValueItems(DataGrid grid)
        {
            if (_writerValueMenuItem == null)
                return;
            grid.ContextMenu.Items.Remove(_writerValueMenuItem);
            grid.ContextMenu.Items.Remove(_writerValueSeparator);
            _writerValueRecord = null;
        }

        /// <summary>
        /// Corrects the field that was right clicked using the value from the
        /// matching WriterList record. The MenuItem Tag identifies the field.
        /// </summary>
        private void UseWriterListValue_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item == null || _writerValueRecord == null)
                return;
            _writerValueRecord.ApplyWriterValue(item.Tag as string);
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
                case "Date of Test":
                    return "DOT";
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
                case "DOT":
                    return record.WriterDOT;
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