﻿using UnityEngine;

namespace Game.Management {
    using JToolkit.BasisComponent;

    public class PlayerInput : Input, IDataPersister {

        public bool have_control {
            get; private set;
        }

        [HideInInspector]
        public DataSettings data_settings = new DataSettings();


        public InputAxis horizontal = new InputAxis ( KeyCode.D, KeyCode.A, Xbox_Controller_Axes.Left_Stick_Horizontal );
        public InputAxis vertical = new InputAxis ( KeyCode.W, KeyCode.S, Xbox_Controller_Axes.Left_Stick_Vertical );


        protected override void get_inpts ( bool _fixed_update_happened ) {
            horizontal.get ( input_type );
            vertical.get ( input_type );
        }


        public override void gain_control ( ) {
            have_control = true;

            gain_control ( horizontal );
            gain_control ( vertical );

        }


        public override void release_control ( bool _reset_values = true ) {
            have_control = false;

            release_control ( horizontal, _reset_values );
            release_control ( vertical, _reset_values );

        }


        public DataSettings get_data_settings ( ) {
            return data_settings;
        }


        public void set_data_settings ( string data_tag, DataSettings._Persistence_Type persistence_type ) {
            data_settings.data_tag = data_tag;
            data_settings.persistence_type = persistence_type;
        }


        public Data save_data ( ) {
            // TODO : 입력 저장값
            return new Data ( );
        }


        public void load_data ( Data data ) {
            // TODO : 입력 로드값
            Data d = data;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                                                 Unity                                                                ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            have_control = true;
        }


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }
            PersistentDataManager.register_persister ( this );
        }


        private void OnDisable() {
            PersistentDataManager.unregister_persister ( this );
            instance = null;
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }


        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}