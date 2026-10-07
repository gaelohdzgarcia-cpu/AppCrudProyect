using AutoMapper;
using AppCrud.Models;
using AppCrud.ViewModels;

namespace AppCrud.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Empleado, EmpleadoVM>().ReverseMap();

        }
    }
}
