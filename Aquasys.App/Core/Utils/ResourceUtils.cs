using System.Reflection;

namespace Aquasys.App.Core.Utils
{
    public class ResourceUtils
    {
        public static object GetResourceValue(string key)
        {
            Application.Current!.Resources.TryGetValue(key, out var result);
            return result;
        }

        public static void SetResourceValue(string key, object value)
        {
            Application.Current!.Resources[key] = value;
        }

        public static string GetHexValue(string key)
        {
            Application.Current!.Resources.TryGetValue(key, out var result);
            var color = (Color)result;

            return color.ToHex();
        }

        public static string? GetNameOfBinding(BindableObject self, BindableProperty property)
        {
            var methodInfo = typeof(BindableObject).GetTypeInfo().GetDeclaredMethod("GetContext");
            var context = methodInfo?.Invoke(self, new[] { property });

            FieldInfo propertyInfo = context?.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            dynamic pi = propertyInfo?.GetValue(context);
            string? path = string.Empty;


            foreach (var kvp in pi)
            {
                var valueBinding = ExtensionUtils.GetPropertyValue(kvp, "Value");
                path = (valueBinding as Binding)?.Path;
                break;
            }

            if (path.IsNullOrEmpty())
                return null;

            if (path.Split('.').Length == 2)
                path = path.Split('.')[1];

            return path;
        }

        public static void AddThemeOrDefaultStyle()
        {
            Assembly assembly = Application.Current!.GetType().GetTypeInfo().Assembly;

            string themeStyles = $"Resources/Styles/Themes/{AppInfo.PackageName}/Styles.xaml";
            if (Exists(assembly, themeStyles))
            {
                Uri source = new Uri(themeStyles, UriKind.RelativeOrAbsolute);
                ResourceDictionary styles = new ResourceDictionary();
                styles.SetAndLoadSource(source, themeStyles, assembly, null);

                ResourceDictionary? defaultStyles = Application.Current!.Resources!.MergedDictionaries.FirstOrDefault(d => d.Source.OriginalString.Equals("Resources/Styles/Styles.xaml;assembly=CRMMobileMaui", StringComparison.Ordinal));
                if (defaultStyles != null)
                    foreach (string key in styles.Keys)
                    {
                        Style ds = defaultStyles[key] as Style;
                        Style s = styles[key] as Style;

                        foreach (Setter setter in s.Setters)
                            ds.Setters.Add(setter);
                    }
            }

            string themeColors = $"Resources/Styles/Themes/{AppInfo.PackageName}/Colors.xaml";
            if (Exists(assembly, themeColors))
            {
                Uri source = new Uri(themeColors, UriKind.RelativeOrAbsolute);
                ResourceDictionary colors = new ResourceDictionary();
                colors.SetAndLoadSource(source, themeColors, assembly, null);

                ResourceDictionary? defaultColors = Application.Current!.Resources!.MergedDictionaries.FirstOrDefault(d => d.Source.OriginalString.Equals("Resources/Styles/Colors.xaml;assembly=CRMMobileMaui", StringComparison.Ordinal));
                if (defaultColors != null)
                    foreach (string? key in colors.Keys)
                        defaultColors[key] = colors[key];
            }
        }

        private static bool AddMergedResourceDictionary(string pathResourceDictionary)
        {
            try
            {
                Assembly assembly = Application.Current!.GetType().GetTypeInfo().Assembly;
                Type? type = GetTypeForPath(assembly, pathResourceDictionary);
                if (type == null)
                    return false;

                Uri source = new Uri($"{pathResourceDictionary}", UriKind.RelativeOrAbsolute);
                ResourceDictionary resourceDictionary = new ResourceDictionary();
                resourceDictionary.SetAndLoadSource(source, pathResourceDictionary, assembly, null);

                Application.Current!.Resources.MergedDictionaries.Add(resourceDictionary);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return false;
                return true;
            }
        }

        internal static Type? GetTypeForPath(Assembly assembly, string path)
        {
            foreach (var xria in assembly.GetCustomAttributes<XamlResourceIdAttribute>())
                if (xria.Path == path)
                    return xria.Type;
            return null;
        }

        public static async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource imageSource)
        {
            Stream stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
        }

        public static bool Exists(Assembly assembly, string resourceName)
        {
            //return assembly.GetManifestResourceNames().Any(res => res.Equals(resourceName));
            return GetTypeForPath(assembly, resourceName) != null;
        }



    }
}
