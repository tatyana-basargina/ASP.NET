using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController : Controller
    {
        private readonly IRepository<Preference> _preferenceRepository;
        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        /// <returns>Список предпочтений</returns>
        [HttpGet]
        public async Task<IEnumerable<PreferenceResponse>> GetRolesAsync()
        {
            var preference = await _preferenceRepository.GetAllAsync();

            var preferencesModelList = preference.Select(x =>
                new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return preferencesModelList;
        }
    }
}
