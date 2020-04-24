using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Threading;
using System.Data;
using System.IO;
using System.Web;
using System.Security.AccessControl;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;

namespace xAPI.Entity.General
{
    public class clsExcel
    {
        //public void ExportExcel(System.Data.DataTable dt, String savePath)
        //{
        //    if (dt == null || dt.Rows.Count == 0)
        //        return;
        //    Application xlApp = new Application();

        //    if (xlApp == null)
        //    {
        //        return;
        //    }

        //    CultureInfo CurrentCI = Thread.CurrentThread.CurrentCulture;
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //    Workbook workbooks = xlApp.Workbooks;
        //    Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];
        //    Page range;
        //    long totalCount = dt.Rows.Count;
        //    long rowRead = 0;
        //    float percent = 0;
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
        //        range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
        //        range.Interior.ColorIndex = 15;
        //        range.Font.Bold = true;
        //    }
        //    for (int r = 0; r < dt.Rows.Count; r++)
        //    {
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
        //        }
        //        rowRead++;
        //        percent = ((float)(100 * rowRead)) / totalCount;
        //    }
        //    //Autoajustar columnas
        //    worksheet.Columns.AutoFit();
        //    //Abrir excel
        //    xlApp.Visible = false;

        //    //workbook.Saved = true;            
        //    //workbook.SaveCopyAs(savePath);  

        //    //workbook.SaveAs(savePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);           

        //    workbook.Close(true, Type.Missing, Type.Missing);
        //    workbook = null;
        //    xlApp.Quit();
        //    xlApp = null;

        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();

        //}


        //Fila limitada de la versión anterior de Excel por hoja
        const int rowLimit = 65000;

        private string getWorkbookTemplate()
        {
            var sb = new StringBuilder();
            sb.Append("<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
            sb.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n	xmlns:x=\"urn:schemas- microsoft-com:office:excel\"\r\n	xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n");
            sb.Append(" <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>");
            sb.Append("\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n	<Protection/>\r\n </Style>\r\n	<Style ss:ID=\"BoldColumn\">\r\n <Font ");
            sb.Append("x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n <Interior ss:Color=\"#DFD8DF\" ss:Pattern=\"Solid\"/> </Style>\r\n <Style ss:ID=\"s62\">\r\n <NumberFormat");
            sb.Append(" ss:Format=\"@\"/>\r\n </Style>\r\n<Style ss:ID=\"s23\"><Interior ss:Color=\"#800080\" ss:Pattern=\"Solid\"/></Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.0000\"/>\r\n </Style>\r\n ");
            sb.Append("<Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n </Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ");
            sb.Append("ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n <Style ss:ID=\"s28\">\r\n");
            sb.Append("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Top\" ss:ReadingOrder=\"LeftToRight\" ss:WrapText=\"1\"/>\r\n");
            sb.Append("<Font x:CharSet=\"1\" ss:Size=\"9\" ss:Color=\"#808080\" ss:Underline=\"Single\"/>\r\n");
            sb.Append("<Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/> </Style>\r\n</Styles>\r\n {0}</Workbook>");
            return sb.ToString();
        }

        private string replaceXmlChar(string input)
        {
            input = input.Replace("&", "&");
            input = input.Replace("<", "<");
            input = input.Replace(">", ">");
            input = input.Replace("\"", "\"");
            input = input.Replace("'", "'");
            return input;
        }

        private string getWorksheets(DataSet source)
        {
            var sw = new StringWriter();
            if (source == null || source.Tables.Count == 0)
            {
                sw.Write("<Worksheet ss:Name=\"Sheet1\"><Table><Row> <Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data> </Cell></Row></Table></Worksheet>");
                return sw.ToString();
            }
            foreach (System.Data.DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                    sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\"><Table><Row><Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
                else
                {
                    //Escribe por cada fila
                    var sheetCount = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((i % rowLimit) == 0)
                        {
                            //Agrega etiquetas de cierre para la hoja anterior de la misma tabla de datos
                            if ((i / rowLimit) > sheetCount)
                            {
                                sw.Write("</Table></Worksheet>");
                                sheetCount = (i / rowLimit);
                            }
                            sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\"><Table>");
                            //Escribir fila de la columna Nombre
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write("<Column ss:AutoFitWidth=\"1\"/ ss:Width=\"100\">");
                            sw.Write("<Row>");
                            foreach (DataColumn dc in dt.Columns)
                                sw.Write(string.Format("<Cell ss:StyleID=\"BoldColumn\">	<Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
                            sw.Write("</Row>\r\n");
                        }
                        sw.Write("<Row>\r\n");
                        foreach (DataColumn dc in dt.Columns)
                            sw.Write(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dt.Rows[i][dc.ColumnName].ToString())));
                        sw.Write("</Row>\r\n");
                    }
                    sw.Write("</Table></Worksheet>");
                }
            }

