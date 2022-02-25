
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System.Collections.Generic;

// Coded by admin 2019/11/9 13:42:16
namespace SZY.Platform.WebApi.Model
{
    public class WfprocessParm : BaseQueryParm
    {

    }
    public class WfprocessPageView
    {
        public List<Wfprocess> rows { get; set; }
        public int total { get; set; }
    }

    public class Wfprocess : BaseEntity
    {
        public int ID { get; set; }
        public string ProcessGUID { get; set; }
        public string ProcessName { get; set; }
        public string Version { get; set; }
        public byte IsUsing { get; set; }
        public string AppType { get; set; }
        public string PageUrl { get; set; }
        public string XmlFileName { get; set; }
        public string XmlFilePath { get; set; }
        public string XmlContent { get; set; }
        public byte StartType { get; set; }
        public string StartExpression { get; set; }
        public byte EndType { get; set; }
        public string EndExpression { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime LastUpdatedDateTime { get; set; }
    }

    public class WfprocessMap : EntityMap<Wfprocess>
    {
        public WfprocessMap()
        {
            Map(o => o.ID).ToColumn("ID");
            Map(o => o.ProcessGUID).ToColumn("ProcessGUID");
            Map(o => o.ProcessName).ToColumn("ProcessName");
            Map(o => o.Version).ToColumn("Version");
            Map(o => o.IsUsing).ToColumn("IsUsing");
            Map(o => o.AppType).ToColumn("AppType");
            Map(o => o.PageUrl).ToColumn("PageUrl");
            Map(o => o.XmlFileName).ToColumn("XmlFileName");
            Map(o => o.XmlFilePath).ToColumn("XmlFilePath");
            Map(o => o.XmlContent).ToColumn("XmlContent");
            Map(o => o.StartType).ToColumn("StartType");
            Map(o => o.StartExpression).ToColumn("StartExpression");
            Map(o => o.EndType).ToColumn("EndType");
            Map(o => o.EndExpression).ToColumn("EndExpression");
            Map(o => o.Description).ToColumn("Description");
            Map(o => o.CreatedDateTime).ToColumn("CreatedDateTime");
            Map(o => o.LastUpdatedDateTime).ToColumn("LastUpdatedDateTime");
        }
    }

}