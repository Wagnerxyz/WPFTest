﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExcelTest
{
    public interface IExcelExporterWithHeader
    {
        /// <summary>
        /// 将DataTable导出到Excel，DataTable的名称作为sheet页名称，如果指定的Excel文件存在，则在已有Excel文件中增加sheet页，如果已存在同名的sheet页，则覆盖sheet页
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <remarks>建议传入DataTable命名表名以生成指定的Sheet名</remarks>
        void Export(DataTable table, string excelFilePath, string headerText);
        /// <summary>
        /// 将数据导出到Excel，如果指定的Excel文件存在，则在已有Excel文件中增加sheet页，如果已存在同名的sheet页，则覆盖sheet页
        /// </summary>
        /// <param name="columnNames">所有列名称</param>
        /// <param name="rows">数据行</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="headerText">Sheet名字</param>
        /// 
        void Export(string[] columnNames, IList<string[]> rows, string excelFilePath, string sheetname, string headerText);
        /// <summary>
        /// 使用ExportSession导出到Excel，如果指定的Excel文件存在，则在已有Excel文件中增加sheet页，如果已存在同名的sheet页，则覆盖sheet页
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <returns></returns>
        IExportSession Export(string excelFilePath);
    }

}
