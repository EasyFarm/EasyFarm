using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Parsing.Parsers
{
    public abstract class BaseParser
    {
        /// <summary>
        ///     A collection of resources to search for values.
        /// </summary>
        protected readonly IEnumerable<XElement> Resources;

        /// <summary>
        ///     Retrieve all resources within the given directory.
        /// </summary>
        /// <param name="resourcesPath"></param>
        protected BaseParser(string resourcesPath)
        {
            // Read in all resources in the resourcePath. 
            Resources = LoadResources(resourcesPath);
        }

        /// <summary>
        ///     Ensures that the resource file passed exists
        ///     and returns the XElement obj associated with the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<XElement> LoadResources(string path)
        {
            // List to store all read resources. 

            // Get a list of all resource file names. 
            if (!Directory.Exists(path)) return new List<XElement>();

            var resources = Directory.GetFiles(path, "*.xml");

            // Load all resource files in the given directory. 

            return resources.Select(XElement.Load).ToList();
        }
    }
}