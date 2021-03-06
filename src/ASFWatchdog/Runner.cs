﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ASFWatchdog
{
    public class Runner
    {
        private static IConfiguration Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = configBuilder.Build();

            var watchdog = new Watchdog();

            CancellationTokenSource cts = new CancellationTokenSource();

            while (!cts.IsCancellationRequested)
            {
                await watchdog.InterrogateAppHealth();

                await Task.Delay(int.Parse(Configuration["schedule"]) * 1000, cts.Token);
            }
        }
    }
}