using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SpecificationDemoConsole
{
    public static class IEnumerableConversionExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        where T : class
        {
            var table = new DataTable();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                table.Columns.Add(propertyInfo.Name);
            }

            var countMethodInfo = typeof(System.Linq.Enumerable)
                .GetMethods()
                .Single(method => method.Name == "Count" && method.IsStatic && method.GetParameters().Length == 1);

            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    var isEnumerable = propertyInfo.PropertyType
                        .GetInterfaces()
                        .Any(t => t.IsGenericType &&
                            t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                    if (isEnumerable && propertyInfo.PropertyType != typeof(string))
                    {
                        var collection = propertyInfo.GetValue(item, null);
                        if (collection != null)
                        {
                            var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
                            var localCountMethodInfo = countMethodInfo.MakeGenericMethod(genericArgument);
                            var count = localCountMethodInfo.Invoke(null, new object[] { collection });

                            row[propertyInfo.Name] = $"{count} items";
                        }
                    }
                    else
                    {
                        row[propertyInfo.Name] = propertyInfo.GetValue(item) ?? DBNull.Value;
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}