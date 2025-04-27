using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.BookStorage.Utils
{
    public class JsonDataAccess<T> where T : class
    {
        private readonly string _filePath;
        private readonly object _lockObject = new object();

        public JsonDataAccess(string fileName)
        {
            // Store data in App_Data folder
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", fileName);

            // Create directory if it doesn't exist
            string directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<T>()));
            }
        }

        public List<T> GetAll()
        {
            lock (_lockObject)
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            string json = await Task.Run(() => File.ReadAllText(_filePath));
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        public void SaveAll(List<T> items)
        {
            lock (_lockObject)
            {
                string json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
        }

        public async Task SaveAllAsync(List<T> items)
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            await Task.Run(() => File.WriteAllText(_filePath, json));
        }

        public T GetById(Func<T, bool> predicate)
        {
            List<T> items = GetAll();
            return items.FirstOrDefault(predicate);
        }

        public void Insert(T item)
        {
            List<T> items = GetAll();
            items.Add(item);
            SaveAll(items);
        }

        public void Update(T item, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
            }

            List<T> items = GetAll();
            int index = items.FindIndex(new Predicate<T>(predicate)); // Convert Func<T, bool> to Predicate<T>

            if (index >= 0)
            {
                items[index] = item;
                SaveAll(items);
            }
            else
            {
                throw new InvalidOperationException("Item to update was not found.");
            }
        }

        public void Delete(Func<T, bool> predicate)
        {
            List<T> items = GetAll();
            T itemToRemove = items.FirstOrDefault(predicate);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                SaveAll(items);
            }
        }
    }
}