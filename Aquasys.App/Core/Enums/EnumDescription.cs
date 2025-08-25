using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Aquasys.App.Core.Entities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescription : Attribute
    {
        public string Description { get; }
        public string Tag { get; }

        public EnumDescription(string descricao)
        {
            Description = descricao;
            Tag = descricao;
        }

        public class EnumValues
        {

            public long ID { get; set; }
            public string Value { get; set; }
            public string Tag { get; set; }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}
