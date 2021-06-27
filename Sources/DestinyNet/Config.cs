using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace DestinyNet
{
    public interface IAppSettingsBuilder
    {
        #region Methods
        void CreateConfiguration(string pathToConfigFile);
        TSettingsModel BuildSettingsModel<TSettingsModel>(string sectionKey) where TSettingsModel : class;
        TSettingsModel BuildSettingsModel<TSettingsModel>(IConfigurationSection section) where TSettingsModel : class;
        IList<TSettingsModel> BuildSettingsModels<TSettingsModel>(string sectionKey) where TSettingsModel : class;
        #endregion
    }
    public class AppSettingsBuilder : IAppSettingsBuilder
    {
        #region Fields
        private readonly IConfigurationBuilder _configurationBuilder;
        private IConfiguration _configuration;
        #endregion

        #region ctor
        public AppSettingsBuilder()
        {
            _configurationBuilder = new ConfigurationBuilder();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create configuration from the config file.
        /// </summary>
        /// <param name="pathToConfigFile">Path to config file. Supports *.json and *.xml files</param>
        public void CreateConfiguration(string pathToConfigFile)
        {
            if (!File.Exists(pathToConfigFile))
            {
                throw new FileNotFoundException("File not found by path:", pathToConfigFile);
            }
            string configFileExtension = Path.GetExtension(pathToConfigFile);
            switch (configFileExtension)
            {
                case ".json":
                    CreateConfigurationFromJson(pathToConfigFile);
                    break;

                case ".xml":
                    CreateConfigurationFromXml(pathToConfigFile);
                    break;
                case ".config":
                    CreateConfigurationFromXml(pathToConfigFile);
                    break;

                default:
                    throw new NotSupportedException($"File extention {configFileExtension} is not supported!");
            }
        }
        public TSettingsModel BuildSettingsModel<TSettingsModel>(string sectionKey) where TSettingsModel : class
        {
            return BuildSettingsModel<TSettingsModel>(_configuration.GetSection(sectionKey));
        }

        public TSettingsModel BuildSettingsModel<TSettingsModel>(IConfigurationSection section) where TSettingsModel : class
        {
            return section.Get<TSettingsModel>();
        }

        public IList<TSettingsModel> BuildSettingsModels<TSettingsModel>(string sectionKey) where TSettingsModel : class
        {
            IList<TSettingsModel> settingsModels = new List<TSettingsModel>();

            foreach (IConfigurationSection item in _configuration.GetSection(sectionKey).GetChildren())
            {
                settingsModels.Add(BuildSettingsModel<TSettingsModel>(item));
            }

            return settingsModels;
        }

        private void CreateConfigurationFromJson(string pathToConfigFile)
        {
            _configuration = _configurationBuilder.AddJsonFile(pathToConfigFile).Build();
        }

        private void CreateConfigurationFromXml(string pathToConfigFile)
        {
            _configuration = _configurationBuilder.AddXmlFile(pathToConfigFile).Build();
        }
        #endregion
    }
}