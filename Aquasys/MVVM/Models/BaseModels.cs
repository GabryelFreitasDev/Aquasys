using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CRM.Mobile.Core.Services.AutoMapper;

namespace Aquasys.MVVM.ViewModels
{
    public class BaseModels : ObservableValidator
    {
        public static IMapper mapper = MapperService.CreateMapper();
    }
}
