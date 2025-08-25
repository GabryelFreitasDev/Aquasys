using Aquasys.App.MVVM.ViewModels;

namespace Aquasys.App.MVVM.Views
{
    [QueryProperty("Id", "id")]
    public class BasePages : ContentPage
    {
        public string Id
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Type tipo = BindingContext.GetType();
                    var property = tipo.GetProperty("Id");
                    string id = property?.GetValue(BindingContext)?.ToString();

                    if (value != id)
                    {
                        BindingContext = Activator.CreateInstance(tipo, new object[] { value });
                        property?.SetValue(BindingContext, value);
                    }
                }
            }
        }

        protected override async void OnAppearing()
        {
            (BindingContext as BaseViewModels)?.OnAppearing();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            (BindingContext as BaseViewModels)?.OnDisappearing();
            base.OnDisappearing();
        }
    }
}
