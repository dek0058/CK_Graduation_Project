using UnityEngine;

namespace Game.Management {
    using JToolkit.BasisComponent;

    public class PlayerInput : Input, IDataPersister {

        public bool have_control {
            get; private set;
        }

        [HideInInspector]
        public DataSettings data_settings = new DataSettings();


        public InputAxis horizontal = new InputAxis ( KeyCode.RightArrow, KeyCode.LeftArrow, Xbox_Controller_Axes.Left_Stick_Horizontal );
        public InputAxis vertical = new InputAxis ( KeyCode.UpArrow, KeyCode.DownArrow, Xbox_Controller_Axes.Left_Stick_Vertical );

        public InputButton attack = new InputButton ( KeyCode.Space, Xbox_Controller_Buttons.Left_Bumper );
        public InputButton purgatory = new InputButton ( KeyCode.X, Xbox_Controller_Buttons.X );

        protected override void get_inpts ( bool _fixed_update_happened ) {
            horizontal.get ( input_type );
            vertical.get ( input_type );
            
            attack.get ( _fixed_update_happened, input_type );
            purgatory.get ( _fixed_update_happened, input_type );
        }


        public override void gain_control ( ) {
            have_control = true;

            gain_control ( horizontal );
            gain_control ( vertical );

            gain_control ( attack );
            gain_control ( purgatory );
        }


        public override void release_control ( bool _reset_values = true ) {
            have_control = false;

            release_control ( horizontal, _reset_values );
            release_control ( vertical, _reset_values );

            release_control ( attack, _reset_values );
            release_control ( purgatory, _reset_values );

        }


        public DataSettings get_data_settings ( ) {
            return data_settings;
        }


        public void set_data_settings ( string data_tag, DataSettings.PersistenceType persistence_type ) {
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
