using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


public class ExcelUtils
{
    /// <summary>
    /// Builds the workbook.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="inputData">The input data.</param>
    /// <param name="overwriteContents"></param>
    /// <param name="clearHeader"></param>
    public void SaveWorkbook(string fileName, DataSet inputData, bool overwriteContents, bool clearHeader)
    {
        SpreadsheetDocument spreadsheetDocument;
        bool isNewFile = false;
        try
        {

            if (!File.Exists(fileName))
            {
                spreadsheetDocument = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook); //Create
                isNewFile = true;
            }
            else
            {
                spreadsheetDocument = SpreadsheetDocument.Open(fileName, true); //Opening exisiting
                isNewFile = false;
            }

            if (true)//using (spreadsheetDocument)
            {

                WorkbookPart workbookPart;

                //Instantiates workbookpart
                if (isNewFile)
                    workbookPart = spreadsheetDocument.AddWorkbookPart(); //If file is new file
                else
                    workbookPart = spreadsheetDocument.WorkbookPart;

                //Creates Workbook if workbook not existed(For New Excel File), Existing excel file this is not necessary
                if (workbookPart.Workbook == null)
                    workbookPart.Workbook = new Workbook();

                Sheets sheets;

                //
                if (isNewFile)
                    sheets = workbookPart.Workbook.AppendChild(new Sheets()); //Adding new sheets to the file, if new file
                else
                    sheets = workbookPart.Workbook.Sheets; //Retrieving existing sheets from the file

                FileVersion fileVersion = new FileVersion { ApplicationName = "Microsoft Office Excel" };
                // workbook.Append(fileVersion);

                uint tableIndex = 0;

                //converting the sheets collection to a list of <Sheet>
                List<Sheet> sheetsList = workbookPart.Workbook.Descendants<Sheet>().ToList();

                // If the InputDataSet having 1 or more tables, looping through and
                // creates new sheet for each table and dumps the data to the sheet
                // and saves the excel workbook.
                foreach (DataTable inputDataTable in inputData.Tables)
                {
                    bool hasSheetExists = false;
                    int sheetIndex = 0;
                    string relId = "";
                    Sheet sheet;

                    //Checking sheet exists in the excel file.
                    sheetIndex = sheetsList.FindIndex(c => c.Name == inputDataTable.TableName);

                    if (sheetIndex >= 0)
                    {
                        hasSheetExists = true;
                    }

                    WorksheetPart worksheetPart;

                    if (!hasSheetExists) //If a new sheet
                    {

                        worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        relId = workbookPart.GetIdOfPart(worksheetPart);
                        sheet = new Sheet { Name = inputDataTable.TableName, SheetId = tableIndex + 1, Id = relId };
                    }
                    else // if sheet already exists
                    {
                        sheet = sheetsList[sheetIndex];

                        List<WorksheetPart> partList = workbookPart.WorksheetParts.ToList();

                        // Take the existing the sheet reference from the workbook.
                        WorksheetPart worksheetPart1 = (WorksheetPart)(workbookPart.GetPartById(sheet.Id));

                        worksheetPart = worksheetPart1;

                        //Retriving RelationID form the workbookPart
                        relId = workbookPart.GetIdOfPart(worksheetPart);
                        // partList.FindAll( c=> c.)
                        // worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    }

                    SheetData sheetData = new SheetData();

                    Worksheet workSheet = worksheetPart.Worksheet;

                    int lastRowIndex = 0;

                    if (workSheet != null)
                    {
                        //Retrieving existing sheet data from the worksheet
                        sheetData = workSheet.GetFirstChild<SheetData>();

                        if (overwriteContents) //Clearing the contents of the Sheet, except the header
                        {
                            int endIndex = 1;

                            //if value true clear the existing header
                            if (clearHeader)
                                endIndex = 0;

                            //Deleting content row by row, starting from the bottom.
                            for (int childIndex = sheetData.ChildElements.Count - 1; childIndex >= endIndex; childIndex--)
                            {
                                sheetData.RemoveChild(sheetData.ChildElements[childIndex]);
                            }
                        }

                        //Getting all existing record rows.
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();

                        //Considering the last row index as total row count. append the records to the last index onwards.
                        lastRowIndex = rows.Count();
                    }
                    else //Creating new worksheet
                    {
                        workSheet = new Worksheet(sheetData);
                        worksheetPart.Worksheet = workSheet;
                    }

                    //If Data Table is not empty
                    if (inputDataTable != null && inputDataTable.Rows.Count > 0)
                    {
                        //If Not Sheet already exists(Based on Table Name) -- creating column headers for the excel sheet
                        if (!hasSheetExists || lastRowIndex < 1)
                        {
                            //Creating columns..(INDX = 1 , Header)
                            Row headerRow = CreateContentHeader(1, inputDataTable.Columns);
                            sheetData.Append(headerRow);
                            lastRowIndex = 1;
                        }

                        //Last Row index
                        lastRowIndex++;

                        //Worksheet Data Row Number.. (INDX = 2 onwards data)
                        uint currDataRowIndex = (uint)lastRowIndex; //From this index on data will get appended.

                        //Creating Row Data
                        for (int iterRowIndex = 0; iterRowIndex < inputDataTable.Rows.Count; iterRowIndex++)
                        {
                            //Retrieving current DataRow from DataTable
                            DataRow currentInputRow = inputDataTable.Rows[iterRowIndex];

                            //Creating insertble row for the openxml.
                            Row contentRow = CreateContentRow(currDataRowIndex, currentInputRow,
                            inputDataTable.Columns);
                            currDataRowIndex++;

                            //Appending to sheet data
                            sheetData.AppendChild(contentRow);
                        }
                    }

                    //new Worksheet(sheetData);

                    //Saving worksheet contents
                    worksheetPart.Worksheet.Save();

                    //If sheet new, then appending to sheets collection
                    if (!hasSheetExists)
                        sheets.AppendChild(sheet);

                    tableIndex++;

                }

                //Saving the complete workbook to disk
                spreadsheetDocument.WorkbookPart.Workbook.Save();

                spreadsheetDocument.Close();

            }

        }

        catch (Exception e)
        {
            // spreadsheetDocument.Close();
            throw;

        }

    }

