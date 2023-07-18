using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

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

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

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

    private static string folderPath = null;
    private static bool isToggleOn = false;

    static void Main(string[] args)
    {
      if (args.Length > 0)
      {
        ExtractArguments(args[0]);

        //if ((!string.IsNullOrEmpty(folderPath)) && (IsValidFolderPath(folderPath)))
        if (!string.IsNullOrEmpty(folderPath))
        {
          if (isToggleOn)
          {
            CreateFolder(folderPath);
          }
          else
          {
            DeleteFolder(folderPath);
          }
        }
      }
    }

    private static void ExtractArguments(string path)
    {
      folderPath = ExtractParameters(path, "folder");
      isToggleOn = Convert.ToBoolean(ExtractParameters(path, "toggleSwitch"));

    }

    private static string ExtractParameters(string path, string parameter)
    {
      string folderPath = null;
      Match folderMatch = Regex.Match(path, $"{parameter}=([^?]+)");
      if (folderMatch.Success)
      {
        folderPath = folderMatch.Groups[1].Value;
      }
      return folderPath;
    }
    private static bool IsValidFolderPath(string folderPath)
    {
      string byteBankFolderPath = Path.GetFullPath(@"C:\ByteBank");

      DirectoryInfo applicationDataFolder = new DirectoryInfo(byteBankFolderPath);
      DirectoryInfo targetFolder = new DirectoryInfo(folderPath);

      bool folderValid = targetFolder.FullName.StartsWith(applicationDataFolder.FullName, StringComparison.OrdinalIgnoreCase);

      if (folderValid)
        return true;
      else
        MessageBoxW(IntPtr.Zero, "Caminho do diretório inválido", "Byte Bank", 0);
      return false;
    }
    private static void CreateFolder(string folderPath)
    {
      try
      {
        Directory.CreateDirectory(folderPath);

        string logFilePath = Path.Combine(folderPath, "log.txt");

        using (StreamWriter writer = File.CreateText(logFilePath))
        {
          for (int i = 0; i < 10; i++)
          {
            string logLine = $"{DateTime.Now.ToString()} - Click event #{i + 1}";
            writer.WriteLine(logLine);
          }
        }

        //MessageBoxW(IntPtr.Zero, "Pasta criada com sucesso", "Byte Bank", 0);
      }
      catch (Exception ex)
      {
        MessageBoxW(IntPtr.Zero, "Falha ao criar a pasta " + ex.Message, "Byte Bank", 0);
      }
    }
    private static void DeleteFolder(string folderPath)
    {
      try
      {
        Directory.Delete(folderPath, true);
        MessageBoxW(IntPtr.Zero, "Todos os seus registros foram excluídos", "Byte Bank", 0);
      }
      catch (Exception ex)
      {
        MessageBoxW(IntPtr.Zero, "Falha ao criar a pasta " + ex.Message, "Byte Bank", 0);
      }
    }
    private static bool IsValidFolderPathInAppData(string folderPath)
    {
      string applicationDataFolderPath = ApplicationData.Current.LocalFolder.Path;

      DirectoryInfo applicationDataFolder = new DirectoryInfo(applicationDataFolderPath);
      DirectoryInfo targetFolder = new DirectoryInfo(folderPath);

      return targetFolder.FullName.StartsWith(applicationDataFolder.FullName, StringComparison.OrdinalIgnoreCase);
    }
    private static bool CreateFolderWithWin32(string folderPath)
    {
      return CreateDirectory(folderPath, IntPtr.Zero);
    }
    private static bool DeleteFolderWithWin32(string folderPath)
    {
      SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
      {
        wFunc = FO_DELETE,
        pFrom = folderPath + "\0\0",
        fFlags = FOF_SILENT | FOF_NOCONFIRMATION | FOF_NOERRORUI | FOF_WANTNUKEWARNING
      };

      int result = SHFileOperation(ref fileOp);
      bool success = (result == 0);
      return success;
    }
  }
}
