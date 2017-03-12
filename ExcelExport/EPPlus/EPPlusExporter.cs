using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using ExcelTest;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using DataTable = System.Data.DataTable;

class EPPlusExporter 
{
    /// <summary>
    /// 将DataTable导出到Excel，DataTable的名称作为sheet页名称，如果指定的Excel文件存在，则在已有Excel文件中增加sheet页，如果已存在同名的sheet页，则覆盖sheet页
    /// </summary>
    /// <param name="table">数据表</param>
    /// <param name="excelFilePath">Excel文件路径</param>
    /// <remarks>建议传入DataTable命名表名以生成指定的Sheet名</remarks>
    /// <exception cref="System.IO.IOException"> excel文件未关闭 </exception>   
    /// <exception cref="System.InvalidOperationException"> excel文件未关闭 </exception>
    /// <exception cref="System.ArgumentNullException"></exception>
    public void Export(DataTable table, string excelFilePath)
    {
        bool fileexists = false;
        if (table == null || excelFilePath == string.Empty)
        {
            throw new ArgumentNullException();
        }
        string sheetName = table.TableName;
        var newFile = new FileInfo(excelFilePath);
        if (newFile.Exists)
        {
            fileexists = true;
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorkbook workBook = package.Workbook;
            ExcelWorksheet worksheet = CreatWorkSheet(sheetName, workBook);
            if (!fileexists)
            {
                package.Workbook.Properties.Title = @"导出数据";
                package.Workbook.Properties.Author = "Microsoft";
                package.Workbook.Properties.Subject = @"导出信息";
                package.Workbook.Properties.Company = "CyberStone Inc.";
                // worksheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium6);//多次添加取消样式
                worksheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium6);
            }
            else
            {
                #region Overwrite Sheet
                worksheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium6);
                #endregion
                #region Append Sheet
                //int rowend = worksheet.Dimension.End.Row;
                //for (int rownumber = 0; rownumber < table.Rows.Count; rownumber++)
                //{
                //    DataRow dataRow = table.Rows[rownumber];
                //    for (int column = 0; column < table.Columns.Count; column++)
                //    {

                //        worksheet.Cells[rowend + 1 + rownumber, column + 1].Value = dataRow[column];
                //    }
                //} 
                #endregion

            }
            worksheet.Cells.AutoFitColumns();
            package.SaveAs(newFile);
        }
    }
    public void Exportstream(DataTable table, string excelFilePath)
    {
        bool fileexists = false;
        if (table == null || excelFilePath == string.Empty)
        {
            return;
        }
        string sheetName = EscapeSheetNameInvalidChar(table.TableName);
        if (File.Exists(excelFilePath))
        {
            fileexists = true;
            //  File.Delete(excelFilePath);
        }
        // var newFile = new FileInfo(excelFilePath);
        using (FileStream fs = new FileStream(excelFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (var package = new ExcelPackage(fs))
            {
                ExcelWorkbook workBook = package.Workbook;
                ExcelWorksheet worksheet = null;

                if (workBook.Worksheets.Count > 0)
                {
                    foreach (var sheet in workBook.Worksheets)
                    {
                        if (String.Compare(sheet.Name, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            workBook.Worksheets.Delete(sheet.Name);
                            worksheet = workBook.Worksheets.Add(sheetName);
                            break;
                        }

                        else
                            worksheet = workBook.Worksheets.Add(sheetName);
                    }
                }
                else
                {
                    worksheet = workBook.Worksheets.Add(sheetName);
                }
                if (!fileexists)
                {
                    package.Workbook.Properties.Title = @"导出数据";
                    package.Workbook.Properties.Author = "Microsoft";
                    package.Workbook.Properties.Subject = @"导出信息";
                    package.Workbook.Properties.Company = "CyberStone Inc.";
                    worksheet.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Medium6);
                }
                else
                {
                    worksheet.Cells[worksheet.Dimension.End.Row + 1, 1].LoadFromDataTable(table, false, TableStyles.Medium6);
                }
                worksheet.Cells.AutoFitColumns();
                package.SaveAs(fs);
            }
        }
    }
    public void Export(string[] columnNames, IList<string[]> rows, string excelFilePath, string sheetName)
    {
        bool fileexists = false;
        if (columnNames == null || excelFilePath == string.Empty || rows.Count == null)
        {
            throw new ArgumentNullException();
        }
        var newFile = new FileInfo(excelFilePath);
        if (newFile.Exists)
        {
            fileexists = true;
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            ExcelWorkbook workBook = package.Workbook;
            ExcelWorksheet worksheet = CreatWorkSheet(sheetName, workBook);
            if (!fileexists)
            {
                package.Workbook.Properties.Title = @"导出数据";
                package.Workbook.Properties.Author = "Microsoft";
                package.Workbook.Properties.Subject = @"导出信息";
                package.Workbook.Properties.Company = "CyberStone Inc.";
                RowsToSheet(columnNames, rows, worksheet);
            }
            else
            {
                RowsToSheet(columnNames, rows, worksheet);
            }
            worksheet.Cells.AutoFitColumns();
            package.SaveAs(newFile);
        }
    }

    private ExcelWorksheet CreatWorkSheet(string sheetName, ExcelWorkbook workBook)
    {
        ExcelWorksheet worksheet = null;

        if (workBook.Worksheets.Count > 0)
        {
            foreach (var sheet in workBook.Worksheets)
            {
                if (String.Compare(sheet.Name, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    workBook.Worksheets.Delete(sheet.Name);
                    goto newasheet;
                }
            }
        newasheet:
            worksheet = workBook.Worksheets.Add(sheetName);
        }
        else
        {
            worksheet = workBook.Worksheets.Add(sheetName);
        }
        return worksheet;
    }

    public IExportSession Export(string excelFilePath, string sheetName, string[] columnNames)
    {
        if (excelFilePath == "" || sheetName == "" || columnNames == null)
        {
            throw new ArgumentException();
        }
        var nopisession = new EPPlusSession(excelFilePath, sheetName, columnNames);
        return nopisession;
    }
    public IExportSession Export(string excelFilePath, string sheetName, DataColumn[] columnNames)
    {
        if (excelFilePath == "" || sheetName == "" || columnNames == null)
        {
            throw new ArgumentException();
        }
        var nopisession = new EPPlusSession(excelFilePath, sheetName, columnNames);
        return nopisession;
    }
    private string EscapeSheetNameInvalidChar(string sheetName)
    {
        string escapedSheetName = sheetName
            .Replace("/", "-")
            .Replace("\\", " ")
            .Replace("?", string.Empty)
            .Replace("*", string.Empty)
            .Replace("[", string.Empty)
            .Replace("]", string.Empty)
            .Replace(":", string.Empty);

        return escapedSheetName;
    }

    private void RowsToSheet(string[] columnNames, IList<string[]> rows, ExcelWorksheet worksheet)
    {
        int columncount = columnNames.Length;
        for (int i = 0; i < columncount; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
        }
        //遍历性能测试
        for (int i = 0; i < rows.Count; i++)
        {
            string[] strings = rows[i];
            if (strings.Length > columncount)
            {
                for (int j = 0; j < columncount; j++)
                {
                    worksheet.Cells[i + 2, j + 1].Value = strings[j];
                }
            }
            else
            {
                for (int j = 0; j < strings.Length; j++)
                {
                    worksheet.Cells[i + 2, j + 1].Value = strings[j];
                }
            }

        }
    }
}
public class EPPlusSession : IExportSession
{
    bool fileexists = false;
    private readonly ExcelWorksheet worksheet;
    private ExcelWorkbook workBook;
    private readonly ExcelPackage package;
    private readonly FileInfo file;

    public EPPlusSession(string path, string sheetName, string[] columnNames)
    {

        file = new FileInfo(path);
        package = new ExcelPackage(file);
        if (file.Exists)
        {
            fileexists = true;
        }
        workBook = package.Workbook;
        worksheet = CreatWorkSheet(sheetName, workBook);
        int columncount = columnNames.Length;
        for (int i = 0; i < columncount; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
        }
    }
    public EPPlusSession(string path, string sheetName, DataColumn[] columns)
    {

        file = new FileInfo(path);
        package = new ExcelPackage(file);
        if (file.Exists)
        {
            fileexists = true;
        }
        workBook = package.Workbook;
        worksheet = CreatWorkSheet(sheetName, workBook);
        int columncount = columns.Length;
        for (int i = 0; i < columncount; i++)
        {
            worksheet.Cells[1, i + 1].Value = columns[i].ColumnName;
        }
    }
    public IExportSession AddRows(IEnumerable<string[]> rows)
    {
        List<string[]> stringses = rows.ToList();
        for (int i = 0; i < stringses.Count; i++)
        {
            string[] strings = stringses[i];
            for (int j = 0; j < strings.Length; j++)
            {
                worksheet.Cells[i + 2, j + 1].Value = strings[j];
            }
        }
        return this;
    }

    public IExportSession AddRows(IEnumerable<DataRow> rows)
    {
        List<DataRow> dataRows = rows.ToList();

        for (int i = 0; i < dataRows.Count; i++)
        {
            List<object> objects = dataRows[i].ItemArray.ToList();

            for (int j = 0; j < objects.Count; j++)
            {
                worksheet.Cells[i + 2, j + 1].Value = objects[j];
            }
        }
        return this;
    }

    public void Finish()
    {
        worksheet.Cells.AutoFitColumns();
        package.SaveAs(file);
    }

    private ExcelWorksheet CreatWorkSheet(string sheetName, ExcelWorkbook workBook)
    {
        ExcelWorksheet worksheet = null;

        if (workBook.Worksheets.Count > 0)
        {
            foreach (var sheet in workBook.Worksheets)
            {
                if (String.Compare(sheet.Name, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    workBook.Worksheets.Delete(sheet.Name);
                    goto newasheet;
                }
            }
        newasheet:
            worksheet = workBook.Worksheets.Add(sheetName);
        }
        else
        {
            worksheet = workBook.Worksheets.Add(sheetName);
        }
        return worksheet;
    }
}

