
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;


// Coded by admin 2020/5/29 14:19:11
namespace SZY.Platform.WebApi.Model
{
    public class MyfundParm : BaseQueryParm
    {

    }
    public class MyfundPageView
    {
        public List<Myfund> rows { get; set; }
        public int total { get; set; }
    }

    public class Myfund : BaseEntity
    {
        public int Num { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal Costavg { get; set; }
        public decimal Networth { get; set; }
        public decimal Totalworth { get; set; }
        public System.DateTime Worthdate { get; set; }
        public decimal Daygrowth { get; set; }
        public System.DateTime Updatetime { get; set; }
        public decimal Percent { get; set; }//持仓比例
        public decimal PercentGrowth { get; set; }//成本收益率
        public decimal ExpectGrowth { get; set; }//估值涨幅
    }
    public class FundRetComm
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<FundRet> data { get; set; }
    }

    public class Datas
    {
        /// <summary>
        /// 
        /// </summary>
        public string FCODE { get; set; }
        /// <summary>
        /// 广发中证传媒ETF联接A
        /// </summary>
        public string SHORTNAME { get; set; }
        /// <summary>
        /// 联接基金
        /// </summary>
        public string FTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FUNDTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BFUNDTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FEATURE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PLTDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DWJZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FSRQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RZDF { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYI { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LJJZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYIRANK { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYIDSC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYL_Y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RANKM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MSC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYL_3Y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RANKQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QSC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYL_6Y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RANKHY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HYSC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SYL_1N { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RANKY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string YSC { get; set; }
        /// <summary>
        /// 开放申购
        /// </summary>
        public string SGZT { get; set; }
        /// <summary>
        /// 开放赎回
        /// </summary>
        public string SHZT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SOURCERATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SOURCEHRG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MINRG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MINSG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MINDT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SSBCFMDATA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ISSBDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ISSEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KFR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CYCLE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DUEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OPEYIELD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FIXINCOME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OPESTART { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OPEEND { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RISKLEVEL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FEGM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ENDNAV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RLEVEL_SZ { get; set; }
        /// <summary>
        /// 广发基金
        /// </summary>
        public string JJGS { get; set; }
        /// <summary>
        /// 
        /// </summary>

    }

    public class DatasItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string fundcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dwjz { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gsz { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gszzl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gszze { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gztime { get; set; }
    }

    public class Root2
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DatasItem> Datas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Expansion { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public Datas Datas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Expansion { get; set; }
    }

    public class FundRet
    {
        public string code { get; set; }
        public string name { get; set; }
        public decimal netWorth { get; set; }
        public decimal totalWorth { get; set; }
        public decimal dayGrowth { get; set; }
        public DateTime worthDate { get; set; }
        public decimal expectGrowth { get; set; }
    }

    public class MyfundMap : EntityMap<Myfund>
    {
        public MyfundMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.Code).ToColumn("code");
            Map(o => o.Name).ToColumn("name");
            Map(o => o.Balance).ToColumn("balance");
            Map(o => o.Costavg).ToColumn("costavg");
            Map(o => o.Networth).ToColumn("networth");
            Map(o => o.Totalworth).ToColumn("totalworth");
            Map(o => o.Worthdate).ToColumn("worthdate");
            Map(o => o.Daygrowth).ToColumn("daygrowth");
            Map(o => o.Updatetime).ToColumn("updatetime");
            Map(o => o.ExpectGrowth).ToColumn("expectgrowth");
        }
    }

}