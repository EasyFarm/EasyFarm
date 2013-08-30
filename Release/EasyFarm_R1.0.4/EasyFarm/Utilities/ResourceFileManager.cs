using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using EasyFarm.Properties;

namespace EasyFarm.UtilityTools
{
    public static class ResourceFileManager
    {
        /// <summary>
        /// Sets a file as a settings property
        /// </summary>
        /// <param name="Name">Name to save the file as a property under</param>
        /// <param name="PathToResource">Path to the file</param>
        public static void SetResource(String Name, String PathToResource)
        {
            ResourceEncoder.EncodeResourceToProperties(Name, PathToResource);
        }

        /// <summary>
        /// Returns a FileInfo obj associated with the resource.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static byte[] GetResource(String Name)
        {
            return ResourceEncoder.DecodePropertyToBytes(Name);
        }

        /// <summary>
        /// Encodes and Decodes Files to be stored as properties so that,
        /// if resource files don't exist, they can be generated.
        /// </summary>
        private static class ResourceEncoder
        {
            /// <summary>
            /// Encodes a resource from a series of bytes to Base64.
            /// Returns the resource file as a base64 string.
            /// </summary>
            /// <param name="ResourceAsBytes">Resource file in byte form</param>
            /// <returns>Base64 encoded string</returns>
            public static String EncodeResourceToBase64FromBytes(byte[] ResourceAsBytes)
            {
                return Convert.ToBase64String(ResourceAsBytes);
            }

            /// <summary>
            /// Encodes a resource file from a path.
            /// Returns the base 64 string of that file.
            /// </summary>
            /// <param name="ResourceAsPath">Path to the resource</param>
            /// <returns>Base 64 encoded string</returns>
            public static String EncodeResourceToBase64FromString(String ResourceAsPath)
            {
                byte[] resourceAsBytes = File.ReadAllBytes(ResourceAsPath);
                return Convert.ToBase64String(resourceAsBytes);
            }

            /// <summary>
            /// Takes in a path to a resource as a string, encodes it to base 64,
            /// and sets it as a property under PropertyName
            /// </summary>
            /// <param name="PropertyName">Name to save the property under</param>
            /// <param name="Resource">Path to the resource file</param>
            public static void EncodeResourceToProperties(String PropertyName, String Resource)
            {
                // Encode the string to base 64
                String EncodedResource = EncodeResourceToBase64FromString(Resource);

                // Create the property             
                //EasyFarm.Properties.Settings.Default.

                // Remove the property if it already exists.
                //Settings.Default.Properties.Remove(PropertyName);

                // Add the property.
                //Settings.Default.Properties.Add(ResourceProperty);
            }

            /// <summary>
            /// Decodes the property specified by PropertyName to a string.
            /// It returns the unencoded base64 string.
            /// </summary>
            /// <param name="FileName"></param>
            /// <param name="PropertyName"></param>
            /// <returns></returns>
            public static String DecodePropertyToString(String PropertyName)
            {
                // Get out desired resource property
                var ResourceProperty = Settings.Default.Properties[PropertyName];

                // No property existed!
                if (ResourceProperty == null)
                    throw new Exception("No property found; Error in DecodePropertyToString");

                // Grab its resource value; it's ok to cast, since this method should only be used for base64 strings.
                String ResourceValue = (String)ResourceProperty.GetType().GetProperty(PropertyName).GetValue(ResourceProperty, null);

                // Comvert base 64 string to an array of bytes.
                String ResourceAsString = DecodeResourceFromBase64AsString(ResourceValue);

                // Return the decoded resource.
                return ResourceAsString;
            }

            /// <summary>
            /// Decodes the property specified by PropertyName to an array of bytes.
            /// It returns the unencoded base64 string.
            /// </summary>
            /// <param name="PropertyName"></param>
            /// <returns></returns>
            public static byte[] DecodePropertyToBytes(String PropertyName)
            {
                return Encoding.ASCII.GetBytes(DecodePropertyToString(PropertyName));
            }

            /// <summary>
            /// Returns the decoded base 64 string as an array of bytes.
            /// </summary>
            /// <param name="ResourceAsBase64String">Base64 String</param>
            /// <returns>Decoded byte[]</returns>
            public static byte[] DecodeResourceFromBase64AsBytes(String ResourceAsBase64String)
            {
                return Convert.FromBase64String(ResourceAsBase64String);
            }

            /// <summary>
            /// Returns the decoded base64 string as string.
            /// </summary>
            /// <param name="ResourceAsBase64String">Base64 String</param>
            /// <returns>Decoded String</returns>
            public static String DecodeResourceFromBase64AsString(String ResourceAsBase64String)
            {
                return Convert.FromBase64String(ResourceAsBase64String).ToString();
            }
        }
    }
}
