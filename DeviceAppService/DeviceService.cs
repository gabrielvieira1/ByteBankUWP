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
using Windows.System;

namespace DeviceAppService
{
  public sealed class DeviceService : IBackgroundTask
  {
    private BackgroundTaskDeferral backgroundTaskDeferral;
    private AppServiceConnection appServiceconnection;

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
      var messageDeferral = args.GetDeferral();

      ValueSet message = args.Request.Message;
      ValueSet returnData = new ValueSet();

      if (message.TryGetValue("FolderPath", out object folderPath))
      {
        returnData.Add("Result", "Success");
        returnData.Add("Status", "OK");
      }
      else
      {
        returnData.Add("Status", "Error");
        returnData.Add("ErrorMessage", "Parâmetros inválidos");
      }

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
