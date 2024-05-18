using AqbaServer.Dto;
using AqbaServer.Models;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;
using AutoMapper;

namespace AqbaServer.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();
            CreateMap<Manufacturer, ManufacturerDto>();
            CreateMap<ManufacturerDto, Manufacturer>();
            CreateMap<Kind, KindDto>();
            CreateMap<KindDto, Kind>();
            CreateMap<KindParameter, KindParameterDto>();
            CreateMap<KindParameterDto, KindParameter>();
            CreateMap<Model, ModelDto>();
            CreateMap<ModelDto, Model>();
            CreateMap<EquipmentParameter, EquipmentParameterDto>();
            CreateMap<EquipmentParameterDto, EquipmentParameter>();
            CreateMap<MaintenanceEntity, MaintenanceEntityDto>();
            CreateMap<MaintenanceEntityDto, MaintenanceEntity>();
            CreateMap<EquipmentDto, Equipment>();
            CreateMap<Equipment, EquipmentDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<StatusDto, Status>();
            CreateMap<Status, StatusDto>();
            CreateMap<PriorityDto, Priority>();
            CreateMap<Priority, PriorityDto>();
            CreateMap<TaskTypeDto, IssueType>();
            CreateMap<IssueType, TaskTypeDto>();
        }
    }
}
