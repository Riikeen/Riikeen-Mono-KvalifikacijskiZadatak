using ProjectService.DTO_s;
using ProjectService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.VehicleService
{
    public interface IVehicleMakeService
    {
        public Task<PagedList<VehicleMakeResponse>> GetAllVehicleMakes(int pageNumber, int pageSize, string searchString,string sortOrder);
        public Task<List<VehicleMakeResponse>> GetAllVehicleMakes();
        public Task<VehicleMakeResponse> GetMakeByIdAsync(Guid id);
        public Task AddVehicleMake(VehicleMakeCreate vehicleMakeDTO);
        public Task UpdateMakeAsync(VehicleMakeResponse make);
        public Task DeleteMakeAsync(Guid id);
    }
}
