using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace xAPI.Entity.General
{
    public static class clsUtilities
    {

        static Regex regularExpressionEmail = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        
        
        public static Boolean CardValidation(String card)
        {
            Boolean validate = false;
            card = card.Trim();
            try
            {
                Int32 card_len = card.Length; /* card length */
                int count; /* a counter */
                int weight; /* weight to apply to digit being checked */
                int sum; /* sum of weights */
                int digit; /* digit being checked */
                Int32 mod;
                weight = 2;
                sum = 0;
                for (count = card_len - 2; count >= 0; count = count - 1)
                {
                    digit = weight * (Convert.ToInt32(card[count].ToString()));
                    sum = sum + (digit / 10) + (digit % 10);
                    if (weight == 2)
                        weight = 1;
                    else
                        weight = 2;
                }
                mod = (10 - sum % 10) % 10;
                if (Convert.ToInt32(card[card.Length - 1].ToString()) == mod)
                    validate = true;
                else
                    validate = false;
            }
            catch (Exception)
            {

                validate = false;
            }

            return validate;
        }

        /// <summary>
        /// Generate a random number.
        /// </summary>
        /// <returns>String of six random digits.</returns>
        public static string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public static String ValidateAndReplace(String text, String replace)
        {
            String value = "";
            if (String.IsNullOrEmpty(text))
            {
                if (String.IsNullOrEmpty(replace))
                {
                    value = "";
                }
                else
                {
                    value = replace.Trim();
                }

            }
            else
            {
                value = text.Trim();
            }
            return value;
        }

        //public static String Get(String type, String key, CultureInfo culture)
        //{
        //    String text = (String)HttpContext.GetGlobalResourceObject(type, key);

        //    //String value = "";
        //    if (String.IsNullOrEmpty(text))
        //    {
        //        value = replace;

        //    }
        //    else
        //        value = text;
        //    return value;
        //}



        #region Image
          /// <summary>
          /// Method to resize, convert and save the image.
          /// </summary>   
          public static void ResizeImage(string filePath, int maxWidth, int maxHeight)
          {
              try
              {
                  if (!File.Exists(filePath))
                  {
                      return;
                  
                  }

                  int quality = byte.MaxValue;
                  Bitmap image = new Bitmap(filePath, true);
                  // Get the image's original width and height
                  int originalWidth = image.Width;
                  int originalHeight = image.Height;
                  if (image.Width <= maxWidth && image.Height <= maxHeight)
                  {
                      return;
                  }

                  // To preserve the aspect ratio
                  float ratioX = (float)maxWidth / (float)originalWidth;
                  float ratioY = (float)maxHeight / (float)originalHeight;
                  float ratio = Math.Min(ratioX, ratioY);

                  // New width and height based on aspect ratio
                  int newWidth = (int)(originalWidth * ratio);
                  int newHeight = (int)(originalHeight * ratio);

                  // Convert other formats (including CMYK) to RGB.
                  Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

                  // Draws the image in the specified size with quality mode set to HighQuality
                  using (Graphics graphics = Graphics.FromImage(newImage))
                  {
                      graphics.CompositingQuality = CompositingQuality.HighQuality;
                      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                      graphics.SmoothingMode = SmoothingMode.HighQuality;
                      graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                  }
                  ImageFormat format = ImageFormat.Jpeg;

                  if (filePath.Contains(".png"))
                      format = ImageFormat.Png;
                  else if (filePath.Contains(".gif"))
                      format = ImageFormat.Gif;
                  else if (filePath.Contains(".bmp"))
                      format = ImageFormat.Bmp;
                  // Get an ImageCodecInfo object that represents the JPEG codec.
                  ImageCodecInfo imageCodecInfo = GetEncoderInfo(format);

                  // Create an Encoder object for the Quality parameter.
                  System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

                  // Create an EncoderParameters object. 
                  EncoderParameters encoderParameters = new EncoderParameters(1);

                  // Save the image as a JPEG file with quality level.
                  EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
                  encoderParameters.Param[0] = encoderParameter;

                  byte[] imageBytes = null;// newImage.ToString(ImageFormat.Bmp);

                  using (MemoryStream ms = new MemoryStream())
                  {
                      newImage.Save(ms, ImageFormat.Jpeg);
                      imageBytes = ms.ToArray();
                  }

                  // Create the full path to the file.
                  // Path.Combine(photoFilePath, strFilename);
                  image.Dispose();
                  string fullPath = filePath;
                  // Write the file.
                  File.WriteAllBytes(fullPath, imageBytes);

                  //newImage.Save(filePath2, imageCodecInfo, encoderParameters);
              }
              catch (Exception)
              {

              }
          }

          /// <summary>
          /// Method to get encoder infor for given image format.
          /// </summary>
          /// <param name="format">Image format</param>
          /// <returns>image codec info.</returns>
          private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
          {
              return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
          }
#endregion


        public static string StripHTML(string source)
          {
              try
              {
                  string result;

                  // Remove HTML Development formatting
                  // Replace line breaks with space
                  // because browsers inserts space
                  result = source.Replace("\r", " ");
                  // Replace line breaks with space
                  // because browsers inserts space
                  result = result.Replace("\n", " ");
                  // Remove step-formatting
                  result = result.Replace("\t", "");
                  // Remove repeating spaces because browsers ignore them
                  result = System.Text.RegularExpressions.Regex.Replace(result, @"( )+", " ");

                  // Remove the header (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*head([^>])*>", "<head>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*head( )*>)", "</head>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(<head>).*(</head>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // remove all scripts (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*script([^>])*>", "<script>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*script( )*>)", "</script>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  //result = System.Text.RegularExpressions.Regex.Replace(result,
                  //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                  //         string.Empty,
                  //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<script>).*(</script>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // remove all styles (prepare first by clearing attributes)
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*style([^>])*>", "<style>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"(<( )*(/)( )*style( )*>)", "</style>",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(<style>).*(</style>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert tabs in spaces of <td> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*td([^>])*>", "\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert line breaks in places of <BR> and <LI> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*br( )*>", "\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*li( )*>", "\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // insert line paragraphs (double line breaks) in place
                  // if <P>, <DIV> and <TR> tags
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*div([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*tr([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<( )*p([^>])*>", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // Remove remaining tags like <a>, links, images,
                  // comments etc - anything that's enclosed inside < >
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"<[^>]*>", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // replace special characters:
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @" ", " ",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&bull;", " * ",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&lsaquo;", "<",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&rsaquo;", ">",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&trade;", "(tm)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&frasl;", "/",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&lt;", "<",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&gt;", ">",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&copy;", "(c)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&reg;", "(r)",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove all others. More can be added, see
                  // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           @"&(.{2,6});", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // for testing
                  //System.Text.RegularExpressions.Regex.Replace(result,
                  //       this.txtRegex.Text,string.Empty,
                  //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                  // make line breaking consistent
                  result = result.Replace("\n", "\r");

                  // Remove extra line breaks and tabs:
                  // replace over 2 breaks with 2 and over 4 tabs with 4.
                  // Prepare first to remove any whitespaces in between
                  // the escaped characters and remove redundant tabs in between line breaks
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)( )+(\r)", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\t)( )+(\t)", "\t\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\t)( )+(\r)", "\t\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)( )+(\t)", "\r\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove redundant tabs
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)(\t)+(\r)", "\r\r",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Remove multiple tabs following a line break with just one tab
                  result = System.Text.RegularExpressions.Regex.Replace(result,
                           "(\r)(\t)+", "\r\t",
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                  // Initial replacement target string for line breaks
                  string breaks = "\r\r\r";
                  // Initial replacement target string for tabs
                  string tabs = "\t\t\t\t\t";
                  for (int index = 0; index < result.Length; index++)
                  {
                      result = result.Replace(tabs, "\t\t\t\t");
                      breaks = breaks + "\r";
                      tabs = tabs + "\t";
                  }

                  // That's it.
                  return result;
              }
              catch
              {
                  return null;
              }
          }

        #region WebRequest Calls

        public static string WebResquestGet(string url, string contentType = "application/json; charset=utf-8")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url/*Config.UrlApiAutoship.Replace("{0}", "Autoship_GetProductByMarket?marketId=") + marketId*/);
                request.Method = "GET";
                request.ContentType = contentType/*"application/json; charset=utf-8"*/;
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex) 
            {
                if (ex != null)
                {
                    WebResponse errorResponse = ex.Response;
                    if (errorResponse != null)
                    {
                        using (Stream responseStream = errorResponse.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            //throw new Exception(reader.ReadToEnd());
                        }
                    }
                }
            }

            return null;
        }

        public static string WebResquestPost(string url, string s)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.UTF8.GetBytes(s);
                request.ContentLength = data.Length;
                request.Timeout = 500000;//20 SECONDS
                request.ContentType = "application/json";
                request.Method = "POST";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return result;
                //txtResponse.Text = result;
            }
            catch (WebException ex)
            {
                //strRespSession = ex.Message;
                //isCorrect = false;
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    throw new Exception(reader.ReadToEnd());
                }
            }
        }

        #endregion


        public static String Call_ws_xPos_POST(string _uri, string _parmjson)
          {
                String _resultjson = "";
                try
                {
                    WebRequest wr = WebRequest.Create(_uri);
                    wr.Method = "POST";
                    wr.ContentType = "application/json";
                    wr.ContentLength = _parmjson.Length;
                    var requestStream = wr.GetRequestStream();

                    byte[] postBytes = Encoding.UTF8.GetBytes(_parmjson);
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();

                    // grab te response and print it out to the console along with the status code
                    var response = (HttpWebResponse)wr.GetResponse();
             
                    using (var rdr = new StreamReader(response.GetResponseStream()))
                    {
                        _resultjson = rdr.ReadToEnd();
                    }


                }
                catch (Exception ex)
                {

                    throw ex;
                }        

              return _resultjson;
          }


        public static String Call_ws_xPos_GET(string _uri){

              String _resultjson = "";
              
                  try
                  {
                      WebRequest wr = WebRequest.Create(_uri);
                      //wr.Method = "POST";
                      //wr.ContentType = "application/json";
                      //wr.ContentLength = _parmjson.Length;
                      //var requestStream = wr.GetRequestStream();

                      //byte[] postBytes = Encoding.UTF8.GetBytes(_parmjson);
                      //requestStream.Write(postBytes, 0, postBytes.Length);
                      //requestStream.Close();

                      // grab te response and print it out to the console along with the status code
                      var response = (HttpWebResponse)wr.GetResponse();  
                      using (var rdr = new StreamReader(response.GetResponseStream()))
                      {
                          _resultjson = rdr.ReadToEnd();
                      }


                  }
                  catch (Exception)
                  {

                  }

                  return _resultjson;
          }

        public static String EmailFormatReport(String Template
            , String FullName = ""           
            , String Code = ""
            , String Number = ""
            , String Domain = ""
            , String Address1 = ""
            , String Address2 = ""
            , String City = ""
            , String Country = ""
            , String Email = ""
            , String Phone = ""
            , String Date = ""
            , String Representative = ""
            , String Detail = ""
            , String Card = ""
            , String Balance = ""
            , String Subtotal = ""
            , String Shipping = ""
            , String Total = ""
            , String Type = ""
            , String Sponsor_name = ""
            , String Sponsor_lastname = ""
            , String Sponsor_phone = ""
            , String Taxes = ""
            , String SAddress1 = ""
            , String SAddress2 = ""
            , String SCity = ""
            , String SCountry = ""
            )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;
                    srEmail = srEmail.Replace("{FullName}", !String.IsNullOrEmpty(FullName) ? FullName : "");                 
                    srEmail = srEmail.Replace("{Code}", !String.IsNullOrEmpty(Code) ? Code : "");
                    srEmail = srEmail.Replace("{Number}", !String.IsNullOrEmpty(Number) ? Number : "");
                    if (Type == "join")
                    {
                        srEmail = srEmail.Replace("{Domain}", !String.IsNullOrEmpty(Domain) ? Domain : "");
                    }                    

                    srEmail = srEmail.Replace("{Address1}", !String.IsNullOrEmpty(Address1) ? Address1 : "-");
                    srEmail = srEmail.Replace("{Address2}", !String.IsNullOrEmpty(Address2) ? Address2 : "-");
                    srEmail = srEmail.Replace("{City}", !String.IsNullOrEmpty(City) ? City : "-");
                    srEmail = srEmail.Replace("{Country}", !String.IsNullOrEmpty(Country) ? Country : "-");

                    srEmail = srEmail.Replace("{SAddress1}", !String.IsNullOrEmpty(SAddress1) ? SAddress1 : "-");
                    srEmail = srEmail.Replace("{SAddress2}", !String.IsNullOrEmpty(SAddress2) ? SAddress2 : "-");
                    srEmail = srEmail.Replace("{SCity}", !String.IsNullOrEmpty(SCity) ? SCity : "-");
                    srEmail = srEmail.Replace("{SCountry}", !String.IsNullOrEmpty(SCountry) ? SCountry : "-");

                    srEmail = srEmail.Replace("{Email}", !String.IsNullOrEmpty(Email) ? Email : "");
                    srEmail = srEmail.Replace("{Phone}", !String.IsNullOrEmpty(Phone) ? Phone : "");
                    srEmail = srEmail.Replace("{Date}", !String.IsNullOrEmpty(Date) ? Date : "");

                    srEmail = srEmail.Replace("<tr><td></td><td colspan=\"9\">{Detail}</td></tr>", !String.IsNullOrEmpty(Detail) ? Detail : "");
                    srEmail = srEmail.Replace("{Card}", !String.IsNullOrEmpty(Card) ? Card : "");
                    srEmail = srEmail.Replace("{Balance}", !String.IsNullOrEmpty(Balance) ? Balance : "0.00");
                    srEmail = srEmail.Replace("{Taxes}", !String.IsNullOrEmpty(Taxes) ? Taxes : "0.00");
                    srEmail = srEmail.Replace("{Subtotal}", !String.IsNullOrEmpty(Subtotal) ? Subtotal : "0.00");
                    srEmail = srEmail.Replace("{Shipping}", !String.IsNullOrEmpty(Shipping) ? Shipping : "0.00");
                    srEmail = srEmail.Replace("{Total}", !String.IsNullOrEmpty(Total) ? Total : "0.00");
                }
            }
            catch (Exception) { }
            return srEmail;

        }


        #region Credit Card Validator


           //private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|                               (?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";
             private const string cardRegex = "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<JCB>(?:2131\\d{3})|(?:1800\\{3})|(?:35\\d{3}))|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

           public static string IsValidNumber(string cardNum)
           {
               Regex cardTest = new Regex(cardRegex);

               //Determine the card type based on the number
               CreditCardTypeType? cardType = GetCardTypeFromNumber(cardNum);
               //Call the base version of IsValidNumber and pass the 
               //number and card type
               if (IsValidNumber(cardNum, cardType))
                   return cardType.ToString();
               else
                   return null;
           }

           public static bool IsValidNumber(string cardNum, CreditCardTypeType? cardType)
           {
               //Create new instance of Regex comparer with our 
               //credit card regex pattern
               Regex cardTest = new Regex(cardRegex);

               //Make sure the supplied number matches the supplied
               //card type
               if (cardTest.Match(cardNum).Groups[cardType.ToString()].Success)
               {
                   //If the card type matches the number, then run it
                   //through Luhn's test to make sure the number appears correct
                   if (PassesLuhnTest(cardNum))
                       return true;
                   else
                       //The card fails Luhn's test
                       return false;
               }
               else
                   //The card number does not match the card type
                   return false;
           }

           public static bool PassesLuhnTest(string cardNumber)
           {
               //Clean the card number- remove dashes and spaces
               cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

               //Convert card number into digits array
               int[] digits = new int[cardNumber.Length];
               for (int len = 0; len < cardNumber.Length; len++)
               {
                   digits[len] = Int32.Parse(cardNumber.Substring(len, 1));
               }

               //Luhn Algorithm
               //Adapted from code availabe on Wikipedia at
               //http://en.wikipedia.org/wiki/Luhn_algorithm
               int sum = 0;
               bool alt = false;
               for (int i = digits.Length - 1; i >= 0; i--)
               {
                   int curDigit = digits[i];
                   if (alt)
                   {
                       curDigit *= 2;
                       if (curDigit > 9)
                       {
                           curDigit -= 9;
                       }
                   }
                   sum += curDigit;
                   alt = !alt;
               }

               //If Mod 10 equals 0, the number is good and this will return true
               return sum % 10 == 0;
           }

           public static string GetIDCreditCardType_toGC(string CreditCardNumber)
           {
               Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
               Regex regMaster = new Regex("^5[1-5][0-9]{14}$");
               Regex regExpress = new Regex("^3[47][0-9]{13}$");
               Regex regDiners = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
               Regex regDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
               Regex regJSB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");


               if (regVisa.IsMatch(CreditCardNumber))
                   return "1";//VISA
               if (regMaster.IsMatch(CreditCardNumber))
                   return "3";//MASTER
               if (regExpress.IsMatch(CreditCardNumber))
                   return "2";//AEXPRESS
               if (regDiners.IsMatch(CreditCardNumber))
                   return "132";//DINERS
               if (regDiscover.IsMatch(CreditCardNumber))
                   return "128";//DISCOVERS
               if (regJSB.IsMatch(CreditCardNumber))
                   return "125";//JSB
               return "invalid";
           }

           public static string GetCreditCardType(string CreditCardNumber)
           {
               Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
               Regex regMaster = new Regex("^5[1-5][0-9]{14}$");
               Regex regExpress = new Regex("^3[47][0-9]{13}$");
               Regex regDiners = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
               Regex regDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
               Regex regJSB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");


               if (regVisa.IsMatch(CreditCardNumber))
                   return "VISA";
               if (regMaster.IsMatch(CreditCardNumber))
                   return "MASTER";
               if (regExpress.IsMatch(CreditCardNumber))
                   return "AEXPRESS";
               if (regDiners.IsMatch(CreditCardNumber))
                   return "DINERS";
               if (regDiscover.IsMatch(CreditCardNumber))
                   return "DISCOVERS";
               if (regJSB.IsMatch(CreditCardNumber))
                   return "JSB";
               return "invalid";
           }

           public static string GetTypeByCardNumber(string CreditCardNumber)
           {
               Regex regVisa = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
               Regex regMaster = new Regex("^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
               Regex regExpress = new Regex("^3[47][0-9]{13}$");
               Regex regDiners = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
               Regex regDiscover = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
               Regex regJCB = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");

               //Validate Card Number using Luhn Check
               bool isCorrect = PassesLuhnTest(CreditCardNumber);
               if (!isCorrect) return null;

               //Match with type regex 
               if (regVisa.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.Visa);
               if (regMaster.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.MasterCard);
               if (regExpress.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.Amex);
               if (regDiners.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.DinersClub);
               if (regDiscover.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.Discover);
               if (regJCB.IsMatch(CreditCardNumber))
                   return Convert.ToString(CreditCardTypeType.JCB);
               return null;
           }

           public static CreditCardTypeType? GetCardTypeFromNumber(string cardNum)
           {
               //Create new instance of Regex comparer with our
               //credit card regex patter
               Regex cardTest = new Regex(cardRegex);

               //Compare the supplied card number with the regex
               //pattern and get reference regex named groups
               GroupCollection gc = cardTest.Match(cardNum).Groups;

               //Compare each card type to the named groups to 
               //determine which card type the number matches
               if (gc[CreditCardTypeType.Amex.ToString()].Success)
               {
                   return CreditCardTypeType.Amex;
               }
               else if (gc[CreditCardTypeType.MasterCard.ToString()].Success)
               {
                   return CreditCardTypeType.MasterCard;
               }
               else if (gc[CreditCardTypeType.Visa.ToString()].Success)
               {
                   return CreditCardTypeType.Visa;
               }
               else if (gc[CreditCardTypeType.Discover.ToString()].Success)
               {
                   return CreditCardTypeType.Discover;
               }
               else if (gc[CreditCardTypeType.JCB.ToString()].Success)
               {
                   return CreditCardTypeType.JCB;
               }
               else
               {
                   //Card type is not supported by our system, return null
                   //(You can modify this code to support more (or less)
                   // card types as it pertains to your application)
                   return null;
               }
           }

           public enum CreditCardTypeType
           {
               Visa,
               MasterCard,
               Discover,
               Amex,
               Switch,
               Solo,
               JCB,
               DinersClub
           }


           #endregion

        public static String GetCulturebyCultureBrowser(List<String> List, String ci)
        {
            String ci_db = "";

            if (List != null)
            {
                foreach (String item in List)
                {

                    if (item.ToLower().Contains(ci.ToLower()))
                    {
                        ci_db = item;
                        break;
                    }

                }
                if (String.IsNullOrEmpty(ci_db))
                {
                    String[] array_ci = ci.Split('-');
                    foreach (String item in List)
                    {
                        for (int i = 0; i < array_ci.Length; i++)
                        {
                            if (item.ToLower().Contains(array_ci[i].ToLower()))
                            {
                                ci_db = item;
                                break;
                            }

                        }
                        if (!String.IsNullOrEmpty(ci_db))
                        {
                            break;
                        }

                    }
                }
            }
            if (String.IsNullOrEmpty(ci_db))
            {
                ci_db = ci;
            }
            return ci_db;
        }
        public static String GetInfotraxLanguageCodebyCulture(String lancode)
        {
            lancode = lancode.ToLower();
            String Culture = "en-US";
            switch (lancode)
            {
                case "hr-hr":
                    Culture = "hv";
                    break;
                case "cs-cz":
                    Culture = "cs";
                    break;
                case "da":
                    Culture = "da";
                    break;
                case "nl":
                    Culture = "nl";
                    break;
                case "nl-be":
                    Culture = "nl_bel";
                    break;
                case "en-us":
                    Culture = "en_asea";
                    break;
                case "fi-fi":
                    Culture = "fi";
                    break;
                case "fr-be":
                    Culture = "fr_bel";
                    break;
                case "fr-ca":
                    Culture = "fr_can";
                    break;
                case "fr-fr":
                    Culture = "fr";
                    break;
                case "de-de":
                    Culture = "de";
                    break;
                case "hu-hu":
                    Culture = "hu";
                    break;
                case "it-it":
                    Culture = "it";
                    break;
                case "nn-no":
                    Culture = "no";
                    break;
                case "pt-pt":
                case "pt-br":
                    Culture = "pt";
                    break;
                case "ro":
                    Culture = "ro";
                    break;
                case "sk-sk":
                    Culture = "sk";
                    break;
                case "sl-si":
                    Culture = "sl";
                    break;
                case "es-mx":
                    Culture = "es";
                    break;
                case "es-es":
                    Culture = "es_esp";
                    break;
                case "sv-se":
                    Culture = "sv";
                    break;
                default:
                    Culture = "en_asea";
                    break;

            }

            return Culture;


        }
        public static String GenerateAuthKey_Jixiti(String apikey, String method)
        {
            String signature = "";

            DateTime date = DateTime.Now;
            List<String> lstTime = new List<String>();

            String concat = apikey + Base64Encode(method) + date.ToUniversalTime().ToString("yyyyMMddmm");
            signature = Encryption.Encrypt_Sha1(concat, false);

            return signature;
        }
        public static string Base64Encode(string plainText)
           {
               var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
               return System.Convert.ToBase64String(plainTextBytes);
           }




        public static string ValidateDataRowKey(DataRow dr, string key) 
        {
            try
            {
                return dr[key] is DBNull ? String.Empty : Convert.ToString(dr[key], CultureInfo.InvariantCulture);
            }
            catch (Exception) { return String.Empty; }
        }

        public static string ValidateDataRowDateTimeKey(DataRow dr, string key)
        {
            try
            {
                DateTime tmpDate = default(DateTime);
                string strDate = "";
                if (dr[key] is DateTime)
                {
                    //strDate = Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture).ToStringDateTime();
                    tmpDate = Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture);
                    if (tmpDate == default(DateTime) || tmpDate == DateTime.MinValue || tmpDate == new DateTime(1900, 01, 01))
                    {
                        strDate = String.Empty;
                    }
                    else
                    {
                        strDate = tmpDate.ToStringDateTime();
                    }
                    //else if (Convert.ToDateTime(strDate).ToStringDate() == "01/01/1900")
                    //{
                    //    strDate = String.Empty;
                    //}
                }

                //return dr[key] is DateTime ? Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture).ToStringDateTime() : String.Empty;
                return strDate;
            }
            catch (Exception) { return String.Empty; }
        }

        public static string ValidateDataRowDateKey(DataRow dr, string key)
        {
            try
            {
                DateTime tmpDate = default(DateTime);
                string strDate = "";
                if (dr[key] is DateTime)
                {
                    //strDate = Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture).ToStringDateTime();
                    tmpDate = Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture);
                    if (tmpDate == default(DateTime) || tmpDate == DateTime.MinValue || tmpDate == new DateTime(1900, 01, 01))
                    {
                        strDate = String.Empty;
                    }
                    else
                    {
                        strDate = tmpDate.ToStringDate();
                    }
                    //else if (Convert.ToDateTime(strDate).ToStringDate() == "01/01/1900")
                    //{
                    //    strDate = String.Empty;
                    //}
                }

                //return dr[key] is DateTime ? Convert.ToDateTime(dr[key], CultureInfo.InvariantCulture).ToStringDateTime() : String.Empty;
                return strDate;
            }
            catch (Exception) { return String.Empty; }
        }

        

        public static string ValidateDataRowKeyDefault(DataRow dr, string key, string vdefault)
        {
            try
            {
                return dr[key] is DBNull ? vdefault : Convert.ToString(dr[key], CultureInfo.InvariantCulture);
            }
            catch (Exception) { return vdefault; }
        }

        
        public static String EmailFormatSubject(String Template, String partyName = "")
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PARTY_NAME}", !String.IsNullOrEmpty(partyName) ? partyName : "");


                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailSendFormatParty(String Template, String PersonalMessage = "", String G_FirstName = "", String G_LastName = "",
        String G_Country = "", String G_Phone = "", String G_Address = "", String PTY_Name = "", String PTY_Country = "", String PTY_City = "", String PTY_State = "",
            String PTY_Address = "", String PTY_Start = "", String PTY_End = "", String Hostes_Email = "", String Distributor_RSite = "", String PTY_ID = "", String G_ID = "", String ANS_Y = "", String ANS_M = "", String ANS_N = "", String Password = "", String Siteid = ""
        )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PERSONALMESSAGE}", !String.IsNullOrEmpty(PersonalMessage) ? PersonalMessage : "");
                    srEmail = srEmail.Replace("{G_FIRSTNAME}", !String.IsNullOrEmpty(G_FirstName) ? G_FirstName : "");
                    srEmail = srEmail.Replace("{G_LASTNAME}", !String.IsNullOrEmpty(G_LastName) ? G_LastName : "");
                    srEmail = srEmail.Replace("{G_COUNTRY}", !String.IsNullOrEmpty(G_Country) ? G_Country : "");
                    srEmail = srEmail.Replace("{G_PHONE}", !String.IsNullOrEmpty(G_Phone) ? G_Phone : "");
                    srEmail = srEmail.Replace("{G_ADDRESS}", !String.IsNullOrEmpty(G_Address) ? G_Address : "");
                    srEmail = srEmail.Replace("{PTY_NAME}", !String.IsNullOrEmpty(PTY_Name) ? PTY_Name : "");
                    srEmail = srEmail.Replace("{PTY_COUNTRY}", !String.IsNullOrEmpty(PTY_Country) ? PTY_Country : "");
                    srEmail = srEmail.Replace("{PTY_CITY}", !String.IsNullOrEmpty(PTY_City) ? PTY_City : "");
                    srEmail = srEmail.Replace("{PTY_STATE}", !String.IsNullOrEmpty(PTY_State) ? PTY_State : "");
                    srEmail = srEmail.Replace("{PTY_ADDRESS}", !String.IsNullOrEmpty(PTY_Address) ? PTY_Address : "");
                    srEmail = srEmail.Replace("{PTY_START}", !String.IsNullOrEmpty(PTY_Start) ? PTY_Start : "");
                    srEmail = srEmail.Replace("{PTY_END}", !String.IsNullOrEmpty(PTY_End) ? PTY_End : "");
                    srEmail = srEmail.Replace("{HOSTES_EMAIL}", !String.IsNullOrEmpty(Hostes_Email) ? Hostes_Email : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_REPLICATEDSITE}", !String.IsNullOrEmpty(Distributor_RSite) ? Distributor_RSite : "");
                    srEmail = srEmail.Replace("{PTY_ID}", !String.IsNullOrEmpty(PTY_ID) ? PTY_ID : String.Empty);
                    srEmail = srEmail.Replace("{G_ID}", !String.IsNullOrEmpty(G_ID) ? G_ID : String.Empty);
                    srEmail = srEmail.Replace("{ANS_Y}", !String.IsNullOrEmpty(ANS_Y) ? ANS_Y : String.Empty);
                    srEmail = srEmail.Replace("{ANS_M}", !String.IsNullOrEmpty(ANS_M) ? ANS_M : String.Empty);
                    srEmail = srEmail.Replace("{ANS_N}", !String.IsNullOrEmpty(ANS_N) ? ANS_N : String.Empty);
                    srEmail = srEmail.Replace("{HOSTES_PASSWORD}", !String.IsNullOrEmpty(Password) ? Password : String.Empty);
                    srEmail = srEmail.Replace("{SITE_ID}", !String.IsNullOrEmpty(Siteid) ? Siteid : String.Empty);
                }
            }
            catch (Exception) { }
            return srEmail;

        }

        public static String EmailFormatParty(String Template, String PersonalMessage = "", String G_FirstName = "", String G_LastName = "",
         String G_Country = "", String G_Phone = "", String G_Address = "", String PTY_Name = "", String PTY_Country = "", String PTY_City = "", String PTY_State = "",
             String PTY_Address = "", String PTY_Start = "", String PTY_End = "", String Hostes_Email = "", String Distributor_RSite = "", String PTY_ID = "", String G_ID = "", String ANS_Y = "", String ANS_M = "", String ANS_N = "", String Hostess_fname = "", String Hostess_lname = "", String Hostess_phone = "", String PTY_Eventdate = "", String Hostess_id = ""
         )
        {
            String srEmail = "";
            try
            {
                if (!String.IsNullOrEmpty(Template))
                {
                    srEmail = Template;

                    srEmail = srEmail.Replace("{PERSONALMESSAGE}", !String.IsNullOrEmpty(PersonalMessage) ? PersonalMessage : "");
                    srEmail = srEmail.Replace("{G_FIRSTNAME}", !String.IsNullOrEmpty(G_FirstName) ? G_FirstName : "");
                    srEmail = srEmail.Replace("{G_LASTNAME}", !String.IsNullOrEmpty(G_LastName) ? G_LastName : "");
                    srEmail = srEmail.Replace("{G_COUNTRY}", !String.IsNullOrEmpty(G_Country) ? G_Country : "");
                    srEmail = srEmail.Replace("{G_PHONE}", !String.IsNullOrEmpty(G_Phone) ? G_Phone : "");
                    srEmail = srEmail.Replace("{G_ADDRESS}", !String.IsNullOrEmpty(G_Address) ? G_Address : "");
                    srEmail = srEmail.Replace("{PTY_NAME}", !String.IsNullOrEmpty(PTY_Name) ? PTY_Name : "");
                    srEmail = srEmail.Replace("{PTY_COUNTRY}", !String.IsNullOrEmpty(PTY_Country) ? PTY_Country : "");
                    srEmail = srEmail.Replace("{PTY_CITY}", !String.IsNullOrEmpty(PTY_City) ? PTY_City : "");
                    srEmail = srEmail.Replace("{PTY_STATE}", !String.IsNullOrEmpty(PTY_State) ? PTY_State : "");
                    srEmail = srEmail.Replace("{PTY_ADDRESS}", !String.IsNullOrEmpty(PTY_Address) ? PTY_Address : "");
                    srEmail = srEmail.Replace("{PTY_START}", !String.IsNullOrEmpty(PTY_Start) ? PTY_Start : "");
                    srEmail = srEmail.Replace("{PTY_END}", !String.IsNullOrEmpty(PTY_End) ? PTY_End : "");
                    srEmail = srEmail.Replace("{HOSTES_EMAIL}", !String.IsNullOrEmpty(Hostes_Email) ? Hostes_Email : "");
                    srEmail = srEmail.Replace("{DISTRIBUTOR_REPLICATEDSITE}", !String.IsNullOrEmpty(Distributor_RSite) ? Distributor_RSite : "");
                    srEmail = srEmail.Replace("{PTY_ID}", !String.IsNullOrEmpty(PTY_ID) ? PTY_ID : String.Empty);
                    srEmail = srEmail.Replace("{G_ID}", !String.IsNullOrEmpty(G_ID) ? G_ID : String.Empty);
                    srEmail = srEmail.Replace("{ANS_Y}", !String.IsNullOrEmpty(ANS_Y) ? ANS_Y : String.Empty);
                    srEmail = srEmail.Replace("{ANS_M}", !String.IsNullOrEmpty(ANS_M) ? ANS_M : String.Empty);
                    srEmail = srEmail.Replace("{ANS_N}", !String.IsNullOrEmpty(ANS_N) ? ANS_N : String.Empty);
                    srEmail = srEmail.Replace("{HOSTES_FIRSTNAME}", !String.IsNullOrEmpty(Hostess_fname) ? Hostess_fname : "");
                    srEmail = srEmail.Replace("{HOSTES_LASTNAME}", !String.IsNullOrEmpty(Hostess_lname) ? Hostess_lname : "");
                    srEmail = srEmail.Replace("{HOSTES_PHONE}", !String.IsNullOrEmpty(Hostess_phone) ? Hostess_phone : "");
                    srEmail = srEmail.Replace("{ID_INFOTRAX}", !String.IsNullOrEmpty(Hostess_id) ? Hostess_id : "");
                    srEmail = srEmail.Replace("{PTY_EVENTDATE}", !String.IsNullOrEmpty(PTY_Eventdate) ? PTY_Eventdate : "");
                }
            }
            catch (Exception) { }
            return srEmail;

        }






        /// <summary>
        /// TO VALIDATE IF EXITS RESOURCE KEY
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static String ValidateResourceEntry(string text, string replace)
        {
            return String.IsNullOrEmpty(text) ? replace : text;
        }


        public static string GetCardFormatToShow(string cardNumber)
        {
            int cardNumberLength = cardNumber.Length;
            //NO VALID CREDIT CARD LENGTH
            if (cardNumberLength < 13 || cardNumberLength > 16) return null;

            //COMPLETE CHARACTERS WITH *
            string chars = String.Empty;
            for (int i = 4; i < cardNumberLength; i++) chars += "*";

            return chars + cardNumber.Substring(cardNumberLength - 4);
        }


        public static string HTMLToText(string HTMLCode)
        {
            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\n", " ");

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
              , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;", 
       "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p ", "\n<p ");

            // Finally, remove all HTML tags and return plain text
            return System.Text.RegularExpressions.Regex.Replace(
              sbHTML.ToString(), "<[^>]*>", "");
        }


        public static Boolean IsValidEmail(String email)
        {
            return regularExpressionEmail.IsMatch(email.ToLower().Trim());
        }


        //public static Boolean IsPropertyExist(dynamic settings, String name)
        //{
        //    try
        //    {
        //        //return settings.GetType().GetProperty(name) != null;
        //        dynamic value = settings[name];
        //        return true;
        //    }
        //    catch (RuntimeBinderException)
        //    {
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}


        /// <summary>
        /// Get object value by reflection
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns>String</returns>
        public static String GetStringPropDynamic(dynamic obj, string propName, string defaultProp = "")
        {
            String value = "";
            try
            {
                //String value = "";
                //if (IsPropertyExist(obj, propName))
                //{
                //    //dynamic tmpValue = src.GetType().GetProperty(propName);
                //    value = Convert.ToString(obj[propName]);
                //}
                //else
                //{
                //    value = defaultProp;
                //}
                value = Convert.ToString(obj[propName]);
            }
            catch (Exception ex)
            {
                value = defaultProp;
            }
            return value;

        }

        /// <summary>
        /// Get object value by reflection
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        public static T GetPropValue<T>(object src, string propName)
        {
            return (T)src.GetType().GetProperty(propName).GetValue(src, null);
        }



        /// <summary>
        /// Gets an attribute value contained on an xml string
        /// </summary>
        /// <param name="xml">String containing xml</param>
        /// <param name="AttributeName"></param>
        /// <param name="AttributeValueToFind"></param>
        /// <returns></returns>
        public static String GetValueFromXmlString(String xml, String AttributeName, String AttributeValueToFind)
        {
            String idhost = "0";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // XmlNode node = xmlDoc.DocumentElement.FirstChild;
            XmlNodeList elemList = xmlDoc.GetElementsByTagName("data");
            XmlNode node = elemList[0].FirstChild;
            XmlNodeList lststruct = node.ChildNodes;
            for (int i = 0; i < lststruct.Count; i++)
            {

                if (lststruct[i].Attributes[AttributeName].Value == AttributeValueToFind)
                {
                    XmlNodeList lstId = lststruct[i].ChildNodes;
                    for (int j = 0; j < lstId.Count; j++)
                    {
                        idhost = lstId[j].InnerText;
                    }

                }
                //String node = lststruct[i].InneAttributeValueToFindrXml;
            }
            return idhost;
        }

        public static T GetDataRowDefaultValue<T>(DataRow dr, string fieldName)
        {
            try { return GetDefaultValue<T>(dr[fieldName]); }
            catch (Exception ex) { return default(T); }
        }


        public static T GetDefaultValue<T>(object value)
        {
            if (value is DBNull || String.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (typeof(T) == typeof(String)) return (T)(object)String.Empty;

                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static String GeneratePublicName(Int32 Id)
        {
            String publicName = Id.ToString();

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 50; i++)
                publicName += (Char)random.Next(65, 90);

            return publicName;
        }
    }
}
