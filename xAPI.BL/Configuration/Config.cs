using System;
using System.Web.Configuration;

namespace xAPI.BL.Configuration
{
    public class OrderConfig
    {
        public static String EnterpriseVirtualPath { get { return WebConfigurationManager.AppSettings["evPath"]; } }

    }
}
