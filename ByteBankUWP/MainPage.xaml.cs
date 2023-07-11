using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

    public MainPage()
    {
      this.InitializeComponent();
    }

    //private async void btnClick_Click(object sender, RoutedEventArgs e)
    //{
    //  if (this.deviceService == null)
    //  {
    //    this.deviceService = new AppServiceConnection();

    //    this.deviceService.AppServiceName = "com.microsoft.deviceManager";

    //    this.deviceService.PackageFamilyName = "17b8585e-3de1-4032-9cff-6321d7eeb80a_03w1eqkrn0fpt";

    //    var status = await this.deviceService.OpenAsync();

    //    if (status != AppServiceConnectionStatus.Success)
    //    {
    //      textBox.Text = "Failed to connect";
    //      this.deviceService = null;
    //      return;
    //    }

    //    int idx = int.Parse(textBox.Text);
    //    //string folderPath = textBox.Text;
    //    var message = new ValueSet();
    //    message.Add("Command", "Item");
    //    message.Add("ID", idx);
    //    //message.Add("FolderPath", folderPath);
    //    AppServiceResponse response = await this.deviceService.SendMessageAsync(message);
    //    string result = "";

    //    if (response.Status == AppServiceResponseStatus.Success)
    //    {
    //      if (response.Message["Status"] as string == "OK")
    //      {
    //        result = response.Message["Result"] as string;
    //      }
    //    }

    //    textBox.Text = result;
    //  }
    //}

    private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
      // Add the connection.
      if (this.deviceService == null)
      {
        this.deviceService = new AppServiceConnection();

        this.deviceService.AppServiceName = "com.microsoft.deviceManager";

        this.deviceService.PackageFamilyName = "17b8585e-3de1-4032-9cff-6321d7eeb80a_03w1eqkrn0fpt";

        var status = await this.deviceService.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
          textBox.Text = "Failed to connect";
          this.deviceService = null;
          return;
        }

        string folderPath = "C:\\Users\\gabri\\OneDrive\\Documentos\\Teste1";
        var message = new ValueSet();
        message.Add("FolderPath", folderPath);
        AppServiceResponse response = await this.deviceService.SendMessageAsync(message);
        string result = "";

        if (response.Status == AppServiceResponseStatus.Success)
        {
          if (response.Message["Status"] as string == "OK")
          {
            result = response.Message["Result"] as string;
          }
        }
        textBox.Text = result;
      }
    }
  }
}
