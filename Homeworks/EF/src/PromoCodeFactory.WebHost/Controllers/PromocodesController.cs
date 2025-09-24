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
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        protected readonly IRepository<PromoCode> _promoCodesRepository;
        protected readonly IRepository<Preference> _preferenceRepository;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository, IRepository<Preference> preferenceRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            // TODO: Получить все промокоды
            var promoCodes = await _promoCodesRepository.GetAllAsync();

            var promoCodesModelList = promoCodes.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo.ToString(),
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName
                }).ToList();

            return Ok(promoCodesModelList);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            // TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            Guid promoCodeId;
            try
            {
                var preferences = await _preferenceRepository.GetAllAsync();
                var preference = preferences.Where(p => p.Name == request.Preference).FirstOrDefault();

                PromoCode promoCode = new()
                {
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    PartnerName = request.PartnerName,
                    Preference = preference,
                    Customers = preference.Customers
                };

                promoCodeId = await _promoCodesRepository.AddAsync(promoCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(promoCodeId);
        }
    }
}