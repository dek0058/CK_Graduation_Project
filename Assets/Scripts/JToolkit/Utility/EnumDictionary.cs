using System.Collections.Generic;

namespace JToolkit.Utility {
    public class EnumDictionary<TKey, TValue> : IEnumerable<KeyValuePair<int, TValue>> where TKey : unmanaged {
        private Dictionary<int, TValue> internalDictionary = new Dictionary<int, TValue> ( );

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator ( ) => internalDictionary.GetEnumerator ( );

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ( ) => internalDictionary.GetEnumerator ( );

        public TValue this[TKey key] {
            get => internalDictionary[ConvertToIndex ( key )];
            set => Add ( key, value );
        }

        public void Add ( TKey key, TValue values ) {
            if ( !internalDictionary.TryGetValue ( ConvertToIndex ( key ), out TValue storedValues ) ) {
                internalDictionary.Add ( ConvertToIndex ( key ), values );
            }
            storedValues = values;
        }

        public static unsafe int ConvertToIndex ( TKey key ) {
            return *(int*)&key;
        }
    }
}