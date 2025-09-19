using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<Guid> AddAsync(T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            data.Id = Guid.NewGuid();
            Data = [.. Data, data];
            return Task.FromResult(data.Id);
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            List<T> list = Data as List<T> ?? Data?.ToList();
            int countRemovedItems = list.RemoveAll(x => x.Id == id);

            if (countRemovedItems > 0)
            {
                Data = list;
            }

            return Task.FromResult(countRemovedItems > 0);
        }

        public Task<T> UpdateAsync(T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            List<T> list = Data as List<T> ?? Data?.ToList();
            int index = list.FindIndex(x => x.Id == data.Id);

            list[index] = data;
            Data = list;

            return Task.FromResult(data);
        }
    }
}