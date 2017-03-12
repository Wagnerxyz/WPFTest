using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DataTable = System.Data.DataTable;
using System.Collections.Generic;

namespace ExcelTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SqlConnectionStringBuilder _cnStrBuilder;
        private DataSet _dataSet;
        private DataTable tesTable;
        private DataTable _readerTable;
        ExcelUtils excelUtils = new ExcelUtils();

        EPPlusExporter exporter = new EPPlusExporter();
        private string[] columns = { "AA", "BBB", "CCC", "DDD" };
        private List<string[]> list = new List<string[]>
        {
                new[] { "2", "2" }, new[] { "2", "2222232222222222222222" ,"33","123"}, new[] { "2", "2" }, new[] { "2", "2" }, 
                new[] { "2", "2", "3", "4" }, new[] { "2", "2" }, new[] { "2", "2", "3", "ASA" }, new[] { "2", "2" }, 
                new[] { "2", "2", "3", "4" }, new[] { "2", "2" }, new[] { "2", "2", "3", "qwe" }, new[] { "2", "2" },
                new[] { "2", "2", "3", "5" }
            };

        public MainWindow()
        {
            InitializeComponent();
            _cnStrBuilder = new SqlConnectionStringBuilder();
            _cnStrBuilder.InitialCatalog = "AdventureWorks2012";
            _cnStrBuilder.DataSource = @".";
            _cnStrBuilder.ConnectTimeout = 10;
            _cnStrBuilder.IntegratedSecurity = true;
        }
        private void Test(object sender, RoutedEventArgs e)
        {

            tesTable.TableName = DateTime.Now.Second.ToString();
        //    CreateExcelFile.CreateExcelDocument(CreateDummyDataSet(1, 60000, 20).Tables[0], @"c:\test.xlsx");
          //  FileStream fs = new FileStream(@"c:\openxmlexporttable.xlsx", FileMode.Create); fs.Dispose();
          //  tesTable.TableName = DateTime.Now.ToString();
            OpenXMLExporter openXMLExporter = new OpenXMLExporter(); 
            var sw = Stopwatch.StartNew();
         //   openXMLExporter.ExportSAX(tesTable, @"c:\openxmlexporttableSAX.xlsx"); //50wan 1:59  1:57
            openXMLExporter.Export(tesTable, @"c:\openxmlexporttable.xlsx","asadasd"); //50wan 1:59  1:57
           // openXMLExporter.ExportStream(tesTable, fs); //50wan 1:33 2:27
          
          //  openXMLExporter.Export(columns, list, @"c:\openxmlexportarray.xlsx", "SSDAD" + DateTime.Now.ToString());
           // openXMLExporter.Export(@"c:\fluentinterface.xlsx", "AAA", columns).AddRows(list).Finish();
            TimeSpan elapsed1 = sw.Elapsed;
            MessageBox.Show(elapsed1.ToString());
        }

        private void NPOI(object sender, RoutedEventArgs e)
        {
            // DataSet dummyDataSet = CreateDummyDataSet(1, 5, 3);
            NPOIExporter npoi = new NPOIExporter();
            var sw = Stopwatch.StartNew();
            npoi.Export(tesTable, @"c:\npoiexporttable.xlsx", "This is a Header");
            TimeSpan elapsed1 = sw.Elapsed;

            GC.Collect();
            sw.Restart();
            TimeSpan elapsed2 = sw.Elapsed;
            DataTableRenderToExcel.RenderDataTableToExcel(tesTable, @"c:\RenderDataTableToExcel.xlsx");
            GC.Collect();
            sw.Restart();
            TimeSpan elapsed3 = sw.Elapsed;
            GC.Collect();
            sw.Restart();
            NPOIHelper.Export(tesTable, "AAAA", @"c:\npoihelper.xlsx");
            TimeSpan elapsed4 = sw.Elapsed;
            MessageBox.Show(elapsed1 + "----" + elapsed2 + "---" + elapsed3 + "---" + elapsed4);

        }
        private void EPPExport_OnClick(object sender, RoutedEventArgs e)
        {
            //if (_dataSet == null)
            //{
            //    MessageBox.Show("数据集为空");
            //    return;
            //}
            var sw = Stopwatch.StartNew();

            // IExportSession exportSession = exporter.Export(@"c:\expresion.xlsx", "aAA", columns);
            // exportSession.AddRows(list).Finish();

            exporter.Export(tesTable, @"c:\EPPHelper1GenerateExcel.xlsx"); //50万行 1分24秒 3分24秒//100万3分57 4分26 2分58 2.5G RAM
            TimeSpan elapsed1 = sw.Elapsed;
            MessageBox.Show(elapsed1.ToString());
        }
        private void OpenXml_Click(object sender, RoutedEventArgs e)
        {
            //  DataSet dummySet = CreateDummyDataSet();
            if (_dataSet == null)
            {
                return;
            }

            ////dataset多个datatable，产生多sheet 
            //for (int i = 0; i < 3; i++)
            //{
            //    DataTable dataTable = tesTable.Copy();
            //    dataTable.TableName = dataTable.TableName + i;
            //    _dataSet.Tables.Add(dataTable);
            //}

            CreateExcelFile createExcelFile = new CreateExcelFile();
            var sw = Stopwatch.StartNew();
            excelUtils.Create(@"c:\ExcelUtils.xlsx", _dataSet);//12万行39s 50万行2分35秒 2分45
            TimeSpan elapsed1 = sw.Elapsed;
            GC.Collect();

            sw.Restart();
            //  exporter.Export(tesTable, @"c:\EPPHelper1GenerateExcel.xlsx"); //100万行 2分
            //  createExcelFile.CreateExcelDocument(_dataSet, @"c:\CreateExcelFile.CreateExcelDocument.xlsx");//12万行33s 50万行2分22秒 1分28
            TimeSpan elapsed2 = sw.Elapsed;

            //  GC.Collect();
            //  sw.Restart();
            //   OpenXMLExcel.ExportDataSet(_dataSet, "c:\\OpenXMLExcel.ExportDataSet.xlsx");//12万行33s 50万行2分04秒 2分46 100万行 2分51秒 3分40秒 3分14秒 4.6G
            TimeSpan elapsed3 = sw.Elapsed;


            MessageBox.Show(elapsed1 + "-------" + elapsed2 + "------" + elapsed3);
        }

        private void GetFromDB(object sender, RoutedEventArgs e)
        {
            // Create an open a connection.
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = _cnStrBuilder.ConnectionString;
                    cn.Open();
                    string strSQL = "Select  * From Sales.SalesOrderDetail";
                    if (tb.Text != String.Empty)
                    {
                        strSQL = "Select top " + tb.Text + " * From Sales.SalesOrderDetail";
                    }
                    _dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(strSQL, cn);
                    // adapter.FillSchema(_dataSet, SchemaType.Mapped);
                    adapter.Fill(_dataSet);
                    tesTable = _dataSet.Tables[0];

                    // return _dataSet.Tables[0];
                    //500000行 翻番
                    for (int i = 0; i < 2; i++)
                    {
                        tesTable.Merge(tesTable.Copy());
                    }
                    dg.ItemsSource = tesTable.DefaultView;
                    tb.Text = tesTable.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private DataSet CreateDummyDataSet(int table, int row, int column)
        {
            if (table < 1 || row < 1 || column < 1)
            {
                MessageBox.Show("input createdummydataset number error");
                return null;
            }
            DataSet resultSet = new DataSet();

            for (int tableIndex = 0; tableIndex < table; tableIndex++)
            {
                DataTable dataTable = new DataTable("Table" + tableIndex);

                for (int colIndex = 0; colIndex < column; colIndex++)
                {
                    if (colIndex % 2 == 0)
                    {
                        dataTable.Columns.Add("Column" + colIndex, Type.GetType("System.Decimal"));
                    }
                    else
                    {
                        dataTable.Columns.Add("Column" + colIndex, Type.GetType("System.String"));
                    }
                }

                for (int rowIndex = 0; rowIndex < row; rowIndex++)
                {
                    DataRow dataRow = dataTable.NewRow();

                    for (int colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                    {
                        if (colIndex % 2 == 0)
                        {
                            dataRow[dataTable.Columns[colIndex].ColumnName] = (rowIndex * (colIndex + 1)) + colIndex;
                        }
                        else
                        {
                            dataRow[dataTable.Columns[colIndex].ColumnName] = "STR : " + (rowIndex * (colIndex + 1)) +
                                                                              colIndex;
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
                resultSet.Tables.Add(dataTable);
            }

            return resultSet;
        }
        private void GCollect(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void TableNameChange(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbname.Text))
            {
                MessageBox.Show("InpuT!!!!");
                return;
            }
            tesTable.TableName = tbname.Text;
        }

        private void SAX(object sender, RoutedEventArgs e)
        {
            using (SpreadsheetDocument xl = SpreadsheetDocument.Create("c:\\LargeFile.xlsx", SpreadsheetDocumentType.Workbook))
            {
               // List<OpenXmlAttribute> oxa;
                OpenXmlWriter writer;

                xl.AddWorkbookPart();
                WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                writer = OpenXmlWriter.Create(wsp);
                writer.WriteStartElement(new Worksheet());
                writer.WriteStartElement(new SheetData());

                for (int i = 1; i <= 50000; ++i)
                {
                //    oxa = new List<OpenXmlAttribute>();
                    // this is the row index
                //    oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                    writer.WriteStartElement(new Row());
                //    oxw.WriteStartElement(new Row(), oxa);

                    for (int j = 1; j <= 10; ++j)
                    {
                     //   oxa = new List<OpenXmlAttribute>();
                        // this is the data type ("t"), with CellValues.String ("str")
                      //  oxa.Add(new OpenXmlAttribute("t", null, "str"));

                        // it's suggested you also have the cell reference, but
                        // you'll have to calculate the correct cell reference yourself.
                        // Here's an example:
                        //oxa.Add(new OpenXmlAttribute("r", null, "A1"));

                    //    oxw.WriteStartElement(new Cell(), oxa);

                        writer.WriteElement(new CellValue(string.Format("R{0}C{1}", i, j)));

                        // this is for Cell
                   //     oxw.WriteEndElement();
                    }

                    // this is for Row
                    writer.WriteEndElement();
                }

                // this is for SheetData
            //    oxw.WriteEndElement(); 
                //oxw.WriteElement(new Sheet()
                //{
                //    Name = "Sheet1",
                //    SheetId = 1,
                //    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                //});
                // this is for Worksheet
         //       oxw.WriteEndElement();
          //      oxw.Close();

                //oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                //oxw.WriteStartElement(new Workbook());
                //oxw.WriteStartElement(new Sheets());

                // you can use object initialisers like this only when the properties
                // are actual properties. SDK classes sometimes have property-like properties
                // but are actually classes. For example, the Cell class has the CellValue
                // "property" but is actually a child class internally.
                // If the properties correspond to actual XML attributes, then you're fine.
             

                // this is for Sheets
                writer.WriteEndElement();
                // this is for Workbook
                writer.WriteEndElement();
                writer.Close();

            //    xl.Close();
            }
        }
    }
}