    /// <summary>
    /// Creates the content header.
    /// </summary>
    /// <param name="rowDataIndex">Index of the row data.</param>
    /// <param name="dataColumns">The data columns.</param>
    /// <returns></returns>
    private Row CreateContentHeader(UInt32 rowDataIndex, DataColumnCollection dataColumns)
    {
        Row resultRow = new Row { RowIndex = rowDataIndex };

        for (int iterColIndex = 0; iterColIndex < dataColumns.Count; iterColIndex++)
        {
            Cell cell1 = CreateHeaderCell(dataColumns[iterColIndex].ColumnName, rowDataIndex, dataColumns[iterColIndex].ColumnName); //CreateTextCell("A", rowDataIndex, Convert.ToString(dataRow[iterColIndex]));
            resultRow.Append(cell1);

        }

        return resultRow;
    }

    /// <summary>
    /// Creates the content row.
    /// </summary>
    /// <param name="rowDataIndex">The rowDataIndex.</param>
    /// <param name="dataRow">DataRow</param>
    /// <param name="dataColumns">DataColumnCollection</param>
    /// <returns></returns>
    private Row CreateContentRow(UInt32 rowDataIndex, DataRow dataRow, DataColumnCollection dataColumns)
    {

        Row resultRow = new Row { RowIndex = rowDataIndex };

        for (int iterColIndex = 0; iterColIndex < dataColumns.Count; iterColIndex++)
        {
            Cell cell1 = CreateContentCell(dataColumns[iterColIndex].ColumnName, rowDataIndex, dataRow[iterColIndex]); //CreateTextCell("A", rowDataIndex, Convert.ToString(dataRow[iterColIndex]));
            resultRow.Append(cell1);

        }

        return resultRow;

    }

