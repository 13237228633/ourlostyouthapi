namespace Common
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///  线程安全Dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Property

        private readonly object _syncLock;

        private readonly Dictionary<TKey, TValue> _dictionaryBase;

        #endregion

        #region Constructor
        /// <summary>
        ///  构造方法
        /// </summary>
        public SynchronizedDictionary(int capacity)
        {
            this._syncLock = new object();
            this._dictionaryBase = new Dictionary<TKey, TValue>(capacity);
        }

        public SynchronizedDictionary()
        {
            this._syncLock = new object();
            this._dictionaryBase = new Dictionary<TKey, TValue>();
        }
        #endregion

        #region Values
        /// <summary>
        ///  
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                lock (this._syncLock)
                {
                    return this._dictionaryBase.Values;
                }
            }
        }
        #endregion

        #region Keys
        /// <summary>
        ///  
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (this._syncLock)
                {
                    return this._dictionaryBase.Keys;
                }
            }
        }
        #endregion

        #region Count
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                lock (this._syncLock)
                {
                    return this._dictionaryBase.Count;
                }
            }
        }
        #endregion

        #region IsReadOnly
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Indexer
        /// <summary>
        ///  索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                this.TryGetValue(key, out value);
                return value;
            }
            set
            {
                lock (this._syncLock)
                {
                    this._dictionaryBase[key] = value;
                }
            }
        }
        #endregion

        #region TryGetValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this._syncLock)
            {
                return this._dictionaryBase.TryGetValue(key, out value);
            }
        }
        #endregion

        #region Add
        /// <summary>
        ///  
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (this._syncLock)
            {
                this._dictionaryBase.Add(item.Key, item.Value);
            }
        }
        #endregion

        #region Remove
        /// <summary>
        ///  
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            lock (this._syncLock)
            {
                return this._dictionaryBase.Remove(key);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (this._syncLock)
            {
                if (this._dictionaryBase.ContainsKey(item.Key) && this._dictionaryBase[item.Key].Equals(item.Value))
                {
                    return this._dictionaryBase.Remove(item.Key);
                }
            }
            return false;
        }
        #endregion

        #region Contains
        /// <summary>
        ///  
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (this._syncLock)
            {
                if (this._dictionaryBase.ContainsKey(item.Key))
                {
                    return this._dictionaryBase[item.Key].Equals(item.Value);
                }
            }
            return false;
        }
        #endregion

        #region CopyTo
        /// <summary>
        ///  
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (this._syncLock)
            {
                int count = 0;
                foreach (KeyValuePair<TKey, TValue> pair in this._dictionaryBase)
                {
                    if (count < this._dictionaryBase.Count && count < array.Length && count >= arrayIndex)
                    {
                        array[count] = new KeyValuePair<TKey, TValue>(pair.Key, pair.Value);
                    }
                    count++;
                }
            }
        }
        #endregion

        #region Clear
        /// <summary>
        ///  
        /// </summary>
        public void Clear()
        {
            lock (this._syncLock)
            {
                this._dictionaryBase.Clear();
            }
        }
        #endregion

        #region ContainsKey
        /// <summary>
        ///  
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            lock (this._syncLock)
            {
                return _dictionaryBase.ContainsKey(key);
            }
        }
        #endregion

        #region GetEnumerator
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (this._syncLock)
            {
                return this._dictionaryBase.GetEnumerator();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    } 
}
