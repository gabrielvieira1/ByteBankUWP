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
    static void Main(string[] args)
    {
      //string folderPath = message["FolderPath"] as string;
      string byteBankWin32 = null;

      // Verificar se há argumentos passados na linha de comando
      if (args.Length > 0)
      {
        byteBankWin32 = args[0] as string;
        MessageBoxW(IntPtr.Zero, byteBankWin32, "This is window title", 0);
      }

      string folderPath = "C:\\Users\\gabri\\OneDrive\\Documentos\\Teste1";


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

      //string folderPath = "C:\\Users\\gv.santos\\Documents\\TesteFolderAppService";

      //Directory.Delete(folderPath);
    }
    private static bool DeleteFolder(string folderPath)
    {
      try
      {
        bool res = RemoveDirectoryW(folderPath);
        return res;
      }
      catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException || ex is Win32Exception)
      {
        return false;
      }
    }
  }
}
