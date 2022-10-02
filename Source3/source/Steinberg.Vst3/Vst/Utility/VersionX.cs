using System;

namespace Steinberg.Vst3.Utility
{
    public static class VersionX
    {
        public static Version Parse(string str)
        {
            var part = 0;
            var parts = new int[4];
            // skip non digits in the front
            var it = 0; while (it < str.Length && !char.IsDigit(str[it])) ++it;
            if (it == str.Length) return new Version(0, 0, 0, 0);
            str = str.Remove(0, it);
            int index;
            while (str.Length != 0)
            {
                index = str.IndexOf('.');
                if (index == -1)
                {
                    // skip non digits in the back
                    var itBack = 0; while (itBack < str.Length && char.IsDigit(str[itBack])) ++itBack;
                    index = itBack;
                    if (index != str.Length) str = str.Remove(index);
                    index = str.Length;
                }
                var numberStr = str[..index];
                if (int.TryParse(numberStr, out var z)) parts[part] = z;
                if (++part > parts.Length) break;
                if (str.Length - index == 0) break;
                ++index;
                str = str[index..];
            }
            return part switch
            {
                0 => new Version(0, 0, 0, 0),
                1 => new Version(parts[0], 0, 0, 0),
                2 => new Version(parts[0], parts[1], 0, 0),
                3 => new Version(parts[0], parts[1], parts[2], 0),
                _ => new Version(parts[0], parts[1], parts[2], parts[3]),
            };
        }
    }
}
