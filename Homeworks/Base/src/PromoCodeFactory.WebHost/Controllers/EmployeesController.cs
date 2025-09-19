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
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository,
            IRepository<Role> roleRepository
        )
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
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

            return await _employeeRepository.AddAsync(employee);
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteEmployeeAsync(Guid id)
        {
            return await _employeeRepository.RemoveAsync(id);
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeShortResponse>> UpdateEmployeeAsync(Guid id, [FromBody] UpdateEmployeeRequest employeeModel)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = employeeModel.FirstName;
            employee.LastName = employeeModel.LastName;
            employee.Email = employeeModel.Email;

            await _employeeRepository.UpdateAsync(employee);

            return new EmployeeShortResponse()
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
            };
        }

        /// <summary>
        /// Получить список ролей сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}/Roles")]
        public async Task<ActionResult<List<RoleItemResponse>>> GetEmployeeRolesByIdAsync(Guid id)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);

            List<Role> employeeRoles = employee?.Roles ?? [];

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
            Employee? employee = await _employeeRepository.GetByIdAsync(id);

            if (employee is null)
            {
                return false;
            }

            Role? role = await _roleRepository.GetByIdAsync(roleId);

            if (role is null)
            {
                return false;
            }

            bool roleExists = employee.Roles?.Exists(r => r.Id == role.Id) ?? false;

            if (roleExists)
            {
                return false;
            }

            if (employee?.Roles is null)
            {
                employee.Roles = [];
            }

            employee.Roles.Add(role);

            return true;
        }

        /// <summary>
        /// Удалить роль сотруднику по Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}/DeleteRole")]
        public async Task<ActionResult<bool>> DeleteEmployeeRolesAsync(Guid id, [FromQuery] Guid roleId)
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(id);

            if (employee is null)
            {
                return false;
            }

            Role? role = employee.Roles.FirstOrDefault(r => r.Id == roleId);

            if (role is null)
            {
                return false;
            }

            return employee.Roles.Remove(role);
        }
    }
}