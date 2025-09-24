using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    // 5. добавить новый Generic класс EfRepository
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DataContext _dataContext;

        public EfRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Получение списка
        /// </summary>
        /// <returns>Список</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Получение одного элемента
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dataContext.Set<T>().FirstAsync(t => t.Id == id);
        }

        /// <summary>
        /// Создание
        /// </summary>
        /// <param name="data">Новый элемент</param>
        /// <returns>Id</returns>
        public async Task<Guid> AddAsync(T data)
        {
            data.Id = Guid.NewGuid();
            await _dataContext.Set<T>().AddAsync(data);
            await _dataContext.SaveChangesAsync();
            return data.Id;
        }

        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="data">Измененный элемент</param>
        /// <returns>Результат изменения: true, false</returns>
        public async Task<bool> UpdateAsync(T data)
        {
            var dataUpdated = _dataContext.Set<T>().Update(data);

            if (dataUpdated is null)
            {
                return false;
            }

            await _dataContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Результат удаления: true, false</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await _dataContext.Set<T>().FirstAsync(t => t.Id == id);
            var dataRomoved = _dataContext.Set<T>().Remove(data);

            if (dataRomoved is null)
            {
                return false;
            }

            await _dataContext.SaveChangesAsync(true);
            return true;
        }
    }
}
