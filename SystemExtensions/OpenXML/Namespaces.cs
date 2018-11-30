/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.

THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.OpenXML
{
    /// <summary>
    /// Provides a standard way to access the namespace prefixes for the Open XML document namespaces.
    /// </summary>
    public static class Prefixes
    {
        // Standard Open XML Namespace prefixes
        public const string SpreadsheetML = "x";
        public const string Relationships = "r";
    }

    /// <summary>
    /// Provides a standard way to access the namespace URIs for the Open XML document namespaces.
    /// Also provides a preconstructed namespace manager for the namespaces and prefixes.
    /// </summary>
    public static class Namespaces
    {
        // Standard Open XML Namespace URIs
        public const string SpreadsheetML = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        public const string Relationships = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        #region Namespace Manager Methods
        private static XmlNamespaceManager m_namespaceManager = new XmlNamespaceManager(new NameTable());

        /// <summary>
        /// Initializes the namespace manager with all of the Open XML document namespaces.
        /// </summary>
        static Namespaces()
        {
            // add each namespace to the namespace manager
            m_namespaceManager.AddNamespace(Prefixes.SpreadsheetML, Namespaces.SpreadsheetML);
            m_namespaceManager.AddNamespace(Prefixes.Relationships, Namespaces.Relationships);
        }

        /// <summary>
        /// Returns the static namespace manager 
        /// </summary>
        public static XmlNamespaceManager NamespaceManager
        {
            get { return m_namespaceManager; }
        }
        #endregion
    }
}