    /// <summary>
    /// Creates the content cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The rowDataIndex.</param>
    /// <param name="inputValue">The input value.</param>
    /// <returns></returns>
    private Cell CreateContentCell(string header, UInt32 index, object inputValue)
    {
        Cell resultCell = null;

        Type objectType = inputValue.GetType();

        TypeCode objectTypeCode;

        bool parseSuccess = Enum.TryParse(objectType.Name, true, out objectTypeCode);

        if (parseSuccess)
        {
            switch (objectTypeCode)
            {
                // Number Fields
                case TypeCode.UInt64:
                case TypeCode.UInt32:
                case TypeCode.UInt16:
                case TypeCode.Int64:
                case TypeCode.Int32:
                case TypeCode.Int16:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    resultCell = CreateNumberCell(header, index, inputValue);
                    break;
                // Date Time Field
                case TypeCode.DateTime:
                    resultCell = CreateDateCell(header, index, inputValue);
                    break;
                // Boolean Field
                case TypeCode.Boolean:
                    resultCell = CreateBooleanCell(header, index, inputValue);
                    break;

                default:
                    resultCell = CreateTextCell(header, index, inputValue);
                    break;
                //case TypeCode.
            }
        }
        else
            resultCell = CreateTextCell(header, index, inputValue);
        return resultCell;
    }

    /// <summary>
    /// Creates the header cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The index.</param>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    private Cell CreateHeaderCell(string header, UInt32 index, object text)
    {
        Cell c = new Cell { DataType = CellValues.String, CellReference = header + index };

        CellValue cellValue = new CellValue
        {
            Text = Convert.ToString(text),
        };

        c.Append(cellValue);

        return c;

    }

    /// <summary>
    /// Creates the text cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The rowDataIndex.</param>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    private Cell CreateTextCell(string header, UInt32 index, object text)
    {

        Cell c = new Cell { DataType = CellValues.InlineString, CellReference = header + index };

        InlineString istring = new InlineString();

        Text t = new Text { Text = Convert.ToString(text) };

        istring.Append(t);

        c.Append(istring);

        return c;

    }

    /// <summary>
    /// Creates the number cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The rowDataIndex.</param>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    private Cell CreateNumberCell(string header, UInt32 index, object number)
    {

        Cell c = new Cell
        {
            CellReference = header + index,
            DataType = CellValues.Number
        };

        CellValue v = new CellValue
        {
            Text = Convert.ToString(number),
            // DataType = CellValues.Number,
        };

        c.Append(v);

        return c;

    }

    /// <summary>
    /// Creates the date cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The rowDataIndex.</param>
    /// <param name="number">The date.</param>
    /// <returns></returns>
    private Cell CreateDateCell(string header, UInt32 index, object date)
    {

        Cell c = new Cell
        {
            CellReference = header + index,
            DataType = CellValues.Date
        };

        CellValue v = new CellValue
        {
            Text = Convert.ToString(date),
            // DataType = CellValues.Number,
        };

        c.Append(v);

        return c;

    }

    /// <summary>
    /// Creates the date cell.
    /// </summary>
    /// <param name="header">The header.</param>
    /// <param name="index">The rowDataIndex.</param>
    /// <param name="number">The date.</param>
    /// <returns></returns>
    private Cell CreateBooleanCell(string header, UInt32 index, object boolVal)
    {

        Cell c = new Cell
        {
            CellReference = header + index,
            DataType = CellValues.Boolean
        };

        CellValue v = new CellValue
        {
            Text = Convert.ToString(boolVal),
            // DataType = CellValues.Number,
        };

        c.Append(v);

        return c;

    }

    #region DeleteWorksheet()

