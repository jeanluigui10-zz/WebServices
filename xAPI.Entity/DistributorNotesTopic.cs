using System;
using System.Data;
namespace xAPI.Entity
{
    [Serializable]
    public class DistributorNotesTopic
    {
        public DistributorNotesTopic()
        {

        }
        public DistributorNotesTopic(DataRow row)
        {
            //Id = Id.ValidateDataRowKey(row, "ID");
            //TopicName = TopicName.ValidateDataRowKey(row, "TOPICNAME");
            //Status = Status.ValidateDataRowKey(row, "STATUS") == 1 ? (Int16)EnumStatus.Enabled : (Int16)EnumStatus.Disabled;
            //CreatedDate = CreatedDate.ValidateDataRowKey(row, "CREATEDDATE");
            //CreatedBy = CreatedBy.ValidateDataRowKey(row, "CREATEDBY");
            //UpdatedBy = UpdatedBy.ValidateDataRowKey(row, "UPDATEDBY");
        }
        public Int32 Id { get; set; }
        public String TopicName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 Status { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 UpdatedBy { get; set; }
    }
}
