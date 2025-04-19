using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Test.Services;

public class NotificationService: INotificationService
{
    public async Task ShowSuccessAsync(string title, string message)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Success);
        await box.ShowAsync();
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Error);
        await box.ShowAsync();
    }

    public async Task ShowWarningAsync(string title, string message)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Warning);
        await box.ShowAsync();
    }
}