using System.Reflection;

namespace Aquasys.WebApi.Services
{
    public class SyncTypeRegistry
    {
        private readonly Dictionary<string, Type> _typeMap = new();

        public SyncTypeRegistry()
        {
            var syncableTypes = Assembly.GetAssembly(typeof(SyncableEntity))
                                        .GetTypes()
                                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(SyncableEntity)));

            foreach (var type in syncableTypes)
            {
                _typeMap[type.Name] = type;
            }
        }

        public Type GetType(string name) => _typeMap.GetValueOrDefault(name);

        public IEnumerable<KeyValuePair<string, Type>> GetAllTypes()
        {
            return _typeMap;
        }
    }
}
