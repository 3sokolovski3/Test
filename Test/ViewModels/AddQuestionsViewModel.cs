using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using Test.Enums;
using Test.Models;
using Test.Services;

namespace Test.ViewModels;

public class AddQuestionsViewModel : ViewModelBase
{
        private readonly Models.Test _test;
        private readonly ITestService _testService;
        private readonly INavigationService _navigationService;
        private readonly INotificationService _notificationService;
        
        private string _newQuestion = string.Empty;
        private string _newAns1 = string.Empty;
        private bool _isAns1Correct;
        private QuestionTypeItem? _selectedQuestionType;

        public ObservableCollection<Answer> AnswersList { get; } = new();
        public ObservableCollection<QuestionTypeItem> QuestionTypes { get; } = new()
        {
            new QuestionTypeItem(QuestionType.RadioButton, "Одиночный выбор"),
            new QuestionTypeItem(QuestionType.CheckBox, "Множественный выбор"),
            new QuestionTypeItem(QuestionType.TextAnswer, "Текстовый ответ")
        };

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveQuestionCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveAnswerCommand { get; }

        public AddQuestionsViewModel(
            Models.Test test,
            ITestService testService,
            INavigationService navigationService,
            INotificationService notificationService)
        {
            _test = test ?? throw new ArgumentNullException(nameof(test));
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            GoBackCommand = ReactiveCommand.Create(GoBack);
            SaveQuestionCommand = ReactiveCommand.Create(SaveQuestion);
            SaveAnswerCommand = ReactiveCommand.Create(SaveAnswer);
        }

        public Models.Test Test
        {
            get => _test;
        }

        public string NewQuestion
        {
            get => _newQuestion;
            set => this.RaiseAndSetIfChanged(ref _newQuestion, value);
        }

        public string NewAns1
        {
            get => _newAns1;
            set => this.RaiseAndSetIfChanged(ref _newAns1, value);
        }

        public bool IsAns1Correct
        {
            get => _isAns1Correct;
            set => this.RaiseAndSetIfChanged(ref _isAns1Correct, value);
        }

        public QuestionTypeItem? SelectedQuestionType
        {
            get => _selectedQuestionType;
            set => this.RaiseAndSetIfChanged(ref _selectedQuestionType, value);
        }

        private void GoBack()
        {
            _navigationService.NavigateTo(new TestsViewModel(
                _testService,
                _navigationService,
                _notificationService));
        }

        private async void SaveQuestion()
        {
            if (SelectedQuestionType == null)
            {
                await _notificationService.ShowWarningAsync("Ошибка", "Выберите тип вопроса");
                return;
            }

            try
            {
                var question = new Qusestion
                {
                    Text = NewQuestion,
                    Type = (int)SelectedQuestionType.Type,
                    TestId = _test.Id
                };

                await _testService.CreateQuestionAsync(question);
                await _notificationService.ShowSuccessAsync("Успех", "Вопрос сохранен");

                ////////////////////////////////////////////////////////////////
                if (!string.IsNullOrWhiteSpace(_newAns1))
                {
                    Answer adans1 = new Answer
                    {
                        Text = _newAns1,
                        IsCorrect = IsAns1Correct ? 1 : 0,
                        QuestionId = 0
                    };
                    AnswersList.Add(adans1);
                }

                NewQuestion = string.Empty;
                SelectedQuestionType = null;

                if (AnswersList.Any())
                {
                    foreach (var i in AnswersList)
                    {
                      i.QuestionId = question.Id;
                      await _testService.SaveAnswrsAsync(i);
                    }
                }
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Ошибка", ex.Message);
            }
        }

        private void SaveAnswer()
        {
            if (string.IsNullOrWhiteSpace(NewAns1))
                return;

            AnswersList.Add(new Answer
            {
                Text = NewAns1,
                IsCorrect = IsAns1Correct ? 1 : 0,
                QuestionId = 0
            });

            NewAns1 = string.Empty;
            IsAns1Correct = false;
        }
}


