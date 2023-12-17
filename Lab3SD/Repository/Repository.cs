using System.Reflection;
using Lab3SD.Context;
using Lab3SD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3SD.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly TablewareStoreDbContext _context;

    public Repository(TablewareStoreDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<T>> GetItems()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetItem(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public async Task<T> Create(T item)
    {
        _context.Set<T>().Add(item);
        await _context.SaveChangesAsync();
        return item;
    }
    
    public async Task<T> Update(T item)
    {
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return item;
    }
    
    public async Task<T> Delete(int id)
    {
        var item = await _context.Set<T>().FindAsync(id);
        if (item != null)
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }
        return item;
    }

    public bool ItemExists(int id)
    {
        return _context.Set<T>().ToList().Any(e => (int)e.GetType().GetProperties()[0].GetValue(e) == id);
        
        //TODO: remove toList() and filter on database side
    }
    
    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    //TODO: read IDisposable && SuppressFinalize
}
