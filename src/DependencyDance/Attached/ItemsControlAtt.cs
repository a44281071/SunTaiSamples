using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DependencyDance
{
  public class ItemsControlAtt
  {
    #region IsScrollToEnd

    public static readonly DependencyProperty IsScrollToEndProperty =
        DependencyProperty.RegisterAttached("IsScrollToEnd", typeof(bool), typeof(ItemsControlAtt),
            new FrameworkPropertyMetadata((bool)false,
                new PropertyChangedCallback(OnIsScrollToEndChanged)));

    public static bool GetIsScrollToEnd(ItemsControl d)
    {
      return (bool)d.GetValue(IsScrollToEndProperty);
    }

    public static void SetIsScrollToEnd(ItemsControl d, bool value)
    {
      d.SetValue(IsScrollToEndProperty, value);
    }

    private static void OnIsScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      bool oldIsScrollToEnd = (bool)e.OldValue;
      bool newIsScrollToEnd = (bool)d.GetValue(IsScrollToEndProperty);

      var itemsControl = d as ItemsControl;
      if (itemsControl == null)
        return;

      if (newIsScrollToEnd)
      {
        itemsControl.Loaded += (ss, ee) =>
            {
              ScrollViewer scrollviewer = WpfTreeHelper.FindChild<ScrollViewer>(itemsControl);
              if (scrollviewer != null)
              {
                ((ICollectionView)itemsControl.Items).CollectionChanged += (sss, eee) =>
                            {
                          scrollviewer.ScrollToEnd();
                        };
              }
            };
      }
    }

    #endregion IsScrollToEnd
  }
}