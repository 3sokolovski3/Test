using System;
using Test.ViewModels;

namespace Test.Services;

public class NavigationService : INavigationService
{
    public event Action<ViewModelBase>? CurrentViewModelChanged;

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentViewModelChanged?.Invoke(viewModel);
    }
    /*public event Action<ViewModelBase>? CurrentViewModelChanged;

    public void NavigateTo(ViewModelBase viewModel)
    {
        CurrentViewModelChanged?.Invoke(viewModel);
    }
    public ViewModelBase CurrentViewModel { get; set; }
    
    private readonly MainWindowViewModel _mainViewModel;
        
    public event Action<ViewModelBase>? NavigationRequested;

    public NavigationService(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
            
        // Подписываемся на собственное событие
        NavigationRequested += OnNavigationRequested;
    }



    private void OnNavigationRequested(ViewModelBase viewModel)
    {
        _mainViewModel.CurrentViewModel = viewModel;
    }*/
}