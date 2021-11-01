using Jawabkom_Generator3.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3
{
    public class ExcelFactory
    {
        public static void ExportExcelTable(IList<IList<Object>> values, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();
            package.Workbook.Worksheets.Add("Worksheets1");

            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            //IList<IList<Object>> values = response.Values;
            int currentRow = 1;
            foreach (var row in values)
            {
                for (var i = 0; i < row.Count; i++)
                {
                    worksheet.Cells[currentRow, i + 1].Value = row[i];
                }
                currentRow++;
                continue;
            }
            worksheet.Protection.IsProtected = false;
            worksheet.Protection.AllowSelectLockedCells = false;
            string filePath = fileName + ".xlsx";
            var exportPath = Path.Combine(@"C:\temp\", filePath);
            package.SaveAs(new FileInfo(exportPath));
        }

        public static List<OperatorPayout> ImportExcelTable(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo newfile = new FileInfo(filePath);
            List<OperatorPayout> list = new List<OperatorPayout>();

            using (var package = new ExcelPackage(newfile))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                int rowCount = workSheet.Dimension.Rows;


                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var operatorName = workSheet.Cells[row, 1].Value?.ToString();
                        var payoutPercentage = workSheet.Cells[row, 2].Value?.ToString();

                        OperatorPayout oppayout = new OperatorPayout()
                        {

                            operator_name = operatorName,
                            payout_percentage = payoutPercentage
                        };
                        list.Add(oppayout);
                    }

                    catch (Exception)
                    {

                        throw;
                    }
                }

            }
            return list;
        }

    }



}
