using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parsing.Parsers
{
    public abstract class BaseParser
    {
        /// <summary>
        /// A collection of resources to search for values. 
        /// </summary>
        protected readonly IEnumerable<XElement> _resources;

        /// <summary>
        /// Retrieve all resources within the given directory. 
        /// </summary>
        /// <param name="resourcesPath"></param>
        protected BaseParser(string resourcesPath)
        {
            // Read in all resources in the resourcePath. 
            _resources = LoadResources(resourcesPath);
        }

        /// <summary>
        /// Ensures that the resource file passed exists
        /// and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XElement> LoadResources(string path)
        {
            // List to store all read resources. 
            List<XElement> XmlDocuments = new List<XElement>();

            // Get a list of all resource file names. 
            if (!Directory.Exists(path)) return new List<XElement>();

            string[] resources = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory. 
            foreach (var resource in resources)
            {
                XmlDocuments.Add(XElement.Load(resource));
            }

            return XmlDocuments;
        }
    }
}
