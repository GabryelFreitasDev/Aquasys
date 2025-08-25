
using Aquasys.App.Core.Entities;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace CRM.Entidades
{
    public class DCUtils
    {
        public static async Task<byte[]> ToByteArrayAsync(Stream stream)
        {
            var array = new byte[stream.Length];
            await stream.ReadAsync(array, 0, (int)stream.Length);
            return array;
        }
    }
}
