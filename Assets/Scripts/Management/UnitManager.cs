using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    
    public class UnitManager : Singleton<UnitManager> {
        
        public UnitData[] unit_data;


        public enum DataType {
            Id = 0,
            Nickname,
            Hp,
            Mspeed,
            Aspeed,
            Rspeed,
            Damage,
            Armor,
        }
        public List<EnumDictionary<DataType, object>> data_table = null;
        


        public void load_data() {
            if(data_table == null) {
                data_table = new List<EnumDictionary<DataType, object>> ( );
            }
            data_table?.Clear ( );

            List<Dictionary<string, object>> table = CSVReader.read ( "DataTable/UnitDataTable" );
            foreach ( var data in table ) {
                data_table.Add ( new EnumDictionary<DataType, object> {
                    { DataType.Id,          Convert.ToUInt32 ( data["id"] ) },
                    { DataType.Nickname,    data["nickname"] },
                    { DataType.Hp,          Convert.ToSingle ( data["hp"] ) },
                    { DataType.Mspeed,      Convert.ToSingle ( data["mspeed"] ) },
                    { DataType.Aspeed,      Convert.ToSingle ( data["aspeed"] ) },
                    { DataType.Rspeed,      Convert.ToSingle ( data["rspeed"] ) },
                    { DataType.Damage,      Convert.ToSingle ( data["damage"] ) },
                    { DataType.Armor,       Convert.ToSingle ( data["armor"] ) },
                });
            }
        }


        /// <summary>
        /// ID를 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<DataType, object> get_data ( uint id ) {
            return data_table?.Find ( data => (uint)data[DataType.Id] == id );
        }

        /// <summary>
        /// Nickname을 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<DataType, object> get_data ( string nickname ) {
            return data_table?.Find ( data => (string)data[DataType.Nickname] == nickname );
        }



        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////


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


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
