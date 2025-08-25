using Aquasys.App.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Utils
{
    public class ContextUtils
    {
        public static User ContextUser { get; private set; } = new();

        public ContextUtils(User contextUser) { ContextUser = contextUser; }
    }
}
