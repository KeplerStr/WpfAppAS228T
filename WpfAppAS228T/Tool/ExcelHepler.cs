using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppAS228T.Tool
{
    class ExcelHepler
    {

        private List<double> m_loadValue = new List<double>();
        private List<double>[] m_testValue = new List<double>[4];
        public ExcelHepler()
        {
            string filePath = $"{Environment.CurrentDirectory}" + "/report/模板.xlsx";

            string newFile = $"{Environment.CurrentDirectory}" + "/report/20211109.xlsx";
            ReadFromExcelFile(filePath);



            //this.m_testValue.Add = 0;
            //this.m_testValue[0].Add = 200;
            //this.m_testValue[0].Add = 300;
            //this.m_testValue[0].Add = 400;
            //this.m_testValue[0].Add = 600;
            //this.m_testValue[0].Add = 800;



            if (wb != null)
            {
                WriteToExcel(newFile);
            }       
        }

       

        private IWorkbook wb = null;

        public void ReadFromExcelFile(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            try
            {
                FileStream fs = File.OpenRead(filePath);

                fs.Position = 0;
                if (extension.Equals(".xls"))
                {
                    wb = new HSSFWorkbook(fs);
                }
                else
                {
                    wb = new XSSFWorkbook(fs);
                }
                fs.Close();

                ISheet sheet = wb.GetSheetAt(1);
                IRow row = sheet.GetRow(0);

                //int offset = 0;
                for (int i = 0; i < sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    if (row != null)
                    {
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            if (row.GetCell(j) != null)  // 进行判断
                            {
                                string value;
                                if (row.GetCell(j).CellType == CellType.Formula) // 判断公式
                                {
                                    //row.GetCell(j).SetCellType(CellType.String); // 转换成值
                                    value = row.GetCell(j).StringCellValue;
                                }
                                else
                                {
                                    value = row.GetCell(j).ToString();
                                }
                                Console.Write(value.ToString() + " ");
                            }
                        }
                        Console.WriteLine("\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public object GetCellValue(ICell cell)
        {
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date comes here
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Formula:
                            value = cell.CellFormula;
                            break;
                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                value = "";
            }
            return value;
        }

        public static void SetCellValue(ICell cell, object obj)
        {
            if (obj.GetType() == typeof(int))
            {
                cell.SetCellValue((int)obj);
            }
            else if (obj.GetType() == typeof(double))
            {
                cell.SetCellValue((double)obj);
            }
            else if (obj.GetType() == typeof(IRichTextString)) 
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj.GetType() == typeof(bool))
            {
                cell.SetCellValue((bool)obj);
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }

        public void WriteToExcel(string filePath) //写入一个新的表格中
        {
            ISheet sheet = wb.GetSheetAt(1);
            IRow row;
            ICell cell;

            for (int i = 4; i < 4 + this.m_testValue[0].Count; i++)
            {
                row = sheet.GetRow(i);
                cell = row.GetCell(1);
                if (cell != null)
                {
                    SetCellValue(cell, this.m_testValue[0][i - 4]);
                }
            }
            FileStream fsW = File.Create(filePath);
            fsW.Position = 0;
            wb.Write(fsW);
            fsW.Close();

            ////根据指定的文件格式创建对应的类
            //FileStream fsread = File.OpenRead(filePath);
            //if (extension.Equals(".xls"))
            //{
            //    wb = new HSSFWorkbook(fsread);
            //}
            //else
            //{
            //    wb = new XSSFWorkbook(fsread);
            //}

            //fsread.Close();

            //ISheet sheet = wb.GetSheetAt(1);
            //IRow row;
            //ICell cell;

            //for (int i = 4; i < 10; i++)
            //{
            //    row = sheet.GetRow(i);
            //    cell = row.GetCell(1);
            //    if (cell != null)
            //    {
            //        SetCellValue(cell, (double)(i-4)*50);
            //    }
            //}

            //try
            //{
            //    FileStream fswrite = File.OpenWrite(filePath);
            //    wb.Write(fswrite);
            //    fswrite.Close();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}
        }
    }
}
