using Aquasys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Utils
{
    public static class ObjectExtensions
    {
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
}
