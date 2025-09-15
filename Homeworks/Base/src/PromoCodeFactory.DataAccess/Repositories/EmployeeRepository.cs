using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EmployeeRepository : InMemoryRepository<Employee>, IEmployeeRepository
    {
        private IEnumerable<Role> _rolesSource;
        public EmployeeRepository(IList<Employee> employeeSource, IEnumerable<Role> rolesSource) : base(employeeSource)
        {
            _rolesSource = rolesSource;
        }

        public Task<IEnumerable<Role>> GetEmployeeRolesAsync(Guid employeeId)
        {
            List<Role> roles = Data.FirstOrDefault(x => x.Id == employeeId)?.Roles ?? [];
            return Task.FromResult(roles.AsEnumerable());
        }

        public Task<bool> AddEmployeeRoleAsync(Guid employeeId, Guid roleId)
        {
            Employee employee = Data.First(x => x.Id == employeeId);

            Role role = _rolesSource.First(r => r.Id == roleId);

            bool roleExists = employee.Roles?.Exists(r => r.Id == role.Id) ?? false;

            if (roleExists)
            {
                return Task.FromResult(false);
            }

            if (employee.Roles is null)
            {
                employee.Roles = [];
            }

            employee.Roles.Add(role);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteEmployeeRoleAsync(Guid employeeId, Guid roleId)
        {
            Employee? employee = Data.FirstOrDefault(x => x.Id == employeeId);
            if (employee is null)
            {
                return Task.FromResult(false);
            }

            Role? role = employee.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role is null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(employee.Roles.Remove(role));
        }
    }
}
