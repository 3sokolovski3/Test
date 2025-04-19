using ReactiveUI;
using Test.Services;

namespace Test.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public MainWindowViewModel(INavigationService navigationService, ITestService testService , INotificationService notificationService)
    {
        // Подписываемся на изменения навигации
        navigationService.CurrentViewModelChanged += viewModel => 
        {
            CurrentViewModel = viewModel;
        };

        // Устанавливаем начальный ViewModel
        navigationService.NavigateTo(new TestsViewModel(
            testService, 
            navigationService,
            notificationService));
    }
}