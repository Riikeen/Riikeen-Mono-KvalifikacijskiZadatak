using ProjectService.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.VehicleService
{
    public interface IVehicleModelService
    {
        public Task<List<VehicleModelResponse>> GetAllVehicleModels();
        public Task<PagedList<VehicleModelResponse>> GetAllVehicleModels(int pageNumber, int pageSize, string searchString, string sortOrder, string selectedMake);
        public Task<VehicleModelResponse?> GetModelByIdAsync(Guid id);
        public Task AddVehicleModel(VehicleModelCreate vehicleMakeDTO);
        public Task UpdateModelAsync(VehicleModelResponse model);
        public Task DeleteModelAsync(Guid id);
    }


}
