using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectService.DTO_s;
using ProjectService.Entities;
using ProjectService.VehicleService;

namespace Mono_testni_zadatak.Controllers
{
    public class VehicleMakeController : Controller
    {
        private readonly IVehicleMakeService _vehicleMakeService;

        public VehicleMakeController(IVehicleMakeService service)
        {
            _vehicleMakeService = service;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page )
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParm"] = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";
            ViewData["CurrentFilter"] = searchString;

            int pageSize = 5;
            int pageNumber = (page ?? 1 );

            var makes = await _vehicleMakeService.GetAllVehicleMakes(pageNumber,pageSize,searchString,sortOrder);


            ViewData["TotalPages"] = (int)Math.Ceiling(makes.TotalCount / (double)pageSize);
            ViewData["CurrentPage"] = pageNumber;



            return View(makes.Items.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var make = await _vehicleMakeService.GetMakeByIdAsync(id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Abrv")] VehicleMakeCreate vehicleMakeDTO)
        {
            if (ModelState.IsValid)
            {
                await _vehicleMakeService.AddVehicleMake(vehicleMakeDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleMakeDTO);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var make = await _vehicleMakeService.GetMakeByIdAsync(id);
            return View(make);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleMakeResponse model)
        {
            await _vehicleMakeService.UpdateMakeAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleMakeService.DeleteMakeAsync(id);
            ViewBag.Message = "Record deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}

