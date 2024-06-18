using Aquasys.Core.Entities;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Aquasys
{
    public static class ObjectExtensions
    {
        public static bool In(this object obj, params object[] obj2)
        {
            return obj2.Where(x => x.Equals(obj)).Any();
        }
        public static string GetEnumDescription(this Enum enumVal)
        {
            try
            {

                if (enumVal == null)
                    return string.Empty;

                var typeInfo = enumVal.GetType().GetTypeInfo();
                var v = typeInfo.DeclaredMembers.FirstOrDefault(x => x.Name == enumVal.ToString());
                EnumDescription? customAttribute = v?.GetCustomAttribute<EnumDescription>();
                if (customAttribute == null)
                    return string.Empty;
                return customAttribute.Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string TruncateDC(this string String, int numeroString, bool inserirReticencias = true)
        {
            if (!string.IsNullOrEmpty(String))
            {
                if (String.Length > numeroString)
                    return Regex.Replace(String.Substring(0, numeroString), @"\s", " ") + (inserirReticencias ? "..." : string.Empty);
            }
            return String;
        }

        public static bool Between(this DateTime Data, DateTime DataInicio, DateTime? DataFim, bool Inclusivo = true)
        {
            if (Inclusivo)
                return Data >= DataInicio && (!DataFim.HasValue || Data <= DataFim.Value);
            else
                return Data > DataInicio && (!DataFim.HasValue || Data < DataFim.Value);

        }

        public static bool Between(this decimal valor, decimal inicio, decimal fim)
        {
            return valor >= inicio && valor <= fim;
        }

        public static DateTime DataHoraMinuto(this DateTime Data)
        {
            return new DateTime(Data.Year, Data.Month, Data.Day, Data.Hour, Data.Minute, 0);
        }
        public static string DataFormatada(this DateTime Data)
        {
            if (Data == null)
                return string.Empty;
            return Data.ToString("dd/MM/yyyy");
        }

        public static bool IsEmpty(this IList Lista)
        {
            return (Lista?.Count ?? 0) == 0;
        }

        public static bool IsNotEmpty(this IList Lista)
        {
            return !Lista.IsEmpty();
        }

        public static List<T> TakeAndRemove<T>(this List<T> source, int takeValues)
        {
            var entries = source.Take(takeValues).ToList();
            foreach (var entry in entries)
                source.Remove(entry);
            return entries;
        }

        public static string ToStringWithMask(this decimal numero, string mascara = "{0:#,##0.00}") =>
             string.Format(mascara, numero);
        public static string ToStringWithMask(this double numero, string mascara = "{0:#,##0.00}") =>
             string.Format(mascara, numero);
        public static string ToStringWithMask(this float numero, string mascara = "{0:#,##0.00}") =>
             string.Format(mascara, numero);
        public static string ToStringWithMask(this int numero, string mascara = "{0:#00}") =>
             string.Format(mascara, numero);

        public static string ToStringCurrency(this decimal val, IFormatProvider format) =>
            string.Format(format, "{0:C}", val);

        public static string ToStringCurrency(this decimal? val, IFormatProvider format) =>
            string.Format(format, "{0:C}", val.HasValue ? val : 0);

        public static bool EhIdNovoItem(this long val) =>
            val == 0 || val == -1;

        public static string ToBase64(this byte[] array)
        {
            if (array is null)
                return string.Empty;

            return Convert.ToBase64String(array, 0, array.Length);
        }

        #region ToBoolean
        public static bool ToBoolean(this int? value)
        {
            if (value.HasValue)
                return Convert.ToBoolean(value);
            return false;
        }

        public static bool ToBoolean(this decimal? value)
        {
            if (value.HasValue)
                return Convert.ToBoolean(value);
            return false;
        }
        public static bool ToBoolean(this object value)
        {
            if (value != null)
                return Convert.ToBoolean(value);
            return false;
        }

        #endregion ToBoolean


        public static int? ToInt(this string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
            {
                return Convert.ToInt32(s);
            }
            return null;
        }

        public static int ToInt(this object s)
        {
            if (s != null)
                return Convert.ToInt32(s);

            return 0;
        }

        public static long ToLong(this object s)
        {
            if (s != null)
                return Convert.ToInt64(s);

            return -1;
        }

        public static float ToFloat(this object s)
        {
            if (s is null)
                return 0f;

            return (float)Convert.ToDouble(s);
        }

        public static long ToInt64(this string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                return Convert.ToInt64(s);

            return -1;
        }

        public static decimal ToDecimal(this string valor)
        {
            if (!string.IsNullOrEmpty(valor) && valor.Trim().Length > 0)
                return Convert.ToDecimal(valor);

            return 0;
        }

        public static decimal ToDecimal(this decimal input, int casasDecimais = 2)
        {
            decimal fatorDeMultiplicacao = (decimal)Math.Pow(10, casasDecimais);
            decimal valorArredondado = Math.Round(input * fatorDeMultiplicacao) / fatorDeMultiplicacao;

            return valorArredondado;
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> funcBody, int maxDoP = 4)
        {
            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    { await funcBody(partition.Current); }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(maxDoP)
                    .AsParallel()
                    .Select(p => AwaitPartition(p)));
        }

        public static bool Remove<T>(this List<T> source, Func<T, bool> funcBody)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                if (funcBody(source[i]))
                {
                    source.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static List<List<T>> Chunck<T>(this List<T> locations, int nSize)
        {
            var list = new List<List<T>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }


        public static object ChangeType(this object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }
            else
            {
                if (t.IsEnum)
                {
                    if (t.IsGenericTypeDefinition && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        return Enum.Parse(t, value?.ToString());
                    else
                        return Enum.Parse(t, (value?.ToString() ?? "0"));
                }
            }


            if (value == null)
                return value;

            if (t == typeof(Boolean))
                value = value.ToString().Equals("1") || value.ToString().Equals("True");

            if (t.IsEnum)
            {
                return Enum.Parse(t, (value?.ToString() ?? "0"));
            }

            return Convert.ChangeType(value, t);
        }


        public static IEnumerable Include<T>(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            return null;
        }

        /*
        public static void Include<TEntity, TProperty>(
            this IQueryable<TEntity> source,
            System.Linq.Expressions.Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {

            return new Microsoft.EntityFrameworkCore.Query.IIncludableQueryable IncludableQueryable<TEntity, TProperty>(
               source.Provider is EntityQueryProvider
                   ? source.Provider.CreateQuery<TEntity>(
                       Expression.Call(
                           null,
                           IncludeMethodInfo.MakeGenericMethod(typeof(TEntity), typeof(TProperty)),
                           new[] { source.Expression, Expression.Quote(navigationPropertyPath) }))
                   : source);

            return;
        }*/

        public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }

        public static DateTime? ParseTime(this string value, string format = "dd:MM:yyyy")
        {
            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            return null;
        }

        public static IList<T> ForAll<T>(this IList<T> source, Action<T> action) where T : class
        {
            foreach (var item in source)
                action(item);

            return source.ToList();
        }

        public static string FormataValorMoeda(this string valor, string DisplayFormat, string SimboloMonetario = "")
        {
            try
            {
                decimal result = decimal.Parse(valor.Replace(".", ","));
                valor = result.ToString(DisplayFormat);

                if (!string.IsNullOrEmpty(SimboloMonetario))
                    valor = string.Format("{0} {1}", SimboloMonetario, valor);

                return valor;
            }
            catch
            {
                return valor;
            }
        }

        public static decimal ParseCurrency(this object obj, NumberFormatInfo numberFormatter, out decimal value)
        {
            string valor = obj as string;

            if (string.IsNullOrEmpty(valor))
                valor = "0";

            if (numberFormatter.CurrencyGroupSeparator.Equals(","))
                valor = valor.Replace(",", "");
            else
                valor = valor.Replace(".", "");

            if (!decimal.TryParse(valor, NumberStyles.Currency, numberFormatter, out value))
                throw new Exception("Não foi possível converter o texto em número!");
            else
                return value;
        }
    }

    public static class DCTrdzExtensions
    {
        public static List<EntidadeGenerica> ILikeByValor(this List<EntidadeGenerica> list, string q)
        {
            if (q != string.Empty && q != null)
                list = list.Where(x => x.Valor.ToString().RemoveSpecialCharacters().ToLower().Contains(q.RemoveSpecialCharacters().ToLower())).ToList();
            return list;
        }

        //FORMATACOES
        public static string FormatCNPJ(this string s)
        {
            if (long.TryParse(s, out var i))
                return i.ToString(@"00\.000\.000\/0000\-00");
            else
                return s;
        }

        public static string FormataNumeroTelefone(this string s)
        {
            if (!s.Contains('(') || !s.Contains('-'))
                if (long.TryParse(s, out var i))
                    return i.ToString(@"\(00\) 00000\-0000");

            return s;
        }
        public static string FormatCPF(this string s)
        {
            if (long.TryParse(s, out var i))
                return i.ToString(@"000\.000\.000\-00");
            else
                return s;
        }

        public static string ColocarND(this string value)
        {
            return (value == null || value == string.Empty) ? "N/D" : value;
        }
        public static string ColocarVazio(this string value)
        {
            return (value == null || value == string.Empty) ? "-" : value;
        }
        public static string PrimeiraLetraMaiuscula(this string value)
        {
            return value == null || value == string.Empty ? "" : Regex.Replace(value.ToLower(), @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }
        public static string ApenasPrimeiraLetraMaiuscula(this string value)
          => value == null || value == string.Empty ? "" : (value.Length > 1 ? value[0].ToString().ToUpperInvariant() + value.Substring(1).ToLowerInvariant() : value.ToUpperInvariant());


        //public static Image ByteArrayToImage(this byte[] value)
        //{
        //    using (MemoryStream memstr = new MemoryStream(value))
        //    {
        //        Image img = Image.FromStream(memstr);
        //        return img;
        //    }
        //}
        //public static byte[] ImageToByteArray(this Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, ImageFormat.Png);
        //        return ms.ToArray();
        //    }
        //}

        public static string ParseASCII_TO_UTF8(this string descricao)
        {

            string utf8String = descricao;
            string propEncodeString = string.Empty;

            byte[] utf8_Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                utf8_Bytes[i] = (byte)utf8String[i];
            }
            string saida = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
            return saida.Contains("�") ? descricao : saida;

        }

        public static string InserePontosParaRelatorio(this string texto, int tamanho, bool Upper = true)
        {

            if (texto.Length > tamanho)
                texto = texto + ":";
            else
            {
                while (texto.Length < tamanho) texto += ".";
                texto += ":";
            }

            if (Upper)
            {
                texto = texto.ToUpper();
            }

            return texto;


        }

        private static string InsereReticenciasParaRelatorio(string texto, int tamanho, bool Upper = true)
        {

            if (texto == null)
                return "";

            if (texto.Length > tamanho)
                texto = texto.Substring(0, tamanho) + "...";

            if (Upper)
            {
                texto = texto.ToUpper();
            }
            return texto;

        }

        public static List<TResult> Converta<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            source.Select(selector).ToList();


        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, bool> metodoEquals,
            Func<TSource, int> metodoGetHashCode)
            => source.Distinct(
            ComparadorGenerico<TSource>.Criar(
            metodoEquals,
            metodoGetHashCode)
        );

        public static string RemoveSpecialCharacters(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        public static IEnumerable<T[]> Batch<T>(this T[] source, int size)
        {
            int batchCount = (source.Length + size - 1) / size;

            for (int i = 0; i < batchCount; i++)
            {
                int start = i * size;
                int length = Math.Min(size, source.Length - start);
                T[] batch = new T[length];
                Array.Copy(source, start, batch, 0, length);
                yield return batch;
            }
        }

    }

    public class ComparadorGenerico<T> : IEqualityComparer<T>
    {
        public Func<T, T, bool> MetodoEquals { get; }
        public Func<T, int> MetodoGetHashCode { get; }
        private ComparadorGenerico(
            Func<T, T, bool> metodoEquals,
            Func<T, int> metodoGetHashCode)
        {
            this.MetodoEquals = metodoEquals;
            this.MetodoGetHashCode = metodoGetHashCode;
        }

        public static ComparadorGenerico<T> Criar(
            Func<T, T, bool> metodoEquals,
            Func<T, int> metodoGetHashCode)
                => new ComparadorGenerico<T>(
                        metodoEquals,
                        metodoGetHashCode
                    );

        public bool Equals(T x, T y)
            => MetodoEquals(x, y);

        public int GetHashCode(T obj)
            => MetodoGetHashCode(obj);



    }
}
