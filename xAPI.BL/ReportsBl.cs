using System;
using System.Data;
using xAPI.Dao;
using xAPI.Entity.Base;

namespace xAPI.BL
{
    public class ReportsBl
    {
        #region Singleton
        private static ReportsBl instance = null;
        public static ReportsBl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReportsBl();
                }

                return instance;
            }
        }
        #endregion
        public DataTable Distributor_GetList_ByQuery(ref BaseEntity baseEntity, String queryString, String connection)
        {
            baseEntity = new BaseEntity();
            DataTable distributorTable = new DataTable();
            try
            {
                if (!String.IsNullOrEmpty(queryString) && !String.IsNullOrEmpty(connection))
                    distributorTable = clsReportDao.Instance.Distributor_GetList_ByQuery(ref baseEntity, queryString, connection);
            }
            catch (Exception exception)
            {
                distributorTable = null;
                baseEntity.Errors.Add(new BaseEntity.ListError(new Exception { }, "An error occurred on application level 2."));
            }

            return distributorTable;
        }

    }
}
