using Autofac;
using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace CaliburnMicroDance
{
  internal class AppAutofacBootStrapper : BootstrapperBase
  {
    private IContainer container;

    public AppAutofacBootStrapper()
    {
      Initialize();
    }

    protected override void Configure()
    {
      var builder = new ContainerBuilder();

      builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
      builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
      builder.RegisterType<ShellViewModel>().As<IShell>();

      container = builder.Build();
    }

    protected override object GetInstance(Type service, string key)
    {
      return String.IsNullOrEmpty(key)
           ? container.Resolve(service)
           : container.ResolveKeyed(key, service);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
      return container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
    }

    protected override void BuildUp(object instance)
    {
      container.InjectProperties(instance);
    }

    protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
      DisplayRootViewFor<IShell>();
    }
  }
}