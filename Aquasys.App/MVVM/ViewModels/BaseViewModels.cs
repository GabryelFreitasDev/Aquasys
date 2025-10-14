using Aquasys.App.Core.Services;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Aquasys.App.MVVM.ViewModels
{
    public partial class BaseViewModels : ObservableObject
    {
        public string Title { get; protected set; }
        public bool IsLoadedViewModel;
        public Page Page { get; private set; }
        public bool IsProcessRunning { get; set; } = false;
        public IMapper mapper = MapperService.CreateMapper();
        public void SetInstancePage(Page page) => this.Page = page;
        public string Id { get; set; }
        public virtual async Task OnAppearing() { }
        public virtual async Task OnDisappearing() { }

        public BaseViewModels() { }
    }
}
