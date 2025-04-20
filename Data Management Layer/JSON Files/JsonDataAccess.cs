// JsonDataAccess.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

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

    // Replace this method
    public async Task<List<T>> GetAllAsync()
    {
        using (StreamReader reader = new StreamReader(_filePath))
        {
            string json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }
    }

    public void SaveAll(List<T> items)
    {
        lock (_lockObject)
        {
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }

    // Replace this method
    public async Task SaveAllAsync(List<T> items)
    {
        string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
        using (StreamWriter writer = new StreamWriter(_filePath, false))
        {
            await writer.WriteAsync(json);
        }
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

    public void Update(T item, Predicate<T> predicate)
    {
        List<T> items = GetAll();
        int index = items.FindIndex(predicate);
        if (index >= 0)
        {
            items[index] = item;
            SaveAll(items);
        }
    }

    public void Delete(Predicate<T> predicate)
    {
        List<T> items = GetAll();
        T itemToRemove = items.Find(predicate);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
            SaveAll(items);
        }
    }
}