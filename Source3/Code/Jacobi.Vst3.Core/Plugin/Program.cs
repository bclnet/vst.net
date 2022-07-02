using System.Collections.Generic;

namespace Jacobi.Vst3.Plugin
{
    public class Program : IEnumerable<KeyValuePair<string, string>>
    {
        public Program(string name) => Name = name;

        public string Name { get; private set; }

        private readonly Dictionary<string, string> _attrValues = new();

        public IDictionary<string, string> AttributeValues => _attrValues;

        public string this[string attrId]
        {
            get => _attrValues.ContainsKey(attrId) ? _attrValues[attrId] : null;
            set => _attrValues[attrId] = value;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _attrValues.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
