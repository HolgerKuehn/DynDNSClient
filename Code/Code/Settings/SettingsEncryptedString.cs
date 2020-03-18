// <copyright file="SettingsEncryptedString.cs" company="Holger Kühn">
// Copyright (c) 2020 - 2020 Holger Kühn. All rights reserved.
// </copyright>

namespace DynDNSClient
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// represents a single setting
    /// </summary>
    public class SettingsEncryptedString
    {
        #region attributes

        /// <summary>
        /// encrypted String used in settingsfile
        /// </summary>
        private string encryptedString;

        /// <summary>
        /// unencrypted String
        /// </summary>
        private string decryptedString;

        /// <summary>
        /// secure string
        /// </summary>
        private SecureString secureString;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsEncryptedString"/> class.
        /// </summary>
        /// <param name="mode">defines the conversion to perform</param>
        /// <param name="value">value of setting</param>
        public SettingsEncryptedString(SettingsEncryptedStringMode mode, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            //if (mode = )
            //{
            //    this.encryptedString = value;
            //    this.decryptedString = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(value), Encoding.UTF8.GetBytes(string.Empty), DataProtectionScope.CurrentUser));
            //}

            if (mode == SettingsEncryptedStringMode.DecryptedToEncryptedString)
            {
                this.encryptedString = Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(string.Empty), DataProtectionScope.CurrentUser));
                this.decryptedString = value;

                this.secureString = new SecureString();
                foreach (char c in value)
                {
                    this.secureString.AppendChar(c);
                }
            }
        }

        #endregion
        #region Enums Part 1

        /// <summary>
        /// conversion mode for EncryptedString
        /// </summary>
        public enum SettingsEncryptedStringMode
        {
            /// <summary>
            /// converts DecryptedString to EncryptedString and SecureString
            /// </summary>
            DecryptedToEncryptedString,
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets encrypted string of EncryptedString
        /// </summary>
        public string EncryptedString
        {
            get { return this.encryptedString; }
        }

        /// <summary>
        /// Gets decrypted string of EncryptedString
        /// </summary>
        public string DecryptedString
        {
            get { return this.decryptedString; }
        }

        /// <summary>
        /// Gets secure string of EncryptedString
        /// </summary>
        public SecureString SecureString
        {
            get { return this.secureString; }
        }
        #endregion
    }
}
