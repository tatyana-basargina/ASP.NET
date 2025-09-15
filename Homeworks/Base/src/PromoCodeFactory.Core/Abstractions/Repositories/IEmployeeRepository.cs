using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        Task<IEnumerable<Role>> GetEmployeeRolesAsync(Guid employeeId);

        Task<bool> AddEmployeeRoleAsync(Guid employeeId, Guid roleId);

        Task<bool> DeleteEmployeeRoleAsync(Guid employeeId, Guid roleId);
    }
}