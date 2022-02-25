using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MSS.API.Common;
using SZY.Platform.WebApi.Model;
using SZY.Platform.WebApi.Service;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using MQTTnet;
using MQTTnet.Client.Options;
using System.Threading;

namespace SZY.Platform.WebApi.Controllers
{
    //[Route("api/v1/[controller]")]
    [ApiController]
    public class WfController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private QuartzStart _quart;

        private readonly IWorkTaskService _service;
        public WfController(IWorkTaskService service, ISchedulerFactory schedulerFactory, QuartzStart quart)
        {
            _service = service;
            this._schedulerFactory = schedulerFactory;
            _quart = quart;
        }

        [HttpGet("QueryReadyTasks")]
        public async Task<ActionResult<ApiResult>> QueryReadyTasks([FromQuery] WorkTaskQueryParm parm)
        {
            ApiResult ret = new ApiResult { code = Code.Failure };
            try
            {
                ret = await _service.GetReadyTasks(parm);

            }
            catch (System.Exception ex)
            {
                ret.msg = string.Format(
                    "获取当前用户待办任务数据失败, 异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        [HttpPost("JingGaiData")]
        public async Task<ActionResult<JingGaiRet>> JingGaiData(JingGaiObj parm)
        {
            JingGaiRet ret = new JingGaiRet {  success = true };
            try
            {

                //ret = await _service.QueryReadyActivityInstance(parm);
                JingGai data = new JingGai() { device_id = parm.device_id,
                    device_type = parm.device_type,
                    fv = parm.data.fv,
                    soc = parm.data.soc,
                    sor = parm.data.sor,
                    rtd = parm.data.rtd,
                    rad = parm.data.rad,
                    rqd = parm.data.rqd,
                    sol = parm.data.sol,
                    lng = parm.data.lng,
                    lat = parm.data.lat,
                    sensor_water_level = parm.data.sensor_water_level,
                    sensor_water_depth = parm.data.sensor_water_depth,
                    sensor_temperature = parm.data.sensor_temperature,
                    sensor_humidity = parm.data.sensor_humidity,
                    sensor_smoke = parm.data.sensor_smoke,
                    sensor_ch4 = parm.data.sensor_ch4,
                    sensor_toxic = parm.data.sensor_toxic,
                    sensor_water_alarm = parm.data.sensor_water_alarm,
                    sensor_water_warn = parm.data.sensor_water_warn,
                    sensor_ph = parm.data.sensor_ph,
                    sensor_ch4_conc = parm.data.sensor_ch4_conc,
                    sensor_toxic_conc = parm.data.sensor_toxic_conc,
                    date1 = DateTime.Now.ToString() 
                };
                await _service.AddJingGai(data);
            }
            catch (System.Exception ex)
            {
                ret.errmsg = string.Format(
                    "异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }


        [HttpPost("JingGaiDevice")]
        public async Task<ActionResult<JingGaiRet>> JingGaiDevice(JingGaiDevice parm)
        {
            JingGaiRet ret = new JingGaiRet { success = true };
            try
            {

                //ret = await _service.QueryReadyActivityInstance(parm);
                JingGaiDevice data = new JingGaiDevice()
                {
                    device_id = parm.device_id,
                    device_type = parm.device_type,
                    device_name = parm.device_name,
                    install_addr = parm.install_addr,
                    install_time = parm.install_time,
                    lng = parm.lng,
                    lat = parm.lat,
                    date1 = DateTime.Now.ToString()
                };
                await _service.AddJingGaiDevice(data);
            }
            catch (System.Exception ex)
            {
                ret.errmsg = string.Format(
                    "异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }


        [HttpPost("JingGaiAlarm")]
        public async Task<ActionResult<JingGaiRet>> JingGaiAlarm(JingGaiAlarmObj parm)
        {
            JingGaiRet ret = new JingGaiRet { success = true };
            try
            {
                await Publish_Application_Message();
                List<JingGaiAlarm> jgalarmlist = new List<JingGaiAlarm>();
                jgalarmlist.Add(new Model.JingGaiAlarm() { alarm_item = "soc", alarm_item_name = "剩余电量" });
                jgalarmlist.Add(new Model.JingGaiAlarm() { alarm_item = "rtd", alarm_item_name = "实时温度" });
                jgalarmlist.Add(new Model.JingGaiAlarm() { alarm_item = "rad", alarm_item_name = "倾斜角度" });
                jgalarmlist.Add(new Model.JingGaiAlarm() { alarm_item = "rqd", alarm_item_name = "震动幅度" });
                jgalarmlist.Add(new Model.JingGaiAlarm() { alarm_item = "sensor_water_warn", alarm_item_name = "水位预警" });

                JingGaiDevicePageView device = await _service.GetPageJingGaiDevice(new JingGaiDeviceParm() { });
                if (parm != null)
                {
                    string device_id = parm.device_id;
                    string device_type = parm.device_type;
                    string device_name = string.Empty;
                    string device_addr = string.Empty;
                    if (device.rows != null && device.rows.Count > 0)
                    {
                        List<JingGaiDevice> list = device.rows;
                        JingGaiDevice deviceobj = list.Where(c=>c.device_id == device_id).FirstOrDefault();
                        device_name = deviceobj.device_name;
                        device_addr = deviceobj.install_addr;
                    }
                    if (parm.alarm_data != null && parm.alarm_data.Count > 0)
                    {
                        for (int i = 0; i < parm.alarm_data.Count; i++)
                        {
                            string alarm_item = parm.alarm_data[i].alarm_item;
                            string alarm_value = parm.alarm_data[i].alarm_value;
                            var temp  = jgalarmlist.Where(c => c.alarm_item == alarm_item).FirstOrDefault();
                            string alarm_item_name = string.Empty;
                            if (temp != null)
                            {
                                alarm_item_name = temp.alarm_item_name;
                            }
                            
                            var threshold_conf = parm.alarm_data[i].threshold_conf;
                            if (threshold_conf != null && threshold_conf.Count > 0)
                            {
                                for (int j = 0; j < threshold_conf.Count; j++)
                                {
                                    string compare_type = threshold_conf[j].compare_type;
                                    string threshold = threshold_conf[j].threshold;
                                    
                                    JingGaiAlarm obj = new JingGaiAlarm() { device_id = device_id
                                        , device_type = int.Parse(device_type)
                                        ,device_name = device_name
                                    , device_addr = device_addr
                                    , alarm_item = alarm_item
                                    , alarm_value = alarm_value
                                    ,alarm_item_name = alarm_item_name
                                    , compare_type = compare_type
                                    , threshold = threshold,
                                    date1 = DateTime.Now.ToString()};
                                    await _service.AddJingGaiAlarm(obj);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ret.errmsg = string.Format(
                    "异常信息:{0}",
                    ex.Message);
            }
            return ret;
        }

        public static async Task Publish_Application_Message()
        {
            /*
             * This sample pushes a simple application message including a topic and a payload.
             *
             * Always use builders where they exist. Builders (in this project) are designed to be
             * backward compatible. Creating an _MqttApplicationMessage_ via its constructor is also
             * supported but the class might change often in future releases where the builder does not
             * or at least provides backward compatibility where possible.
             */

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    //.WithTcpServer("broker.hivemq.com")
                    .WithWebSocketServer("ws://47.101.220.2:8083/mqtt")
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("alarm")
                    .WithPayload(DateTime.Now.ToString())
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                Console.WriteLine("MQTT application message is published.");
            }
        }

    }



    public class JingGaiRet
    {
        public bool success { get; set; }
        public string errmsg { get; set; }
    }


    //如果好用，请收藏地址，帮忙分享。
    public class JingGaiData
    {
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
    }

    public class JingGaiObj
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// 设备类型 1:井盖
        /// </summary>
        public int device_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public JingGaiData data { get; set; }
    }

    public class ThresholdConf
    {
        public string compare_type { get; set; }
        public string threshold { get; set; }

    }

    public class JingGaiAlarmData
    {
        public string alarm_item { get; set; }
        public string alarm_value { get; set; }
        public List<ThresholdConf> threshold_conf { get; set; }
    }

    public class JingGaiAlarmObj
    {
        public string device_id { get; set; }
        public string device_type { get; set; }
        public List<JingGaiAlarmData> alarm_data { get; set; }
    }

}
