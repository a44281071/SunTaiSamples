using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DependencyDance.Easing
{
  class SpdEase : EasingFunctionBase
  {
    private double a, w;
    private int angel = 1;

    protected override Freezable CreateInstanceCore()
    {
      return new SpdEase();
    }

    protected override double EaseInCore(double normalizedTime)
    {
      a = angel * normalizedTime;
      w = 360 / (2 * Math.PI);
      return a * Math.Sin(normalizedTime * w);
    }
  }
}
