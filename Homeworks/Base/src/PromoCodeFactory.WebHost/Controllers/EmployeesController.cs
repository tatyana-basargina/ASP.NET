using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using System.Data;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles?.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> AddEmployeeAsync([FromBody] AddEmployeeRequest employeeModel)
        {
            Employee employee = new()
            {
                FirstName = employeeModel.FirstName,
                LastName = employeeModel.LastName,
                Email = employeeModel.Email
            };

            return Ok(await _employeeRepository.AddAsync(employee));
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteEmployeeAsync(Guid id)
        {
            return Ok(await _employeeRepository.RemoveAsync(id));
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeShortResponse>> UpdateEmployeeAsync(Guid id, [FromBody] UpdateEmployeeRequest employeeModel)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            employee.FirstName = employeeModel.FirstName;
            employee.LastName = employeeModel.LastName;
            employee.Email = employeeModel.Email;

            return Ok(await _employeeRepository.UpdateAsync(employee));
        }

        /// <summary>
        /// Получить список ролей сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}/Roles")]
        public async Task<ActionResult<List<RoleItemResponse>>> GetEmployeeRolesByIdAsync(Guid id)
        {
            var employeeRoles = await _employeeRepository.GetEmployeeRolesAsync(id);

            if (employeeRoles == null)
                return NotFound();

            var employeeRolesModelList = employeeRoles.Select(x =>
                new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

            return employeeRolesModelList;
        }

        /// <summary>
        /// Добавить роль сотруднику по Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id:guid}/AddRole")]
        public async Task<ActionResult<bool>> AddEmployeeRolesAsync(Guid id, [FromQuery] Guid roleId)
        {
            return Ok(_employeeRepository.AddEmployeeRoleAsync(id, roleId));
        }

        /// <summary>
        /// Удалить роль сотруднику по Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}/DeleteRole")]
        public async Task<ActionResult<bool>> DeleteEmployeeRolesAsync(Guid id, [FromQuery] Guid roleId)
        {
            return Ok(_employeeRepository.DeleteEmployeeRoleAsync(id, roleId));
        }
    }
}