using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using xAPI.Entity.Base;

namespace xAPI.Entity.General
{
    public static class clsSendEmail
    {

        public static bool SendInvitationPartie(string mailTo)
        {
            string text = "<p>Thank your for your Request. We will let you know when the party has been scheduled.</p><br>";
            return SendEmail(mailTo, text, "Thank your for your Request.");
        }

        public static bool SendEmailSupport(string mailTo,string subject,string text)
        {
            StringBuilder mail = new StringBuilder();
            try
            {
                mail.AppendLine(String.Format("Error al enviar email.Los detalles son:"));
                mail.AppendLine(String.Format("<br>"));
                mail.AppendLine(String.Format("To:<br>"));
                mail.AppendLine(String.Format(mailTo));
                mail.AppendLine(String.Format("<br>"));
                mail.AppendLine(String.Format("Subject:<br>"));
                mail.AppendLine(String.Format(subject));
                mail.AppendLine(String.Format("<br>"));
                mail.AppendLine(String.Format("Mensaje:<br>"));
                mail.AppendLine(String.Format(text));
                mail.AppendLine(String.Format("<br>"));
                mail.AppendLine(String.Format("Hora de envio:"));
                mail.AppendLine(String.Format(DateTime.Now.ToString()));
            }
            catch (Exception)
            {         
            }
            return SendEmailAsyncSupport(mail.ToString(), "Error al enviar email");
        }
       
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            MailView token = (MailView)e.UserState;
            BaseEntity ent = new BaseEntity();
            Boolean isCancelled = false;
         
            if (e.Cancelled)
            {
                isCancelled = true;
            }
            if (e.Error != null)
            {
                //Debug.WriteLine(token.subject + "  " + e.Error.ToString() + "  Error");
                for (int i = 0; i < token.mailTo.Count; i++)
                {
                    //clsSendEmail.SendEmailAsync("andres_13_83@hotmail.com"
                    //                , "holaaaaaaa"
                    //                , "hola mundo", "noreply@aseamail.com");
                    SendEmailSupport(token.mailTo[i],  token.subject, token.message);
                    
                  //  clsUtilities.RegisterFailDomain("", "", "LogEMAIL - 1", "ERROR - 1");
                }
            }
            else
            {
                //Debug.WriteLine(token.subject + "Enviado!!!!!!!!!!!!!!!!!");
                for (int i = 0; i < token.mailTo.Count; i++)
                {
                  
                    //clsUtilities.RegisterFailDomain("", "", "LogEMAIL - 2", "ERROR - 1");
                }
            }
        }

        class MailView
        {
            public MailView()
            {

            }
            public MailView(String message, String subject, String mailFrom)
            {
                //this.mailTo = mailTo;
                this.message = message;
                this.subject = subject;
                this.mailFrom = mailFrom;
            }
            public List<String> mailTo { get; set; }
            public String message { get; set; }
            public String subject { get; set; }
            public String mailFrom { get; set; }
        }

        public static Boolean SendEmailAsync(string mailTo, string message, string subject, string mailFrom_pseudo = "")//ok
        {
            try
            {
                BaseEntity Base= new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                    //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function


                    //  = obj.EmailSender;
                    // = obj.SMTP;
                    //  = obj.Port;
                    // = obj.Username;
                    //   Encryption.Decrypt(obj.Password);
                    //string mailFrom = "noreply@aseamail.com";
                    string mailFrom = server.EmailSender.ToString();
                    //string mailFrom = "lpacheco@xirectss.com";
                    //""
                    //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                    string mailServer = server.SMTP.ToString();
                    //string mailServer = "'smtp.gmail.com";
                    //host204.hostmonster.com
                    // int mailPort = 25; //465//  = obj.Port;
                    int mailPort = Convert.ToInt32(server.Port);
                    //int mailPort = 587; //465

                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                    // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

                    //string mailFromPassword = "1234.Pass";
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification")), new System.Net.Mail.MailAddress(mailTo));

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)
                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = false;
                    // smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);
                    //smtp.Credentials = new System.Net.NetworkCredential("azure_2e14ddfec7f3b79d60cc6f44dc6724f8@azure.com", "Z3Nn0@M4!l!x$$");

                    //SendCompletedEventHandler a = new SendCompletedEventHandler();

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);

