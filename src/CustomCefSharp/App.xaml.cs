using System.Windows;

namespace CustomCefSharp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigCefSharp.Build();
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                ConfigCefSharp.Exit();
            }
            catch (System.Exception ex)
            {
                throw;
            }
            base.OnExit(e);
        }
    }
}