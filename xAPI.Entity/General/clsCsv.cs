using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace xAPI.Entity.General
{
    public class clsCsv
    {
        public Boolean CreateCSVFromGenericList<T>(List<T> list, String xfile)
        {
            Boolean xResult = false;

            try
            {
                if (list == null || list.Count == 0) return false;

                Type t = list[0].GetType();
                string newLine = Environment.NewLine;

                using (var sw = new StreamWriter(File.Open(xfile, FileMode.Create), Encoding.UTF8))
                {
                    object o = Activator.CreateInstance(t);
                    PropertyInfo[] props = o.GetType().GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        sw.Write(pi.Name.ToUpper() + ",");
                    }
                    sw.Write(newLine);

                    foreach (T item in list)
                    {
                        foreach (PropertyInfo pi in props)
                        {
                            string whatToWrite =
                                Convert.ToString(item.GetType()
                                                     .GetProperty(pi.Name)
                                                     .GetValue(item, null))
                                    .Replace(',', ' ') + ',';

                            sw.Write(whatToWrite);

                        }
                        sw.Write(newLine);
                    }

                }

                xResult = true;
            }
            catch (System.Exception ex)
            {

            }

            return xResult;
        }

        public Boolean CreateCSVFromDataTable(DataTable dt, String xfile)
        {
            Boolean xResult = false;

            try
            {
                if (dt == null || dt.Rows.Count == 0) return false;

                string newLine = Environment.NewLine;
                if (!Directory.Exists(Path.GetDirectoryName(xfile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(xfile));
                using (var sw = new StreamWriter(File.Open(xfile, FileMode.Create), Encoding.UTF8))
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sw.Write(dt.Columns[i].ColumnName + ",");
                    }
                    sw.Write(newLine);

                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string whatToWrite =
                                Convert.ToString(dt.Rows[r][i].ToString())
                                    .Replace(",","");

                            if (i + 1 != dt.Columns.Count)
                            {
                                whatToWrite = whatToWrite + ',';
                            }

                            sw.Write(whatToWrite);
                        }
                        sw.Write("{0}",newLine);
                    }
                }
                xResult = true;
            }
            catch (System.Exception ex)
            {

            }

            return xResult;
        }

        public void toCSV(DataTable dt, String FileName, HttpResponse response)
        {
            StringBuilder sbdlr = new StringBuilder();
            if (dt.Columns.Count != 0)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    sbdlr.Append(col.ColumnName + ',');
                }
                sbdlr.Append("\r\n");
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        sbdlr.Append(row[column].ToString() +',');
                    }
                    sbdlr.Append("\r\n");
                }
            }
            HttpResponse httpResponse = response;
            httpResponse.Clear();
            httpResponse.ContentType = "Application/x-msexcel";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\""+FileName+"\"");
            httpResponse.Write(sbdlr);
            httpResponse.End();
            

        }
    }
}
