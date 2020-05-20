using Abp.Dependency;
using localtour;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace acm.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelImporterBase<TEntity> : localtourAppServiceBase, ITransientDependency
    {

        protected static ExcelWorksheet _worksheet;

        public static List<TEntity> ProcessWorksheets(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                        var entitiesInWorksheet = GetData(worksheet, processExcelRow);

                        entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }

        public static List<TEntity> ProcessWorksheet(byte[] fileBytes, string name, Func<ExcelWorksheet, int, TEntity> processExcelRow, int startingIndex = 0)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Where(e => e.Name == name).Single();
                    var entitiesInWorksheet = GetData(sheet, processExcelRow, startingIndex);
                    entities.AddRange(entitiesInWorksheet);
                }
            }

            return entities;
        }

        private static List<TEntity> GetData(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processExcelRow, int startingIndex = 0)
        {
            var entities = new List<TEntity>();

            for (var i = worksheet.Dimension.Start.Row + startingIndex + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return entities;
        }

        protected static decimal GetDecimalValue(int row, string column)
        {

            string cellValue = GetValue(row, column);

            if (cellValue == null)
            {
                return 0;
            }

            decimal value = 0;

            decimal.TryParse(cellValue.ToString(), out value);

            return value;
        }

        protected static int GetIntValue(int row, string column)
        {
            string cellValue = GetValue(row, column);
            if (cellValue == null)
            {
                return 0;
            }

            int value = 0;

            int.TryParse(cellValue.ToString(), out value);

            return value;
        }

        protected static DateTime GetDateTimeValue(int row, string column)
        {
            string cellValue = GetValue(row, column);
            return DateTime.Parse(cellValue);
        }

        protected static string GetValue(int row, string column)
        {
            int col = _worksheet.Cells["1:1"].First(c => c.Value.ToString() == column).Start.Column;
            return GetValue(row, col);
        }

        protected static string GetValue(int row, int column)
        {
            var cellValue = _worksheet.Cells[row, column].Value;

            if (cellValue == null)
            {
                return null;
            }

            return cellValue.ToString();
        }

        protected static bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
