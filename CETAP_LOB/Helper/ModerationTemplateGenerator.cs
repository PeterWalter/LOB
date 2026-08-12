using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using CETAP_LOB.Model.scoring;

namespace CETAP_LOB.Helper
{
    // Main class for generating the moderation template PDF
    public class ModerationTemplateGenerator
    {
        public void GeneratePDF(string filePath, List<VenueData> tableData, string logoPathLeft, string logoPathRight, ModerationFormData formData)
        {
            // Initialize the PDF document
            PdfDocument document = new PdfDocument();
            try
            {
                // Define commonly used fonts and brush
                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfFont normalFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
                PdfBrush brush = PdfBrushes.Black;

                // Create the data grid for venue information
                PdfGrid grid = CreateVenueGrid(tableData, normalFont);

                // Enable pagination for long tables
                PdfLayoutFormat layoutFormat = new PdfLayoutFormat { Layout = PdfLayoutType.Paginate };

                PdfLayoutResult result;
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                float y = 0;

                // Draw left logo if available, otherwise draw placeholder
                if (File.Exists(logoPathLeft))
                {
                    PdfImage leftLogo = PdfImage.FromFile(logoPathLeft);
                    graphics.DrawImage(leftLogo, new RectangleF(0, y, 100, 50));
                }
                else
                {
                    graphics.DrawRectangle(PdfPens.Black, new RectangleF(0, y, 100, 50));
                    graphics.DrawString("Logo 1", normalFont, brush, new RectangleF(0, y, 100, 50), new PdfStringFormat { Alignment = PdfTextAlignment.Center, LineAlignment = PdfVerticalAlignment.Middle });
                }

                // Draw right logo if available, otherwise draw placeholder
                if (File.Exists(logoPathRight))
                {
                    PdfImage rightLogo = PdfImage.FromFile(logoPathRight);
                    graphics.DrawImage(rightLogo, new RectangleF(page.GetClientSize().Width - 100, y, 100, 50));
                }
                else
                {
                    graphics.DrawRectangle(PdfPens.Black, new RectangleF(page.GetClientSize().Width - 100, y, 100, 50));
                    graphics.DrawString("Logo 2", normalFont, brush, new RectangleF(page.GetClientSize().Width - 100, y, 100, 50), new PdfStringFormat { Alignment = PdfTextAlignment.Center, LineAlignment = PdfVerticalAlignment.Middle });
                }

                // Title
                y += 60;
                graphics.DrawString("NBT Moderation Audit", headerFont, brush, new PointF(page.GetClientSize().Width / 2 - 30, y));
                y += 30;

                float labelWidth = 90;
                float fieldWidth = 120;
                float spacing = 15;

                // Moderation Sheet Header (moved before venue grid)
                graphics.DrawString("Moderation sheet", headerFont, brush, new PointF(0, y));
                y += 30;

                PdfTextBoxField testDateField = new PdfTextBoxField(page, "TestDate")
                {
                    Bounds = new RectangleF(labelWidth, y - 2, fieldWidth, spacing),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(testDateField);
                testDateField.Text = formData.TestDate;
                graphics.DrawString("Test Date", normalFont, brush, new PointF(0, y));

                PdfTextBoxField modDateField = new PdfTextBoxField(page, "ModerationDate")
                {
                    Bounds = new RectangleF(340, y, fieldWidth, 15),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(modDateField);
                modDateField.Text = formData.ModerationDate;
                graphics.DrawString("Moderation Date", normalFont, brush, new PointF(220, y));
                y += 20;

                PdfTextBoxField testTypeField = new PdfTextBoxField(page, "TestType")
                {
                    Bounds = new RectangleF(labelWidth, y - 2, fieldWidth, 15),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(testTypeField);
                testTypeField.Text = formData.TestType;
                graphics.DrawString("Test Type", normalFont, brush, new PointF(0, y));
                y += 20;

                graphics.DrawString("Total Test Writers:", normalFont, brush, new PointF(0, y));
                PdfTextBoxField writerCountField = new PdfTextBoxField(page, "WriterCount")
                {
                    Bounds = new RectangleF(labelWidth, y - 2, fieldWidth, 15),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(writerCountField);
                writerCountField.Text = formData.TotalTestWriters;

                graphics.DrawString("Total Moderation Records:", normalFont, brush, new PointF(220, y));
                PdfTextBoxField moderationCountField = new PdfTextBoxField(page, "ModerationRecordCount")
                {
                    Bounds = new RectangleF(340, y - 2, fieldWidth, 15),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(moderationCountField);
                moderationCountField.Text = formData.TotalModerationRecords;
                y += 20;

                PdfTextBoxField checkedInField = new PdfTextBoxField(page, "CheckedInBy")
                {
                    Bounds = new RectangleF(160, y, 300, 15),
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(checkedInField);
                checkedInField.Text = formData.CheckedInBy;
                graphics.DrawString("Checked–in by Data Administrator", normalFont, brush, new PointF(0, y));
                y += 30;

                // Draw the venue grid with pagination
                result = grid.Draw(page, new PointF(0, y + 10), layoutFormat);
                float nextY = result.Bounds.Bottom + 20;
                page = result.Page;
                graphics = page.Graphics;

                PdfPen dashedPen = new PdfPen(Color.Gray, 0.5f);
                dashedPen.DashStyle = PdfDashStyle.Dash;
                graphics.DrawLine(dashedPen, new PointF(0, nextY), new PointF(500, nextY));
                nextY += 10;

                // If there's insufficient space, create a new page for the signature section
                if (nextY + 120 > page.GetClientSize().Height)
                {
                    page = document.Pages.Add();
                    graphics = page.Graphics;
                    nextY = 0;
                }
                // Draw Moderation Findings section
                graphics.DrawString("Moderation Findings", normalFont, brush, new PointF(0, nextY));
                PdfTextBoxField findingsBox = new PdfTextBoxField(page, "ModerationFindings")
                {
                    Bounds = new RectangleF(0, nextY + 15, 500, 80),
                    Multiline = true,
                    ToolTip = "Enter moderation findings here.",
                    Font = normalFont,
                    BorderColor = Color.Gray,
                    BackColor = Color.White
                };
                document.Form.Fields.Add(findingsBox);
                findingsBox.Text = formData.ModerationFindings;
                nextY += 100;

                //graphics.DrawString("Prepared by: ", normalFont, brush, new PointF(0, nextY));
                //graphics.DrawLine(PdfPens.Black, 70, nextY + 10, 250, nextY + 10);
                nextY += 30;

                //graphics.DrawString("Manager", normalFont, brush, new PointF(0, nextY));

                //graphics.DrawString("Date Checked", normalFont, brush, new PointF(220, nextY));
                //nextY += 30;
                //graphics.DrawString("Signature:", normalFont, brush, new PointF(0, nextY));
                //graphics.DrawLine(PdfPens.Black, 60, nextY + 10, 200, nextY + 10);
                //nextY += 40;
                // If there's insufficient space, create a new page for the signature section
                if (nextY + 120 > page.GetClientSize().Height)
                {
                    page = document.Pages.Add();
                    graphics = page.Graphics;
                    nextY = 0;
                }
                graphics.DrawString("Data Administrator", normalFont, brush, new PointF(0, nextY));
                graphics.DrawString("Date Completed", normalFont, brush, new PointF(340, nextY));
                nextY += 30;
                graphics.DrawString("Signature:", normalFont, brush, new PointF(0, nextY));
                graphics.DrawLine(PdfPens.Black, 60, nextY + 10, 200, nextY + 10);
                nextY += 40;

                graphics.DrawString("Manager", normalFont, brush, new PointF(0, nextY));
                graphics.DrawString("Date Checked", normalFont, brush, new PointF(340, nextY));
                nextY += 30;
                graphics.DrawString("Signature:", normalFont, brush, new PointF(0, nextY));
                graphics.DrawLine(PdfPens.Black, 60, nextY + 10, 200, nextY + 10);

                //PdfTextBoxField chkDateField = new PdfTextBoxField(page, "CheckedDate")
                //{
                //    Bounds = new RectangleF(340, y, fieldWidth, 15),
                //    Font = normalFont,
                //    BorderColor = Color.White,
                //    BackColor = Color.White
                //};
                //document.Form.Fields.Add(chkDateField);
                //chkDateField.Text = formData.CheckedDate;
                graphics.DrawString(formData.CheckedDate, normalFont, brush, new PointF(350, nextY - 10));

                AddPageNumbers(document);

                using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    document.Form.SetDefaultAppearance(false);
                    document.Form.Flatten = true;
                    document.Save(outputFileStream);
                }
            }
            finally
            {
                document.Close(true);
            }
        }

        private PdfGrid CreateVenueGrid(List<CETAP_LOB.Model.scoring.VenueData> tableData, PdfFont font)
        {
            PdfGrid grid = new PdfGrid();
            grid.Style.Font = font;
            grid.Columns.Add(5);
            grid.Headers.Add(1);

            PdfGridRow header = grid.Headers[0];
            header.Cells[0].Value = "Venue Code";
            header.Cells[1].Value = "Venue Name";
            header.Cells[2].Value = "Full Count";
            header.Cells[3].Value = "Moderation Count";
            header.Cells[4].Value = "Percentage";

            foreach (PdfGridCell cell in header.Cells)
                cell.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);

            foreach (var data in tableData)
            {
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = data.VenueCode;
                row.Cells[1].Value = data.VenueName;
                row.Cells[2].Value = data.FullCount.ToString();
                row.Cells[3].Value = data.ModerationCount.ToString();
                row.Cells[4].Value = data.Percentage;

                foreach (PdfGridCell cell in row.Cells)
                    cell.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            }

            return grid;
        }

        private void AddPageNumbers(PdfDocument document)
        {
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
            for (int i = 0; i < document.Pages.Count; i++)
            {
                PdfPage page = document.Pages[i];
                PdfGraphics graphics = page.Graphics;
                string pageNumber = $"Page {i + 1} of {document.Pages.Count}";
                SizeF textSize = font.MeasureString(pageNumber);
                float x = page.GetClientSize().Width - textSize.Width - 10;
                float y = page.GetClientSize().Height - 20;
                graphics.DrawString(pageNumber, font, PdfBrushes.Gray, new PointF(x, y));
            }
        }
    }

    public class ModerationFormData
    {
        private DateTime _moderationDate = DateTime.Today;
        public string TestDate { get; set; }
        public string ModerationDate { get; set; }
        public string TestType { get; set; }
        public string TotalTestWriters { get; set; }
        public string TotalModerationRecords { get; set; }
        public string CheckedInBy { get; set; }
        public string ModerationFindings { get; set; }

        public string CheckedDate { get; set; }
    }

    //public class VenueData
    //{
    //    public string VenueCode { get; set; }
    //    public string VenueName { get; set; }
    //    public int FullCount { get; set; }
    //    public int ModerationCount { get; set; }
    //    public double Percentage { get; set; }
    //}



}
