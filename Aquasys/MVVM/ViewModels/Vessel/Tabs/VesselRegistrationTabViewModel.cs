using Aquasys.Controls.Editors;
using Aquasys.Core.BO;
using Aquasys.Core.Enums;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountryData.Standard;
using CRM.Entidades.entidades;

namespace Aquasys.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselRegistrationTabViewModel : BaseViewModels
    {

        [ObservableProperty]
        private VesselModel vesselModel;

        [ObservableProperty]
        private List<Country> flags;

        [ObservableProperty]
        private Country flagSelecionada;

        [ObservableProperty]
        private List<EntidadeGenerica> vesselsType;

        private VesselBO vesselBO;
        public VesselRegistrationTabViewModel()
        {
            vesselBO = new();
            vesselModel = new();
            flags = new();
            VesselsType = new();
            flagSelecionada = new();
        }

        public override void OnAppearing()
        {
            PopulaSeletores();
        }

        private void PopulaSeletores()
        {
            Flags = new CountryHelper().GetCountryData().ToList();

            if (!string.IsNullOrEmpty(VesselModel.Flag))
                FlagSelecionada = Flags.Where(x => x.CountryName == VesselModel?.Flag).First();
            else
                FlagSelecionada = Flags.First();

            VesselsType = Enum.GetValues(typeof(VesselType)).Cast<VesselType>()
            .Select(x => new EntidadeGenerica
            {
                Chave = x,
                Valor = x.GetEnumDescription(),
            }).ToList();
        }

        [RelayCommand]
        private void ChangeFlag(Country flag)
        {
            if (flag is not null)
                VesselModel.Flag = flag.CountryName?.ToString() ?? string.Empty;
        }
            

        //[RelayCommand]
        //private void FiltraFlags(string nome) =>
        //    Flags = new CountryHelper().GetCountryData().Where(x => x.CountryName.Contains(nome)).ToList();


        //[RelayCommand]
        //private void FiltraVesselsType(string nome) =>
        //    VesselsType = Enum.GetValues(typeof(VesselType)).Cast<VesselType>()
        //    .Where(e => e.GetEnumDescription().Contains(nome))
        //    .Select(x => new EntidadeGenerica
        //    {
        //        Chave = x,
        //        Valor = x.GetEnumDescription(),
        //    }).ToList();


        [RelayCommand]
        private async Task BtnSalvarClick()
        {
            if (VesselModel?.IDVessel is not null && VesselModel?.IDVessel != 0)
            {
                var vesselExists = await vesselBO.GetByIdAsync(VesselModel?.IDVessel ?? -1);
                if (vesselExists is not null)
                {
                    vesselExists = mapper.Map<Core.Entities.Vessel>(VesselModel);
                    if (await vesselBO.UpdateAsync(vesselExists))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var vesselSave = mapper.Map<Core.Entities.Vessel>(VesselModel);

                if (await vesselBO.InsertAsync(vesselSave))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }

        [RelayCommand]
        private async Task AddVesselImages()
        {
            //await Shell.Current.GoToAsync(nameof(VesselImagePage));

            try
            {
                if (IsProcessRunning)
                    return;

                IsProcessRunning = true;

                var anexo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (anexo != null && anexo is DCImagem _anexo && (!_anexo.ImageSource?.IsEmpty ?? false))
                {
                    //MainThread.BeginInvokeOnMainThread(async () => await NavigationUtils.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={EvidenciaModel.iddespesaanexo}"));
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
        }
    }
}
