using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MSS.API.Common.Utility;
using SZY.Platform.WebApi.Service;
using System;

namespace SZY.Platform.WebApi.Infrastructure
{
    public static class EssentialServiceCollectionExtensions
    {
        public static IServiceCollection AddEssentialService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthHelper, AuthHelper>();
            services.AddTransient<IConstructionPlanService, ConstructionPlanService>();
            services.AddTransient<IConstructionPlanImportService, ConstructionPlanImportService>();
            services.AddTransient<IConstructionPlanMonthDetailService, ConstructionPlanMonthDetailService>();
            services.AddTransient<IWorkTaskService, WorkTaskService>();
            services.AddTransient<IWfprocessService, WfprocessService>();
            services.AddTransient<IConstructionPlanMonthChartService, ConstructionPlanMonthChartService>();

            services.AddTransient<IMaintenanceService, MaintenanceService>();
            return services;
        }
    }
}
