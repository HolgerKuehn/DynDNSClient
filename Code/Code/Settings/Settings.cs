// <copyright file="Settings.cs" company="Holger Kühn">
// Copyright (c) 2020 - 2020 Holger Kühn. All rights reserved.
// </copyright>

namespace DynDNSClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// stores all parameters configured by xml-files<br/>
    /// <br/>
    /// used xml-files<br/>
    /// Settings.xml - provides values needed to update DNS-values<br/>
    /// </summary>
    public class Settings
    {
        #region Attributes

        #region Settings

        /// <summary>
        /// xml-settingsfile
        /// </summary>
        private readonly string settingsFileXml;

        /// <summary>
        /// schema-settingsfile
        /// </summary>
        private readonly string settingsFileSchema;

        /// <summary>
        /// NetworkCredential according to settings
        /// </summary>
        private readonly SettingsNetworkCredential settingsNetworkCredential;

        #endregion
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            this.settingsFileXml = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\DynDNSClient\\Data\\Settings.xml";
            this.settingsFileSchema = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\DynDNSClient\\Schema\\Settings.xsd";

            if (!File.Exists(this.settingsFileXml))
            {
                throw new FileNotFoundException("Settings.xml does not exist.");
            }

            if (!File.Exists(this.settingsFileSchema))
            {
                throw new FileNotFoundException("Settings.xsd does not exist.");
            }

            XmlNode settings = BaseClassExtention.XmlReadFile(this.settingsFileXml, this.settingsFileSchema, "DynDNSClient").ChildNodes.Item(0);

            this.settingsNetworkCredential = new SettingsNetworkCredential(settings);

        }

        #endregion
        #region Properties
        #region Settings

        #endregion
        #endregion
        #region Methods

        #endregion
    }
}