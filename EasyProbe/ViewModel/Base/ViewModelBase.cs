using EasyProbe.Service.Bluetooth;
using EasyProbe.Service.Dialog;
using EasyProbe.Service.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyProbe.ViewModel.Base
{
    public abstract class ViewModelBase : MvvmHelpers.BaseViewModel
    {
        protected readonly INavigationService NavigationService;
        protected readonly IDialogService DialogService;
        protected readonly IBluetoothService BluetoothService;

        public ViewModelBase()
        {
            DialogService = Locator.Instance.Resolve<IDialogService>();
            NavigationService = Locator.Instance.Resolve<INavigationService>();
            BluetoothService = Locator.Instance.Resolve<IBluetoothService>();
        }

        public virtual Task InitializeAsync(object navigationData) => Task.FromResult(false);
    }
}
