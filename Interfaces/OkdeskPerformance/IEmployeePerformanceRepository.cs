﻿using AqbaServer.Dto;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeePerformanceRepository
    {
        public Task<EmployeeDto?> GetEmployeePerformance(int employeeId, DateTime date);
        public Task<List<EmployeeDto>?> GetEmployeesPerformance(DateTime dateFrom, DateTime dateTo);
        public Task<bool> CreateEmployeePerformance(Employee employee, DateTime day);
        public Task<bool> UpdateEmployeePerformance(Employee employee, DateTime day);
        public Task<bool> GetEmployeePerformanceFromOkdesk(DateTime dateFrom, DateTime dateTo);
    }
}