using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ByteBankUWP
{
  public class Library
  {
    public async void GenerateLogs()
    {
      string logsFolderPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Logs");

      if (!Directory.Exists(logsFolderPath))
      {
        Directory.CreateDirectory(logsFolderPath);
      }

      string logFilePath = Path.Combine(logsFolderPath, "log.txt");

      using (StreamWriter writer = File.CreateText(logFilePath))
      {
        for (int i = 0; i < 10; i++)
        {
          string logLine = $"{DateTime.Now.ToString()} - Click event #{i + 1}";
          writer.WriteLine(logLine);
        }
      }
    }

    public async void DeleteLogs()
    {
      try
      {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        StorageFolder logsFolder = await localFolder.GetFolderAsync("Logs");

        if (logsFolder != null)
        {
          await logsFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
          Console.WriteLine("Pasta de logs excluída com sucesso!");
        }
        else
        {
          Console.WriteLine("Pasta de logs não encontrada.");
        }
      }
      catch (FileNotFoundException)
      {
        Console.WriteLine("Pasta de logs não encontrada.");
      }
      catch (Exception ex)
      {
        Console.WriteLine("Erro ao excluir a pasta de logs: " + ex.Message);
      }
    }
  }
}
