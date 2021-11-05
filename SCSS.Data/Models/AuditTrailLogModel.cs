using SCSS.Data.Entities;
using System;

namespace SCSS.Data.Models
{
    public class AuditTrailLogModel
    {
        #region Fields

        public Guid? AccountId { get; set; }

        public string TableName { get; set; }

        public string ChangeColumns { get; set; }

        public string State { get; set; }

        public Guid RecordId { get; set; }

        public string OldData { get; set; }

        public string NewData { get; set; }

        public string Reason { get; set; }

        public string OtherData { get; set; }

        #endregion Fields

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns></returns>
        public AuditTrailLog ToEntity(DateTime? timeStamp) => new AuditTrailLog()
        {
            AccountId = AccountId,
            TableName = TableName,
            ChangeColumns = ChangeColumns,
            DateTimeStamp = timeStamp,
            OldData = OldData,
            NewData = NewData,
            RecordId = RecordId,
            Reason = Reason,
            OtherData = OtherData,
            State = State
        };
    }
}
