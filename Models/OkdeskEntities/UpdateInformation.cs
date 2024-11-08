﻿using AqbaServer.Dto;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;

namespace AqbaServer.Models.OkdeskEntities
{
    public class UpdateInformation
    {
        public Company? Company { get; set; }
        public ICollection<MaintenanceEntity>? MaintenanceEntity { get; set; }
        public ICollection<EquipmentDto>? Equipments { get; set; }
    }
}
