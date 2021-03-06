﻿using DependencyDance.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DependencyDance
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void MarqueeWindow_Click(object sender, RoutedEventArgs e)
    {
      new MarqueeWindow().ShowDialog();
    }

    private void MenuCommandWindow_Click(object sender, RoutedEventArgs e)
    {
      new MenuCommandWindow().ShowDialog();
    }

    private void AutoScrollToListEnd_Click(object sender, RoutedEventArgs e)
    {
      new AutoScrollToListEndWindow().ShowDialog();

    }
  }
}