            return sw.ToString();
        }
        public string GetExcelXml(System.Data.DataTable dtInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            var worksheets = getWorksheets(ds);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public string GetExcelXml(DataSet dsInput, string filename)
        {
            var excelTemplate = getWorkbookTemplate();
            var worksheets = getWorksheets(dsInput);
            var excelXml = string.Format(excelTemplate, worksheets);
            return excelXml;
        }

        public void ToExcel(DataSet dsInput, string filename, HttpResponse response)
        {
            var excelXml = GetExcelXml(dsInput, filename);
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            //response.ContentType = "application/vnd.xls";
            // response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AppendHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(filename));
            response.Write(excelXml);
            response.Flush();
            response.End();
        }
        //public String ToExcel(System.Data.DataTable dtInput, string filename, HttpResponse response)
        //{
        //    var ds = new DataSet();
        //    ds.Tables.Add(dtInput.Copy());
        //    ToExcel(ds, filename, response);
        //}
        public String ToExcelOledb(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            var ds = new DataSet();
            ds.Tables.Add(dtInput.Copy());
            // ToExcel(ds, filename, response);

            String msg = String.Empty;

            //var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            string directory = filename.Split('\\').First();
            //var directory = new DirectoryInfo(@"C:\");
            String filname = filename.Split('.').First();
            filname = filname.Split('\\').Last();
            String extension = filename.Split('.').Last();
            Int32 counter = 1;
            String newFullPath = String.Empty;
            if (File.Exists(directory.ToString() + "\\" + filname + "." + extension))
            {
                do
                {
                    String newFile = String.Format("{0}({1}).{2}", filname, counter, extension);
                    newFullPath = Path.Combine(directory.ToString() + "\\", newFile);
                    filename = newFullPath;
                    counter++;
                } while (File.Exists(newFullPath));
            }
            else
            {
                filename = String.Empty;
                filename = directory.ToString() + "\\" + filname + "." + extension;

            }

            System.Data.OleDb.OleDbConnection MyConnection = null;
            try
            {
                System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                string sql = null;
                String sqltable = null;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filename + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                myCommand.Connection = MyConnection;
                String columt = String.Empty;
                sqltable = "CREATE TABLE [Sheet1]({0})";

                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    columt += dtInput.Columns[j].ColumnName + " " + "varchar(250)";
                    if (j < dtInput.Columns.Count - 1)
                        columt += ",";

                }
                myCommand.CommandText = string.Format(sqltable, columt);
                myCommand.ExecuteNonQuery();

                sql = "Insert into [Sheet1]({0})values({1})";
                String column = String.Empty;
                String values = String.Empty;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    column += dtInput.Columns[j].ColumnName;
                    if (j < dtInput.Columns.Count - 1)
                        column += ",";

                }
                for (int i = 0; i < dtInput.Rows.Count; i++)
                {
                    for (int j = 0; j < dtInput.Columns.Count; j++)
                    {
                        values += "'" + dtInput.Rows[i][j].ToString() + "'";
                        if (j < dtInput.Columns.Count - 1)
                            values += ",";
                    }

                    myCommand.CommandText = string.Format(sql, column, values);
                    myCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //response.Redirect("www.google.com");
            }
            finally
            {
                MyConnection.Close();
                //  transmit(filename, response);
            }
            return msg;
        }


        public void ToExcelXListDataTableWebMethod(List<System.Data.DataTable> dtInput, string filename, HttpResponse response, string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            //var ws = workbook.Worksheets.Add(dtInput.TableName);
            var ws = workbook.Worksheets.Add(title);
            ws.Cell(2, 2).SetValue(title);
            int contador = 0;
            for (int m = 0; m < dtInput.Count; m++)
            {
                //  ws.Cell(contador+2, 2).SetValue("probando");
                for (int j = 0; j < dtInput[m].Columns.Count; j++)
                {
                    int c = j + 2;
                    ws.Cell(contador + 3, c).SetValue(dtInput[m].Columns[j].ColumnName.ToString());
                }
                for (int i = 0; i < dtInput[m].Rows.Count; i++)
                {
                    int r = i + contador + 4;
                    for (int j = 0; j < dtInput[m].Columns.Count; j++)
                    {
                        int c = j + 2;
                        if (!dtInput[m].Rows[i][j].ToString().Equals("EMPTY"))
                            ws.Cell(r, c).SetValue(dtInput[m].Rows[i][j]);
                    }
                }
                var rngTable = ws.Range(contador + 2, 2, dtInput[m].Rows.Count + 3 + contador, dtInput[m].Columns.Count + 1);
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                var rngHeaders = ws.Range(contador + 3, 2, contador + 3, dtInput[m].Columns.Count + 1);
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

                rngTable.Cell(1, 1).Style.Font.Bold = true;
                rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngTable.Row(1).Merge();
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Columns(contador + 2, dtInput[m].Columns.Count + 1).AdjustToContents();

                contador = 3 + contador + dtInput[m].Rows.Count;


            }


            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }


        public void ToExcelXLTWO(List<System.Data.DataTable> dtInput, string filename, HttpResponse response, string title)
        {
           
            XLWorkbook workbook = new XLWorkbook();

            for (int m = 0; m < dtInput.Count; m++)
            {
                var ws = workbook.Worksheets.Add(title);
                ws.Cell(2, 2).SetValue(title.ToString());
                for (int j = 0; j < dtInput[m].Columns.Count; j++)
                {
                    int c = j + 2;
                    ws.Cell(3, c).SetValue(dtInput[m].Columns[j].ColumnName.ToString());
                }
                for (int i = 0; i < dtInput[m].Rows.Count; i++)
                {
                    int r = i + 4;
                    for (int j = 0; j < dtInput[m].Columns.Count; j++)
                    {
                        int c = j + 2;
                        if (!dtInput[m].Rows[i][j].ToString().Equals("EMPTY"))
                            ws.Cell(r, c).SetValue(dtInput[m].Rows[i][j].ToString());
                    }
                }
                var rngTable = ws.Range(2, 2, dtInput[m].Rows.Count + 3, dtInput[m].Columns.Count + 1);
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                var rngHeaders = ws.Range(3, 2, 3, dtInput[m].Columns.Count + 1);
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

                rngTable.Cell(1, 1).Style.Font.Bold = true;
                rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngTable.Row(1).Merge();
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Columns(2, dtInput[m].Columns.Count + 1).AdjustToContents();
            }
            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        public void ToExcelXListDataTable(List<System.Data.DataTable> dtInput, string filename, HttpResponse response,string title)
        {
            XLWorkbook workbook = new XLWorkbook();
            //var ws = workbook.Worksheets.Add(dtInput.TableName);
            var ws = workbook.Worksheets.Add(title);
            ws.Cell(2, 2).SetValue(title);
            int contador = 0;
            for (int m = 0; m < dtInput.Count; m++) {
               //  ws.Cell(contador+2, 2).SetValue("probando");
                for (int j = 0; j < dtInput[m].Columns.Count; j++)
                {
                    int c = j + 2;
                    ws.Cell(contador+3, c).SetValue(dtInput[m].Columns[j].ColumnName.ToString());
                }
                for (int i = 0; i < dtInput[m].Rows.Count; i++)
                {
                    int r = i + contador+4;
                    for (int j = 0; j < dtInput[m].Columns.Count; j++)
                    {
                        int c = j + 2;
                        if (!dtInput[m].Rows[i][j].ToString().Equals("EMPTY"))
                            ws.Cell(r, c).SetValue(dtInput[m].Rows[i][j]);
                    }
                }
                var rngTable = ws.Range(contador+2, 2, dtInput[m].Rows.Count + 3 + contador, dtInput[m].Columns.Count + 1);
                rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                var rngHeaders = ws.Range(contador+3, 2, contador+3, dtInput[m].Columns.Count + 1);
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

                rngTable.Cell(1, 1).Style.Font.Bold = true;
                rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                rngTable.Row(1).Merge();
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Columns(contador+2, dtInput[m].Columns.Count + 1).AdjustToContents();
                          
                    contador = 3+contador+dtInput[m].Rows.Count;
                
            
            }
     

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }
        public void ToExcelXL(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(dtInput.TableName);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }
            }
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 3, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();   

        }

        public void ToExcelXL_Rv2(System.Data.DataTable dtInput, string filename, HttpResponse response)
        {
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(dtInput.TableName);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }
            }
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 3, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua; //Header 2

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Font.FontColor = XLColor.White;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;//Header 1
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            // Prepare the response
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");

            //// Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();

        }

        public void ToExcelXLV2(List<string> sendTo, System.Data.DataTable dtInput, string filename, HttpResponse response, string path, string action)
        {
            string table_name = dtInput.TableName.ToString().Replace('/', ' ');
            XLWorkbook workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(table_name);
            ws.Cell(2, 2).SetValue(dtInput.TableName.ToString());
            for (int j = 0; j < dtInput.Columns.Count; j++)
            {
                int c = j + 2;
                ws.Cell(3, c).SetValue(dtInput.Columns[j].ColumnName.ToString());
            }
            for (int i = 0; i < dtInput.Rows.Count; i++)
            {
                int r = i + 4;
                for (int j = 0; j < dtInput.Columns.Count; j++)
                {
                    int c = j + 2;
                    if (!dtInput.Rows[i][j].ToString().Equals("EMPTY"))
                        ws.Cell(r, c).SetValue(dtInput.Rows[i][j].ToString());
                }
            }
            var rngTable = ws.Range(2, 2, dtInput.Rows.Count + 3, dtInput.Columns.Count + 1);
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            var rngHeaders = ws.Range(3, 2, 3, dtInput.Columns.Count + 1);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Row(1).Merge();
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            ws.Columns(2, dtInput.Columns.Count + 1).AdjustToContents();

            if (!action.Equals(""))
            {
                workbook.SaveAs(path + filename);
                if (!action.Equals("RUN"))
                {
                    clsSendEmail.SendEmailAsyncV2(sendTo, "This is a preview file", "Preview Order Export", path + filename, filename);
                }
               
            }
            else
            {
                // Prepare the response (download the file)
                HttpResponse httpResponse = response;
                httpResponse.Clear();
                httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                httpResponse.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
                                
                //// Flush the workbook to the Response.OutputStream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);  // for showing the file 
                    memoryStream.WriteTo(httpResponse.OutputStream);
                    memoryStream.Close();
                }

                httpResponse.End();
            }
        }

    }
}