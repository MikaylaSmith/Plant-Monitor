using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plant_Monitor.Models;

namespace Plant_Monitor.Services
{
    public class MockDataStore : IDataStore<Plant>
    {
        //Creates a list of plants
        readonly List<Plant> items;

        public MockDataStore()
        {
            //MockDatstore constructor
            items = new List<Plant>()
            {
            };
        }

        public async Task<bool> AddItemAsync(Plant item)
        {
            //Adds an item to the list of plants
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Plant item)
        {
            //Retrives the old item from the list of plants
            var oldItem = items.Where((Plant arg) => arg.CommonName == item.CommonName).FirstOrDefault();
            //Removes the old item from the list of plants
            items.Remove(oldItem);
            //Adds the new item to the list of plants
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //Retrives the item to be deleted
            var oldItem = items.Where((Plant arg) => arg.CommonName == id).FirstOrDefault();
            //Deletes the item
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Plant> GetItemAsync(string name)
        {
            //Retrives the item
            return await Task.FromResult(items.FirstOrDefault(s => s.CommonName == name));
        }

        public async Task<IEnumerable<Plant>> GetItemsAsync(bool forceRefresh = false)
        {
            //Retrieve all items
            return await Task.FromResult(items);
        }
    }

}
