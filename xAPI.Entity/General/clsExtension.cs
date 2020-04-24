using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace xAPI.Entity.General
{
    /// <summary>
    /// Clase que Contiene Todos los metodos extensores.
    /// </summary>
    public static class clsExtension
    {
        /// <summary>
        /// Clase para Deserializar un String en formato Plain Old XML en una clase especificada.
        /// </summary>
        /// <typeparam name="T">Clase a Convertir.</typeparam>
        /// <param name="xmlSerializado">String en Formato Plain Old XML.</param>
        /// <returns>Data Casteada en la Clase indicada.</returns>
        public static T DeserializarTo<T>(this string xmlSerializado)
        {
            try
            {
                var serializer = new DataContractSerializer(typeof(T));
                using (var backing = new System.IO.StringReader(xmlSerializado))
                using (var reader = new System.Xml.XmlTextReader(backing))
                {
                    object obj = serializer.ReadObject(reader);
                    return (T)obj;
                }
            }
            catch { return default(T); }
        }
        /// <summary>
        /// Clase para Deserializar un String en formato Plain Old XML en un DataTable.
        /// </summary>
        /// <param name="xmlSerializado">String en Formato Plain Old XML.</param>
        /// <returns>Retorna DataTable</returns>
        public static DataTable DeserializarToDataTable(this string xmlSerializado)
        {
            DataTable dt = new DataTable();
            try
            {
                String lista = xmlSerializado.DeserializarTo<String>();
                StringReader reader = new StringReader(lista);
                DataSet ds = new DataSet();
                ds.ReadXml(reader, XmlReadMode.ReadSchema);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch { dt = null; }
            return dt;
        }
        /// <summary>
        /// Clase para Serializar un Objeto a String
        /// </summary>
        /// <param name="Obj">Objeto a serializar</param>
        /// <param name="ObjType">Tipo de Objeto a Serializar</param>
        /// <returns>String en formato XML</returns>
        public static string SerializeToXml(object Obj, System.Type ObjType)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlSerializer ser = new XmlSerializer(ObjType);
            ser.Serialize(writer, Obj);
            return sb.ToString();
        }
        public static String ToStringDecimal(this double text)
        {
            return text.ToString("N", CultureInfo.InvariantCulture);
        }
        public static string ToStringDecimal(this String text)
        {
            return text.Replace(".", ",");
          
        }
        public static String ToStringDecimal(this Decimal text)
        {
            return text.ToString("N", CultureInfo.InvariantCulture);
        }
  
        public static String ToStringDecimal2(this Decimal text)
        {
            return text.ToString("G", CultureInfo.InvariantCulture);
        }
        public static String ToStringInteger(this Decimal text)
        {
            return text.ToString("N0", CultureInfo.InvariantCulture);
        }
        public static Decimal ToDecimalString(this String text)
        {
            return Convert.ToDecimal(text, CultureInfo.InvariantCulture);
        }

        public static String ToStringDecimal(this float text)
        {
            return text.ToString("N2", CultureInfo.InvariantCulture);
        }

        public static String ToStringFixed(this Decimal text)
        {
            return text.ToString("F", CultureInfo.InvariantCulture);
        }
        public static String ToStringFixed(this Int64 text)
        {
            return text.ToString("F", CultureInfo.InvariantCulture);
        }

        public static string ToStringDateTime(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the record.</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static string ToStringDate(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static string ToStringFormatDate(this DateTime date)
        {
            return date.ToString("MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        }

        public static T GetSession<T>(string namesession)
        {
            T obj = default(T);
            try
            {
                if (HttpContext.Current != null)
                {
                    obj = (T)HttpContext.Current.Session[namesession];
                }
                return (T)obj;

            }
            catch { return default(T); }
        }        

        #region DataExtensions

        //Those static methods extends SqlDataReader in order to avoid DBNull exceptions on casting types
        //on Data Access Layer:

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the record.</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this SqlDataReader record, string columnName)
        {
            return GetColumnValue<T>(record, columnName, default(T));
        }


        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the record</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">The value to return if the column contains a <value>DBNull.Value</value> value.</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this SqlDataReader objDr, String columnName, T defaultValue)
        {
            try
            {
                object value = objDr[columnName];
                if (value == null || value == DBNull.Value)
                {
                    return defaultValue;
                }
                else
                {
                    if (defaultValue == null && objDr[columnName] is DateTime)
                    {
                        DateTime date = (DateTime)value;
                        object sdate = date.ToString("MM/dd/yyyy");
                        return (T)sdate;
                    }
                    else

                        return (T)value;
                }
            }
            catch (Exception ex)
            {

                return defaultValue;
            }
           
        }

        #endregion  



        #region Image Extensions

        public static bool ImageResizer(this Image image, int width, int height, string route, string fileName)
        {
            bool isSuccess = false;
            try
            {
                Decimal scaleFactor = 1.00m;
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                while (newWidth > width && newHeight > height)
                {
                    scaleFactor -= 0.01m;
                    newWidth = (int)(image.Width * scaleFactor);
                    newHeight = (int)(image.Height * scaleFactor);
                }

                var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);

                if (!Directory.Exists(route))
                    Directory.CreateDirectory(route);

                thumbnailBitmap.Save((route + fileName), image.RawFormat);

                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                //image.Dispose();

                isSuccess = true;
            }
            catch (Exception ex) { }

            return isSuccess;
        }
        
        #endregion


        public static String ToStringFromRow(this object row)
        {
            if (row is DBNull)
            {
                return String.Empty;
            }
            return row.ToString();
        }

        public static String ToStringDateFromRow(this object row)
        {
            if (row is DBNull)
            {
                return String.Empty;
            }
            else
            {
                try
                {
                    return Convert.ToDateTime(row).ToString("MM/dd/yyyy");
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }
            }
        }
    }    
}
