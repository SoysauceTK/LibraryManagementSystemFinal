using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace LMS.BookStorage.Utils
{
    public class XmlDataAccess<T> where T : class
    {
        private readonly string _filePath;
        private readonly object _lockObject = new object();
        private readonly XmlSerializer _serializer;

        public XmlDataAccess(string fileName)
        {
            // Configure to use a remote path that will be provided by the client application
            _filePath = fileName;
            _serializer = new XmlSerializer(typeof(List<T>));

            // Create file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating directory: {ex.Message}");
                        // Continue as the directory might be created by another process
                    }
                }
                
                try
                {
                    // Initialize with empty list
                    SaveAll(new List<T>());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error initializing XML file: {ex.Message}");
                    // If we can't create the file, we'll try again on demand
                }
            }
        }

        public List<T> GetAll()
        {
            lock (_lockObject)
            {
                try
                {
                    if (!File.Exists(_filePath))
                    {
                        // If file doesn't exist, create an empty one
                        SaveAll(new List<T>());
                        return new List<T>();
                    }
                    
                    using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        return (List<T>)_serializer.Deserialize(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error reading XML file: {ex.Message}");
                    return new List<T>();
                }
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await Task.Run(() => GetAll());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading XML file asynchronously: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveAll(List<T> items)
        {
            lock (_lockObject)
            {
                try
                {
                    using (var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings
                        {
                            Indent = true,
                            IndentChars = "  "
                        };

                        using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                        {
                            _serializer.Serialize(writer, items);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error saving XML file: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task SaveAllAsync(List<T> items)
        {
            await Task.Run(() => SaveAll(items));
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
            int index = items.FindIndex(new Predicate<T>(predicate));

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