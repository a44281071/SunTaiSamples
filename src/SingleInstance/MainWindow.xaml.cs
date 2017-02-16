using System.Windows;

namespace SingleInstance
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void btnRestart_Click(object sender, RoutedEventArgs e)
    {
      App.Restart();
    }
  }
}