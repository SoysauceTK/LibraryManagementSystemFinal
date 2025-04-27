using System;
using System.Configuration;
using System.IO;

namespace LMS.BookStorage.Utils
{
    /// <summary>
    /// DataConfiguration is maintained for backward compatibility.
    /// BookService no longer uses this class as it now has its own local storage path.
    /// This class may be removed in future versions if no other components depend on it.
    /// </summary>
    public static class DataConfiguration
    {
        private static string _dataPath;

        public static string DataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dataPath))
                {
                    // Get path from configuration or use a default
                    _dataPath = ConfigurationManager.AppSettings["DataFilePath"];
                    
                    // If not configured, use a default path
                    if (string.IsNullOrEmpty(_dataPath))
                    {
                        _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                    }
                }
                return _dataPath;
            }
            set { _dataPath = value; }
        }

        public static string GetDataFilePath(string fileName)
        {
            return Path.Combine(DataPath, fileName);
        }
    }
}