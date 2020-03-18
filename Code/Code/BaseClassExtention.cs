// <copyright file="BaseClassExtention.cs" company="Holger Kühn">
// Copyright (c) 2020 - 2020 Holger Kühn. All rights reserved.
// </copyright>

namespace DynDNSClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Policy;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.XPath;

    /// <summary>
    /// extension class for base types
    /// </summary>
    public static class BaseClassExtention
    {
        #region low-level extensions

        /// <summary>
        /// text left of the first search string
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text left of searched text; if this is not present the whole test is returned</returns>
        public static string LeftOf(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.IndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return text;
            }

            return text.Substring(0, pos);
        }

        /// <summary>
        /// text that is left of last match of searched text
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text that is left of last match of searched text; if this is not present the whole test is returned</returns>
        public static string LeftOfLast(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.LastIndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return string.Empty;
            }

            return text.Substring(0, pos);
        }

        /// <summary>
        ///     text right of the first search string
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text that is right of first match of searched text; if this is not present an empty text is returned</returns>
        public static string RightOf(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.IndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return string.Empty;
            }

            return text.Substring(pos + searchtext.Length);
        }

        /// <summary>
        ///     text that is right of last match of searched text
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text that is right of last match of searched text; if this is not present the whole test is returned</returns>
        public static string RightOfLast(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.LastIndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return text;
            }

            return text.Substring(pos + searchtext.Length);
        }
        #endregion low-level extensions
        #region high-level extensions

        /// <summary>
        ///     durch ein Trennzeichen getrennte Bestandteile eines Textes in eine Liste einfügen
        /// </summary>
        /// <param name="text">der aufzuteilende Text</param>
        /// <param name="delimeter">Trennzeichen (Standardwert: "\n")</param>
        /// <param name="omitCharacters">am Anfang und Ende eines Bestandteiles zu ignorierende Zeichen (Standardwert: "null")</param>
        /// <param name="addEmptyEntries">fügt Bestandteile auch dann hinzu, wenn diese leer sind (Standardwert: "true")</param>
        /// <returns>list with text parts delemited by delimeter</returns>
        public static List<string> Split(this string text, string delimeter = "\n", string omitCharacters = null, bool addEmptyEntries = true)
        {
            if (text == null || delimeter == null)
            {
                return null;
            }

            string[] resultArray = text.Split(new string[] { delimeter }, StringSplitOptions.None);
            List<string> result = new List<string>();

            for (int i = 0; i < resultArray.Length; i++)
            {
                string entry = null;
                if (omitCharacters == null)
                {
                    entry = resultArray[i];
                }
                else
                {
                    entry = resultArray[i].Trim(omitCharacters.ToCharArray());
                }

                if (addEmptyEntries || !string.IsNullOrEmpty(entry))
                {
                    result.Add(entry);
                }
            }

            return result;
        }

        #endregion high-level extensions
        #region XML-Files

        /// <summary>
        /// reads root node of XML file
        /// </summary>
        /// <param name="xmlFile">Name of XML file to be read</param>
        /// <param name="xmlSchema">Name of XML-Schema file to be read</param>
        /// <param name="rootNodeName">Name of root node</param>
        /// <returns>XmlNode containing root node</returns>
        public static XmlNode XmlReadFile(string xmlFile, string xmlSchema, string rootNodeName)
        {
            // check if filename exists
            if (!File.Exists(xmlFile))
            {
                return null;
            }

            // create XmlReader and XmlDocument from file
            XmlReaderSettings settings = new XmlReaderSettings();

            if (xmlSchema != null && File.Exists(xmlSchema)) {
                settings.Schemas.Add("DynDNSClient", xmlSchema);
                settings.ValidationType = ValidationType.Schema;
                settings.DtdProcessing = DtdProcessing.Parse;
            } else
            {
                settings.ValidationType = ValidationType.None;
                settings.DtdProcessing = DtdProcessing.Ignore;
            }

            XmlReader xmlReader = XmlReader.Create(xmlFile, settings);
            Evidence myEvidence = new Evidence();
            XmlDocument xmlInputFile = new XmlDocument();
            xmlInputFile.XmlResolver = new XmlSecureResolver(new XmlUrlResolver(), myEvidence);

            try
            {
                xmlInputFile.Load(xmlReader);
                xmlInputFile.Validate(new ValidationEventHandler(new ValidationEventHandler(XmlReadFileValidation)));
            }
            catch (XmlException ex)
            {
                throw new Exception("The file \"" + xmlFile + "\" does not contain valid XML.\n\n" + ex.ToString());
            }

            return xmlInputFile.SelectSingleNode(rootNodeName);
        }

        /// <summary>
        /// select first node matching subnodename
        /// </summary>
        /// <param name="node">XmlNode, that contains subnodes</param>
        /// <param name="subnodename">name of subsequent XML-node</param>
        /// <returns>first node matching subnodename</returns>
        public static XmlNode XmlReadSubnode(this XmlNode node, string subnodename)
        {
            if (node == null)
            {
                return null;
            }

            return node.SelectSingleNode(subnodename);
        }

        /// <summary>
        /// selects many XML-nodes matching subnodename
        /// </summary>
        /// <param name="node">XmlNode, that contains subnodes</param>
        /// <param name="subnodename">name of subsequent XML-node</param>
        /// <param name="uniqueAttribute">name of unique attribute used to identify node</param>
        /// <returns>List of XmlNodes identified by subnodename</returns>
        public static List<XmlNode> XmlReadSubnodes(this XmlNode node, string subnodename, string uniqueAttribute = null)
        {
            if (node == null)
            {
                return new List<XmlNode>();
            }

            List<XmlNode> result = new List<XmlNode>();

            foreach (XmlNode subnode in node.SelectNodes(subnodename))
            {
                result.Add(subnode);
            }

            if (uniqueAttribute != null)
            {
                List<string> usedAttributes = new List<string>();

                foreach (XmlNode entry in result)
                {
                    string value = entry.XmlReadAttribute(uniqueAttribute, null);

                    if (value == null || usedAttributes.Contains(value))
                    {
                        throw new Exception("The node \"" + node.Name + "\" contains two different nodes with identical \"UniqueAttribute\" \"" + uniqueAttribute + "\".");
                    }

                    usedAttributes.Add(value);
                }
            }

            return result;
        }

        /// <summary>
        /// reads attribute from XML-node
        /// </summary>
        /// <param name="node">XmlNode, that contains attribute</param>
        /// <param name="attributename">name of attribute</param>
        /// <param name="defaultvalue">default value used, when specified attribute does not exist or empty</param>
        /// <returns>value of attribute</returns>
        public static string XmlReadAttribute(this XmlNode node, string attributename, string defaultvalue)
        {
            if (node == null || node.Attributes[attributename] == null)
            {
                return defaultvalue;
            }

            return node.Attributes[attributename].Value;
        }

        /// <summary>
        /// reads inner Text of XML-node
        /// </summary>
        /// <param name="node">XmlNode, that contains inner text</param>
        /// <param name="defaultvalue">default value used, when specified inner text does not exist or empty</param>
        /// <returns>value of inner text</returns>
        public static string XmlReadInnerText(this XmlNode node, string defaultvalue)
        {
            if (node == null)
            {
                return defaultvalue;
            }

            return node.InnerText;
        }

        /// <summary>
        /// writes output of validation to console
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private static void XmlReadFileValidation(object sender, ValidationEventArgs e)
        {
            Console.WriteLine("Validation Error: {0}", e.Message);
        }

        #endregion
    }
}
