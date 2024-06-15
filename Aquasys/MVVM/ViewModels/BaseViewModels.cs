using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CRM.Mobile.Core.Services.AutoMapper;


namespace Aquasys.MVVM.ViewModels
{
    public partial class BaseViewModels : ObservableObject
    {
        public bool IsLoadedViewModel;
        public Page Page { get; private set; }
        public bool IsProcessRunning { get; set; } = false;
        public IMapper mapper = MapperService.CreateMapper();
        public void SetInstancePage(Page page) => this.Page = page;
        public string Id { get; set; }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }

        public BaseViewModels() { }
    }
}
