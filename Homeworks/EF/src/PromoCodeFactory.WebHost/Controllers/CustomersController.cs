using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> repository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = repository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список клиетов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<ActionResult<ICollection<CustomerShortResponse>>> GetCustomersAsync()
        {
            // TODO: Добавить получение списка клиентов
            IEnumerable<Customer> customers;
            try
            {
                customers = await _customerRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(customers.Select(x => new CustomerShortResponse
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList());
        }

        /// <summary>
        /// Получить данные клиента по Id
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Данные клиента</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            // TODO: Добавить получение клиента вместе с выданными ему промомкодами
            Customer customer;
            List<PromoCodeShortResponse> promoCodes;
            List<PreferenceResponse> preferences;
            try
            {
                customer = await _customerRepository.GetByIdAsync(id);
                promoCodes = customer.Promocodes?.Select(p => new PromoCodeShortResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    ServiceInfo = p.ServiceInfo,
                    BeginDate = p.BeginDate.ToString(),
                    EndDate = p.EndDate.ToString(),
                    PartnerName = p.PartnerName
                }).ToList();

                // 5. CustomerResponse также должен возвращать список предпочтений клиента с той же моделью PrefernceResponse
                preferences = customer.Preferences?.Select(p => new PreferenceResponse
                { 
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok(new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = preferences,
                PromoCodes = promoCodes
            });
        }

        /// <summary>
        /// Добавить клиента
        /// </summary>
        /// <param name="request">Данные клиента</param>
        /// <returns>Результат добавления: Id</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCustomerAsync([FromBody] CreateOrEditCustomerRequest request)
        {
            // TODO: Добавить создание нового клиента вместе с его предпочтениями
            Customer customer = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Preferences = []
            };

            foreach (var preferenceId in request?.PreferenceIds.Distinct())
            {
                Preference preference;
                try
                {
                    preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }

                customer.Preferences.Add(preference);
            }

            Guid newId = Guid.Empty;
            try
            {
                newId = await _customerRepository.AddAsync(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(newId);
        }

        /// <summary>
        /// Обновить данные клиента по Id
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <param name="request">Новые данные клиента</param>
        /// <returns>Результат обновления: true, false</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> EditCustomersAsync(Guid id, [FromBody] CreateOrEditCustomerRequest request)
        {
            // TODO: Обновить данные клиента вместе с его предпочтениями
            Customer customer;
            try
            {
                customer = await _customerRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            if (customer.Preferences is null)
            {
                customer.Preferences = [];
            }
            else
            {
                customer.Preferences.Clear();
            }

            if (request.PreferenceIds.Count != 0)
            {
                foreach (var preferenceId in request?.PreferenceIds.Distinct())
                {
                    Preference preference;
                    try
                    {
                        preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                    }
                    catch (Exception ex)
                    {
                        return NotFound(ex.Message);
                    }

                    customer.Preferences.Add(preference);
                }
            }

            bool result;
            try
            {
                result = await _customerRepository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// Удалить клиента по Id
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Результат удаления: true, false</returns>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomer(Guid id)
        {
            // TODO: Удаление клиента вместе с выданными ему промокодами
            bool result;
            try
            {
                result = await _customerRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}