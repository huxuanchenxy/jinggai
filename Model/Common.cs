
using Dapper.FluentMap.Mapping;
using SZY.Platform.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Data;

// Coded by admin 2019/9/26 16:46:46
namespace SZY.Platform.WebApi.Model
{
    public static class Common
    {
        public const int WORK_TYPE = 111;
        public const int PM_TYPE = 119;

        public static string GetLastDay(int month, int year)
        {
            switch (month)
            {
                case 4:
                case 6:
                case 9:
                case 11:
                    return "30";
                case 2:
                    if (year % 4 == 0) return "29";
                    else return "28";
                default:
                    return "31";
            }
        }

    }

    public enum ItemType
    {
        IsStr=0,
        IsBool = 1
    }

    public enum PMTYPE
    {
        Day = 120,
        Month = 121,
        Quarter=122,
        Week=123,
        HalfMonth=124,
        Year=125
    }

    public enum PMStatus
    {
        Init=177,
        Editing=178,
        Finished=179
    }
}