using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyProbe.Service.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void ShowToast(string message, int duration = 2000);
        Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel);
        Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options);
        Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options);
    }
}
