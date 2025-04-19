using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Test.Services;

namespace Test.ViewModels;

public class AddTestViewModel:ViewModelBase
{  
    private readonly ITestService _testService;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
        
    private ObservableCollection<Models.Test> _tests = new();
    private string _testName = string.Empty;
    private string _testDescription = string.Empty;
    private bool _isTestCreationVisible;
    public ObservableCollection<Models.Test> Tests
    {
        get => _tests;
        private set => this.RaiseAndSetIfChanged(ref _tests, value);
    }

    public string TestName
    {
        get => _testName;
        set => this.RaiseAndSetIfChanged(ref _testName, value);
    }

    public string TestDescription
    {
        get => _testDescription;
        set => this.RaiseAndSetIfChanged(ref _testDescription, value);
    }

    public bool IsTestCreationVisible
    {
        get => _isTestCreationVisible;
        private set => this.RaiseAndSetIfChanged(ref _isTestCreationVisible, value);
    }
    

    public ReactiveCommand<Models.Test, Unit> NavigateToAddQuestionsCommand { get; }//установлен
    public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }//установлен
    public ReactiveCommand<Unit, Unit> CreateTestCommand { get; }//установлен
    public ReactiveCommand<Unit, Unit> ToggleTestCreationVisibilityCommand { get; }//


        public AddTestViewModel(/*
            Models.Test test,*/
            ITestService testService,
            INavigationService navigationService,
            INotificationService notificationService)
        {
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            // Инициализация команд
            NavigateToAddQuestionsCommand = ReactiveCommand.Create<Models.Test>(NavigateToAddQuestions);
            NavigateBackCommand = ReactiveCommand.Create(NavigateBack);
            CreateTestCommand = ReactiveCommand.CreateFromTask(CreateTestAsync);
            ToggleTestCreationVisibilityCommand = ReactiveCommand.Create(ToggleTestCreationVisibility);
            IsTestCreationVisible = false;
            // Загрузка тестов
            LoadTests();
        }
        


        private async void LoadTests()
        {
            try
            {
                var tests = await _testService.GetTestsAsync(); // Используем GetTestsAsync
                Tests = new ObservableCollection<Models.Test>(tests);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Ошибка загрузки тестов", ex.Message);
            }
        }

        private void ToggleTestCreationVisibility()
        {
            IsTestCreationVisible = !IsTestCreationVisible;
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo(new TestsViewModel(
                _testService, 
                _navigationService,
                _notificationService));
        }

        private void NavigateToAddQuestions(Models.Test test)
        {
           _navigationService.NavigateTo(
                new AddQuestionsViewModel(
                    test, 
                    _testService, 
                    _navigationService,
                    _notificationService));
        }

        private async Task CreateTestAsync()
        {
            if (string.IsNullOrWhiteSpace(TestName))
            {
                await _notificationService.ShowWarningAsync("Ошибка", "Название теста обязательно");
                return;
            }

            try
            {
                var newTest = new Models.Test
                {
                    Name = TestName.Trim(),
                    Description = TestDescription?.Trim() ?? string.Empty
                };
                await _testService.CreateTestAsync(newTest);
                await _notificationService.ShowSuccessAsync("Успех", "Тест успешно создан");
                
                _navigationService.NavigateTo(
                    new AddQuestionsViewModel(
                        newTest, 
                        _testService, 
                        _navigationService,
                        _notificationService));
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Ошибка создания теста", ex.Message);
            }
        }
}