                    obj.mailTo = new List<string>();
                    obj.mailTo.Add(mailTo);
                    //string userState = "Message 1";
                    smtp.SendAsync(insMail, obj);
                    //smtp.Send(insMail);
                    return true;
                }

                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean SendEmailAsyncListToEmail(List<string> mailTo, string message, string subject, string mailFrom_pseudo = "")//ok
        {
            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                    //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function


                    //  = obj.EmailSender;
                    // = obj.SMTP;
                    //  = obj.Port;
                    // = obj.Username;
                    //   Encryption.Decrypt(obj.Password);
                    //string mailFrom = "noreply@aseamail.com";
                    string mailFrom = server.EmailSender.ToString();
                    //string mailFrom = "lpacheco@xirectss.com";
                    //""
                    //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                    string mailServer = server.SMTP.ToString();
                    //string mailServer = "'smtp.gmail.com";
                    //host204.hostmonster.com
                    // int mailPort = 25; //465//  = obj.Port;
                    int mailPort = Convert.ToInt32(server.Port);
                    //int mailPort = 587; //465

                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                    // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

                    //string mailFromPassword = "1234.Pass";
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;

                    //System.Net.Mail.MailAddressCollection mailtoA = new System.Net.Mail.MailAddressCollection();

                    //mailtoA = new System.Net.Mail.MailAddressCollection()(mailTo);


                    //System.Net.Mail.MailAddress mailtoins = new System.Net.Mail.MailAddress(mailTo[0]);


                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                    for (int i = 0; i < mailTo.Count; i++)
                    {
                        insMail.To.Add(mailTo[i]);
                    }

                    //insMail.To.Remove(mailtoins);

                    //foreach(string item in mailTo)
                    //{
                    //    insMail.To.Add(item);
                    //}

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)
                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = false;
                    // smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    //SendCompletedEventHandler a = new SendCompletedEventHandler();

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);

                    obj.mailTo = mailTo;
                    //string userState = "Message 1";
                    smtp.SendAsync(insMail, obj);
                    //smtp.Send(insMail);
                    return true;
                }

                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean SendEmailAsync_Engage(string mailTo, string message, string subject, string mailFrom_pseudo = "")//ok
        {
            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                    //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function


                    //  = obj.EmailSender;
                    // = obj.SMTP;
                    //  = obj.Port;
                    // = obj.Username;
                    //   Encryption.Decrypt(obj.Password);
                    //string mailFrom = "noreply@aseamail.com";
                    string mailFrom = server.EmailSender.ToString();
                    //string mailFrom = "lpacheco@xirectss.com";
                    //""
                    //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                    string mailServer = server.SMTP.ToString();
                    //string mailServer = "'smtp.gmail.com";
                    //host204.hostmonster.com
                    // int mailPort = 25; //465//  = obj.Port;
                    int mailPort = Convert.ToInt32(server.Port);
                    //int mailPort = 587; //465

                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                    // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

                    //string mailFromPassword = "1234.Pass";
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification")), new System.Net.Mail.MailAddress(mailTo));

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)
                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = false;
                    // smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    //SendCompletedEventHandler a = new SendCompletedEventHandler();

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);

                    obj.mailTo = new List<string>();
                    obj.mailTo.Add(mailTo);
                    //string userState = "Message 1";
                    smtp.SendAsync(insMail, obj);
                    //smtp.Send(insMail);
                    return true;
                }

                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean SendEmailAsync_Recognition(string mailTo, string message, string subject, string mailFrom_pseudo = "Notification")//ok
        {
            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServerRecognition server = GetEmailServer_Recognition(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                    //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function


                    //  = obj.EmailSender;
                    // = obj.SMTP;
                    //  = obj.Port;
                    // = obj.Username;
                    //   Encryption.Decrypt(obj.Password);
                    //string mailFrom = "noreply@aseamail.com";
                    string mailFrom = server.EmailSender.ToString();
                    //string mailFrom = "lpacheco@xirectss.com";
                    //""
                    //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                    string mailServer = server.SMTP.ToString();
                    //string mailServer = "'smtp.gmail.com";
                    //host204.hostmonster.com
                    // int mailPort = 25; //465//  = obj.Port;
                    int mailPort = Convert.ToInt32(server.Port);
                    //int mailPort = 587; //465

                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                    // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

                    //string mailFromPassword = "1234.Pass";
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo), new System.Net.Mail.MailAddress(mailTo));

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)
                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo);
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = false;
                    // smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    //SendCompletedEventHandler a = new SendCompletedEventHandler();

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);

                    obj.mailTo = new List<string>();
                    obj.mailTo.Add(mailTo);
                    //string userState = "Message 1";
                    smtp.SendAsync(insMail, obj);
                    //smtp.Send(insMail);
                    return true;
                }

                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean SendEmailAsync_test(string mailTo, string message, string subject, string mailFrom_pseudo = "")//ok
        {
            try
            {
                //NUEVAS CREDENCIALES AQUI!!!
                //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function

                //string mailFrom = "noreply@aseamail.com";
                string mailFrom = "info@xirectss.com";
                //""
                //string mailServer = "host204.hostmonster.com";
                string mailServer = "smtp.gmail.com";
                //host204.hostmonster.com
                int mailPort = 587; //465
                //int mailPort = ; //465

                //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                //  string mailFromAccount = "noreply@aseamail.com";
                string mailFromAccount = "info@xirectss.com";
                //"activosfijos@grupoxentry.com"
                //string mailFromPassword = "N0R3Pl1";
                string mailFromPassword = "1nF02013";
                //"13011970"

                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
                //   System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(, new System.Net.Mail.MailAddress());
                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();

                //insMail.To.Add("ppadilla@xirectss.com");
                insMail.To.Add(mailTo);
                //insMail.To.Add("vquiroz@xirectss.com");

                insMail.From = new System.Net.Mail.MailAddress(mailFrom, "Notification");


                //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                //insMail.AlternateViews.Add(HTMLContent)
                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom, "Notification");
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = true;
                // smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                //SendCompletedEventHandler a = new SendCompletedEventHandler();

                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                MailView obj = new MailView(message, subject, mailFrom);
                
                obj.mailTo = new List<string>();
                obj.mailTo.Add("ppadilla@xirectss.com");
                //obj.mailTo.Add("vquiroz@xirectss.com");
                obj.mailTo.Add(mailTo);
                //string userState = "Message 1";
                smtp.SendAsync(insMail, obj);
                //smtp.Send(insMail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static Boolean SendEmailAsyncSupport( string message, string subject)
        {
            try
            {
                //NUEVAS CREDENCIALES AQUI!!!
                //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function

                //string mailFrom = "noreply@aseamail.com";
                string mailFrom = "support911@xirectss.com";
                //""
                //string mailServer = "host204.hostmonster.com";
                string mailServer = "smtp.gmail.com";
                //host204.hostmonster.com
                int mailPort = 587; //465
                //int mailPort = ; //465

                //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

              //  string mailFromAccount = "noreply@aseamail.com";
                string mailFromAccount = "support911@xirectss.com";
                //"activosfijos@grupoxentry.com"
                //string mailFromPassword = "N0R3Pl1";
                string mailFromPassword = "A@SeaSupport";
                //"13011970"

                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
             //   System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(, new System.Net.Mail.MailAddress());
                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();

                insMail.To.Add("votiniano@xirectss.com");
                insMail.To.Add("lpacheco@xirectss.com");
                //insMail.To.Add("vquiroz@xirectss.com");

                insMail.From = new System.Net.Mail.MailAddress(mailFrom, "Support 911");


                //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                //insMail.AlternateViews.Add(HTMLContent)
                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom, "Support 911");
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = true;
                // smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                //SendCompletedEventHandler a = new SendCompletedEventHandler();

                //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                MailView obj = new MailView(message, subject, mailFrom);
                obj.mailTo = new List<string>();
                
                obj.mailTo.Add("votiniano@xirectss.com");
                obj.mailTo.Add("lpacheco@xirectss.com");
                //string userState = "Message 1";
                smtp.SendAsync(insMail, obj);
                //smtp.Send(insMail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static Boolean SendEmailAsync(string mailTo, string message, string subject, EnumEmailAccess emailSend)
        {
             try
            {
                
                string mailServer = "smtp.fatcow.com";
                
                int mailPort = 587;                
                String[] EmailDestination = emailSend.GetStringValue().Split('|');//[0] = emailFrom, [1] = pass, [2] = displayName
                
                //SendAccount
                string mailFromAccount = EmailDestination[0];//"mastermail@tru-friends.com";                
                string mailFromPassword = Encryption.Decrypt(EmailDestination[1]);// "Vqu1r0z1+0.";
                
                //SendMaskAccount
                string mailFrom = EmailDestination[0];
                string displayName = EmailDestination[2];

                string mailSubject = subject;
                string mailBody = message;                
                mailBody = message;

                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, displayName), new System.Net.Mail.MailAddress(mailTo));
                 
                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom,displayName);
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom,displayName);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                //SendCompletedEventHandler a = new SendCompletedEventHandler();
                 
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                MailView obj = new MailView(message, subject, mailFrom);
                obj.mailTo = new List<string>();
                obj.mailTo.Add(mailTo);
                //string userState = "Message 1";
                smtp.SendAsync(insMail, obj);
                //smtp.Send(insMail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool SendEmailAsync(List<String> mailTo, string subject, string message, String document, Int32 op) 
        {
            try 
            {   
                BaseEntity Base= new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);
                if (server != null)
                {
                    string mailFrom = server.EmailSender.ToString();
                    string mailServer = server.SMTP.ToString();
                    int mailPort = Convert.ToInt32(server.Port);
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                    string mailSubject = subject;
                    string mailBody = message;

                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                    for (int i = 0; i < mailTo.Count; i++)
                    {
                        insMail.To.Add(mailTo[i]);
                    }

                    if (!String.IsNullOrEmpty(document))
                    {
                        StringBuilder sbCalendar = new StringBuilder();
                        using (StreamReader sr = File.OpenText(document))
                        {
                            string s = "";
                            while ((s = sr.ReadLine()) != null)
                            {
                                sbCalendar.AppendLine(s);
                            }
                        }

                        byte[] byteArray = Encoding.UTF8.GetBytes(sbCalendar.ToString());

                        Stream contentStream = new MemoryStream(byteArray);

                        Attachment attachment = new Attachment(contentStream, "Schedule.ics", MediaTypeNames.Application.Octet);
                        attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                        //  memo.Attachments.Add(attachment);


                        // Add the file attachment to this e-mail message.
                        insMail.Attachments.Add(attachment);
                    }

                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom);
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);
                    obj.mailTo = new List<string>();
                    obj.mailTo = mailTo;
                    //string userState = "Message 1";
                    smtp.SendAsync(insMail, obj);
                    //smtp.Send(insMail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // TESTING METHOD
        public static Boolean SendEmailAsyncV2(List<string> mailTo, string message, string subject, string document, string docName)//ok
        {
            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                    //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function


                    //  = obj.EmailSender;
                    // = obj.SMTP;
                    //  = obj.Port;
                    // = obj.Username;
                    //   Encryption.Decrypt(obj.Password);
                    //string mailFrom = "noreply@aseamail.com";
                    string mailFrom = server.EmailSender.ToString();
                    //string mailFrom = "lpacheco@xirectss.com";
                    //""
                    //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                    string mailServer = server.SMTP.ToString();
                    //string mailServer = "'smtp.gmail.com";
                    //host204.hostmonster.com
                    // int mailPort = 25; //465//  = obj.Port;
                    int mailPort = Convert.ToInt32(server.Port);
                    //int mailPort = 587; //465

                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                    // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                    string mailFromAccount = server.Username.ToString();

                    //string mailFromAccount = "lpacheco@xirectss.com";
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

                    //string mailFromPassword = "1234.Pass";
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;

                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();

                    for (int i = 0; i < mailTo.Count; i++)
                    {
                        insMail.To.Add(mailTo[i]);
                    }

                    String[] format = docName.Split(new char[]{'.'});


                    if (!format[format.Length - 1].Equals("xlsx"))
                    {
                        if (!String.IsNullOrEmpty(document))
                        {
                            StringBuilder sbCalendar = new StringBuilder();
                            using (StreamReader sr = File.OpenText(document))
                            {
                                string s = "";
                                while ((s = sr.ReadLine()) != null)
                                {
                                    sbCalendar.AppendLine(s);
                                }
                            }

                            byte[] byteArray = Encoding.UTF8.GetBytes(sbCalendar.ToString());

                            Stream contentStream = new MemoryStream(byteArray);
                            //"Schedule.ics"
                            Attachment attachment = new Attachment(contentStream, docName, MediaTypeNames.Application.Octet);
                            attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                            //  memo.Attachments.Add(attachment);


                            // Add the file attachment to this e-mail message.
                            insMail.Attachments.Add(attachment);
                        }
                    }
                    else 
                    {
                        Attachment attachment = new Attachment(@""+document);
                        attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                        insMail.Attachments.Add(attachment);
                    }

                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;

                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = true;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    MailView obj = new MailView(message, subject, mailFrom);

                    obj.mailTo = new List<string>();
                    obj.mailTo = mailTo;


                    smtp.SendAsync(insMail, obj);
                    return true;
                }

                else
                    return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }






        public static bool SendEmailAsync(List<String> mailTo, string message, string subject)
        {
            try
            {
                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function

                //string mailFrom = "test@xsscenter.com";
                ////""
                //string mailServer = "smtp.fatcow.com";
                ////""
                //int mailPort = 587;
                ////Dim mailSSL As Boolean = gObjConfiguracion.MailSSL
                //string mailFromAccount = "test@xsscenter.com";
                ////"activosfijos@grupoxentry.com"
                //string mailFromPassword = "@Xirect2012";
                ////"13011970"
                BaseEntity Base= new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //NUEVAS CREDENCIALES AQUI!!!
                    //NO OLVIDAR PONER DISPLAY NAME "Replicated Sites Test"

                string mailFrom = server.EmailSender.ToString();
                string mailServer = server.SMTP.ToString();
                int mailPort = Convert.ToInt32(server.Port);
                string mailFromAccount = server.Username.ToString();

                //string mailFromAccount = "lpacheco@xirectss.com";
                //"activosfijos@grupoxentry.com"
                //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                for (int i = 0; i < mailTo.Count; i++)
                {
                    insMail.To.Add(mailTo[i]);
                }

                //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                //insMail.AlternateViews.Add(HTMLContent)

                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom);
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                //SendCompletedEventHandler a = new SendCompletedEventHandler();
                 
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                MailView obj = new MailView(message, subject, mailFrom);
                obj.mailTo = new List<string>();
                obj.mailTo = mailTo;
                //string userState = "Message 1";
                smtp.SendAsync(insMail, obj);
                //smtp.Send(insMail);
                return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Envio de Correo sincrono
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool SendEmail(string mailTo, string message, string subject)
        {

            try
            {
                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);

                if (server != null)
                {
                    //string mailFrom = "test@xsscenter.com";
                    string mailFrom = server.EmailSender.ToString();
                    //""
                    //string mailServer = "smtp.fatcow.com";
                    string mailServer = server.SMTP.ToString();
                    //""
                    //int mailPort = 587;
                    int mailPort = Convert.ToInt32(server.Port);
                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL
                    //string mailFromAccount = "test@xsscenter.com";
                    string mailFromAccount = server.Username.ToString();
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "@Xirect2012";
                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom), new System.Net.Mail.MailAddress(mailTo));

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)

                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom);
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = mailPort == 25 ? true : false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    smtp.Send(insMail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool SendEmailAutoResponders(string mailTo, string message, string subject, string mailFrom_pseudo = "Team Recognition", string mailcopy = "")
        {

            try
            {
                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function
                BaseEntity Base = new BaseEntity();
                clsEmailServerRecognition server = GetEmailServer_Recognition(ref Base);

                if (server != null)
                {
                    //string mailFrom = "test@xsscenter.com";
                    string mailFrom = server.EmailSender.ToString();
                    //""
                    //string mailServer = "smtp.fatcow.com";
                    string mailServer = server.SMTP.ToString();
                    //""
                    //int mailPort = 587;
                    int mailPort = Convert.ToInt32(server.Port);
                    //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL
                    //string mailFromAccount = "test@xsscenter.com";
                    string mailFromAccount = server.Username.ToString();
                    //"activosfijos@grupoxentry.com"
                    //string mailFromPassword = "@Xirect2012";
                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                    //"13011970"

                    string mailSubject = subject;
                    string mailBody = message;
                    //Dim MediaType As String = "text/html"
                    mailBody = message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo), new System.Net.Mail.MailAddress(mailTo));

                    //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                    //insMail.AlternateViews.Add(HTMLContent)

                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    //envia el mensaje como html
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo);
                    if (!String.IsNullOrEmpty(mailcopy))
                        _with1.Bcc.Add(mailcopy);
                    _with1.Bcc.Add("votiniano@xirectss.com");
                    //mailcopy
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = mailPort == 25 ? true : false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    smtp.Send(insMail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendEmail(ref BaseEntity Base, List<string> mailTo, List<string> mailCc, List<string> mailCo, string message, string subject, string mailFrom)
        {
            try
            {
                string mailServer = "smtp.fatcow.com";
                int mailPort = 587;
                string mailFromAccount = "test@xsscenter.com";
                string mailFromPassword = "@Xirect2012";
                string mailSubject = subject;
                string mailBody = message;
                MailMessage insMail = new MailMessage();
                insMail.From = new MailAddress(mailFrom);
                foreach (string item in mailTo)
                {
                    insMail.To.Add(item);
                }
                foreach (string item in mailCc)
                {
                    insMail.CC.Add(item);
                }
                foreach (string item in mailCo)
                {
                    insMail.Bcc.Add(item);
                }
                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                _with1.From = new MailAddress(mailFrom);
                _with1.ReplyTo = new MailAddress(mailFrom);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);
                smtp.Send(insMail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendEmail(List<string> mailTo, string message, string subject, string file = "")
        {


            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);
                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function

                string mailFrom = server.EmailSender.ToString();
                //string mailFrom = "lpacheco@xirectss.com";
                //""
                //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
                string mailServer = server.SMTP.ToString();
                //string mailServer = "'smtp.gmail.com";
                //host204.hostmonster.com
                // int mailPort = 25; //465//  = obj.Port;
                int mailPort = Convert.ToInt32(server.Port);
                //int mailPort = 587; //465

                //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

                // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
                string mailFromAccount = server.Username.ToString();

                //string mailFromAccount = "lpacheco@xirectss.com";
                //"activosfijos@grupoxentry.com"
                //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

                string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                //"13011970"

                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
                //  System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, "Xirect"), new System.Net.Mail.MailAddress(mailTo));

                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                for (int i = 0; i < mailTo.Count; i++)
                {
                    insMail.To.Add(mailTo[i]);
                }

                //if (!String.IsNullOrEmpty(bcc))
                //    insMail.Bcc.Add(bcc);
                //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                //insMail.AlternateViews.Add(HTMLContent)

                if (!String.IsNullOrEmpty(file))
                {
                    // string file = "data.xls";
                    // Create a message and set up the recipients.

                    // Create  the file attachment for this e-mail message.
                    //Attachment data = new Attachment(file, MediaTypeNames.Text.1); //.Application.Pdf
                    //// Add time stamp information for the file.
                    //ContentDisposition disposition = data.ContentDisposition;
                    //disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    StringBuilder sbCalendar = new StringBuilder();
                    using (StreamReader sr = File.OpenText(file))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            sbCalendar.AppendLine(s);
                        }
                    }

                    byte[] byteArray = Encoding.UTF8.GetBytes(sbCalendar.ToString());

                    Stream contentStream = new MemoryStream(byteArray);

                    Attachment attachment = new Attachment(contentStream, "Schedule.ics", MediaTypeNames.Application.Octet);
                    attachment.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                    //  memo.Attachments.Add(attachment);


                    // Add the file attachment to this e-mail message.
                    insMail.Attachments.Add(attachment);
                }


                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom, String.Concat(server.CompanyName, " Notification"));
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                smtp.Send(insMail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //This is a valid method, it s used for xbackoffice->contacts->sendmail
        public static bool SendEmailAsync(ref BaseEntity Base, List<string> mailTo, List<string> mailCc, List<string> mailCo, string message, string subject, string mailfrom)
        {
            try
            {
                string mailFrom = mailfrom;//"noreply@aseamail.com";
                string mailServer = "smtp.sendgrid.net";
                int mailPort = 25;
                string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";
                string mailFromPassword = "3vtgbn7u";
                ////If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function
                ////""
                //string mailServer = "smtp.fatcow.com";
                ////""
                //int mailPort = 587;
                ////Dim mailSSL As Boolean = gObjConfiguracion.MailSSL
                //string mailFromAccount = "test@xsscenter.com";
                ////"activosfijos@grupoxentry.com"
                //string mailFromPassword = "@Xirect2012";
                ////"13011970"

                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                for (int i = 0; i < mailTo.Count; i++)
                {
                    insMail.To.Add(mailTo[i]);
                }
                foreach (string item in mailCc)
                {
                    insMail.CC.Add(item);
                }
                foreach (string item in mailCo)
                {
                    insMail.Bcc.Add(item);
                }
                //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
                //insMail.AlternateViews.Add(HTMLContent)

                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom);
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;                
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                //SendCompletedEventHandler a = new SendCompletedEventHandler();

                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                MailView obj = new MailView(message, subject, mailFrom);
                obj.mailTo = new List<string>();
                obj.mailTo = mailTo;
                //string userState = "Message 1";
                smtp.SendAsync(insMail, obj);
                //smtp.Send(insMail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SendEmail(ref BaseEntity Base, List<String> mailTo, string message, string subject,string mailFrom)
        {
            try
            {
                //If LoadConfiguration("EMAIL_DISABLED") = "1" Then Exit Function
                //""
                string mailServer = "smtp.fatcow.com";
                //""
                int mailPort = 587;
                //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL
                string mailFromAccount = "test@xsscenter.com";
                //"activosfijos@grupoxentry.com"
                string mailFromPassword = "@Xirect2012";
                //"13011970"

                string mailSubject = subject;
                string mailBody = message;
                //Dim MediaType As String = "text/html"
                mailBody = message;
                //System.Net.Mail.MailMessage insMail = 
                //    new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom), 
                //        new System.Net.Mail.MailAddress(mailTo));
                System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();
                insMail.From = new System.Net.Mail.MailAddress(mailFrom);
                foreach (String email in mailTo)
                {
                    insMail.To.Add(email);               
                }
                var _with1 = insMail;
                _with1.Subject = mailSubject;
                _with1.Body = mailBody;
                _with1.IsBodyHtml = true;
                //envia el mensaje como html
                _with1.From = new System.Net.Mail.MailAddress(mailFrom);
                _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailServer;
                smtp.Port = mailPort;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                smtp.Send(insMail);

                return true;
            }
            catch (Exception ex)
            {
                Base.Errors.Add(new BaseEntity.ListError {Error=ex,MessageClient="No email found."});
                
                return false;
            }
        }

   
        
        #region Email Server 

        public class clsEmailServer
        {

            public String EmailName { get; set; }
            public String EmailSender { get; set; }
            public String SMTP { get; set; }
            public String Port { get; set; }
            public String Username { get; set; }
            public String Password { get; set; }
            public Boolean EnableSSL { get; set; }
            public String CompanyName { get; set; }
        }

        public class clsEmailServerRecognition
        {

            public String EmailName { get; set; }
            public String EmailSender { get; set; }
            public String SMTP { get; set; }
            public String Port { get; set; }
            public String Username { get; set; }
            public String Password { get; set; }
            public Boolean EnableSSL { get; set; }
        }

        private static clsEmailServer GetEmailServer(ref BaseEntity Base)
        {
            clsEmailServer obj = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                //cmd = new SqlCommand("SP_SETTINGS_GETALL",clsConexion.ObtenerConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    obj = new clsEmailServer();
                    if (dr.Read())
                    {
                        obj.SMTP = dr.GetColumnValue<String>("EMAILSERVER");
                        obj.Port = dr.GetColumnValue<String>("EMAILPORT");
                        obj.Username = dr.GetColumnValue<String>("EMAILUSERNAME");
                        obj.Password = dr.GetColumnValue<String>("EMAILPASSWORD");
                        obj.EmailSender = dr.GetColumnValue<String>("EMAILSENDER");
                        obj.CompanyName = dr.GetColumnValue<String>("COMPANYNAME");
                    }
                }
            }
            catch (Exception ex)
            {
                obj = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                //clsConexion.DisposeCommand(cmd);
            }
            return obj;
        }

        private static clsEmailServerRecognition GetEmailServer_Recognition(ref BaseEntity Base)
        {
            clsEmailServerRecognition obj = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                //cmd = new SqlCommand("SP_SETTINGS_GETRECOGNITIONSETTINGS", clsConexion.ObtenerConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    obj = new clsEmailServerRecognition();
                    if (dr.Read())
                    {

                        obj.SMTP = dr.GetColumnValue<String>("EMAILSERVER_RECOGNITION");
                        obj.Port = dr.GetColumnValue<String>("EMAILPORT_RECOGNITION");
                        obj.Username = dr.GetColumnValue<String>("EMAILUSERNAME_RECOGNITION");
                        obj.Password = dr.GetColumnValue<String>("EMAILPASSWORD_RECOGNITION");
                        obj.EmailSender = dr.GetColumnValue<String>("EMAILSENDER_RECOGNITION");

                    }
                }
            }
            catch (Exception ex)
            {
                obj = null;
                Base.Errors.Add(new BaseEntity.ListError(ex, ex.Message));
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                cmd.Connection.Close();
            }
            return obj;
        }

        #endregion




        //public static Boolean SendEmailAsync(List<string> mailTo, string message, string subject, Int16 appid)
        //{
        //    try
        //    {
        //        BaseEntity Base = new BaseEntity();
        //        clsEmailServer server = GetEmailServerByAppId(ref Base, appid);

        //        if (server != null)
        //        {
        //            string mailFrom = server.EmailSender.ToString();
        //            //string mailFrom = "lpacheco@xirectss.com";
        //            //""
        //            //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
        //            string mailServer = server.SMTP.ToString();
        //            //string mailServer = "'smtp.gmail.com";
        //            //host204.hostmonster.com
        //            // int mailPort = 25; //465//  = obj.Port;
        //            int mailPort = Convert.ToInt32(server.Port);
        //            //int mailPort = 587; //465

        //            //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

        //            // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
        //            string mailFromAccount = server.Username.ToString();

        //            //string mailFromAccount = "lpacheco@xirectss.com";
        //            //"activosfijos@grupoxentry.com"
        //            //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

        //            string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

        //            //string mailFromPassword = "1234.Pass";
        //            //"13011970"

        //            string mailSubject = subject;
        //            string mailBody = message;
        //            //Dim MediaType As String = "text/html"
        //            mailBody = message;
        //            //System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, server.EmailName), new System.Net.Mail.MailAddress(mailTo));
        //            System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage();

        //            for (int i = 0; i < mailTo.Count; i++)
        //            {
        //                insMail.To.Add(mailTo[i]);
        //                // insMail.From.;
        //            }
        //            //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
        //            //insMail.AlternateViews.Add(HTMLContent)
        //            var _with1 = insMail;
        //            _with1.Subject = mailSubject;
        //            _with1.Body = mailBody;
        //            _with1.IsBodyHtml = true;
        //            //envia el mensaje como html
        //            _with1.From = new System.Net.Mail.MailAddress(mailFrom, server.EmailName);
        //            _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom, server.EmailName);
        //            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        //            smtp.Host = mailServer;
        //            smtp.Port = mailPort;
        //            smtp.EnableSsl = server.EnableSSL;
        //            //smtp.EnableSsl = true;
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

        //            //SendCompletedEventHandler a = new SendCompletedEventHandler();

        //            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //            MailView obj = new MailView(message, subject, mailFrom);

        //            obj.mailTo = new List<string>();
        //            obj.mailTo = mailTo;
        //            //string userState = "Message 1";

        //            //smtp.Send(insMail);


        //            smtp.SendAsync(insMail, obj);
        //            return true;
        //        }

        //        else
        //            return false;

        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        //private static clsEmailServer GetEmailServerByAppId(ref BaseEntity Base, Int16 AppId)
        //{
        //    clsEmailServer obj = null;

        //    SqlDataReader dr = null;
        //    SqlCommand ObjCmd = null;
        //    try
        //    {
        //        if (AppId == Convert.ToInt16(EnumTemplateModule.PartyPlan))
        //        {
        //            ObjCmd = new SqlCommand("SP_PARTYPLAN_SETTINGS_GET", clsConexion.ObtenerConexion());
        //            ObjCmd.CommandType = CommandType.StoredProcedure;
        //            dr = ObjCmd.ExecuteReader();
        //            if (dr.Read())
        //            {
        //                obj = new clsEmailServer();
        //                obj.SMTP = dr.GetColumnValue<String>("SMTPSERVER");
        //                obj.Port = dr.GetColumnValue<String>("PORT");
        //                obj.Username = dr.GetColumnValue<String>("EMAILUSERNAME");
        //                obj.Password = dr.GetColumnValue<String>("EMAILPASSWORD");
        //                obj.EmailSender = dr.GetColumnValue<String>("SENDEREMAILADDRESS");
        //                obj.EmailName = dr.GetColumnValue<String>("SENDEREMAILADDRESS");
        //            }
        //        }
        //        else
        //        {
        //            ObjCmd = new SqlCommand("SP_EMAILSERVER_GETBYAPPID", clsConexion.ObtenerConexion());
        //            ObjCmd.CommandType = CommandType.StoredProcedure;
        //            ObjCmd.Parameters.AddWithValue("@APPID", AppId);
        //            dr = ObjCmd.ExecuteReader();
        //            if (dr.Read())
        //            {
        //                obj = new clsEmailServer();
        //                obj.SMTP = dr.GetColumnValue<String>("EMAILSERVER");
        //                obj.Port = dr.GetColumnValue<String>("EMAILPORT");
        //                obj.Username = dr.GetColumnValue<String>("EMAILUSERNAME");
        //                obj.Password = dr.GetColumnValue<String>("EMAILPASSWORD");
        //                obj.EmailSender = dr.GetColumnValue<String>("EMAILSENDER");
        //                obj.EmailName = dr.GetColumnValue<String>("EMAILNAME");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obj = null;
        //        Base.Errors.Add(new BaseEntity.ListError(ex, "No found."));
        //    }
        //    finally
        //    {
        //        //dr.Close();
        //        //ObjCmd.Connection.Close();
        //        clsConexion.DisposeCommand(ObjCmd);
        //    }
        //    return obj;
        //}
        
        //public static Boolean SendEmailAsync(string mailTo, string message, string subject, Int16 appid)
        //{
        //    try
        //    {
        //        BaseEntity Base = new BaseEntity();
        //        clsEmailServer server = GetEmailServerByAppId(ref Base, appid);

        //        if (server != null)
        //        {
        //            string mailFrom = server.EmailSender.ToString();
        //            //string mailFrom = "lpacheco@xirectss.com";
        //            //""
        //            //string mailServer = "smtp.sendgrid.net"; // = obj.SMTP;
        //            string mailServer = server.SMTP.ToString();
        //            //string mailServer = "'smtp.gmail.com";
        //            //host204.hostmonster.com
        //            // int mailPort = 25; //465//  = obj.Port;
        //            int mailPort = Convert.ToInt32(server.Port);
        //            //int mailPort = 587; //465

        //            //Dim mailSSL As Boolean = gObjConfiguracion.MailSSL

        //            // string mailFromAccount = "azure_245c3156137dc40c84bcb7fc79fc845b@azure.com";  // = obj.Username;
        //            string mailFromAccount = server.Username.ToString();

        //            //string mailFromAccount = "lpacheco@xirectss.com";
        //            //"activosfijos@grupoxentry.com"
        //            //string mailFromPassword = "3vtgbn7u";//   Encryption.Decrypt(obj.Password);

        //            string mailFromPassword = Encryption.Decrypt(server.Password.ToString());

        //            //string mailFromPassword = "1234.Pass";
        //            //"13011970"

        //            string mailSubject = subject;
        //            string mailBody = message;
        //            //Dim MediaType As String = "text/html"
        //            mailBody = message;
        //            System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, server.EmailName), new System.Net.Mail.MailAddress(mailTo));

        //            //Dim HTMLContent As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, Nothing, MediaType)
        //            //insMail.AlternateViews.Add(HTMLContent)
        //            var _with1 = insMail;
        //            _with1.Subject = mailSubject;
        //            _with1.Body = mailBody;
        //            _with1.IsBodyHtml = true;
        //            //envia el mensaje como html
        //            _with1.From = new System.Net.Mail.MailAddress(mailFrom, server.EmailName);
        //            _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom, server.EmailName);
        //            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        //            smtp.Host = mailServer;
        //            smtp.Port = mailPort;
        //            smtp.EnableSsl = server.EnableSSL;
        //            //smtp.EnableSsl = true;
        //            smtp.UseDefaultCredentials = false;
        //            smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

        //            //SendCompletedEventHandler a = new SendCompletedEventHandler();

        //            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //            MailView obj = new MailView(message, subject, mailFrom);

        //            obj.mailTo = new List<string>();
        //            obj.mailTo.Add(mailTo);
        //            //string userState = "Message 1";

        //            //smtp.Send(insMail);


        //            smtp.SendAsync(insMail, obj);
        //            return true;
        //        }

        //        else
        //            return false;

        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public static bool SendEmailFantasticAchievements(string mailTo, string Message, string Subject, string mailFrom_pseudo = "Team Asea", string mailcopy = "")
        {
            try
            {
                BaseEntity Base = new BaseEntity();
                clsEmailServer server = GetEmailServer(ref Base);
                if (server != null)
                {
                    //string mailFrom = "aquispe@xirectss.com";
                    string mailFrom = server.EmailSender.ToString();
                    string mailServer = server.SMTP.ToString();
                    int mailPort = Convert.ToInt32(server.Port);
                    string mailFromAccount = server.Username.ToString();
                    string mailFromPassword = Encryption.Decrypt(server.Password.ToString());
                    string mailSubject = Subject;
                    string mailBody = Message;
                    mailBody = Message;
                    System.Net.Mail.MailMessage insMail = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo), new System.Net.Mail.MailAddress(mailTo));
                    var _with1 = insMail;
                    _with1.Subject = mailSubject;
                    _with1.Body = mailBody;
                    _with1.IsBodyHtml = true;
                    _with1.From = new System.Net.Mail.MailAddress(mailFrom, mailFrom_pseudo);
                    if (!String.IsNullOrEmpty(mailcopy))
                        _with1.Bcc.Add(mailcopy);
                    _with1.Bcc.Add("arios@xirectss.com");
                    _with1.ReplyTo = new System.Net.Mail.MailAddress(mailFrom);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = mailServer;
                    smtp.Port = mailPort;
                    smtp.EnableSsl = mailPort == 25 ? true : false;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailFromAccount, mailFromPassword);

                    smtp.Send(insMail);

                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
