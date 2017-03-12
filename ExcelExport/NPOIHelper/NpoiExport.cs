using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;
using NPOI.XSSF.UserModel;



public class NpoiExport : IDisposable
{
    const int MaximumNumberOfRowsPerSheet = 1000000;
    const int MaximumSheetNameLength = 25;
    protected XSSFWorkbook Workbook { get; set; }

    public NpoiExport()
    {
        this.Workbook = new XSSFWorkbook();
    }

    protected string EscapeSheetName(string sheetName)
    {
        var escapedSheetName = sheetName
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

    protected ISheet CreateExportDataTableSheetAndHeaderRow(DataTable exportData, ICellStyle headerRowStyle,string sheetname)
    {
        ISheet sheet;
        if (exportData.TableName=="")
        {
            string sheetname1 = "Sheet" + (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            sheet = Workbook.CreateSheet(sheetname1);
        }
        else
        {
             sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetname)); 
        }
        

        // Create the header row
        var row = sheet.CreateRow(0);

        for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
        {
            var cell = row.CreateCell(colIndex);
            cell.SetCellValue(exportData.Columns[colIndex].ColumnName);

            if (headerRowStyle != null)
                cell.CellStyle = headerRowStyle;
        }

        return sheet;
    }

    public void ExportDataTableToWorkbook(DataTable exportData,string excelFilePath, string headertext){
        using (FileStream fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
        {
            // Create the header row cell style
            var headerLabelCellStyle = this.Workbook.CreateCellStyle();
            headerLabelCellStyle.BorderBottom = BorderStyle.THIN;
            var headerLabelFont = this.Workbook.CreateFont();
            headerLabelFont.Boldweight = (short) FontBoldWeight.BOLD;
            headerLabelCellStyle.SetFont(headerLabelFont);
            string sheetName = exportData.TableName;
            var sheet = CreateExportDataTableSheetAndHeaderRow(exportData, headerLabelCellStyle, sheetName);
            var currentNPOIRowIndex = 1;
            var sheetCount = 1;

            for (var rowIndex = 0; rowIndex < exportData.Rows.Count; rowIndex++)
            {
                if (currentNPOIRowIndex >= MaximumNumberOfRowsPerSheet)
                {
                    sheetCount++;
                    currentNPOIRowIndex = 1;

                    sheet = CreateExportDataTableSheetAndHeaderRow(exportData,
                                                                   headerLabelCellStyle,
                                                                   sheetName + " - " + sheetCount);
                }

                var row = sheet.CreateRow(currentNPOIRowIndex++);

                for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
                {
                    var cell = row.CreateCell(colIndex);
                    cell.SetCellValue(exportData.Rows[rowIndex][colIndex].ToString());
                }
            }
            Workbook.Write(fs);
        }
    }

    public byte[] GetBytes()
    {
        var buffer = new MemoryStream();
        
            this.Workbook.Write(buffer);
            return buffer.ToArray();
        
    }

    public void Dispose()
    {
        //if (this.Workbook != null)
            //this.Workbook.di;
    }
}