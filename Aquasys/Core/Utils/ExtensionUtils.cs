using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json;
using DevExpress.Maui.Core.Internal;
using static Aquasys.Core.Utils.ObjectExtensions;

namespace Aquasys.Core.Utils
{
    public static class ExtensionUtils
    {

        /// <summary>
        /// Inserts an item into a list in the correct place, based on the provided key and key comparer. Use like OrderBy(o => o.PropertyWithKey).
        /// </summary>
        public static void InsertInPlace<TItem, TKey>(this ObservableCollection<TItem> collection, TItem itemToAdd, Func<TItem, TKey> keyGetter)
        {
            int index = collection.ToList().BinarySearch(keyGetter(itemToAdd), Comparer<TKey>.Default, keyGetter);
            collection.Insert(index, itemToAdd);
        }

        /// <summary>
        /// Binary search.
        /// </summary>
        /// <returns>Index of item in collection.</returns> 
        /// <notes>This version tops out at approximately 25% faster than the equivalent recursive version. This 25% speedup is for list
        /// lengths more of than 1000 items, with less performance advantage for smaller lists.</notes>
        public static int BinarySearch<TItem, TKey>(this IList<TItem> collection, TKey keyToFind, IComparer<TKey> comparer, Func<TItem, TKey> keyGetter)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int lower = 0;
            int upper = collection.Count - 1;

            while (lower <= upper)
            {
                int middle = lower + (upper - lower) / 2;
                int comparisonResult = comparer.Compare(keyToFind, keyGetter.Invoke(collection[middle]));
                if (comparisonResult == 0)
                {
                    return middle;
                }
                else if (comparisonResult < 0)
                {
                    upper = middle - 1;
                }
                else
                {
                    lower = middle + 1;
                }
            }

            // If we cannot find the item, return the item below it, so the new item will be inserted next.
            return lower;
        }

        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }

        public static bool HasProperty(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetProperty(methodName) != null;
        }



        public static bool IsNullOrEmpty(this string String)
        {
            return String == null || string.IsNullOrEmpty(String.Trim());
        }

        public static bool IsNotNullOrEmpty(this string valor) =>
            !string.IsNullOrEmpty(valor);

        /// <summary>
        /// Obtém o valor de determinada propriedade do objeto recebido.
        /// </summary>
        /// <param name="entity">Entidade da qual será obtido o valor</param>
        /// <param name="propertyInfoObject">Campo da entidade</param>
        /// <returns>Valor de retorno</returns>
        public static object GetPropertyValue(this object entity,
                                              string propertyName, bool withFullPath = false)
        {
            if (propertyName.IsNullOrEmpty()) return null;


            if (propertyName.Contains("."))
            {
                if (withFullPath)
                    return GetValueByPath(entity, propertyName);

                var propriedades = propertyName.Split('.').Where(x => x != propertyName.Split('.').First()).ToList();
                object valor = entity;
                foreach (var item in propriedades)
                {
                    PropertyInfo p = valor.GetType().GetProperty(item);
                    valor = p.GetValue(valor);
                }
                return valor;
            }

            PropertyInfo propertyinfo = entity.GetType().GetProperty(propertyName);

            return propertyinfo?.GetValue(entity);
        }

        public static void SetPropertyValue(this object entity, string propertyName, object value)
        {
            PropertyInfo p = entity.GetType().GetProperty(propertyName);
            p.SetValue(entity, value, null);
        }

        private static object GetValueByPath(object obj, string path)
        {
            var parts = path.Split('.');
            foreach (var part in parts)
            {
                if (obj == null)
                    return null;

                if (Array.IndexOf(parts, part) == 0)
                    continue;

                if (part.Contains("["))
                {
                    // Se a parte contém [ e ], trata-se de uma lista
                    var propName = part.Substring(0, part.IndexOf("["));
                    var index = int.Parse(part
                        .Substring(part.IndexOf("[") + 1, part.IndexOf("]") - part.IndexOf("[") - 1));

                    var prop = obj.GetType().GetProperty(propName);
                    if (prop == null)
                        return null;

                    obj = prop.GetValue(obj);

                    if (obj is IEnumerable<object> enumerable)
                    {
                        obj = enumerable.ElementAtOrDefault(index);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    // Caso contrário, trata-se de uma propriedade simples
                    var prop = obj.GetType().GetProperty(part);
                    if (prop == null)
                        return null;

                    obj = prop.GetValue(obj);
                }
            }

            return obj;
        }

        public static IEnumerable<TSource> DistinctList<TSource>(
           this IEnumerable<TSource> source,
           Func<TSource, TSource, bool> metodoEquals,
           Func<TSource, int> metodoGetHashCode)
           => source.Distinct(
           ComparadorGenerico<TSource>.Criar(
           metodoEquals,
           metodoGetHashCode)
       );

        public static long? ToInt64Nullable(this string valor) =>
           valor.IsNullOrEmpty() ? (long?)null : Convert.ToInt64(valor);
        public static long ToInt64(this string valor) =>
            Convert.ToInt64(valor);

        public static bool ToBoolean(this string valor) =>
            Convert.ToBoolean(valor);
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            rest = list.Skip(2).ToList();
        }
        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T terceiro, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            terceiro = list.Count > 2 ? list[2] : default(T); // or throw
            rest = list.Skip(3).ToList();
        }

        public static bool HasDuplicate<TEntity>(this IEnumerable<TEntity> lista)
        {
            var set = new HashSet<TEntity>();
            foreach (var element in lista)
            {
                if (!set.Add(element))
                {
                    return true;
                }
            }
            return false;
        }

        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj);

    }
}
