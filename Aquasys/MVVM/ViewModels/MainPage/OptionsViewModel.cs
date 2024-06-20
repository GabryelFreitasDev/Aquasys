using Aquasys.Core.Entities;
using Aquasys.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Aquasys.MVVM.Views.MainPage;
using CommunityToolkit.Mvvm.Input;
using Aquasys.Core.BO;
using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.ViewModels.Login;
namespace Aquasys.MVVM.ViewModels.MainPage
{
    public partial class OptionsViewModel : BaseViewModels
    {
        private UserBO userBO = new UserBO();

        [ObservableProperty]
        public User user;

        public OptionsViewModel() 
        {
            user = ContextUtils.ContextUser;
        }

        [RelayCommand]
        private async Task BtnConfigClick()
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(OptionsPage));
        }
        [RelayCommand]
        private async Task BtnEditUser()
        {
            if (user?.IDUser is not null && user?.IDUser != 0)
            {
                var userEdit = await new UserBO().GetByIdAsync(user?.IDUser ?? -1);
                if (userEdit is not null)
                {
                    userEdit = mapper.Map<User>(user);
                    userEdit.RememberMe = false;
                    if (await new UserBO().UpdateAsync(userEdit))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                        Shell.Current.BindingContext = this;
                    }
                }
            }
        }

        [RelayCommand]
        private async Task BtnDeleteUser()
        {
            try
            {
                if (IsProcessRunning || user is null)
                    return;

                IsProcessRunning = true;

                var userDelete = mapper.Map<User>(user);

                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                {
                    try
                    {
                        await new UserBO().DeleteAsync(userDelete);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                IsProcessRunning = false;
            }
            Shell.Current.FlyoutIsPresented = false;
            var currentPage = Application.Current!.MainPage;
            Shell.SetNavBarIsVisible(currentPage, false);
            await Application.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
        }

        [RelayCommand]
        private async Task BtnLogoutClick()
        {
            if (user?.IDUser is not null && user.IDUser != 0)
            {
                var userBO = new UserBO();
                var userEdit = await userBO.GetByIdAsync(user.IDUser);

                if (userEdit is not null)
                {
                    userEdit = mapper.Map<User>(user);
                    userEdit.RememberMe = false;
                    await userBO.UpdateAsync(userEdit);
                }
            }

            Shell.Current.FlyoutIsPresented = false;
            var currentPage = Application.Current!.MainPage;
            Shell.SetNavBarIsVisible(currentPage, false);
            await Application.Current!.MainPage!.Navigation.PushAsync(new LoginPage());
        }


    }
}
