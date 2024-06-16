using Aquasys.Core.Entities;
using Aquasys.MVVM.Models.Vessel;
using AutoMapper;

namespace CRM.Mobile.Core.Services.AutoMapper
{
    public static class MapperService
    {
        private static IMapper iMapper = null;

        public static IMapper CreateMapper()
        {
            if (iMapper != null)
                return iMapper;

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VesselModel, Vessel>();
                cfg.CreateMap<Vessel, VesselModel>();

                cfg.CreateMap<VesselImageModel, VesselImage>();
                cfg.CreateMap<VesselImage, VesselImageModel>();
            });

            iMapper = mapperConfiguration.CreateMapper();

            return iMapper;
        }
    }
}

