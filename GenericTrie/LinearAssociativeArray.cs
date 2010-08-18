using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericTrie
{
    /// <summary>
    /// An associative array implementing the IDictionary interface, 
    /// intended to have as small of a memory footprint as possible.
    /// Uses a linear search for all operations.
    /// </summary>
    /// <typeparam name="TKey">The type of the key (must implement IComparable(TKey)</typeparam>
    /// <typeparam name="TValue">The type of the associated value</typeparam>
    public class LinearAssociativeArray<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        #region container
        private KeyValuePair<TKey, TValue>[] Items;
        #endregion
        #region Interface Implementation
        public int Count { get { return Items.Length; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(KeyValuePair<TKey, TValue> item) 
        {
            if (ContainsKey(item.Key))
            {
                throw new ArgumentException("Key " + item.Key + " already present");
            }
            if (Items == null)
            {
                Items = new KeyValuePair<TKey, TValue>[] { item };
            }
            else
            {
                KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[Items.Length + 1];
                Items.CopyTo(newItems, 0);
                newItems[newItems.Length - 1] = item;
                Items = newItems;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) 
        {
            foreach (var kvp in Items)
            {
                if (kvp.Key.CompareTo(item.Key) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) 
        {
            if (Items != null)
            {
                if (arrayIndex >= Items.Length)
                {
                    throw new ArgumentException("Array index must be less than or equal to length");
                }

                for (int i = arrayIndex; i < Items.Length; i++)
                {
                    array[i-arrayIndex] = Items[i];
                }
            }
            return;
        }
        public void Clear()
        {
            Items = null; 
        }
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Items == null || ContainsKey(item.Key))
            {
                KeyValuePair<TKey, TValue>[] newArray = new KeyValuePair<TKey, TValue>[Items.Length - 1];
                int NewIndex = 0;
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i].Key.CompareTo(item.Key) == 0)
                    {
                        continue;
                    }
                    else
                    {
                        newArray[NewIndex] = Items[i];
                        NewIndex++;
                    }
                }
                Items = newArray;
                return true;
            }
            else
            {
                return false;
            }
        }
        public ICollection<TKey> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<TValue> Values 
        {
            get
            {
                throw new NotImplementedException();
            }

        }

        public TValue this[TKey key]
        {
            get
            {
                if (Items != null)
                {


                    foreach (var item in Items)
                    {
                        if (item.Key.CompareTo(key) == 0)
                        {
                            return item.Value;
                        }
                    }
                }
                throw new KeyNotFoundException("Key \"" + key.ToString() + "\" not found");
            }
            set
            {
                if (Items != null)
                {
                    for (int i = 0; i < Items.Length; i++)
                    {
                        if (Items[i].Key.CompareTo(key) == 0)
                        {
                            Items[i] = new KeyValuePair<TKey, TValue>(key, value);
                        }
                    }
                }
                throw new KeyNotFoundException("Key \"" + key.ToString() + "\" not found");
            }
        }

        public void Add(TKey key, TValue value)
        {

            Add(new KeyValuePair<TKey, TValue>(key, value));
            
        }

        public bool ContainsKey(TKey key)
        {
            if (Items == null)
            {
                return false;
            }
            foreach (var item in Items)
            {
                if (item.Key.CompareTo(key) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                Remove(new KeyValuePair<TKey,TValue>(key,default(TValue)));
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = this[key];
                return true;
            }

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    yield return item;
                }
            }
            yield break;
        }
        #endregion
    }
}
