using EasyProbe.ViewModel;
using EasyProbe.ViewModel.Base;
using EasyProbe.ViewModel.Popups;
using EasyProbe.Views;
using EasyProbe.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyProbe.Service.Navigation
{
    public partial class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> mappings;
        protected Application CurrentApplication => Application.Current;

        public NavigationService()
        {
            mappings = new Dictionary<Type, Type>();
            CreatePageViewModelMappings();
        }

        void CreatePageViewModelMappings()
        {
            mappings.Add(typeof(MainViewModel), typeof(MainPage));
            mappings.Add(typeof(ListTagsViewModel), typeof(ListTags));
        }

        public async Task InitializeAsync()
        {
            await NavigateToAsync<MainViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase => InternalNavigateToAsync(typeof(TViewModel), null);
        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase => InternalNavigateToAsync(typeof(TViewModel), parameter);
        public Task NavigateToAsync(Type viewModelType) => InternalNavigateToAsync(viewModelType, null);
        public Task NavigateToAsync(Type viewModelType, object parameter) => InternalNavigateToAsync(viewModelType, parameter);

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainPage)
            {
                var mainPage = CurrentApplication.MainPage as MainPage;
                await mainPage.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is MainPage mainPage)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }
            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainPage)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is NavigationPage)
            {
                var mainPage = CurrentApplication.MainPage as NavigationPage;
                await mainPage.PushAsync(page);
            }
            else if (CurrentApplication.MainPage is MainPage)
            {
                var mainPage = new NavigationPage(CurrentApplication.MainPage as MainPage);
                CurrentApplication.MainPage = mainPage;
                await mainPage.PushAsync(new NavigationPage(page));
            }
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }
            return mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }
            try
            {
                var page = Activator.CreateInstance(pageType) as Page;
                var viewModel = Locator.Instance.Resolve(viewModelType) as ViewModelBase;
                page.BindingContext = viewModel;

                return page;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

}
