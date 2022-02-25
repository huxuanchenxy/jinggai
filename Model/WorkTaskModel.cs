using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;

namespace SZY.Platform.WebApi.Model
{
    public class WorkTaskModel
    {
    }


    public class WorkTaskQueryParm : BaseQueryParm
    {
        public int? ActivityState { get; set; }
        public int? AssignedToUserID { get; set; }
        public string AppName { get; set; }
    }

    public class WorkQueryParm : BaseQueryParm
    {
        public int UserID { get; set; }
        public int? ProcessInstanceID { get; set; }
    }

    public class WorkTaskPageView
    {
        public List<TaskViewModel> rows { get; set; }
        public int total { get; set; }
    }
    public class TaskViewModel : BaseEntity
    {
        public int ID { get; set; }

        public string AppName { get; set; }

        public string AppInstanceID { get; set; }

        public string AppInstanceCode { get; set; }

        public string ProcessName { get; set; }

        public string ProcessGUID { get; set; }

        public string Version { get; set; }

        public int ProcessInstanceID { get; set; }

        public string ActivityGUID { get; set; }

        public int ActivityInstanceID { get; set; }

        public string ActivityName { get; set; }

        public short ActivityType { get; set; }

        public short WorkItemType { get; set; }

        public string PreviousUserID { get; set; }          //上一步审核人ID

        public string PreviousUserName { get; set; }

        public Nullable<DateTime> PreviousDateTime { get; set; }

        public short TaskType { get; set; }

        public Nullable<int> EntrustedTaskID { get; set; }        //被委托任务ID

        public string AssignedToUserID { get; set; }

        public string AssignedToUserName { get; set; }

        public System.DateTime CreatedDateTime { get; set; }

        public Nullable<DateTime> LastUpdatedDateTime { get; set; }

        public Nullable<System.DateTime> EndedDateTime { get; set; }

        public string EndedByUserID { get; set; }

        public string EndedByUserName { get; set; }

        public short TaskState { get; set; }

        public short ActivityState { get; set; }

        public byte RecordStatusInvalid { get; set; }

        public short ProcessState { get; set; }

        public Nullable<short> ComplexType { get; set; }

        public Nullable<int> MIHostActivityInstanceID { get; set; }

        public string PCreatedByUserID { get; set; }

        public string PCreatedByUserName { get; set; }

        public Nullable<DateTime> PCreatdDateTime { get; set; }

        public short MiHostState { get; set; }

        public string LastUpdatedByUserName { get; set; }
    }

    public class WfRet
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string NewID { get; set; }
        public object ExtraData { get; set; }
        public object Entity { get; set; }
    }

    public class WfReq
    {

        public string AppName { get; set; }
        public long AppInstanceID { get; set; }
        public string ProcessGUID { get; set; }
        public int ProcessID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public dynamic Conditions { get; set; }
        public dynamic NextActivityPerformers { get; set; }
    }


}
