using EasyProbe.ViewModel.Base;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyProbe.Service.Navigation
{
    public partial class NavigationService : INavigationService
    {
        public Task NavigateToPopupAsync<TViewModel>(bool animate) where TViewModel : ViewModelBase => NavigateToPopupAsync<TViewModel>(null, animate);

        public async Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate) where TViewModel : ViewModelBase
        {
            var page = CreateAndBindPage(typeof(TViewModel), parameter);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);

            if (page is PopupPage)
            {
                await PopupNavigation.Instance.PushAsync(page as PopupPage, animate);
            }
            else
            {
                throw new ArgumentException($"The type ${typeof(TViewModel)} its not a PopupPage type");
            }
        }

        public async Task RemovePopupAsync(bool animate)
        {
            Console.WriteLine(">>>>>>>>>>>>>>> Remove Popup");
            await PopupNavigation.Instance.PopAllAsync();
        }
    }

}
