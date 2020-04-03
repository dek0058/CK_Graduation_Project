using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Management {
    using JToolkit.Utility;
    using Game.Unit;
    
    public class UnitManager : Singleton<UnitManager> {
        
        public UnitData[] unit_data;

        private void Start()
        {
            load_data();
        }

        public enum DataType {
            Id = 0,
            Nickname,
            Hp,
            Mspeed,
            Aspeed,
            Rspeed,
            Damage,
            Armor,
            ATime,
        }
        public List<EnumDictionary<DataType, object>> data_table = null;
        
        public void load_data() {
            if(data_table == null) {
                data_table = new List<EnumDictionary<DataType, object>> ( );
            }
            data_table?.Clear ( );

            List<Dictionary<string, object>> table = CSVReader.read ( "DataTable/UnitDataTable" );
            for (int i = 0; i < table.Count; i++ ) {
                data_table.Add ( new EnumDictionary<DataType, object> {
                    { DataType.Id,          Convert.ToUInt32 ( table[i]["id"] ) },
                    { DataType.Nickname,    table[i]["nickname"] },
                    { DataType.Hp,          Convert.ToSingle ( table[i]["hp"] ) },
                    { DataType.Mspeed,      Convert.ToSingle ( table[i]["mspeed"] ) },
                    { DataType.Aspeed,      Convert.ToSingle ( table[i]["aspeed"] ) },
                    { DataType.Rspeed,      Convert.ToSingle ( table[i]["rspeed"] ) },
                    { DataType.Damage,      Convert.ToSingle ( table[i]["damage"] ) },
                    { DataType.Armor,       Convert.ToSingle ( table[i]["armor"] ) },
                    { DataType.ATime,       Convert.ToSingle ( table[i]["atime"] ) },
                });

#if UNITY_EDITOR
                UnitData u_data = Resources.Load<UnitData>("UnitData/U_" + table[i]["nickname"]);
                if (u_data == null) continue;
                u_data.id = Convert.ToUInt32(table[i]["id"]);
                u_data.nickname = Convert.ToString(table[i]["nickname"]);
                u_data.hp = Convert.ToSingle(table[i]["hp"]);
                u_data.mspeed = Convert.ToUInt32(table[i]["mspeed"]);
                u_data.aspeed = Convert.ToUInt32(table[i]["aspeed"]);
                u_data.rspeed = Convert.ToUInt32(table[i]["rspeed"]);
                u_data.damage = Convert.ToUInt32(table[i]["damage"]);
                u_data.armor = Convert.ToUInt32(table[i]["armor"]);
                u_data.atime = Convert.ToUInt32(table[i]["atime"]);
#endif
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
