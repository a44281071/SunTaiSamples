using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace DependencyDance.Views
{
  /// <summary>
  /// Auto scroll to ListBox end.
  /// <seealso cref="https://www.mgenware.com/blog/?p=408">WPF：通过附加属性使ListBox/ListView滚动条随数据自动下滑</seealso>
  /// </summary>
  public partial class AutoScrollToListEndWindow : Window
  {
    public AutoScrollToListEndWindow()
    {
      _Timer = new DispatcherTimer(TimeSpan.FromSeconds(0.5), DispatcherPriority.Background, Timer_Callback, Dispatcher);
      InitializeComponent();
    }

    private readonly DispatcherTimer _Timer;

    public ObservableCollection<string> Datas { get; } = new ObservableCollection<string>();

    private void Timer_Callback(object sender, EventArgs e)
    {
      Datas.Add(DateTime.Now.ToString());
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      _Timer.Start();
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
      _Timer.Stop();
    }
  }
}