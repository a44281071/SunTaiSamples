using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace HandleLastInputTime
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      //StartScreenSaver();
      TestTempFile();

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }

    private static void TestTempFile()
    {
      try
      {
        string tempFile = Path.GetTempFileName();
        Console.WriteLine(tempFile);
        File.WriteAllText(tempFile, "我是内容");

        string newFile = "c:/aaa.txt";
        File.Copy(tempFile, newFile);
        File.Delete(tempFile);

        Console.WriteLine(File.ReadAllText(newFile));
        File.Delete(newFile);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private static void StartScreenSaver()
    {
      ScreenSaver ss = new ScreenSaver();
      ss.NotifyNoOperation += Ss_NotifyNoOperation;
      ss.Start();
    }

    private static void Ss_NotifyNoOperation(object sender, EventArgs e)
    {
      Console.WriteLine($"{DateTime.Now} - Too long time not receive input.");
    }
  }
}