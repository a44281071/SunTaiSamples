using CefSharp;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CustomCefSharp
{
    /// <summary>
    /// move cef sharp dependencies to sub folder
    /// </summary>
    internal static class ConfigCefSharp
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Build()
        {
            string lib, browser, locales, res;
            // Assigning file paths to varialbles
            lib = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"x86\cef\libcef.dll");
            browser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"x86\cef\CefSharp.BrowserSubprocess.exe");
            locales = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"x86\cef\locales\");
            res = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"x86\cef\");

            //var libraryLoader = new CefLibraryHandle(lib);
            //bool isValid = !libraryLoader.IsInvalid;

            var settings = new CefSettings();
            settings.BrowserSubprocessPath = browser;
            settings.LocalesDirPath = locales;
            settings.ResourcesDirPath = res;

            Cef.Initialize(settings, true, null);
        }

        internal static void Exit()
        {
            Cef.Shutdown();
        }
    }
}