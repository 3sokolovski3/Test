using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Test.Services;

namespace Test.ViewModels;

public class TestsViewModel: ViewModelBase
{
    
    private readonly ITestService _testService;
    private readonly INavigationService _navigationService;
    private ObservableCollection<Models.Test> _tests = new();
    private readonly INotificationService _notificationService; 
    public ObservableCollection<Models.Test> Tests
    {
        get => _tests;
        set => this.RaiseAndSetIfChanged(ref _tests, value);
    }

    public ReactiveCommand<Models.Test, Unit> GoTestCommand { get; }
    public ReactiveCommand<Unit, Unit> GoCreateTestCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadTestsCommand { get; }

    public TestsViewModel(
        ITestService testService, 
        INavigationService navigationService,
        INotificationService notificationService)
    {
        _testService = testService ?? throw new ArgumentNullException(nameof(testService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        
        GoTestCommand = ReactiveCommand.Create<Models.Test>(GoTest);
        GoCreateTestCommand = ReactiveCommand.Create(GoCreateTest);
        LoadTestsCommand = ReactiveCommand.CreateFromTask(LoadTestsAsync);

        LoadTestsCommand.Execute().Subscribe();
    }

    private async Task LoadTestsAsync()
    {
        try
        {
            var tests = await _testService.GetTestsAsync();
            Tests = new ObservableCollection<Models.Test>(tests);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки тестов: {ex.Message}");
            Tests = new ObservableCollection<Models.Test>();
        }
    }

    private void GoTest(Models.Test test)
    {
        _navigationService.NavigateTo(new QuestionsViewModel(
            test, 
            _testService, 
            _navigationService,
            _notificationService));
    }

    public void GoCreateTest()
    {
        
        _navigationService.NavigateTo(new AddTestViewModel(
            _testService, 
            _navigationService,
            _notificationService));
    }
}

