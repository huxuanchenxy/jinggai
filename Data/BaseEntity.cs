using System;
using System.ComponentModel;

namespace SZY.Platform.WebApi.Data
{
    public abstract class BaseEntity
    {
    }

    public abstract class BaseQueryParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// asc/desc:顺序/降序
        /// </summary>
        public string order { get; set; }
    }



}