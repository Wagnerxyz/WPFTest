using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelTest
{
    /// <summary>
    /// 超过1048576行后打开报错，修复后截断正常
    /// </summary>
    public class OpenXMLExporter 
    {
        public void ExportSAX(DataTable dt, string excelFilePath)
        {
            if (dt == null || excelFilePath == string.Empty)
            {
                throw new ArgumentNullException();
            }
            var fi = new FileInfo(excelFilePath);

            #region 文件已存在

            if (File.Exists(excelFilePath) && fi.Length != 0)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(excelFilePath, true))
                {
                    Sheet sheet = document.WorkbookPart.Workbook.Descendants<Sheet>()
                        .FirstOrDefault(s => s.Name == EscapeSheetNameInvalidChar(dt.TableName));
                    if (sheet != null)
                    {
                        var worksheetPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheet.Id));
                        sheet.Remove();
                        document.WorkbookPart.DeletePart(worksheetPart);
                        document.WorkbookPart.Workbook.Save();
                    }
                    uint sheetId = 1;
                    WorksheetPart wsp = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    if (sheets == null)
                    {
                        throw new OpenXmlPackageException("已有Excel文件损坏，请删除");
                    }
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }
                    //create worksheet part, and add it to the sheets collection in workbook
                    Sheet sheet1new = new Sheet { Id = document.WorkbookPart.GetIdOfPart(wsp), SheetId = sheetId, Name = EscapeSheetNameInvalidChar(dt.TableName) };
                    sheets.Append(sheet1new);
                   // List<OpenXmlAttribute> oxa;

                    OpenXmlWriter writer = OpenXmlWriter.Create(wsp);
                    writer.WriteStartElement(new Worksheet());
                    writer.WriteStartElement(new SheetData());
                    ///TODO:auto row page
                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        //  oxa = new List<OpenXmlAttribute>();
                        //  oxa.Add(new OpenXmlAttribute("r", null, (i + 1).ToString(CultureInfo.InvariantCulture)));

                        writer.WriteStartElement(new Row());
                        for (int j = 0; j < dt.Columns.Count; ++j)
                        {
                            //if use WriteStartElement must manual add OpenXmlAttribute
                            //  oxa = new List<OpenXmlAttribute>();
                            //   oxa.Add(new OpenXmlAttribute("t", null, "str"));

                            writer.WriteElement(new Cell { CellValue = new CellValue(dt.Rows[i].ItemArray[j].ToString()), DataType = CellValues.String });
                            //  writer.WriteStartElement(new Cell());

                            //   writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    // this is for SheetData
                    writer.WriteEndElement();
                    // this is for Worksheet
                    writer.WriteEndElement();
                    writer.Close();


                }
            }
            #endregion

            #region 新建文件

            else
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilePath, SpreadsheetDocumentType.Workbook))
                {
                    document.AddWorkbookPart();
                    //    List<OpenXmlAttribute> oxa;
                    document.WorkbookPart.Workbook = new Workbook();
                    WorksheetPart wsp = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    //create worksheet part, and add it to the sheets collection in workbook
                    Sheet sheet = new Sheet { Id = document.WorkbookPart.GetIdOfPart(wsp), SheetId = 1, Name = dt.TableName };
                    sheets.Append(sheet);
                    OpenXmlWriter writer = OpenXmlWriter.Create(wsp);
                    writer.WriteStartElement(new Worksheet());
                    writer.WriteStartElement(new SheetData());
                    ///TODO:auto row page
                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        //  oxa = new List<OpenXmlAttribute>();
                        //  oxa.Add(new OpenXmlAttribute("r", null, (i + 1).ToString(CultureInfo.InvariantCulture)));

                        writer.WriteStartElement(new Row());

                        for (int j = 0; j < dt.Columns.Count; ++j)
                        {
                            //if use WriteStartElement must manual add OpenXmlAttribute
                            //  oxa = new List<OpenXmlAttribute>();
                            //   oxa.Add(new OpenXmlAttribute("t", null, "str"));

                            // you can use object initialisers like this only when the properties
                            // are actual properties. SDK classes sometimes have property-like properties
                            // but are actually classes. For example, the Cell class has the CellValue
                            // "property" but is actually a child class internally.
                            // If the properties correspond to actual XML attributes, then you're fine.
                            writer.WriteElement(new Cell { CellValue = new CellValue(dt.Rows[i].ItemArray[j].ToString()), DataType = CellValues.String });
                            //  writer.WriteStartElement(new Cell());

                            //   writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Close();
                }
            }
            #endregion
        }

        public void Export(DataTable dt, string excelFilePath,string header)
        {
            if (dt == null || excelFilePath == string.Empty)
            {
                throw new ArgumentNullException();
            }

            var fi = new FileInfo(excelFilePath);

            #region 文件已存在

            if (File.Exists(excelFilePath) && fi.Length != 0)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(excelFilePath, true))
                {
                    Sheet sheet =
                        document.WorkbookPart.Workbook.Descendants<Sheet>()
                            .FirstOrDefault(s => s.Name == EscapeSheetNameInvalidChar(dt.TableName));
                    if (sheet != null)
                    {
                        var worksheetPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheet.Id));
                        sheet.Remove();
                        document.WorkbookPart.DeletePart(worksheetPart);
                        document.WorkbookPart.Workbook.Save();
                    }
                    //  document.AddWorkbookPart();
                    //    document.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                    //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                    //    document.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                    //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                    //   WorkbookStylesPart workbookStylesPart = document.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                    //  Stylesheet stylesheet = new Stylesheet();
                    //  workbookStylesPart.Stylesheet = stylesheet;

                    uint sheetId = 1;

                    var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    newWorksheetPart.Worksheet = new Worksheet(new SheetData());
  

                    // save worksheet
                    WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                    newWorksheetPart.Worksheet.Save();

                    var sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    if (sheets == null)
                    {
                        throw new OpenXmlPackageException("已有Excel文件损坏，请删除");
                    }
                    // create the worksheet to workbook relation
                    //if (worksheetNumber == 1)
                    //    document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }
                    sheets.AppendChild(new Sheet
                    {
                        Id = document.WorkbookPart.GetIdOfPart(newWorksheetPart),
                        SheetId = sheetId,
                        Name = EscapeSheetNameInvalidChar(dt.TableName)
                    });


                    document.WorkbookPart.Workbook.Save();

                    Trace.WriteLine("成功创建: " + excelFilePath);
                }
            }
            #endregion

            #region 新建文件

            else
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilePath,
                    SpreadsheetDocumentType.Workbook))
                {
                    document.AddWorkbookPart();
                    document.WorkbookPart.Workbook = new Workbook();

                    //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                    //    document.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                    //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                    //  WorkbookStylesPart workbookStylesPart = document.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                    //   Stylesheet stylesheet = new Stylesheet();
                    //   workbookStylesPart.Stylesheet = stylesheet;

                    uint sheetId = 1;
                    var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    newWorksheetPart.Worksheet = new Worksheet(new SheetData());
                    HeaderFooter headerFooter = new HeaderFooter();
                    headerFooter.FirstHeader = new FirstHeader();
                    headerFooter.FirstHeader.Text = header;
                    newWorksheetPart.Worksheet.AppendChild<HeaderFooter>(headerFooter); ;
                    // save worksheet
                    WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                    newWorksheetPart.Worksheet.Save();
                    var sheets = new Sheets();
                    document.WorkbookPart.Workbook.AppendChild(sheets);
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    sheets.AppendChild(new Sheet
                    {
                        Id = document.WorkbookPart.GetIdOfPart(newWorksheetPart),
                        SheetId = sheetId,
                        Name = EscapeSheetNameInvalidChar(dt.TableName)
                    });

                    document.WorkbookPart.Workbook.Save();
                }
                Trace.WriteLine("成功创建: " + excelFilePath);
            }

            #endregion

        }


        public void Export(string[] columnNames, IEnumerable<string[]> rows, string excelFilePath, string sheetName)
        {
            if (columnNames == null || excelFilePath == string.Empty || !rows.Any())
            {
                throw new ArgumentNullException();
            }
            //  DataTable dt = ListToDataTable(rows.ToList(), columnNames);
            //  dt.TableName = sheetName;  
            var fi = new FileInfo(excelFilePath);
            if (File.Exists(excelFilePath) && fi.Length != 0)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(excelFilePath, true))
                {
                    Sheet sheet =
                        document.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
                    if (sheet != null)
                    {
                        WorksheetPart worksheetPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheet.Id));
                        sheet.Remove();
                        document.WorkbookPart.DeletePart(worksheetPart);
                        document.WorkbookPart.Workbook.Save();
                    }

                    //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                    //    document.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                    //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                    //   WorkbookStylesPart workbookStylesPart = document.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                    //  Stylesheet stylesheet = new Stylesheet();
                    //  workbookStylesPart.Stylesheet = stylesheet;

                    uint sheetId = 1;

                    var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    newWorksheetPart.Worksheet = new Worksheet(sheetData);
                    var headerRow = new Row();

                    foreach (string column in columnNames)
                    {
                        var cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);
                    // save worksheet
                    // WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                    foreach (var row in rows)
                    {
                        var newRow = new Row();
                        foreach (String col in row)
                        {
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(col);
                            newRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(newRow);
                        //  sheetPart.Worksheet.Save();
                    }

                    newWorksheetPart.Worksheet.Save();
                    Sheets sheets;
                    if (document.WorkbookPart.Workbook.GetFirstChild<Sheets>() != null)
                    {
                        sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    }
                    else
                    {
                        sheets = new Sheets();
                        document.WorkbookPart.Workbook.AppendChild(sheets);
                    }
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }
                    sheets.AppendChild(new Sheet
                    {
                        Id = document.WorkbookPart.GetIdOfPart(newWorksheetPart),
                        SheetId = sheetId,
                        Name = EscapeSheetNameInvalidChar(sheetName)
                    });


                    document.WorkbookPart.Workbook.Save();

                    Trace.WriteLine("成功创建: " + excelFilePath);
                }
            }
            else
            {
                using (
                    SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilePath,
                        SpreadsheetDocumentType.Workbook))
                {
                    document.AddWorkbookPart();
                    document.WorkbookPart.Workbook = new Workbook();

                    //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                    //    document.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                    //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                    //  WorkbookStylesPart workbookStylesPart = document.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                    //   Stylesheet stylesheet = new Stylesheet();
                    //   workbookStylesPart.Stylesheet = stylesheet;

                    uint sheetId = 1;


                    var newWorksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    newWorksheetPart.Worksheet = new Worksheet(sheetData);

                    var headerRow = new Row();
                    foreach (string column in columnNames)
                    {
                        var cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);
                    // save worksheet
                    // WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                    foreach (var row in rows)
                    {
                        var newRow = new Row();
                        foreach (String col in row)
                        {
                            var cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(col);
                            newRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(newRow);
                        //  sheetPart.Worksheet.Save();
                    }

                    newWorksheetPart.Worksheet.Save();

                    document.WorkbookPart.Workbook.AppendChild(new Sheets());
                    var sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    // create the worksheet to workbook relation
                    //if (worksheetNumber == 1)
                    //    document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }
                    sheets.AppendChild(new Sheet
                    {
                        Id = document.WorkbookPart.GetIdOfPart(newWorksheetPart),
                        SheetId = sheetId,
                        Name = EscapeSheetNameInvalidChar(sheetName)
                    });

                    document.WorkbookPart.Workbook.Save();
                    document.Close();
                }
                Trace.WriteLine("成功创建: " + excelFilePath);
            }
        }

        public IExportSession Export(string excelFilePath, string sheetName, string[] columnNames)
        {
            if (excelFilePath == "" || sheetName == "" || columnNames == null)
            {
                throw new ArgumentException();
            }
            return new OpenXMLSession(excelFilePath, sheetName, columnNames);
        }

        public IExportSession Export(string excelFilePath, string sheetName, DataColumn[] columnNames)
        {
            if (excelFilePath == "" || sheetName == "" || columnNames == null)
            {
                throw new ArgumentException();
            }
            return new OpenXMLSession(excelFilePath, sheetName, columnNames);
        }



        private void CreatePartsDT(DataTable dt, SpreadsheetDocument spreadsheet)
        {
            uint sheetId = 1;
            var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());

            // save worksheet
            WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
            newWorksheetPart.Worksheet.Save();
            spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
            var sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            // create the worksheet to workbook relation
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = sheetId,
                Name = EscapeSheetNameInvalidChar(dt.TableName)
            });


            spreadsheet.WorkbookPart.Workbook.Save();
            //   spreadsheet.Close();
        }

        private void CreateParts(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;


            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber;
                string worksheetName = dt.TableName;

                var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = worksheetNumber,
                    Name = EscapeSheetNameInvalidChar(dt.TableName)
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }

        private void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            var IsNumericColumn = new bool[numberOfColumns];

            var excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex }; // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;

            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex }; // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow);
                        }
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow);
                    }
                }
            }
        }

        private void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell { CellReference = cellReference, DataType = CellValues.String };
            var cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell { CellReference = cellReference };
            var cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        //数字太大有缺陷
        private string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        private string GetColumnName(int index)
        {
            string name = "";
            char[] columnNames = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int num = index;
            do
            {
                int i = num % 26;
                name = columnNames[i] + name;
                num = num / 26 - 1;
            } while (num > -1);
            if (string.IsNullOrEmpty(name))
                name = "A";
            return name;
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
    }


    public class OpenXMLSession : IExportSession
    {
        private readonly SpreadsheetDocument document;
        private readonly SheetData sheetData;
        private List<string> columns;
        private bool fileexists = false;
        private WorksheetPart sheetPart;

        public OpenXMLSession(string excelFilePath, string sheetName, string[] columnNames)
        {
            var fi = new FileInfo(excelFilePath);
            if (File.Exists(excelFilePath) && fi.Length != 0)
            {
                document = SpreadsheetDocument.Open(excelFilePath, true);
            }
            else
            {
                document = SpreadsheetDocument.Create(excelFilePath, SpreadsheetDocumentType.Workbook);
                document.AddWorkbookPart();
                document.WorkbookPart.Workbook = new Workbook();
            }
            Sheet sheetold = document.WorkbookPart.Workbook.Descendants<Sheet>()
                .FirstOrDefault(s => s.Name == sheetName);
            if (sheetold != null)
            {
                WorksheetPart worksheetPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheetold.Id));
                sheetold.Remove();
                document.WorkbookPart.DeletePart(worksheetPart);
                document.WorkbookPart.Workbook.Save();
            }

            sheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);
            sheetPart.Worksheet.Save();

            Sheets sheets;
            if (document.WorkbookPart.Workbook.GetFirstChild<Sheets>() != null)
            {
                sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            }
            else
            {
                sheets = new Sheets();
                document.WorkbookPart.Workbook.AppendChild(sheets);
            }

            string relationshipId = document.WorkbookPart.GetIdOfPart(sheetPart);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId =
                    sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            var sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);

            var headerRow = new Row();

            columns = new List<string>();
            foreach (string column in columnNames)
            {
                columns.Add(column);

                var cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);
        }

        public OpenXMLSession(string excelFilePath, string sheetName, DataColumn[] columns)
        {
            var fi = new FileInfo(excelFilePath);
            if (File.Exists(excelFilePath) && fi.Length != 0)
            {
                document = SpreadsheetDocument.Open(excelFilePath, true);
            }

            else
            {
                document = SpreadsheetDocument.Create(excelFilePath, SpreadsheetDocumentType.Workbook);
                document.AddWorkbookPart();
                document.WorkbookPart.Workbook = new Workbook();
            }
            Sheet sheetold = document.WorkbookPart.Workbook.Descendants<Sheet>()
                .FirstOrDefault(s => s.Name == sheetName);
            if (sheetold != null)
            {
                WorksheetPart worksheetPart = (WorksheetPart)(document.WorkbookPart.GetPartById(sheetold.Id));
                sheetold.Remove();
                document.WorkbookPart.DeletePart(worksheetPart);
                document.WorkbookPart.Workbook.Save();
            }

            sheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(sheetData);
            sheetPart.Worksheet.Save();

            Sheets sheets;
            if (document.WorkbookPart.Workbook.GetFirstChild<Sheets>() != null)
            {
                sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            }
            else
            {
                sheets = new Sheets();
                document.WorkbookPart.Workbook.AppendChild(sheets);
            }

            string relationshipId = document.WorkbookPart.GetIdOfPart(sheetPart);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId =
                    sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            var sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);

            var headerRow = new Row();

            this.columns = new List<string>();
            foreach (DataColumn column in columns)
            {
                this.columns.Add(column.ColumnName);

                var cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);
        }

        public IExportSession AddRows(IEnumerable<string[]> rows)
        {
            foreach (var row in rows)
            {
                var newRow = new Row();
                foreach (String col in row)
                {
                    var cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(col);
                    newRow.AppendChild(cell);
                }
                sheetData.AppendChild(newRow);
            }
            return this;
        }

        public IExportSession AddRows(IEnumerable<DataRow> rows)
        {
            foreach (DataRow dataRow in rows)
            {
                List<object> objects = dataRow.ItemArray.ToList();
                var newRow = new Row();
                foreach (object col in objects)
                {
                    var cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(col.ToString());
                    newRow.AppendChild(cell);
                }
                //内部逻辑自己存，手动调用无意义
                //  sheetPart.Worksheet.Save();
                sheetData.AppendChild(newRow);
            }
            return this;
        }

        public void Finish()
        {
            document.WorkbookPart.Workbook.Save();
            document.Close();
        }
    }
}