    /// <summary>
    /// Deletes the A work sheet.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="sheetToDelete">The sheet to delete.</param>
    public void DeleteWorkSheet(string fileName, string sheetToDelete)
    {
        string Sheetid = "";
        //Open the workbook
        using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, true))
        {
            WorkbookPart wbPart = document.WorkbookPart;

            // Get the pivot Table Parts
            IEnumerable<PivotTableCacheDefinitionPart> pvtTableCacheParts = wbPart.PivotTableCacheDefinitionParts;
            Dictionary<PivotTableCacheDefinitionPart, string> pvtTableCacheDefinationPart = new Dictionary<PivotTableCacheDefinitionPart, string>();
            foreach (PivotTableCacheDefinitionPart Item in pvtTableCacheParts)
            {
                PivotCacheDefinition pvtCacheDef = Item.PivotCacheDefinition;
                //Check if this CacheSource is linked to SheetToDelete
                var pvtCahce = pvtCacheDef.Descendants<CacheSource>().Where(s => s.WorksheetSource.Sheet == sheetToDelete);
                if (pvtCahce.Count() > 0)
                {

                    pvtTableCacheDefinationPart.Add(Item, Item.ToString());
                }
            }
            foreach (var Item in pvtTableCacheDefinationPart)
            {
                wbPart.DeletePart(Item.Key);
            }
            //Get the SheetToDelete from workbook.xml
            Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetToDelete).FirstOrDefault();
            if (theSheet == null)
            {
                // The specified sheet doesn't exist.
            }
            //Store the SheetID for the reference
            Sheetid = theSheet.SheetId;

            // Remove the sheet reference from the workbook.
            WorksheetPart worksheetPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));
            theSheet.Remove();

            // Delete the worksheet part.
            wbPart.DeletePart(worksheetPart);

            //Get the DefinedNames
            var definedNames = wbPart.Workbook.Descendants<DefinedNames>().FirstOrDefault();
            if (definedNames != null)
            {
                foreach (DefinedName Item in definedNames)
                {
                    // This condition checks to delete only those names which are part of Sheet in question
                    if (Item.Text.Contains(sheetToDelete + "!"))
                        Item.Remove();
                }
            }
            // Get the CalculationChainPart
            //Note: An instance of this part type contains an ordered set of references to all cells in all worksheets in the
            //workbook whose value is calculated from any formula

            CalculationChainPart calChainPart;
            calChainPart = wbPart.CalculationChainPart;
            if (calChainPart != null)
            {
                var calChainEntries = calChainPart.CalculationChain.Descendants<CalculationCell>().Where(c => c.SheetId == Sheetid);
                foreach (CalculationCell Item in calChainEntries)
                {
                    Item.Remove();
                }
                if (calChainPart.CalculationChain.Count() == 0)
                {
                    wbPart.DeletePart(calChainPart);
                }
            }

            // Save the workbook.
            wbPart.Workbook.Save();
        }
    }

    #endregion

    #region ClearWorkSheetData

    /// <summary>
    /// Deletes the A work sheet.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="sheetToDelete">The sheet to delete.</param>
    public void ClearWorkSheetData(string fileName, string sheetToClear)
    {
        string Sheetid = "";
        //Open the workbook
        using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, true))
        {
            WorkbookPart wbPart = document.WorkbookPart;

            // Get the pivot Table Parts
            IEnumerable<PivotTableCacheDefinitionPart> pvtTableCacheParts = wbPart.PivotTableCacheDefinitionParts;
            Dictionary<PivotTableCacheDefinitionPart, string> pvtTableCacheDefinationPart = new Dictionary<PivotTableCacheDefinitionPart, string>();
            foreach (PivotTableCacheDefinitionPart Item in pvtTableCacheParts)
            {
                PivotCacheDefinition pvtCacheDef = Item.PivotCacheDefinition;
                //Check if this CacheSource is linked to SheetToDelete
                var pvtCahce = pvtCacheDef.Descendants<CacheSource>().Where(s => s.WorksheetSource.Sheet == sheetToClear);
                if (pvtCahce.Count() > 0)
                {

                    pvtTableCacheDefinationPart.Add(Item, Item.ToString());
                }
            }
            foreach (var Item in pvtTableCacheDefinationPart)
            {
                wbPart.DeletePart(Item.Key);
            }
            //Get the SheetToDelete from workbook.xml
            Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetToClear).FirstOrDefault();
            if (theSheet == null)
            {
                // The specified sheet doesn't exist.
            }
            //Store the SheetID for the reference
            Sheetid = theSheet.SheetId;

            // Remove the sheet reference from the workbook.
            WorksheetPart worksheetPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));

            Worksheet workSheet = worksheetPart.Worksheet;
            SheetData sheetData = workSheet.GetFirstChild<SheetData>();

            for (int childIndex = 1; childIndex < sheetData.ChildElements.Count; childIndex++)
            {
                sheetData.RemoveChild(sheetData.ChildElements[childIndex]);
            }

            IEnumerable<Row> rows = sheetData.Descendants<Row>();

            List<Row> rowsList = rows.ToList();

            //rowsList.RemoveRange(1, rowsList.Count - 1);

            // Save the workbook.
            wbPart.Workbook.Save();
        }
    }

    #endregion

    #region Create(string fileSavePath, DataSet dataSet)

    /// <summary>
    /// Saves the specified file save path.
    /// </summary>
    /// <param name="fileSavePath">The file save path.</param>
    /// <param name="dataSet">The data set.</param>
    public void Create(string fileSavePath, DataSet dataSet)
    {

        Dictionary<string, List<OpenXmlElement>> inputDictionary = ToSheets(dataSet);

        Create(fileSavePath, inputDictionary);
        // inputDictionary
    }
    public void Create(Stream stream, DataSet dataSet)
    {

        Dictionary<string, List<OpenXmlElement>> inputDictionary = ToSheets(dataSet);

        Create(stream, inputDictionary);
        // inputDictionary
    }
    private void Create(Stream stream, Dictionary<String, List<OpenXmlElement>> sets)
    {
        using (SpreadsheetDocument package = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookpart = package.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());

            foreach (KeyValuePair<String, List<OpenXmlElement>> set in sets)
            {
                WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetpart.Worksheet = new Worksheet(new SheetData(set.Value));
                worksheetpart.Worksheet.Save();

                Sheet sheet = new Sheet()
                {
                    Id = workbookpart.GetIdOfPart(worksheetpart),
                    SheetId = (uint)(sheets.Count() + 1),
                    Name = set.Key
                };
                sheets.AppendChild(sheet);
            }
            workbookpart.Workbook.Save();
        }
    }
    /// <summary>
    /// Creates the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="sets">The sets.</param>
    private void Create(string path, Dictionary<String, List<OpenXmlElement>> sets)
    {
        using (SpreadsheetDocument package = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
        {
            WorkbookPart workbookpart = package.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());

            foreach (KeyValuePair<String, List<OpenXmlElement>> set in sets)
            {
                WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetpart.Worksheet = new Worksheet(new SheetData(set.Value));
                worksheetpart.Worksheet.Save();

                Sheet sheet = new Sheet()
                {
                    Id = workbookpart.GetIdOfPart(worksheetpart),
                    SheetId = (uint)(sheets.Count() + 1),
                    Name = set.Key
                };
                sheets.AppendChild(sheet);
            }
            workbookpart.Workbook.Save();
        }
    }

    /// <summary>
    /// Toes the sheets.
    /// </summary>
    /// <param name="ds">The ds.</param>
    /// <returns></returns>
    private Dictionary<string, List<OpenXmlElement>> ToSheets(DataSet ds)
    {
        return
        (from dt in ds.Tables.OfType<DataTable>()
         select new
         {
             // Sheet Name
             Key = dt.TableName,
             Value = (
                 // Sheet Columns
             new List<OpenXmlElement>(
             new OpenXmlElement[]
{
new Row(
from d in dt.Columns.OfType<DataColumn>()
select (OpenXmlElement)new Cell()
{
CellValue = new CellValue(d.ColumnName),
DataType = CellValues.String
})
})).Union
                 // Sheet Rows
             ((from dr in dt.Rows.OfType<DataRow>()
               select ((OpenXmlElement)new Row(from dc in dr.ItemArray
                                               select (OpenXmlElement)new Cell()
                                               {
                                                   CellValue = new CellValue(dc.ToString()),
                                                   DataType = CellValues.String
                                               })))).ToList()
         }).ToDictionary(p => p.Key, p => p.Value);
    }

    #endregion

    #region Read

    /// <summary>
    /// Reads the specified file save path.
    /// </summary>
    /// <param name="fileSavePath">The file save path.</param>
    /// <returns></returns>
    public DataSet Read(string fileSavePath)
    {

        DataSet resultSet = new DataSet();

        using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileSavePath, false))
        {

            WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
            IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

            foreach (Sheet sheet in sheets)
            {
                DataTable dt = new DataTable();

                string relationshipId = sheet.Id.Value;
                string sheetName = sheet.SheetId;
                dt.TableName = sheet.SheetId;

                WorksheetPart worksheetPart =
                (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                List<Row> rowsList = new List<Row>();

                rowsList = rows.ToList();

                //Start from 1, first row is header.
                for (int iterRowIndex = 1; iterRowIndex < rowsList.Count; iterRowIndex++) //this will also include your header row...
                {
                    Row row = rowsList[iterRowIndex];

                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }

                    dt.Rows.Add(tempRow);
                }

                resultSet.Tables.Add(dt);
            }

        }

        return resultSet;
    }

    /// <summary>
    /// Gets the cell value.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="cell">The cell.</param>
    /// <returns></returns>
    public static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        string value = cell.CellValue != null ? cell.CellValue.InnerXml : string.Empty;

        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
        }
        else
        {
            return value;
        }
    }

    #endregion
}
