/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using System;
using System.Reflection;
using System.Xml.Linq;
using Parsing.Converters;
using Parsing.Extraction;

namespace Parsing.Augmenting
{
    /// <summary>
    ///     Augment an TObject instance with data TData extracted from TElement.
    /// </summary>
    /// <typeparam name="TObject">Object with which to augment data</typeparam>
    /// <typeparam name="TType"></typeparam>
    public class ResourceValueAugmenter<TObject, TType> : IObjectAugmenter<XElement, TObject>
    {
        /// <summary>
        ///     Name of the XElement attribute to extract from.
        /// </summary>
        protected string AttributeName;

        /// <summary>
        ///     The name of the variable to augment on TObject.
        /// </summary>
        protected string VariableName;

        public ResourceValueAugmenter(string attributeName, string variableName)
        {
            AttributeName = attributeName;
            VariableName = variableName;
            Extractor = new ResourceValueExtractor(attributeName);
            Converter = new ResourceValueConverter<string>();
        }

        /// <summary>
        ///     A data extractor used to extract data from TObject.
        /// </summary>
        protected ResourceValueExtractor Extractor { get; set; }

        /// <summary>
        ///     Converts data to the right format.
        /// </summary>
        protected ResourceValueConverter<string> Converter { get; set; }

        public virtual bool CanAugment(XElement element)
        {
            return Extractor.IsExtractable(element);
        }

        public virtual void Augment(XElement element, TObject obj)
        {
            if (CanAugment(element))
            {
                // Extract the value from the data. 
                var value = Extractor.ExtractData(element);

                // Convert the data only if it's convertable. 
                if (Converter.CanConvert(value))
                {
                    // Convert the data to the new format. 
                    var data = Converter.ConvertObject<TType>(value);
                    AugmentObject(obj, data);
                }
                else
                {
                    AugmentObject(obj, value);
                }
            }
        }

        /// <summary>
        ///     Uses reflection to augment an object with data.
        /// </summary>
        /// <param name="obj">An object to augment</param>
        /// <param name="value">The data to augment the object with. </param>
        protected void AugmentObject(TObject obj, object value)
        {
            // The parameter used to filter out bad object properties and fields.
            // Avoids retrieving private or static data. 
            var retrievalConditions = BindingFlags.Instance | BindingFlags.Public;

            // Try to get the property with the given name. 
            var property = obj.GetType().GetProperty(VariableName, retrievalConditions);

            // Try to get the field with the given name. 
            var field = obj.GetType().GetField(VariableName, retrievalConditions);

            // Set the value to the ability object. 
            if (property != null)
            {
                property.SetValue(obj, value);
            }
            else if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                // Failed to find the variable on the given object
                throw new ArgumentException(
                    string.Format("Failed to find {0} on the object.", VariableName)
                    );
            }
        }
    }
}