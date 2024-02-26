using Aquasys.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.MVVM.Views
{
    public class BaseView : ContentPage
    {
        protected override async void OnAppearing()
        {
            (BindingContext as BaseViewModel)?.OnAppearing();
            base.OnDisappearing();
        }

        protected override void OnDisappearing()
        {
            (BindingContext as BaseViewModel)?.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
