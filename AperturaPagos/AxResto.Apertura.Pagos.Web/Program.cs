using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

namespace AxResto.Apertura.Pagos.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {           
            bool runningAsService = args.Contains("--service");

            string basePath = runningAsService ? Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                : Directory.GetCurrentDirectory();

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .Build();

            if (runningAsService)
            {
                CreateWebHostBuilder(basePath).Build().RunAsService();
            }
            else
            {
                CreateWebHostBuilder(basePath).Build().Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string basePath) =>
            new WebHostBuilder().UseKestrel()
                 .UseContentRoot(basePath)
                 .UseIISIntegration()
                 .UseStartup<Startup>();
    }
}
