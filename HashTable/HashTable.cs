using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    public class HashTable : IDictionary<int, Object>
    {
        List<HashTableElement> hashTable;

        public HashTable()
        {
            hashTable = new List<HashTableElement>();
        }

        public void Add(object value)
        {
            if (FindElement(o => o.HashCode == value.GetHashCode()) != null)
                throw new InvalidOperationException();
            HashTableElement element = new HashTableElement();
            element.Data = value;
            element.HashCode = value.GetHashCode();
            hashTable.Add(element);
        }

        public Object Find(Func<Object, bool> searchFunction)
        {
            return FindElement(o => searchFunction(o.Data));
        }

        void ICollection<KeyValuePair<int, object>>.Add(KeyValuePair<int, object> keyvalue)
        {
            if (FindElement(o => o.HashCode == keyvalue.Key) != null)
                throw new InvalidOperationException();
            HashTableElement element = new HashTableElement();
            element.Data = keyvalue.Value;
            element.HashCode = keyvalue.Key;
            hashTable.Add(element);
        }

        public void Clear()
        {
            hashTable.Clear();
        }

        public bool Contains(KeyValuePair<int, object> keyvalue)
        {
            return FindElement(o => o.HashCode == keyvalue.Key && o.Data == keyvalue.Value) != null;
        }

        public void CopyTo(KeyValuePair<int, object>[] keysvalues, int index)
        {
            throw new InvalidOperationException();
        }

        public int Count
        {
            get
            {
                return hashTable.Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool Remove(KeyValuePair<int, object> elementToRemove)
        {
            HashTableElement element = FindElement(o => o.Data == elementToRemove.Value && o.HashCode == elementToRemove.Key);
            if (element == null)
                return false;
            return hashTable.Remove(element);
        }

        void IDictionary<int, object>.Add(int hash, object value)
        {
            ICollection<KeyValuePair<int, object>> a = this as ICollection<KeyValuePair<int, object>>;
            a.Add(new KeyValuePair<int, object>(hash, value));
        }

        public bool ContainsKey(int hash)
        {
            return FindElement(o => o.HashCode == hash) != null;
        }

        public ICollection<int> Keys
        {
            get
            {
                List<int> keys = new List<int>();
                foreach (HashTableElement element in hashTable)
                {
                    keys.Add(element.HashCode);
                }
                return keys;
            }
        }

        public bool Remove(int key)
        {
            HashTableElement element = FindElement(o => o.HashCode == key);
            if (element == null)
                return false;
            return hashTable.Remove(element);

        }

        public Object this[int index]
        {
            get
            {
                return Find(o => o.GetHashCode() == index);
            }
            set
            {
                HashTableElement element = FindElement(o => o.GetHashCode() == index);
                if (element == null)
                    throw new ArgumentOutOfRangeException();

                element.Data = value;
                element.HashCode = value.GetHashCode();
            }
        }

        public bool TryGetValue(int key, out object value)
        {
            value = Find(o => o.GetHashCode() == key);
            return value != null;
        }

        public ICollection<Object> Values
        {
            get
            {
                List<object> values = new List<object>();
                foreach (HashTableElement element in hashTable)
                {
                    values.Add(element.Data);
                }
                return values;
            }
        }

        #region einumerable
        public IEnumerator<KeyValuePair<int, object>> GetEnumerator()
        {
            return new CustomEnumerator(hashTable);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
        #region ienumerator

        private class CustomEnumerator : IEnumerator<KeyValuePair<int, object>>
        {
            private int currentIndex;
            private readonly List<HashTableElement> hashTable;
            public CustomEnumerator(List<HashTableElement> hashTable)
            {
                this.hashTable = hashTable;
                currentIndex = -1;
            }
            public bool MoveNext()
            {
                return ++currentIndex < hashTable.Count();
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose()
            {

            }

            public KeyValuePair<int, object> Current
            {
                get
                {
                    if (currentIndex < 0 || currentIndex >= hashTable.Count())
                    {
                        throw new InvalidOperationException();
                    }
                    return new KeyValuePair<int, object>(hashTable[currentIndex].HashCode, hashTable[currentIndex].Data);
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

        }

        #endregion
        private HashTableElement FindElement(Func<HashTableElement, bool> searchFunction)
        {
            foreach (HashTableElement element in hashTable)
            {
                if (searchFunction(element))
                    return element;
            }
            return null;
        }
    }
}
