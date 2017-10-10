using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfCore;

namespace DependencyDance.Views
{
  public class MenuCommandViewModel : PropertyChangedBase
  {
    public MenuCommandViewModel()
    {
      Datas.Add(1);
      Datas.Add(2);
      Datas.Add(3);
      Datas.Add(4);
      Datas.Add(5);

      Commands.Add(new RelayCommand(ShowMessage));
    }

    public ObservableCollection<object> Datas { get; } = new ObservableCollection<object>();
    public ObservableCollection<ICommand> Commands { get; } = new ObservableCollection<ICommand>();

    private void ShowMessage()
    {
      MessageBox.Show("我是命令");
    }

  }
}
