using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;

namespace LCW.Framework.Common.Util
{
	/// <summary>
	/// Utility methods that are used to convert objects from one type into another.
	/// </summary>
	/// <author>Aleksandar Seovic</author>
	/// <author>Marko Lahma</author>
	public static class ObjectUtils
	{
	    /// <summary>
		/// Convert the value to the required <see cref="System.Type"/> (if necessary from a string).
		/// </summary>
		/// <param name="newValue">The proposed change value.</param>
		/// <param name="requiredType">
		/// The <see cref="System.Type"/> we must convert to.
		/// </param>
		/// <returns>The new value, possibly the result of type conversion.</returns>
		public static object ConvertValueIfNecessary(Type requiredType, object newValue)
		{
            if (newValue != null)
            {
                // if it is assignable, return the value right away
                if (IsAssignableFrom(newValue, requiredType))
                {
                    return newValue;
                }

                // try to convert using type converter
                TypeConverter typeConverter = TypeDescriptor.GetConverter(requiredType);
                if (typeConverter != null && typeConverter.CanConvertFrom(newValue.GetType()))
                {
                    return typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, newValue);
                }
                typeConverter = TypeDescriptor.GetConverter(newValue);
                if (typeConverter != null && typeConverter.CanConvertTo(requiredType))
                {
                    return typeConverter.ConvertTo(null, CultureInfo.InvariantCulture, newValue, requiredType);
                }
                if (requiredType == typeof(Type))
                {
                    return Type.GetType(newValue.ToString(), true);
                }

                throw new NotSupportedException(newValue + " is no a supported value for a target of type " + requiredType);
            }
	        if (requiredType.IsValueType)
	        {
	            return Activator.CreateInstance(requiredType);
	        }

            // return default
	        return newValue;
		}


		/// <summary>
		/// Determines whether value is assignable to required type.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="requiredType">Type of the required.</param>
		/// <returns>
		/// 	<c>true</c> if value can be assigned as given type; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsAssignableFrom(object value, Type requiredType)
		{
			return requiredType.IsAssignableFrom(value.GetType());
		}
		
		/// <summary>
		/// Instantiates an instance of the type specified.
		/// </summary>
		/// <returns></returns>
		public static T InstantiateType<T>()
		{
            return InstantiateType<T>(null);
		}

        public static T InstantiateType<T>(IDictionary<Type,object> dictionary)
        {
            Type type = typeof(T);
            if (type == null)
            {
                throw new ArgumentNullException("type", "Cannot instantiate null");
            }

            List<Type> types = new List<Type>();
            List<object> parameters = new List<object>();
            if (dictionary != null)
            {
                foreach (var obj in dictionary)
                {
                    types.Add(obj.Key);
                    parameters.Add(obj.Value);
                }
            }
            ConstructorInfo ci = type.GetConstructor(types.ToArray());
            if (ci == null)
            {
                throw new ArgumentException("Cannot instantiate type which has no empty constructor", type.Name);
            }
            return (T)ci.Invoke(parameters.ToArray());
        }

	    /// <summary>
		/// Sets the object properties using reflection.
		/// </summary>
        public static void SetObjectProperties(object obj, string[] propertyNames, object[] propertyValues)
		{
			for (int i = 0; i < propertyNames.Length; i++)
			{
				string name = propertyNames[i];
				string propertyName = name.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + name.Substring(1);

				try
				{
				    SetPropertyValue(obj, propertyName, propertyValues[i]);
				}
				catch (Exception nfe)
				{
                    throw nfe;    
                }
			}
		}

	    /// <summary>
		/// Sets the object properties using reflection.
		/// </summary>
		/// <param name="obj">The object to set values to.</param>
		/// <param name="props">The properties to set to object.</param>
		public static void SetObjectProperties(object obj, NameValueCollection props)
		{
            // remove the type
			props.Remove("type");

            foreach (string name in props.Keys)
			{
				string propertyName = name.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + name.Substring(1);

				try
				{
					object value = props[name];
                    SetPropertyValue(obj, propertyName, value);
				}
				catch (Exception nfe)
				{
                    throw nfe;	
                }
			}
		}

        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            Type t = target.GetType();				

            PropertyInfo pi = t.GetProperty(propertyName);

			if (pi == null)
			{
				throw new MemberAccessException(string.Format(CultureInfo.InvariantCulture, "No property '{0}'", propertyName));
			}
			
			MethodInfo mi = pi.GetSetMethod();

            if (mi == null)
            {
                throw new MemberAccessException(string.Format(CultureInfo.InvariantCulture, "Property '{0}' has no setter", propertyName));
            }

		    if (mi.GetParameters()[0].ParameterType == typeof(TimeSpan))
		    {
		        // special handling
		        value = GetTimeSpanValueForProperty(pi, value);
		    }
		    else
            {
                value = ConvertValueIfNecessary(mi.GetParameters()[0].ParameterType, value);
            }

            mi.Invoke(target, new object[] { value });
		}

	    public static TimeSpan GetTimeSpanValueForProperty(PropertyInfo pi, object value)
	    {
            object[] attributes = pi.GetCustomAttributes(typeof(TimeSpanParseRuleAttribute), false);

            if (attributes.Length == 0)
            {
                return (TimeSpan) ConvertValueIfNecessary(typeof(TimeSpan), value);
            }

            TimeSpanParseRuleAttribute attribute = (TimeSpanParseRuleAttribute)attributes[0];
            long longValue = Convert.ToInt64(value);
            switch (attribute.Rule)
            {
                case TimeSpanParseRule.Milliseconds:
                    return TimeSpan.FromMilliseconds(longValue);
                case TimeSpanParseRule.Seconds:
                    return TimeSpan.FromSeconds(longValue);
                case TimeSpanParseRule.Minutes:
                    return TimeSpan.FromMinutes(longValue);
                case TimeSpanParseRule.Hours:
                    return TimeSpan.FromHours(longValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
	    }

	    public static bool IsAttributePresent(Type typeToExamine, Type attributeType)
	    {
	        return typeToExamine.GetCustomAttributes(attributeType, true).Length > 0;
	    }
	}
}
