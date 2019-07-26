using Autofac;
using Autofac.Extensions.DependencyInjection;
using AxResto.Apertura.Pagos.Web.Config;
using AxResto.Apertura.Pagos.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AxResto.Apertura.Pagos.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            //Console.WriteLine(env.ContentRootPath);
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices();

            #region Configuración de log4net
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
           
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            #endregion

            #region Inyección nativa Aspnet core. Se agrega las secciones de appSettings.
            services.Configure<AmipassConfig>(Configuration.GetSection("Amipass"));
            services.Configure<PipolConfig>(Configuration.GetSection("Pipol"));
            #endregion

            var builder = new ContainerBuilder();
            ServicesRegisterHelper.Register(builder);
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
