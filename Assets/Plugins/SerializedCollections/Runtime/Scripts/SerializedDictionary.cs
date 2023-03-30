using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [System.Serializable]
    public partial class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        internal List<SerializedKeyValuePair<TKey, TValue>> _serializedList = new List<SerializedKeyValuePair<TKey, TValue>>();

#if UNITY_EDITOR
        internal IKeyable LookupTable
        {
            get
            {
                if (_lookupTable == null)
                    _lookupTable = new DictionaryLookupTable<TKey, TValue>(this);
                return _lookupTable;
            }
        }

        private DictionaryLookupTable<TKey, TValue> _lookupTable;
#endif

#if UNITY_EDITOR
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            _serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
        }

        public new void Clear()
        {
            base.Clear();
            _serializedList.Clear();
        }

        public new bool Remove(TKey key)
        {
            if (TryGetValue(key, out var value))
            {
                base.Remove(key);
                _serializedList.Remove(new SerializedKeyValuePair<TKey, TValue>(key, value));
                return true;
            }

            return false;
        }

        public new bool TryAdd(TKey key, TValue value)
        {
            if (base.TryAdd(key, value))
            {
                _serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Only available in Editor. Add a key value pair, even if the key already exists in the dictionary.
        /// </summary>
        public void AddConflictAllowed(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, value);
            _serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
        }
#endif

        public void OnAfterDeserialize()
        {
            base.Clear();

            foreach (var kvp in _serializedList)
            {
#if UNITY_EDITOR
                if (SerializedCollectionsUtility.IsValidKey(kvp.Key) && !ContainsKey(kvp.Key))
                    base.Add(kvp.Key, kvp.Value);
#else
                    Add(kvp.Key, kvp.Value);
#endif
            }

#if UNITY_EDITOR
            LookupTable.RecalculateOccurences();
#else
            _serializedList.Clear();
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                LookupTable.RemoveDuplicates();
#else
            foreach (var kvp in this)
                _serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
#endif
        }
    }
}
