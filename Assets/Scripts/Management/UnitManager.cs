using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    
    public class UnitManager : Singleton<UnitManager> {
        
        public UnitData[] unit_data;


        public enum Data_Type {
            Id = 0,
            Nickname,
            Hp,
            Mspeed,
            Aspeed,
            Rspeed,
            Damage,
            Armor,
        }
        public List<EnumDictionary<Data_Type, object>> data_table = null;
        


        public void load_data() {
            if(data_table == null) {
                data_table = new List<EnumDictionary<Data_Type, object>> ( );
            }
            data_table?.Clear ( );

            List<Dictionary<string, object>> table = CSVReader.read ( "DataTable/UnitDataTable" );
            foreach ( var data in table ) {
                data_table.Add ( new EnumDictionary<Data_Type, object> {
                    { Data_Type.Id,          Convert.ToUInt32 ( data["id"] ) },
                    { Data_Type.Nickname,    data["nickname"] },
                    { Data_Type.Hp,          Convert.ToSingle ( data["hp"] ) },
                    { Data_Type.Mspeed,      Convert.ToSingle ( data["mspeed"] ) },
                    { Data_Type.Aspeed,      Convert.ToSingle ( data["aspeed"] ) },
                    { Data_Type.Rspeed,      Convert.ToSingle ( data["rspeed"] ) },
                    { Data_Type.Damage,      Convert.ToSingle ( data["damage"] ) },
                    { Data_Type.Armor,       Convert.ToSingle ( data["armor"] ) },
                });
            }
        }


        /// <summary>
        /// ID를 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<Data_Type, object> get_data ( uint id ) {
            return data_table?.Find ( data => (uint)data[Data_Type.Id] == id );
        }

        /// <summary>
        /// Nickname을 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<Data_Type, object> get_data ( string nickname ) {
            return data_table?.Find ( data => (string)data[Data_Type.Nickname] == nickname );
        }



        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
                DontDestroyOnLoad ( gameObject );
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
