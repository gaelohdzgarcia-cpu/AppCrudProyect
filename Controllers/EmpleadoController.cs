using AppCrud.Models;
using AppCrud.Services;
using AppCrud.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace AppCrud.Controllers
{
    //Cookie Y/N
    [Authorize]
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IMapper _mapper;

        public EmpleadoController(
            IEmpleadoService empleadoService,
            IMapper mapper)
        {
            _empleadoService = empleadoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Empleado> lista = await _empleadoService
                .ObtenerTodos();

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> NuevoEditar(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new EmpleadoVM());
            }

            EmpleadoVM? empleado = await _empleadoService
                .ObtenerPorId(id.Value);

            if (empleado == null)
            {
                return NotFound("Empleado no encontrado");
            }

            EmpleadoVM empleadoVM = _mapper.Map<EmpleadoVM>(empleado);

            return View(empleadoVM);
        }

        [HttpPost]
        public async Task<IActionResult> NuevoEditar(EmpleadoVM empleadoVM)
        {
            if (!ModelState.IsValid)
            {
                return View(empleadoVM);
            }

            if (empleadoVM.IdEmpleado == 0)
            {
                Empleado empleado = _mapper.Map<Empleado>(empleadoVM);

                await _empleadoService.Crear(empleado);
            }
            else
            {
                Empleado empleado = _mapper.Map<Empleado>(empleadoVM);

                bool actualizado = await _empleadoService
                    .Editar(empleadoVM.IdEmpleado, empleado);

                if (!actualizado)
                {
                    return NotFound("Empleado no encontrado");
                }
            }

            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool eliminado = await _empleadoService
                .Eliminar(id);

            if (!eliminado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Lista));
        }
    }
}