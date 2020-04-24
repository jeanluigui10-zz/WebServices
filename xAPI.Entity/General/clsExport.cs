using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace xAPI.Entity.General
{
    public class clsExport
    {

        public static clsExcel ExcelExport(DataTable dt, string fileName, string resourcePath)
        {
            if (!Directory.Exists(resourcePath))
                Directory.CreateDirectory(resourcePath);
            clsExcel objExcel = new clsExcel();
            return objExcel;
        }
        //public static clsCsv CSVExport(DataTable dt, string fileName, string resourcePath)
        //{
        //    if (!Directory.Exists(resourcePath))
        //        Directory.CreateDirectory(resourcePath);
        //    clsCsv objCSV = new clsCsv();
        //    return objCSV;
        //}






        /// <summary>
        /// CREATE AND SAVE PDF FILE
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static bool iTextSharpCreatePdfFile(DataTable dt, string fileName,float[] columnsWidth,HttpResponse Response)
        {
            bool isSuccess = false;
            try
            {
             
                Document document = new Document();
                MemoryStream mStream = new MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream((resourcePath + "\\" + fileName), FileMode.Create));
                PdfWriter writer = PdfWriter.GetInstance(document, mStream);
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font4 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 11);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                PdfPRow row = null;
                //float[] columnsWidth = new float[] { 4f, 4f, 4f, 4f, 4f, 4f };

                table.SetWidths(columnsWidth);
                table.WidthPercentage = 100;

                //int iCol = 0;
                //string colname = "";
                //PdfPCell cell = new PdfPCell(new Phrase("Products"));
                //cell.Colspan = dt[0].Columns.Count;

                foreach (DataColumn c in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(c.ColumnName, font5));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                foreach (DataRow r in dt.Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(r[0].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[1].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[2].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[3].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[4].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(r[5].ToString(), font4));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                Response.Clear();
                Response.BinaryWrite(mStream.ToArray());
                Response.End();

                isSuccess = true;
            }
            catch (Exception ex) {  }

            return isSuccess;
        }
    }
}
