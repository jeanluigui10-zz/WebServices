using System;
using System.Data;
using xAPI.Entity.Base;

namespace xAPI.Dao
{
    public class clsReportDao
    {
        #region Singleton
        private static clsReportDao instance = null;
        public static clsReportDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new clsReportDao();
                }

                return instance;
            }
        }
        #endregion

        //public List<DistributorNotesTopic> GetAllTopicsV2()
        //{
        //    List<DistributorNotesTopic> topicsList = null;
        //    ListParameters parameters = new ListParameters();
        //    CommandParameter queryCommand = new CommandParameter("SP_GETALLTPOCIS", parameters);
        //    DataTable distributorTable = DbManager.Instance.ExecuteTable(queryCommand);
        //    if (distributorTable != null && distributorTable.Rows.Count > 0)
        //    {
        //        topicsList = new List<DistributorNotesTopic>();
        //        foreach (DataRow data in distributorTable.Rows)
        //            topicsList.Add(new DistributorNotesTopic(data));
        //    }
        //    return topicsList;
        //}
        public DataTable Distributor_GetList_ByQuery(ref BaseEntity baseEntity, String queryString, String connection)
        {
            DataTable dt = new DataTable();
            //    ListParameters parameters = new ListParameters();
            //    CommandParameter queryCommand = new CommandParameter("Distributor_GetAll_ByQuery_Sp", parameters)
            //    {
            //        ConnectionKey = connection
            //    };
            //    return DbManager.Instance.ExecuteTable(queryCommand);
            //}
            return dt;
        }
    }
}
