// <copyright file="SettingsNetworkCredential.cs" company="Holger Kühn">
// Copyright (c) 2020 - 2020 Holger Kühn. All rights reserved.
// </copyright>
namespace DynDNSClient
{
    using System.Net;
    using System.Xml;

    /// <summary>
    /// provides username und password for login to DNS-Service
    /// </summary>
    public class SettingsNetworkCredential
    {
        #region Attributes

        #region SettingsNetworkCredential

        /// <summary>
        /// is settingsfile encrypted
        /// </summary>
        private readonly bool isEncrypted;

        /// <summary>
        /// username of DynDNS-Service
        /// </summary>
        private readonly SettingsEncryptedString username;

        /// <summary>
        /// username of DynDNS-Service
        /// </summary>
        private readonly SettingsEncryptedString password;

        /// <summary>
        /// network credentials for web client
        /// </summary>
        private readonly NetworkCredential networkCredential;

        #endregion
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsNetworkCredential"/> class.
        /// </summary>
        /// <param name="settings">provides XmlNode with settings</param>
        public SettingsNetworkCredential(XmlNode settings)
        {
            this.isEncrypted = true;

            // read username from Xml
            XmlNode xmlUsername = settings.XmlReadSubnode("Username");
            string usernameIsEncrypted = xmlUsername.XmlReadSubnode("IsEncrypted").InnerText;
            string usernameValue = xmlUsername.XmlReadSubnode("Value").InnerText;

            // read password from Xml
            XmlNode xmlPassword = settings.XmlReadSubnode("Username");
            string passwordIsEncrypted = xmlUsername.XmlReadSubnode("IsEncrypted").InnerText;
            string passwordValue = xmlUsername.XmlReadSubnode("Value").InnerText;

            // setting values accordingly
            if (usernameIsEncrypted == "false")
            {
                this.isEncrypted = false;

                this.username = new SettingsEncryptedString(SettingsEncryptedString.SettingsEncryptedStringMode.DecryptedToEncryptedString, usernameValue);
            }

            if (passwordIsEncrypted == "false")
            {
                this.isEncrypted = false;

                this.password = new SettingsEncryptedString(SettingsEncryptedString.SettingsEncryptedStringMode.DecryptedToEncryptedString, passwordValue);
            }

            this.networkCredential = new NetworkCredential(this.username.DecryptedString, this.password.SecureString);
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
