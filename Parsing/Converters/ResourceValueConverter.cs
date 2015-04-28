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

namespace Parsing.Converters
{
    /// <summary>
    ///     Provides conversion for .Nets standard predefined types.
    /// </summary>
    /// <typeparam name="TData">
    ///     The data object that needs conversion.
    /// </typeparam>
    public class ResourceValueConverter<TData> : IObjectConverter<TData>
    {
        public bool CanConvert(TData obj)
        {
            var typeCode = (Type.GetTypeCode(obj.GetType()));

            return typeCode != TypeCode.Object ||
                   typeCode != TypeCode.Empty ||
                   typeCode != TypeCode.DBNull;
        }

        public TType ConvertObject<TType>(TData obj)
        {
            if (CanConvert(obj))
            {
                return ChangeType(obj, typeof (TType));
            }

            return default(TType);
        }

        /// <summary>
        ///     Converts the value to the target type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private dynamic ChangeType(TData value, Type targetType)
        {
            var typeCode = Type.GetTypeCode(targetType);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return Convert.ToBoolean(value);
                case TypeCode.Byte:
                    return Convert.ToByte(value);
                case TypeCode.Char:
                    return Convert.ToChar(value);
                case TypeCode.DBNull:
                    return Convert.DBNull;
                case TypeCode.DateTime:
                    return Convert.ToDateTime(value);
                case TypeCode.Decimal:
                    return Convert.ToDecimal(value);
                case TypeCode.Double:
                    return Convert.ToDouble(value);
                case TypeCode.Empty:
                    return null;
                case TypeCode.Int16:
                    return Convert.ToInt16(value);
                case TypeCode.Int32:
                    return Convert.ToInt32(value);
                case TypeCode.Int64:
                    return Convert.ToInt64(value);
                case TypeCode.Object:
                    return value;
                case TypeCode.SByte:
                    return Convert.ToSByte(value);
                case TypeCode.Single:
                    return Convert.ToSingle(value);
                case TypeCode.String:
                    return value;
                case TypeCode.UInt16:
                    return Convert.ToUInt16(value);
                case TypeCode.UInt32:
                    return Convert.ToUInt32(value);
                case TypeCode.UInt64:
                    return Convert.ToUInt64(value);
                default:
                    // Create a friendly error message informing the user that we cannot 
                    // convert their type. 
                    var errorMessage =
                        string.Format("Conversion from type {0} to {1} not supported. ",
                            value.GetType(), targetType);
                    throw new NotSupportedException(errorMessage);
            }
        }
    }
}