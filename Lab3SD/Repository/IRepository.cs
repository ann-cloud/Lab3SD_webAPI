using Microsoft.AspNetCore.Mvc;

namespace Lab3SD.Repository;

public interface IRepository<T> : IDisposable where T : class
{
    public Task<IEnumerable<T>> GetItems(); 
    public Task<T> GetItem(int id); 
    public Task<T> Create(T item); 
    public Task<T> Update(T item); 
    public Task<T> Delete(int id);
    public bool ItemExists(int id);
}