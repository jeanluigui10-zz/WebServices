using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using xAPI.Entity.Base;
using xAPI.Entity.General;

namespace xWebServices
{
    public partial class CallService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void DecryptInput()
        {
            BaseEntity Base = new BaseEntity();
            txtResponse.Text = HttpUtility.UrlEncode(Encryption.Encrypt(HttpUtility.UrlDecode(txtRequest.Text)));
        }


        protected void btnRequest_Click(Object sender, EventArgs e)
        {
            try
            {
               DecryptInput();
               
            }
            catch (Exception exception)
            {
            }
        }
      
        private static string webService(String s, String url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(s);
                request.ContentLength = data.Length;
                request.Timeout = 500000; 
                request.ContentType = "application/json";
                request.Method = "POST";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return result;
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    return reader.ReadToEnd();
                }
            }
        }

    }
}
