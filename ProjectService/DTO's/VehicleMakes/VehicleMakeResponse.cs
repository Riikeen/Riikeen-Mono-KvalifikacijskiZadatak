using ProjectService.DTO_s;
using ProjectService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.DTO_s
{
    public class VehicleMakeResponse
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public ICollection<VehicleModelResponse> VehicleModels { get; set; }
    }
}
