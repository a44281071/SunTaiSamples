using System;

namespace HandleLastInputTime
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      ScreenSaver ss = new ScreenSaver();
      ss.NotifyNoOperation += Ss_NotifyNoOperation;
      ss.Start();

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }

    private static void Ss_NotifyNoOperation(object sender, EventArgs e)
    {
      Console.WriteLine($"{DateTime.Now} - Too long time not receive input.");
    }
  }
}