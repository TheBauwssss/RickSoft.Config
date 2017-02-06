using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace RickSoft.Config
{
    public abstract class JsonConfig
    {
        [JsonIgnore]
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        [JsonIgnore]
        public virtual string ApplicationName { get; private set; }

        public void Reload()
        {
            string path = GetFilePath(ApplicationName);

            if (!File.Exists(path))
            {
                logger.Warn("Unable to reload json config file becase the file does not exist anymore!");
                return;
            }

            bool success = true;
            try
            {
                logger.Info($"Attempting to reload json config file from {path}");
                JsonConvert.PopulateObject(File.ReadAllText(path), this);
            }
            catch (Exception ex)
            {
                success = false;
                logger.Warn(ex, $"Unable to reload json config file from {path}!");
            }
            finally
            {
                if (success)
                    logger.Info("Successfully reloaded json config file from disk!");
            }
        }

        public static T Load<T>(string applicationName = "config") where T : JsonConfig, new()
        {
            string path = GetFilePath(applicationName);

            if (!File.Exists(path))
                return CreateNew<T>(applicationName);

            bool success = false;

            try
            {
                logger.Info($"Attempting to load json config file from {path}");

                var obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                obj.ApplicationName = applicationName;
                success = true;
                return obj;
            }
            catch (Exception ex)
            {
                success = false;
                logger.Warn(ex, $"Unable to load json config file from {path}! Deleting old file and saving a new one.");

                try
                {
                    File.Delete(path);
                }
                catch (Exception ex2)
                {
                    logger.Warn(ex2, "Unable to delete corrupt json config file from disk!");
                }

                return CreateNew<T>(applicationName);
            }
            finally
            {
                if (success)
                    logger.Info("Successfully loaded json config file from disk!");
            }
        }

        public virtual void Save()
        {
            try
            {
                File.WriteAllText(GetFilePath(ApplicationName), JsonConvert.SerializeObject(this, Formatting.Indented));

                logger.Info("Successfully saved json config file to disk!");
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Unable to save json config file to disk!");
            }
        }

        private static string GetFilePath(string applicationName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, applicationName + ".json");
        }

        private static T CreateNew<T>(string applicationName = "config") where T : JsonConfig, new()
        {
            T obj = new T { ApplicationName = applicationName };
            obj.Save();
            return obj;
        }
    }
}
