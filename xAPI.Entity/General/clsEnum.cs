using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace xAPI.Entity.General
{
    public static class clsEnum
    {
        public static String SubGetStringValue(this Type value, Int32 id)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this typeAddress
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];


            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
        public static String GetValueById(Type _enum, Int32 attr)
        {

            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            String Name = "";
            for (int i = 1; i < fi.Length; i++)
            {

                if (Convert.ToInt32(values.GetValue(i - 1)) == attr)
                {

                    StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                    {
                        Name = attrs[0].StringValue;
                    }
                    //Name = values.GetValue(i - 1).ToString();
                    break;
                }
            }

            return Name;
        }
        public static String GetMessageById(Type _enum, Int32 attr)
        {

            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            String Name = "";
            for (int i = 1; i < fi.Length; i++)
            {

                if (Convert.ToInt32(values.GetValue(i - 1)) == attr)
                {

                    StringMessageAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringMessageAttribute), false) as StringMessageAttribute[];
                    if (attrs.Length > 0)
                    {
                        Name = attrs[0].StringMessage;
                    }
                    //Name = values.GetValue(i - 1).ToString();
                    break;
                }
            }

            return Name;
        }
        public static String GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].StringValue;
            }
            return output;
        }
        public static Int32 GetValueByStringAttr(Type _enum, String attr)
        {
            Int32 ret = 0;
            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();

            for (int i = 1; i < fi.Length; i++)
            {
                StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs[0].StringValue == attr)
                {
                    ret = Convert.ToInt32(values.GetValue(i - 1));
                }
            }

            return ret;
        }
        public static String GetStringValue(Type _enum, Int32 IdEnum)
        {

            String stringValue = String.Empty;
            Array arrayValues = Enum.GetValues(_enum);

            string[] names = System.Enum.GetNames(_enum);
            for (int i = 0; i < arrayValues.Length; i++)
            {
                if ((Int32)arrayValues.GetValue(i) == IdEnum)
                {
                    Enum enumObj = (Enum)Enum.Parse(_enum, names[i]);
                    stringValue = enumObj.GetStringValue();
                    break;
                }
            }
            return stringValue;
        }
        public static String GetMessageValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringMessageAttribute[] attrs = fi.GetCustomAttributes(typeof(StringMessageAttribute), false) as StringMessageAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].StringMessage;
            }
            return output;
        }

        public static string GetLocalizationValue(this Enum value)
        {
            string output = String.Empty;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            LocalizationAttribute[] attrs = fi.GetCustomAttributes(typeof(LocalizationAttribute), false) as LocalizationAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].LocalizationValue;
            }

            return output;
        }
    }

    #region Extension of Attribute
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }
    public class StringMessageAttribute : Attribute
    {
        public string StringMessage { get; protected set; }
        public StringMessageAttribute(string value)
        {
            this.StringMessage = value;
        }
    }
    public class LocalizationAttribute : Attribute
    {
        public string LocalizationValue { get; protected set; }
        public LocalizationAttribute(string value)
        {
            this.LocalizationValue = value;
        }
    }
    #endregion

    #region Tipos de Usuario

    public enum EnumTipoUsuario
    {
        [StringValue("Administrador")]
        Administrador = 1,
        [StringValue("Asistente")]
        Asistente = 2,
        [StringValue("Lider")]
        Lider = 3
    }
    public enum EnumEsatado
    {
        [StringValue("Activo")]
        Activo = 1,
        [StringValue("Inactivo")]
        Inactivo = 0
    }
    #endregion

    public enum EnumAppCode
    {
        [StringValue("All")]
        All = -1,
        [StringValue("xBackOffice")]
        xBackOffice = 2,
        [StringValue("xCorporate")]
        xCorporate = 3,
        [StringValue("Products")]
        Products = 4,
        [StringValue("MyProductPhotos")]
        MyProductPhotos = 7,
        [StringValue("PartyPlan")]
        PartyPlan = 11,
        [StringValue("Soxial")]
        SoxialMarketing = 12
    }


    public enum EnumEmailAccess
    {
        [StringValue("mastermail@tru-friends.com|k+aRJKKGcsOsxh3P5u3gcQ==|Tru-Friends")]
        Mastermail = 1,
        [StringValue("billing@tru-friends.com|LAI7MxUXVD+KVMNFqFAAnCpWoe6JqALA4hWKI/0KoWI=|Tru-Friends Billing")]
        Billing = 2
    }
    public enum EnumEmailStatus
    {
        Success = 1,
        Error = 2,
        Cancelled = 3
    }
    public enum EnumAlertType
    {
        [StringValue("s")]
        Success = 1,
        [StringValue("e")]
        Error = 2,
        [StringValue("i")]
        Info = 3,
        [StringValue("c")]
        Confirm = 4,
        [StringValue("b")]
        Custom = 5
    }
    public enum EnumVideoFileFormat
    {
        avi,
        mp4,
        wmv,
        fla,
        flv,
        mpeg,
        mpg
    }
    public enum EnumPresentationFileFormat
    {
        ppt,
        pptx
    }
    public enum EnumImageFileFormat
    {
        gif,
        jpeg,
        jpg,
        tif,
        png,
        psd,
        ai,
        eps,
        bmp

    }
    public enum EnumAudioFileFormat
    {
        mp3,
        wma,
        wav,
        midi
    }

    public enum EnumDocumentFileFormat
    {
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pdf,
        pptx,
        txt,
        html,
        csv
    }
    public enum EnumDocumenImagetFileFormat
    {
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pdf,
        pptx,
        txt,
        html,
        jpeg,
        jpg,
        png,
        gif,
        bmp,
    }
    public enum EnumExtensionURL
    {
        [StringValue("ext")]
        ext = 0,
    }
    public enum EnumFolderSettings
    {
        [StringValue("export\\")]
        FolderExport = 1,
        [StringValue("resources\\")]
        FolderResources = 2,
        [StringValue("images\\")]
        FolderImages = 3,
        FolderUpdates = 4,
        [StringValue("review\\")]
        FolderReview = 5,
        [StringValue("docs\\")]
        FolderDocs = 6,
        [StringValue("shippingFiles\\")]
        FolderShipping = 7,
        [StringValue("docs/")]
        FolderPDF = 8,
        [StringValue("images/")]
        ImageFolder = 9,
        [StringValue("Products\\")]
        Products = 11,
        [StringValue("invoiceFiles\\")]
        invoiceFiles = 12,
        [StringValue("cashierFiles\\")]
        cashierFiles = 13
    }
    public enum EnumStatus
    {
        [StringValue("n")]
        Disabled = 0,//Inactive
        [StringValue("y")]
        Enabled = 1,//Active
        Deleted = 2,
        ShowAll = 3,
        [StringValue("s")]
        Suspended = 4,//Suspended
        [StringValue("u")]
        notSuspended = 5,
        [StringValue("p")]
        purged = 6
    }
}
