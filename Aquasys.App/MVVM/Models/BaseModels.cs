using Aquasys.App.Core;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.App.MVVM.Models
{
    public class BaseModels : ObservableValidator
    {
        public static IMapper mapper = Core.Services.MapperService.CreateMapper();
    }
}
