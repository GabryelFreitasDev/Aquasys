using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.MVVM.ViewModels
{
    public partial class BaseViewModels : ObservableObject
    {
        public bool IsLoadedViewModel;
        public string Id { get; set; }
        public virtual void OnAppearing() { }
        public virtual void OnDisappearing() { }

        public BaseViewModels() { }
    }
}
