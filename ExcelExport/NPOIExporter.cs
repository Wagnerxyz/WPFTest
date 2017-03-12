using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace ExcelTest
{
    //sheet行数满不自动判断新建sheet，由用户新建sheet
    public class NPOIExporter : IExcelExporterWithHeader
    {
        //性能考虑暂不判断
        const int MaximumNumberOfRowsPerSheet = 1000000;
        const int MaximumSheetNameLength = 250;


        //直接cell存 string
        public void Export(DataTable dtSource, string excelFilePath, string headerText = "")
        {
            if (dtSource == null || excelFilePath == string.Empty)
            {
                return;
            }
            XSSFWorkbook workbook;
            if (File.Exists(excelFilePath))
            {
                FileStream readfs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
                workbook = new XSSFWorkbook(readfs);
                readfs.Dispose();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }
            using (FileStream fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
            {
                int columncount = dtSource.Columns.Count;
                ISheet sheet = CreateSheetFromdtName(dtSource, workbook);
                ICellStyle dateStyle = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy/mm/dd hh:mm");

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    #region 新建表，填充表头，填充列头，样式

                    if (i == 0 && sheet.LastRowNum == 0)
                    {
                        #region 表头及样式

                        CreateHeaderRow(workbook, sheet, headerText, columncount);

                        #endregion

                        #region 列头及样式

                        IRow headerRow = sheet.CreateRow(1);
                        var columnrowstyle = CreatColumnRowStyle(workbook);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = columnrowstyle;
                        }

                        #endregion

                        //  rowIndex = 2;
                    }

                    #endregion

                    #region 填充内容

                    IRow dataRow = sheet.CreateRow(sheet.LastRowNum + 1);

                    for (int colIndex = 0; colIndex < columncount; colIndex++)
                    {
                        ICell cell = dataRow.CreateCell(colIndex);
                        cell.SetCellValue(dtSource.Rows[i][colIndex].ToString());
                    }
                    #endregion
                }
                for (int i = 0; i < columncount; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(fs);
            }
        }

        public void Export(string[] columnNames, IList<string[]> rows, string excelFilePath, string sheetName = "Sheet", string headerText = "")
        {
            if (rows == null || excelFilePath == string.Empty || columnNames == null)
            {
                return;
            }
            XSSFWorkbook workbook;
            if (File.Exists(excelFilePath))
            {
                FileStream readfs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
                workbook = new XSSFWorkbook(readfs);
                readfs.Dispose();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }
            int length = columnNames.Length;
            using (var fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
            {
                ISheet sheet = UseExistOrCreateNewSheet(workbook, sheetName);

                //ICellStyle dateStyle = workbook.CreateCellStyle();
                //IDataFormat format = workbook.CreateDataFormat();
                //dateStyle.DataFormat = format.GetFormat("yyyy/m/d hh:mm:ss:ms");
                int maxcolumncount = 0;
                for (int i = 0; i < rows.Count(); i++)
                {
                    string[] row = rows[i];
                    if (row.Length - 1 > maxcolumncount)
                    {
                        maxcolumncount = row.Length - 1;
                    }
                    #region 新建表，填充表头，填充列头，样式

                    if (i == 0 && sheet.LastRowNum == 0)
                    {
                        #region 表头及样式

                        CreateHeaderRow(workbook, sheet, headerText, row.Length);

                        #endregion

                        #region 列头及样式
                        var _columnRow = sheet.CreateRow(1);
                        var columnrowstyle = CreatColumnRowStyle(workbook);
                        for (int colIndex = 0; colIndex < length; colIndex++)
                        {
                            ICell cell = _columnRow.CreateCell(colIndex);
                            cell.SetCellValue(columnNames[colIndex]);
                            cell.CellStyle = columnrowstyle;
                        }
                        #endregion
                        //  rowIndex = 2;
                    }

                    #endregion

                    #region 填充内容

                    IRow dataRow = sheet.CreateRow(sheet.LastRowNum + 1);
                    for (int colIndex = 0; colIndex < row.Length; colIndex++)
                    {
                        ICell cell = dataRow.CreateCell(colIndex);
                        cell.SetCellValue(row[colIndex]);
                    }

                    #endregion
                }
                for (int j = 0; j < maxcolumncount; j++)
                {
                    sheet.AutoSizeColumn(j);
                }
                workbook.Write(fs);
            }
        }

        public IExportSession Export(string excelFilePath)
        {
            var fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write);
            return new NPOIExportSession(fs);
        }

        #region Utility methods
        //判断sheetname是否已存在,已存在则append,不存在则create
        private ISheet CreateSheetFromdtName(DataTable dtSource, XSSFWorkbook workbook)
        {
            ISheet sheet;
            if (dtSource.TableName == "")
            {
                // POSIX Time避免重复
                string sheetname = "Sheet" + (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                sheet = workbook.CreateSheet(sheetname);
            }
            else
            {
                sheet = UseExistOrCreateNewSheet(workbook, dtSource.TableName);
            }
            return sheet;
        }

        private ISheet UseExistOrCreateNewSheet(XSSFWorkbook workbook, string sheetname)
        {
            ISheet sheet = workbook.GetSheet(sheetname);
            if (sheet != null)
            {
                return sheet;
            }
            return workbook.CreateSheet(EscapeSheetNameInvalidChar(sheetname));
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

            if (escapedSheetName.Length > MaximumSheetNameLength)
                escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

            return escapedSheetName;
        }

        private static void CreateHeaderRow(XSSFWorkbook workbook, ISheet sheet, string headerText, int headrowwidth)
        {
            IRow headerRow = sheet.CreateRow(0);
            headerRow.HeightInPoints = 25;
            headerRow.CreateCell(0).SetCellValue(headerText);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.CENTER;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            headerRow.GetCell(0).CellStyle = headStyle;
            //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, headrowwidth - 1));
        }

        private ICellStyle CreatColumnRowStyle(XSSFWorkbook workbook)
        {
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.CENTER;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            return headStyle;
        }

        private void FillRowFromDataTable(DataTable dtSource, ISheet sheet, int i, DataRow row, ICellStyle dateStyle)
        {
            IRow dataRow = sheet.CreateRow(sheet.LastRowNum + 1);
            //TODO 好判断 单独提出来！
            foreach (DataColumn column in dtSource.Columns)
            {
                ICell newCell = dataRow.CreateCell(column.Ordinal);
                //TODO: perf 后续考虑用序号
                string drValue = row[column].ToString();

                switch (column.DataType.ToString())
                {
                    case "System.String": //字符串类型
                        newCell.SetCellValue(drValue);
                        break;
                    case "System.DateTime": //日期类型
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        newCell.SetCellValue(dateV);

                        newCell.CellStyle = dateStyle; //格式化显示
                        break;
                    case "System.Boolean": //布尔型
                        bool boolV;
                        bool.TryParse(drValue, out boolV);
                        newCell.SetCellValue(boolV);
                        break;
                    case "System.Int16": //整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV;
                        int.TryParse(drValue, out intV);
                        newCell.SetCellValue(intV);
                        break;
                    case "System.Decimal": //浮点型
                    case "System.Double":
                        double doubV;
                        double.TryParse(drValue, out doubV);
                        newCell.SetCellValue(doubV);
                        break;
                    case "System.DBNull": //空值处理
                        newCell.SetCellValue("");
                        break;
                    default:
                        newCell.SetCellValue(drValue);
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        ///     将DataTable导出到Excel，Excel每个元素根据Datatable实际类型判断。
        ///     DataTable的名称作为sheet页名称，如果指定的Excel文件存在，则在已有Excel文件中增加sheet页，如果已存在同名的sheet页，则覆盖sheet页
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="excelFilePath"></param>
        /// <param name="headerText"></param>
        public void ExportAsConcreteType(DataTable dtSource, string excelFilePath, string headerText = "")
        {
            if (dtSource == null || excelFilePath == null)
            {
                return;
            }
            XSSFWorkbook workbook;
            if (File.Exists(excelFilePath))
            {
                FileStream readfs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
                workbook = new XSSFWorkbook(readfs);
                readfs.Dispose();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }
            using (var fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
            {
                int columncount = dtSource.Columns.Count;
                ISheet sheet = CreateSheetFromdtName(dtSource, workbook);

                ICellStyle dateStyle = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy/mm/dd hh:mm");

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    DataRow row = dtSource.Rows[i];

                    #region 新建表，填充表头，填充列头，样式

                    //if (rowIndex == 65535 || rowIndex == 0)
                    //{
                    //    if (rowIndex != 0)
                    //    {
                    //        sheet = workbook.CreateSheet();
                    //    }
                    if (i == 0 && sheet.LastRowNum == 0)
                    {
                        #region 表头及样式

                        CreateHeaderRow(workbook, sheet, headerText, dtSource.Columns.Count);

                        #endregion

                        #region 列头及样式

                        {
                            IRow headerRow = sheet.CreateRow(1);
                            var columnrowstyle = CreatColumnRowStyle(workbook);
                            foreach (DataColumn column in dtSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = columnrowstyle;

                            }
                        }

                        #endregion

                        //  rowIndex = 2;
                    }
                    //}

                    #endregion

                    #region 填充内容

                    FillRowFromDataTable(dtSource, sheet, i, row, dateStyle);

                    #endregion
                }
                for (int i = 0; i < columncount; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(fs);
                //  workbook = null;
            }
        }

    }

    public class NPOIExportSession : IExportSession
    {
        private readonly ICellStyle _dateStyle;
        private readonly ISheet _sheet1;
        private readonly FileStream fileStream;
        private readonly XSSFWorkbook sessionworkbook;
        private int rownumber;
        private int maxcountnumber = 0;

        //列宽不确定 暂不考虑第一行作为headerrow
        public NPOIExportSession(FileStream fs)
        {
            fileStream = fs;
            sessionworkbook = new XSSFWorkbook();
            _sheet1 = sessionworkbook.CreateSheet();
            _dateStyle = sessionworkbook.CreateCellStyle();
            IDataFormat format = sessionworkbook.CreateDataFormat();
            _dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
        }

        public IExportSession AddRows(IEnumerable<string[]> rows)
        {
            List<string[]> rowslist = rows.ToList();
            for (int i = 0; i < rowslist.Count(); i++)
            {
                if (rowslist[i].Length - 1 > maxcountnumber)
                {
                    maxcountnumber = rowslist[i].Length - 1;
                }
                IRow row = _sheet1.CreateRow(rownumber++);
                for (int colIndex = 0; colIndex < rowslist[i].Length; colIndex++)
                {
                    row.CreateCell(colIndex).SetCellValue(rowslist[i][colIndex]);
                }
            }
            return this;
        }

        public IExportSession AddRows(IEnumerable<DataRow> rows)
        {
            List<DataRow> rowslist = rows.ToList();
            for (int i = 0; i < rowslist.Count(); i++)
            {
                DataRow dataRow = rowslist[i];
                IRow row = _sheet1.CreateRow(rownumber++);
                //需要传DataTable.Columns.Count
                for (var colIndex = 0; colIndex < rowslist.Count; colIndex++)
                {
                    row.CreateCell(colIndex).SetCellValue(dataRow[colIndex].ToString());
                    //ICell cell = headerRow.CreateCell(colIndex);
                    //cell.SetCellValue(columnNames[colIndex]);
                    //cell.CellStyle = headStyle;

                    //设置列宽
                    //    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                }
            }
            return this;
        }

        public void Finish()
        {
            sessionworkbook.Write(fileStream);
        }
    }
}