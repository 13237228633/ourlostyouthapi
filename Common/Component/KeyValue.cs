namespace Common
{
    public struct KeyValue<TKey, TValue>
    {
        public TKey Key;

        public TValue Value;

        public KeyValue(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public struct  KeyValue
    {
        public string Key { get; set; }

        public object Value { get; set; }

    }
}
