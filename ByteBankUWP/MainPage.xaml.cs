using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ByteBankUWP
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private AppServiceConnection deviceService;
    Library lib = new Library();

    public MainPage()
    {
      this.InitializeComponent();
    }

    private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
      string folderPath = Path.Combine("C:\\ByteBank", "Logs");
      ToggleSwitch toggleSwitch = sender as ToggleSwitch;

      if (!toggleSwitch.IsOn)
      {
        await Launcher.LaunchUriAsync(new Uri($"com.byte.bank.win32:///?folder={folderPath}?toggleSwitch=false"));
      }
      else
      {
        await Launcher.LaunchUriAsync(new Uri($"com.byte.bank.win32:///?folder={folderPath}?toggleSwitch=true"));
      }
    }

    private void GenerateLogs_Click(object sender, RoutedEventArgs e)
    {
      lib.GenerateLogs();
    }

    private void DeleteLogs_Click(object sender, RoutedEventArgs e)
    {
      lib.DeleteLogs();
    }

    private async Task<bool> createDeviceAppService()
    {
      if (this.deviceService == null)
      {
        this.deviceService = new AppServiceConnection();

        this.deviceService.AppServiceName = "device-manager";

        this.deviceService.PackageFamilyName = "17b8585e-3de1-4032-9cff-6321d7eeb80a_03w1eqkrn0fpt";

        var status = await this.deviceService.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
          //textBox.Text = "Failed to connect";
          this.deviceService = null;
          return false;
        }
      }
      return true;
    }
  }
}
