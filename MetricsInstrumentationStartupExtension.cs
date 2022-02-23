/* ------------------------------------------------------------------------- *
thZero.NetCore.Library.Asp.Instrumentation.Healthchecks
Copyright (C) 2016-2022 thZero.com

<development [at] thzero [dot] com>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 * ------------------------------------------------------------------------- */

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using App.Metrics;

namespace thZero.AspNetCore
{
    public class MetricsInstrumentationStartupExtension : BaseStartupExtension
    {
        #region Public Methods
        public override void ConfigureInitializePost(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            base.ConfigureInitializePost(app, env, loggerFactory, svp);

            ConfigureInitializeMetrics(app, env, loggerFactory, svp);
        }

        public override void ConfigureServicesInitializeMvcPost(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            base.ConfigureServicesInitializeMvcPost(services, env, configuration);

            ConfigureServicesInitializeMvcMetrics(services, env, configuration);
        }

        public override void ConfigureServicesInitializeMvcBuilderPre(IMvcBuilder builder)
        {
            base.ConfigureServicesInitializeMvcBuilderPre(builder);

            ConfigureServicesInitializeMvcMetrics(builder);
        }
        #endregion

        #region Protected Methods
        protected virtual void ConfigureServicesInitializeMvcMetrics(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            var metrics = AppMetrics.CreateDefaultBuilder() // configure other options
            .Build();

            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();
            services.AddMetricsReportingHostedService();
        }

        protected virtual void ConfigureServicesInitializeMvcMetrics(IMvcBuilder builder)
        {
            builder.AddMetrics();
        }

        protected virtual void ConfigureInitializeMetrics(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            app.UseMetricsAllMiddleware();
            // app.UseMetricsActiveRequestMiddleware();
            // app.UseMetricsErrorTrackingMiddleware();
            // app.UseMetricsPostAndPutSizeTrackingMiddleware();
            // app.UseMetricsRequestTrackingMiddleware();
            // app.UseMetricsOAuth2TrackingMiddleware();
            // app.UseMetricsApdexTrackingMiddleware();

            //app.UseMetricsAllEndpoints();
            app.UseMetricsEndpoint();
            // app.UseMetricsTextEndpoint();
            app.UseEnvInfoEndpoint();
        }
        #endregion
    }
}
