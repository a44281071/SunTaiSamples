using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MpvNet.Native;

namespace WpfMpvPlayer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private string videoPath = @"D:\temp\videos\VID_20241115_085028.mp4";

    private static byte[] GetUtf8Bytes(string s)
    {
        return Encoding.UTF8.GetBytes(s + "\0");
    }

    private static IntPtr AllocateUtf8IntPtrArrayWithSentinel(string[] arr, out IntPtr[] byteArrayPointers)
    {
        int numberOfStrings = arr.Length + 1; // add extra element for extra null pointer last (sentinel)
        byteArrayPointers = new IntPtr[numberOfStrings];
        IntPtr rootPointer = Marshal.AllocCoTaskMem(IntPtr.Size * numberOfStrings);
        for (int index = 0; index < arr.Length; index++)
        {
            var bytes = GetUtf8Bytes(arr[index]);
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
            byteArrayPointers[index] = unmanagedPointer;
        }
        Marshal.Copy(byteArrayPointers, 0, rootPointer, numberOfStrings);
        return rootPointer;
    }

    private void DoMpvCommand(nint hMpv, params string[] args)
    {
        IntPtr[] byteArrayPointers;
        var mainPtr = AllocateUtf8IntPtrArrayWithSentinel(args, out byteArrayPointers);

        LibMpv.mpv_command(hMpv, mainPtr);

        foreach (var ptr in byteArrayPointers)
        {
            Marshal.FreeHGlobal(ptr);
        }
        Marshal.FreeHGlobal(mainPtr);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var wfPic = new System.Windows.Forms.PictureBox();
        wfHost.Child = wfPic;

        var hHost = wfPic.Handle;
        var hHost64 = hHost.ToInt64();

        var hMpv = LibMpv.mpv_create();
        LibMpv.mpv_initialize(hMpv);

        // _mpvSetOptionString(_mpvHandle, GetUtf8Bytes("keep-open"), GetUtf8Bytes("always"));

        LibMpv.mpv_set_option(hMpv, GetUtf8Bytes("wid"), LibMpv.mpv_format.MPV_FORMAT_INT64, ref hHost64);

        DoMpvCommand(hMpv, "loadfile", videoPath);
    }
}