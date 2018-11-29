using System.Collections.Generic;

namespace Common
{
   public  static class GenericExtension
    {

       public static void InsertFirst<TKey, TValue>(this IDictionary<TKey, TValue> original, TKey key, TValue value)
       {
           IDictionary<TKey,TValue> dictionary = new Dictionary<TKey, TValue>();
           if (!original.ContainsKey(key))
           {
               dictionary.Add(key, value);
           }
           foreach (KeyValuePair<TKey, TValue> kvp in original)
           {
               dictionary.Add(kvp);
           }
           original.Clear();
           foreach (KeyValuePair<TKey, TValue> valuePair in dictionary)
           {
               original.Add(valuePair);
           }
       }

       public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
       {
           TValue result;
           dictionary.TryGetValue(key, out result);
           return result;
       }
    }
}
