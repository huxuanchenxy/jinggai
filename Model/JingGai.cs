
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/11/9 13:42:16
namespace SZY.Platform.WebApi.Model
{
    public class JingGaiParm : BaseQueryParm
    {

    }
    public class JingGaiPageView
    {
        public List<JingGai> rows { get; set; }
        public int total { get; set; }
    }

    public class JingGai : BaseEntity
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
        public string fv { get; set; }
        /// <summary>
        /// 剩余电量
        /// </summary>
        public string soc { get; set; }
        /// <summary>
        /// 运行状态 0维护 1正常
        /// </summary>
        public string sor { get; set; }
        /// <summary>
        /// 实时温度
        /// </summary>
        public string rtd { get; set; }
        /// <summary>
        /// 倾斜角度
        /// </summary>
        public string rad { get; set; }
        /// <summary>
        /// 震动幅度
        /// </summary>
        public string rqd { get; set; }
        /// <summary>
        /// 锁状态 0开 1关 E故障 X未知
        /// </summary>
        public string sol { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 水面距离 单位厘米
        /// </summary>
        public string sensor_water_level { get; set; }
        /// <summary>
        /// 水面深度 单位厘米
        /// </summary>
        public string sensor_water_depth { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string sensor_temperature { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        public string sensor_humidity { get; set; }
        /// <summary>
        /// 烟雾报警 0正常 1报警
        /// </summary>
        public string sensor_smoke { get; set; }
        /// <summary>
        /// 可燃气体报警 0正常 1报警
        /// </summary>
        public string sensor_ch4 { get; set; }
        /// <summary>
        /// 有毒气体报警 0正常 1报警
        /// </summary>
        public string sensor_toxic { get; set; }
        /// <summary>
        /// 水位报警 0正常 1报警
        /// </summary>
        public string sensor_water_alarm { get; set; }
        /// <summary>
        /// 水位预警 0正常 1报警
        /// </summary>
        public string sensor_water_warn { get; set; }
        /// <summary>
        /// 酸碱度
        /// </summary>
        public string sensor_ph { get; set; }
        /// <summary>
        /// 可燃气体浓度
        /// </summary>
        public string sensor_ch4_conc { get; set; }
        /// <summary>
        /// 有毒气体浓度
        /// </summary>
        public string sensor_toxic_conc { get; set; }
        public string date1 { get; set; }
    }

    public class JingGaiMap : EntityMap<JingGai>
    {
        public JingGaiMap()
        {
            Map(o => o.id).ToColumn("id");
            Map(o => o.device_id).ToColumn("device_id");
            Map(o => o.device_type).ToColumn("device_type");
            Map(o => o.fv).ToColumn("fv");
            Map(o => o.soc).ToColumn("soc");
            Map(o => o.sor).ToColumn("sor");
            Map(o => o.rtd).ToColumn("rtd");
            Map(o => o.rad).ToColumn("rad");
            Map(o => o.rqd).ToColumn("rqd");
            Map(o => o.sol).ToColumn("sol");
            Map(o => o.lng).ToColumn("lng");
            Map(o => o.lat).ToColumn("lat");
            Map(o => o.sensor_water_level).ToColumn("sensor_water_level");
            Map(o => o.sensor_water_depth).ToColumn("sensor_water_depth");
            Map(o => o.sensor_temperature).ToColumn("sensor_temperature");
            Map(o => o.sensor_humidity).ToColumn("sensor_humidity");
            Map(o => o.sensor_smoke).ToColumn("sensor_smoke");
            Map(o => o.sensor_ch4).ToColumn("sensor_ch4");
            Map(o => o.sensor_toxic).ToColumn("sensor_toxic");
            Map(o => o.sensor_water_alarm).ToColumn("sensor_water_alarm");
            Map(o => o.sensor_water_warn).ToColumn("sensor_water_warn");
            Map(o => o.sensor_ph).ToColumn("sensor_ph");
            Map(o => o.sensor_ch4_conc).ToColumn("sensor_ch4_conc");
            Map(o => o.sensor_toxic_conc).ToColumn("sensor_toxic_conc");
            Map(o => o.date1).ToColumn("date1");
        }
    }

}