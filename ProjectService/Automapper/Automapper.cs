using AutoMapper;
using ProjectService.DTO_s;
using ProjectService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.Automapper
{
    public class Automapper : Profile
    {
        public Automapper() 
        {
            CreateMap<VehicleMake,VehicleMakeCreate>().ReverseMap();
            CreateMap<VehicleMake,VehicleMakeResponse>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelCreate>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelResponse>().ReverseMap();
        }
    }
}
