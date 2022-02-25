
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/11/9 13:42:16
namespace SZY.Platform.WebApi.Model
{
    public class JingGaiDeviceParm : BaseQueryParm
    {

    }
    public class JingGaiDevicePageView
    {
        public List<JingGaiDevice> rows { get; set; }
        public int total { get; set; }
    }

    public class JingGaiDevice : BaseEntity
    {
        public int id { get; set; }

        public string device_id { get; set; }
        /// <summary>
        /// 设备类型 1:井盖
        /// </summary>
        public int device_type { get; set; }
        /// <summary>
        /// 固件版本
        /// </summary>
        public string device_name { get; set; }

        public string install_addr { get; set; }

        public string install_time { get; set; }
        /// <summary>


        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
       
        public string date1 { get; set; }
    }

    public class JingGaiDeviceMap : EntityMap<JingGaiDevice>
    {
        public JingGaiDeviceMap()
        {
            Map(o => o.id).ToColumn("id");
            Map(o => o.device_id).ToColumn("device_id");
            Map(o => o.device_type).ToColumn("device_type");
            Map(o => o.device_name).ToColumn("device_name");
            Map(o => o.install_addr).ToColumn("install_addr");
            Map(o => o.install_time).ToColumn("install_time");
            Map(o => o.lng).ToColumn("lng");
            Map(o => o.lat).ToColumn("lat");
            Map(o => o.date1).ToColumn("date1");
        }
    }

}