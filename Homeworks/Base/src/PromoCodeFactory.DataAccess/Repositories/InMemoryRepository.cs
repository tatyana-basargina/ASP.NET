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
        protected IList<T> Data { get; set; }

        public InMemoryRepository(IList<T> data)
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
            data.Id = Guid.NewGuid();
            Data.Add(data);
            return Task.FromResult(data.Id);
        }

        public Task<bool> RemoveAsync(Guid id)
        {
            return Task.FromResult(Data.Remove(Data.FirstOrDefault(x => x.Id == id)));
        }

        public Task<T> UpdateAsync(T data)
        {
            var dataOld = Data.FirstOrDefault(x => x.Id == data.Id);

            int index = Data.IndexOf(dataOld);

            Data[index] = data;

            return Task.FromResult(data);
        }
    }
}