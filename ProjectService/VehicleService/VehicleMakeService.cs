using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectService.Automapper;
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
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public VehicleMakeService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddVehicleMake(VehicleMakeCreate vehicleMakeDTO)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeDTO);
            _context.VehicleMake.Add(vehicleMake);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMakeAsync(Guid id)
        {
            var vehicleMake = await _context.VehicleMake.Where(model => model.Id == id).FirstOrDefaultAsync();
            if(vehicleMake != null)
            {
                _context.VehicleMake.Remove(vehicleMake);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<VehicleMakeResponse>> GetAllVehicleMakes(int pageNumber, int pageSize, string searchString, string sortOrder)
        {
            IQueryable<VehicleMake> vehcileMakeQuery = _context.VehicleMake;

            //filtering logic
            if(!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                vehcileMakeQuery = vehcileMakeQuery.Where(make => make.Name.Contains(searchString) || 
                                                                  make.Abrv.Contains(searchString));
            }

            //sorting logic
            vehcileMakeQuery =  sortOrder switch
            {
                "name_desc" => vehcileMakeQuery.OrderByDescending(v => v.Name),
                "abrv" => vehcileMakeQuery.OrderBy(v => v.Abrv),
                "abrv_desc" => vehcileMakeQuery.OrderByDescending(v => v.Abrv),
                _ => vehcileMakeQuery.OrderBy(v => v.Name)
            };

            var vehicleMakeResponseQuery = vehcileMakeQuery
                .Select(m => new VehicleMakeResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    Abrv = m.Abrv
                });

            var vehicleMakes = await PagedList<VehicleMakeResponse>.CreateAsync(vehicleMakeResponseQuery, pageNumber, pageSize);
            return vehicleMakes;
        }

        public async Task<List<VehicleMakeResponse>> GetAllVehicleMakes()
        {
            var vehicleMakes = await _context.VehicleMake.AsNoTracking().ToListAsync();
            return _mapper.Map<List<VehicleMakeResponse>>(vehicleMakes);
        }

        public async Task<VehicleMakeResponse> GetMakeByIdAsync(Guid id)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(id);
            return _mapper.Map<VehicleMakeResponse>(vehicleMake);
        }

        public async Task UpdateMakeAsync(VehicleMakeResponse make)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(make.Id);
            vehicleMake.Abrv = make.Abrv;
            vehicleMake.Name = make.Name;
            await _context.SaveChangesAsync();

        }
    }
}
