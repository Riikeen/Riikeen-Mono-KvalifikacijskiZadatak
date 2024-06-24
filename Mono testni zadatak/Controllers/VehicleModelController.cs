using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectService.DTO_s;
using ProjectService.VehicleService;

namespace Mono_testni_zadatak.Controllers
{
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IVehicleMakeService _vehicleMakeService;

        public VehicleModelController(IVehicleModelService vehicleModelService, IVehicleMakeService vehicleMakeService)
        {
            _vehicleModelService = vehicleModelService;
            _vehicleMakeService = vehicleMakeService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page, string selectedMake)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParm"] = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";
            ViewData["CurrentFilter"] = searchString;
            ViewData["SelectedMake"] = selectedMake;

            var makes = await _vehicleMakeService.GetAllVehicleMakes();
          
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            

            var models = await _vehicleModelService.GetAllVehicleModels(pageNumber,pageSize,searchString,sortOrder,selectedMake);

            ViewData["TotalPages"] = (int)Math.Ceiling(models.TotalCount / (double)pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewBag.Makes = new SelectList(makes, "Id", "Name");
            
            return View(models.Items.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await _vehicleModelService.GetModelByIdAsync(id);
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var makes = await _vehicleMakeService.GetAllVehicleMakes();
            ViewBag.Makes = new SelectList(makes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakeId,Name, Abrv")] VehicleModelCreate vehicleMakeDTO)
        {
            if (ModelState.IsValid)
            {
                await _vehicleModelService.AddVehicleModel (vehicleMakeDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMakeDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var makes = await _vehicleMakeService.GetAllVehicleMakes();
            ViewBag.Makes = new SelectList(makes, "Id", "Name");
            var make = await _vehicleModelService.GetModelByIdAsync(id);
            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleModelResponse model)
        {
            await _vehicleModelService.UpdateModelAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleModelService.DeleteModelAsync(id);
            ViewBag.Message = "Record deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
