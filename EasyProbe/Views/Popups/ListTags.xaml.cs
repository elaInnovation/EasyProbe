using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyProbe.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListTags : PopupPage
    {
        public ListTags()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override bool OnBackgroundClicked()
        {
            return true;
        }
    }
}