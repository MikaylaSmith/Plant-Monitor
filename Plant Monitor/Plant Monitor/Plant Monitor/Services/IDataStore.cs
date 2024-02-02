using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plant_Monitor.Services
{
    public interface IDataStore<T>
    {
        //Creating function to add an item
        Task<bool> AddItemAsync(T item);
        //Creating function to update an item
        Task<bool> UpdateItemAsync(T item);
        //Creating function to delete an item
        Task<bool> DeleteItemAsync(string id);
        //Creating function to get an item
        Task<T> GetItemAsync(string id);
        //Creating function go get items
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

    }
}

