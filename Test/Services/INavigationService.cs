using System;
using Test.ViewModels;

namespace Test.Services;

public interface INavigationService
{
    event Action<ViewModelBase> CurrentViewModelChanged;
    void NavigateTo(ViewModelBase viewModel);
    /*void NavigateTo(ViewModelBase viewModel);
    event Action<ViewModelBase> NavigationRequested;
    ViewModelBase CurrentViewModel { get; set; }*/
 
}