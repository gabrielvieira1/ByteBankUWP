using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace DeviceAppService
{
  public sealed class DeviceService : IBackgroundTask
  {
    private BackgroundTaskDeferral backgroundTaskDeferral;
    private AppServiceConnection appServiceconnection;

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(
      IntPtr hWnd,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpText,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpCaption,
      UInt32 utype);


    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern bool GetUserNameW(
      [param: MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpBuffer,
      ref UInt32 pcbBuffer);


    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool RemoveDirectory(string lpPathName);


    public void Run(IBackgroundTaskInstance taskInstance)
    {
      this.backgroundTaskDeferral = taskInstance.GetDeferral();

      taskInstance.Canceled += OnTaskCanceled;

      var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
      appServiceconnection = details.AppServiceConnection;
      appServiceconnection.RequestReceived += OnRequestReceived;
    }

    private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
      await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("ByteBankWin32");

      var messageDeferral = args.GetDeferral();

      ValueSet message = args.Request.Message;
      ValueSet returnData = new ValueSet();

      //if (message.TryGetValue("ByteBankWin32", out object byteBankWin32) &&
      //  message.TryGetValue("FolderPath", out object folderPath))
      //{
      //  // Execute o código relacionado ao processamento com os parâmetros recebidos
      //  await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync(byteBankWin32.ToString());
      //  //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("ByteBankWin32", new ValueSet { { "ByteBankWin32", "Caminho do ByteBankWin32" } });

      //  returnData.Add("Result", "Rodou");
      //  returnData.Add("Status", "OK");
      //}
      //else
      //{
      //  returnData.Add("Status", "Error");
      //  returnData.Add("ErrorMessage", "Parâmetros inválidos");
      //}

      returnData.Add("Result", "Rodou");
      returnData.Add("Status", "OK");

      await args.Request.SendResponseAsync(returnData);

      messageDeferral.Complete();
    }

    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
      if (this.backgroundTaskDeferral != null)
      {
        // Complete the service deferral.
        this.backgroundTaskDeferral.Complete();
      }
    }
  }
}
