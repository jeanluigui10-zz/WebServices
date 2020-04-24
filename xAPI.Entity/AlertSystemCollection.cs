using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
namespace xAPI.Entity
{
    [Serializable]
    public class AlertSystemCollection : List<AlertSystemLanguage>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("ALERTID", SqlDbType.Int),
                new SqlMetaData("LANGUAGEID", SqlDbType.Int),
                new SqlMetaData("STATUS", SqlDbType.SmallInt)
                );
            foreach (AlertSystemLanguage data in this)
            {
                ret.SetInt32(0, data.AlertId);
                ret.SetInt32(1, data.LanguageId);
                ret.SetInt16(2, data.Status);
                yield return ret;
            }
        }
    }

}
