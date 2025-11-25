using Aquasys.App.Core.Data;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDVessel), nameof(IDVessel))]
    public partial class HoldViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Hold> _holdRepository;
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;

        [ObservableProperty] private HoldModel holdModel;
        [ObservableProperty] private bool expanded = true;
        [ObservableProperty] private bool isNew = false;

        public long IDVessel { get; set; }

        public HoldViewModel(ILocalRepository<Hold> holdRepository, ILocalRepository<HoldInspection> holdInspectionRepository)
        {
            _holdRepository = holdRepository;
            _holdInspectionRepository = holdInspectionRepository;
            holdModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var hold = await _holdRepository.GetByIdAsync(Id.ToLong());
                HoldModel = mapper.Map<HoldModel>(hold);
            }
            else
            {
                IsNew = true;
                HoldModel = new();
            }
        }

        [RelayCommand]
        private async Task Inspection()
        {
            var inspection = await _holdInspectionRepository.GetFilteredAsync(x => x.IDHold == HoldModel.IDHold);
            if (inspection?.Any() ?? false)
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?{nameof(Id)}={inspection.First().IDHoldInspection.ToString()}");
            else
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?IDHold={HoldModel.IDHold}");
        }

        [RelayCommand]
        private async Task SaveHold()
        {
            if (HoldModel == null ||
                HoldModel.Capacity == 0 ||
                string.IsNullOrWhiteSpace(HoldModel.Agent) ||
                string.IsNullOrWhiteSpace(HoldModel.BasementNumber?.ToString())) 
            {
                await Shell.Current.DisplayAlert("Alert", "Please fill the required fields.", "OK");
                return;
            }

            if (HoldModel.IDHold != 0)
            {
                var hold = await _holdRepository.GetByIdAsync(HoldModel.IDHold);
                hold.IDVessel = IDVessel;
                if (hold is not null)
                {
                    hold = mapper.Map<Hold>(HoldModel);
                    if (await _holdRepository.UpdateAsync(hold))
                    {
                        await Shell.Current.DisplayAlert("Alert", "Saved successfully", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var hold = mapper.Map<Hold>(HoldModel);
                hold.IDVessel = IDVessel;
                if (await _holdRepository.InsertAsync(hold))
                {
                    await Shell.Current.DisplayAlert("Alert", "Saved successfully", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }

        [RelayCommand]
        private void Expand()
        {
            Expanded = !Expanded;
        }
    }
}