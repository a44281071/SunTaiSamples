using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DependencyDance
{
  public static class RelativeElementHelper
  {    
    public static T GetParentObject<T>(DependencyObject obj, string name = null) where T : FrameworkElement
    {
      DependencyObject parent = VisualTreeHelper.GetParent(obj);

      while (parent != null)
      {
        if (parent is T && (((T)parent).Name == name | String.IsNullOrEmpty(name)))
        {
          return (T)parent;
        }

        parent = VisualTreeHelper.GetParent(parent);
      }

      return null;
    }

    public static T GetChildObject<T>(DependencyObject obj, string name = null) where T : FrameworkElement
    {
      DependencyObject child = null;
      T grandChild = null;

      for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
      {
        child = VisualTreeHelper.GetChild(obj, i);

        if (child is T && (((T)child).Name == name | String.IsNullOrEmpty(name)))
        {
          return (T)child;
        }
        else
        {
          grandChild = GetChildObject<T>(child, name);
          if (grandChild != null)
            return grandChild;
        }
      }

      return null;

    }

    public static List<T> GetChildObjects<T>(DependencyObject obj, string name = null) where T : FrameworkElement
    {
      DependencyObject child = null;
      List<T> childList = new List<T>();

      for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
      {
        child = VisualTreeHelper.GetChild(obj, i);

        if (child is T && (((T)child).Name == name || String.IsNullOrEmpty(name)))
        {
          childList.Add((T)child);
        }

        childList.AddRange(GetChildObjects<T>(child, ""));
      }

      return childList;

    }
  }
}
