using Autofac;
using Autofac.Core;
using AxResto.Amipass.Plugin;
using AxResto.Pipol.Plugin;
using log4net;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AxResto.Apertura.Pagos.Web.Helpers
{
    internal class LogInjectionModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter((p, i) => p.ParameterType == typeof(ILog), (p, i) => LogManager.GetLogger(t))
            });
        }
    }


    /// <summary>
    /// Clase estatica para resolver la inyeccion de dependencias
    /// </summary>
    public static class ServicesRegisterHelper
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterType<Amipass_Imp>().As<IAmipass>();
            builder.RegisterType<Pipol_Imp>().As<IPipol>();

            //inject Log4Net dependency
            builder.RegisterModule(new LogInjectionModule());
        }
    }
}
