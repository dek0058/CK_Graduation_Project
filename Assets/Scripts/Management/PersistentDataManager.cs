using UnityEngine;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;

    public class PersistentDataManager : Singleton<PersistentDataManager> {

        protected HashSet<IDataPersister> data_persisters = new HashSet<IDataPersister> ( );
        protected Dictionary<string, Data> store = new Dictionary<string, Data> ( );
        private event System.Action schedule = null;



        public static void register_persister ( IDataPersister persister ) {
            var ds = persister.get_data_settings ( );
            if ( !string.IsNullOrEmpty ( ds.data_tag ) ) {
                instance.register ( persister );
            }
        }


        public static void unregister_persister ( IDataPersister persister ) {
            if ( !_app_is_close ) {
                instance.unregister ( persister );
            }
        }


        public static void save_all_data ( ) {
            instance.save_all_data_internal ( );
        }


        public static void load_all_data ( ) {
            instance.load_all_data_internal ( );
        }


        public static void clear_persisters ( ) {
            instance.data_persisters.Clear ( );
        }


        public static void set_dirty ( IDataPersister dp ) {
            instance.save ( dp );
        }


        protected void save_all_data_internal ( ) {
            foreach ( var dp in data_persisters ) {
                save ( dp );
            }
        }


        protected void register ( IDataPersister persister ) {
            schedule += ( ) => {
                data_persisters.Add ( persister );
            };
        }


        protected void unregister ( IDataPersister persister ) {
            schedule += ( ) => data_persisters.Remove ( persister );
        }


        protected void save ( IDataPersister dp ) {
            var dataSettings = dp.get_data_settings ( );
            if ( dataSettings.persistence_type == DataSettings.PersistenceType.Read_Only || dataSettings.persistence_type == DataSettings.PersistenceType.Do_Not_Persist )
                return;
            if ( !string.IsNullOrEmpty ( dataSettings.data_tag ) ) {
                store[dataSettings.data_tag] = dp.save_data ( );
            }
        }


        protected void load_all_data_internal ( ) {
            schedule += ( ) => {
                foreach ( var dp in data_persisters ) {
                    var dataSettings = dp.get_data_settings ( );
                    if ( dataSettings.persistence_type == DataSettings.PersistenceType.Write_Only || dataSettings.persistence_type == DataSettings.PersistenceType.Do_Not_Persist )
                        continue;
                    if ( !string.IsNullOrEmpty ( dataSettings.data_tag ) ) {
                        if ( store.ContainsKey ( dataSettings.data_tag ) ) {
                            dp.load_data ( store[dataSettings.data_tag] );
                        }
                    }
                }
            };
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }

            if ( instance == this ) {
                DontDestroyOnLoad ( gameObject );
            }
        }


        private void Update ( ) {
            schedule?.Invoke ( );
            if ( schedule != null ) { schedule = null; }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}