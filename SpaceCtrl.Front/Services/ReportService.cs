using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SpaceCtrl.Front.Services
{
    public class ReportService
    {
        public void Generate()
        {
            var spreadsheetDocument =
                SpreadsheetDocument.Create("", SpreadsheetDocumentType.Workbook);
        }
    }
}