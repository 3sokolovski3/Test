using System.Threading.Tasks;

namespace Test.Services;

public interface INotificationService
{
    Task ShowSuccessAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
}