using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ByteBankWin32
{
  internal class Program
  {
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(
     IntPtr hWnd,
     [param: MarshalAs(UnmanagedType.LPWStr)] string lpText,
     [param: MarshalAs(UnmanagedType.LPWStr)] string lpCaption,
     UInt32 utype);

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool RemoveDirectoryW(string lpPathName);

    private const int FO_DELETE = 3;
    private const int FOF_SILENT = 0x0004;
    private const int FOF_NOCONFIRMATION = 0x0010;
    private const int FOF_NOERRORUI = 0x0400;
    private const int FOF_WANTNUKEWARNING = 0x4000;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
      public IntPtr hwnd;
      public int wFunc;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pFrom;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pTo;
      public short fFlags;
      public bool fAnyOperationsAborted;
      public IntPtr hNameMappings;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string lpszProgressTitle;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

    static void Main(string[] args)
    {
      string folderPath = null;

      if (args.Length > 0)
      {
        folderPath = args[0];
        folderPath = folderPath.Substring(folderPath.IndexOf("=") + 1);
      }

      if (!string.IsNullOrEmpty(folderPath))
      {
        if (DeleteFolder(folderPath))
        {
          MessageBoxW(IntPtr.Zero, "Folder deleted successfully.", "This is window title", 0);
        }
        else
        {
          MessageBoxW(IntPtr.Zero, "Failed to delete the folder.", "This is window title", 0);
        }
      }
      else
      {
        MessageBoxW(IntPtr.Zero, "Failed to delete the folder.", "This is window title", 0);
      }
    }
    private static bool DeleteFolder(string folderPath)
    {
      SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
      {
        wFunc = FO_DELETE,
        pFrom = folderPath + "\0\0", // Duas terminações nulas para indicar o final da lista de arquivos/pastas
        fFlags = FOF_SILENT | FOF_NOCONFIRMATION | FOF_NOERRORUI | FOF_WANTNUKEWARNING
      };

      int result = SHFileOperation(ref fileOp);
      bool success = (result == 0);
      return success;
    }
  }
}
