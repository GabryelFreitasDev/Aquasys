using Aquasys.Core.Entities;
using Aquasys.App.MVVM.Models.Vessel;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquasys.App.Core.Services
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

                cfg.CreateMap<HoldModel, Hold>();
                cfg.CreateMap<Hold, HoldModel>();

                cfg.CreateMap<HoldModel, Hold>();
                cfg.CreateMap<Hold, HoldModel>();

                cfg.CreateMap<HoldConditionModel, HoldCondition>();
                cfg.CreateMap<HoldCondition, HoldConditionModel>();

                cfg.CreateMap<HoldInspectionModel, HoldInspection>();
                cfg.CreateMap<HoldInspection, HoldInspectionModel>();

                cfg.CreateMap<HoldImageModel, HoldImage>();
                cfg.CreateMap<HoldImage, HoldImageModel>();

                cfg.CreateMap<InspectionModel, Inspection>();
                cfg.CreateMap<Inspection, InspectionModel>();

            });

            iMapper = mapperConfiguration.CreateMapper();

            return iMapper;
        }
    }
}

