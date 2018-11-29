namespace Common
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataList
    {
        private readonly Dictionary<string, object> _datas;

        public DataList()
        {
            this._datas = new Dictionary<string, object>();
        }
        public object[] Items
        {
            get
            {
                return this._datas.Values.ToArray();
            }
        }

        public int Count
        {
            get
            {
                return this._datas.Count;
            }
        }

        public object this[string key]
        {
            get
            {
                if(this._datas.ContainsKey(key))
                {
                    return this._datas[key];
                }
                return null;
            }
        }

        public T GetValue<T>(string key)
        {
            if (this._datas.ContainsKey(key))
            {
                try
                {
                    return (T)this._datas[key];
                }
                catch 
                {
                    return default(T);
                }
            }
            return default(T);
        }

        public void Add(string key, object value)
        {
            if(!this._datas.ContainsKey(key))
            {
                this._datas.Add(key, value);
            }
        }

        public bool ContainsKey(string key)
        {
            return this._datas.ContainsKey(key);
        }
    }
}
