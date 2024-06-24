using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectService.Database;
using ProjectService.DTO_s;
using ProjectService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.VehicleService
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public VehicleModelService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddVehicleModel(VehicleModelCreate vehicleModelDTO)
        {
            var vehicleModel = _mapper.Map<VehicleModel>(vehicleModelDTO);
            _context.VehicleModel.Add(vehicleModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteModelAsync(Guid id)
        {
            var vehicleModel = await _context.VehicleModel
                .Where(model => model.Id == id)
                .FirstOrDefaultAsync();
            if (vehicleModel != null)
            {
                _context.VehicleModel.Remove(vehicleModel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<VehicleModelResponse>> GetAllVehicleModels()
        {
            var vehicleModels = await _context.VehicleModel
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<VehicleModelResponse>>(vehicleModels);
        }

        public async Task<PagedList<VehicleModelResponse>> GetAllVehicleModels(int pageNumber, int pageSize, string searchString, string sortOrder, string selectedMake )
        {

            //filtering logic

            IQueryable<VehicleModel> vehicleModelQuery = _context.VehicleModel;

            if (!String.IsNullOrEmpty(selectedMake))
            {
                var id = Guid.Parse(selectedMake);
                vehicleModelQuery = vehicleModelQuery.Where(m => m.MakeId == id);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                vehicleModelQuery = vehicleModelQuery.Where(m => m.Name.Contains(searchString) ||
                                                                  m.Abrv.Contains(searchString));
            }

            

            //sorting logic
            vehicleModelQuery = sortOrder switch
            {
                "name_desc" => vehicleModelQuery.OrderByDescending(v => v.Name),
                "abrv" => vehicleModelQuery.OrderBy(v => v.Abrv),
                "abrv_desc" => vehicleModelQuery.OrderByDescending(v => v.Abrv),
                _ => vehicleModelQuery.OrderBy(v => v.Name)
            };
            var vehicleModelResponseQuery = vehicleModelQuery
                .Select(m => new VehicleModelResponse
                {
                    Abrv = m.Abrv,
                    Name = m.Name,
                    Id = m.Id,
                    VehicleMake = new VehicleMakeResponse
                    {
                        Name = m.VehicleMake.Name,
                        Id = m.VehicleMake.Id,
                        Abrv = m.VehicleMake.Abrv
                    }
                });

            var vehicleModels = await PagedList<VehicleModelResponse>.CreateAsync(vehicleModelResponseQuery, pageNumber, pageSize);

            return vehicleModels;
        }

        public async Task<VehicleModelResponse?> GetModelByIdAsync(Guid id)
        {
            var vehicleModel = await _context.VehicleModel
                .Where(model => model.Id == id)
                .Select(model => new VehicleModel
                {
                    Id =model.Id,
                    MakeId = model.MakeId,
                    Name = model.Name,
                    Abrv = model.Abrv,
                    VehicleMake = new VehicleMake
                    {
                        Id = model.VehicleMake.Id,
                        Name = model.VehicleMake.Name,
                    }
                }).FirstOrDefaultAsync();
            return _mapper.Map<VehicleModelResponse>(vehicleModel);
        }

        public async Task UpdateModelAsync(VehicleModelResponse model)
        {
            var vehicleModel = await _context.VehicleModel.FirstOrDefaultAsync(m => m.Id == model.Id);
            vehicleModel.Abrv = model.Abrv;
            vehicleModel.Name = model.Name;
            vehicleModel.MakeId = model.MakeId;
            
            await _context.SaveChangesAsync();
        }
    }
}
