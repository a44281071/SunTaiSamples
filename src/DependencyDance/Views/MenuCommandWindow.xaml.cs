using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DependencyDance.Views
{
  /// <summary>
  /// MenuCommandWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MenuCommandWindow : Window
  {
    public MenuCommandWindow()
    {
      InitializeComponent();
    }

    public MenuCommandViewModel ViewModel { get; } = new MenuCommandViewModel();
  }

  public class BindingProxy : Freezable
  {
    #region Overrides of Freezable

    protected override Freezable CreateInstanceCore()
    {
      return new BindingProxy();
    }

    #endregion

    public object Data
    {
      get { return (object)GetValue(DataProperty); }
      set { SetValue(DataProperty, value); }
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
  }
}
