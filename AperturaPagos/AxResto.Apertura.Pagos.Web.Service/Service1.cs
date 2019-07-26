using murrayju.ProcessExtensions;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace AxResto.Apertura.Pagos.Web.Service
{
    public partial class Service1 : ServiceBase
    {
        private const string FILE_NAME_CONSOLE = "AxResto.Apertura.Pagos.Web.exe";
        private const string FILE_NAME_CONSOLE_SIN_EXE = "AxResto.Apertura.Pagos.Web";       
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            StartConsole();
        }
        protected override void OnStop()
        {
            base.OnStop();
            KillConsole();
        }
        private void StartConsole()
        {
            string Pathx = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string FileName = string.Format(@"{0}\{1}", Pathx, FILE_NAME_CONSOLE);
            try
            {
                ProcessExtensions.StartProcessAsCurrentUser(FileName, "", Pathx, false);          
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void KillConsole()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(FILE_NAME_CONSOLE_SIN_EXE))
                {
                    if (process.ProcessName.Equals(FILE_NAME_CONSOLE_SIN_EXE))
                    {
                        process.Kill();
                    }
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
