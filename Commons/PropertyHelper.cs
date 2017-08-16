using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace bOS.Commons
{
    public class PropertyHelper
    {
        static String[] typeManaged = new String[] { "System.String", "System.Int16", "System.Int32" , "System.Boolean"};

        public static List<PropertyInfo> GetProperty2Manage(Type type, BindingFlags flags)
        {
            
            List<PropertyInfo> managedProperties = new List<PropertyInfo>();

            var properties = type.GetProperties(flags);
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && typeManaged.Contains(property.PropertyType.FullName))
                {
                    managedProperties.Add(property);
                }
            }

            return managedProperties;
        }

        public static String GetValue(Object obj, PropertyInfo property)
        {
            //retrieve value
            String propertyValue = String.Empty;
            Object objValue = property.GetValue(obj, null);

            if (property.PropertyType == typeof(String))
            {
                propertyValue = (String)objValue;
            }
            else if (property.PropertyType == typeof(Int16))
            {
                propertyValue = ((Int16)objValue).ToString();
            }
            else if (property.PropertyType == typeof(Int32))
            {
                propertyValue = ((Int32)objValue).ToString();
            }
            else if (property.PropertyType == typeof(Boolean))
            {
                propertyValue = ((Boolean)objValue).ToString();
            }

            return propertyValue;
        }

        public static void SetValue(Object obj, PropertyInfo piShared, String value)
        {
            Object objValue = null;
            if (piShared.PropertyType == typeof(String))
            {
                objValue = value;
            }
            else if (piShared.PropertyType == typeof(Int16))
            {
                objValue = Int16.Parse(value);
            }
            else if (piShared.PropertyType == typeof(Int32))
            {
                objValue = Int32.Parse(value);
            }
            else if (piShared.PropertyType == typeof(Boolean))
            {
                objValue = Boolean.Parse(value);
            }
            else
            {
                if (piShared.PropertyType.IsGenericType &&
                    piShared.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type type = piShared.PropertyType.GetGenericArguments()[0];
                    if (type == typeof(String))
                    {
                        objValue = value;
                    }
                    else if (type == typeof(Int16))
                    {
                        objValue = Int16.Parse(value);
                    }
                    else if (type == typeof(Int32))
                    {
                        objValue = Int32.Parse(value);
                    }
                    
                }
            }
            //piShared.SetValue(obj, Convert.ChangeType(value, piShared.PropertyType), null);
            piShared.SetValue(obj, objValue, null);
        }

    }
}
