using UnityEngine;

namespace JToolkit.Utility {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        private static volatile T _instance = null;
        private static Object _sync_root = new Object ( );
        protected static bool _app_is_close = false;

        public static T instance {
            get {
                lock ( _sync_root ) {
                    if ( _instance == null ) {
                        T[] instances = GameObject.FindObjectsOfType<T> ( );
                        if ( instances.Length > 0 ) {
                            _instance = instances[0];
                        } else if ( instances.Length > 1 ) {
                            Debug.Log ( "Scene에 " + typeof ( T ).ToString ( ) + "가 추가적으로 있습니다." );
                            foreach ( var obj in instances ) {
                                Debug.Log ( typeof ( T ).ToString ( ) + ":" + obj.gameObject.name );
                            }
                            Debug.Log ( "-------------------------------------------------" );
                        }
                        if ( _instance == null ) {
                            GameObject new_instance = new GameObject ( typeof ( T ).ToString ( ) + "(Singleton)" );
                            _instance = new_instance.AddComponent<T> ( );
                        }
                    }
                    return _instance;
                }
            }
            set {
                if ( _instance != null ) {
                    string message = string.Format ( "Scene에 이미 {0}가 있습니다.", typeof ( T ).ToString ( ) );
                    Debug.Log ( message );
                    return;
                } else {
                    _instance = value;
                }
            }
        }
    }
}