
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/11/9 13:42:16
namespace SZY.Platform.WebApi.Model
{
    public class JingGaiAlarmParm : BaseQueryParm
    {

    }
    public class JingGaiAlarmPageView
    {
        public List<JingGaiDevice> rows { get; set; }
        public int total { get; set; }
    }

    public class JingGaiAlarm : BaseEntity
    {
        public int id { get; set; }

        public string device_id { get; set; }
        public int device_type { get; set; }

        public string device_addr { get; set; }

        public string device_name { get; set; }

        public string alarm_item { get; set; }

        public string alarm_item_name { get; set; }
        public string alarm_value { get; set; }

        public string compare_type { get; set; }

        public string threshold { get; set; }
       
        public string date1 { get; set; }
    }

    public class JingGaiAlarmMap : EntityMap<JingGaiAlarm>
    {
        public JingGaiAlarmMap()
        {
            Map(o => o.id).ToColumn("id");
            Map(o => o.device_id).ToColumn("device_id");
            Map(o => o.device_type).ToColumn("device_type");
            Map(o => o.device_name).ToColumn("device_name");
            Map(o => o.device_addr).ToColumn("device_addr");
            Map(o => o.alarm_item).ToColumn("alarm_item");
            Map(o => o.alarm_item_name).ToColumn("alarm_item_name");
            Map(o => o.alarm_value).ToColumn("alarm_value");
            Map(o => o.compare_type).ToColumn("compare_type");
            Map(o => o.threshold).ToColumn("threshold");
            Map(o => o.date1).ToColumn("date1");
        }
    }

}