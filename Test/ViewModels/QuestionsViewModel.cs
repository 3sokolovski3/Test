using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Test.Models;
using Test.Services;

namespace Test.ViewModels;

public class QuestionsViewModel : ViewModelBase
{
    private readonly Models.Test _test;
    private readonly ITestService _testService;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    
    private string? _userTextAnswer;
    private ObservableCollection<Qusestion> _qList = new();

    public ObservableCollection<Qusestion> QList
    {
        get => _qList;
        set => this.RaiseAndSetIfChanged(ref _qList, value);
    }

    public static ObservableCollection<Answer> AnswersList { get; } = new();

    public string UserTextAnswer
    {
        get => _userTextAnswer ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _userTextAnswer, value);
    }

    public ReactiveCommand<Unit, Unit> LoadQuestionsCommand { get; }
    public ReactiveCommand<Answer, Unit> OnAnswerSelectedCommand1 { get; }
    public ReactiveCommand<Answer, Unit> OnAnswerSelectedCommand2 { get; }
    public ReactiveCommand<string, Unit> OnAnswerSelectedCommand3 { get; }
    public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
    public ReactiveCommand<Unit, Unit> CheckAnswerCommand { get; }

    public QuestionsViewModel(
        Models.Test test,
        ITestService testService,
        INavigationService navigationService,
        INotificationService notificationService)
    {
        _test = test ?? throw new ArgumentNullException(nameof(test));
        _testService = testService ?? throw new ArgumentNullException(nameof(testService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        // Инициализация команд
        LoadQuestionsCommand = ReactiveCommand.CreateFromTask(LoadQuestionsAsync);
        OnAnswerSelectedCommand1 = ReactiveCommand.CreateFromTask<Answer>(OnAnswerSelectedAsync);
        OnAnswerSelectedCommand2 = ReactiveCommand.CreateFromTask<Answer>(OnAnswerSelectedChAsync);
        OnAnswerSelectedCommand3 = ReactiveCommand.CreateFromTask<string>(OnAnswerSelectedTextAsync);
        GoBackCommand = ReactiveCommand.Create(GoBack);
        CheckAnswerCommand = ReactiveCommand.CreateFromTask(CheckAnswersAsync);

        LoadQuestionsCommand.Execute().Subscribe();
    }

    private async Task LoadQuestionsAsync()
    {
        try
        {
            var questions = await _testService.GetQuestionsForTestAsync(_test.Id);
            QList = new ObservableCollection<Qusestion>(questions);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync("Ошибка загрузки вопросов", ex.Message);
            QList = new ObservableCollection<Qusestion>();
        }
    }

    private async Task OnAnswerSelectedAsync(Answer answer)
    {
        try
        {
            if (answer.Question == null) return;

            await Task.Run(() =>
            {
                var oldAnswer = AnswersList.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
                if (oldAnswer != null) AnswersList.Remove(oldAnswer);
                AnswersList.Add(answer);
            });
        }
        catch (Exception ex)
        {
            await ShowErrorAsync("Ошибка", ex.Message);
        }
    }

    private async Task OnAnswerSelectedChAsync(Answer answer)
    {
        try
        {
            if (answer.Question == null) return;

            await Task.Run(() =>
            {
                if (!AnswersList.Any(x => x.QuestionId == answer.QuestionId))
                {
                    AnswersList.Add(answer);
                }
            });
        }
        catch (Exception ex)
        {
            await ShowErrorAsync("Ошибка", ex.Message);
        }
    }

    private async Task OnAnswerSelectedTextAsync(string questionText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(questionText)) return;

            var result = await _testService.CheckTextAnswerAsync(questionText, UserTextAnswer);

            var answer = new Answer
            {
                QuestionId = result.QuestionId,
                Text = UserTextAnswer,
                IsCorrect = result.IsCorrect ? 1 : 0
            };

            await Task.Run(() =>
            {
                var oldAnswer = AnswersList.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
                if (oldAnswer != null) AnswersList.Remove(oldAnswer);
                AnswersList.Add(answer);
            });
        }
        catch (Exception ex)
        {
            await ShowErrorAsync("Ошибка", ex.Message);
        }
    }

    private void GoBack()
    {
        AnswersList.Clear();
        _navigationService.NavigateTo(new TestsViewModel(
            _testService, 
            _navigationService,
            _notificationService));
    }

    private async Task CheckAnswersAsync()
    {
        if (AnswersList.Count == 0)
        {
            await ShowErrorAsync("Info", "Вы не ответили ни на один вопрос");
            return;
        }

        try
        {
            var correctCount = AnswersList.Count(row => row.IsCorrect == 1);
            await ShowErrorAsync("Info", $"Тест окончен.\nВы набрали {correctCount} балла(ов)");
        }
        finally
        {
            AnswersList.Clear();
        }
    }
    private async Task ShowErrorAsync(string title, string message)
    {
        await MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.Ok)
            .ShowWindowAsync();
    }
}