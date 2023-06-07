using System.ServiceProcess;

namespace WindowService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new PisReadEmailService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
