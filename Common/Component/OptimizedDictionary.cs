namespace Common
{
    using System.Collections.Generic;

    public class OptimizedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (this.ContainsKey(key))
                {
                    base[key] = value;
                }

            }

        }
    }
}
