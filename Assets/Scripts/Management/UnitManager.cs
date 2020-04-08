using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Management
{
    using JToolkit.Utility;
    using Game.Unit;

    public class UnitManager : Singleton<UnitManager>
    {
        public enum DataType
        {
            Id = 0,
            Nickname,
            Hp,
            Mspeed,
            Aspeed,
            Rspeed,
            Damage,
            Armor,
            Atime,
        }

        public List<EnumDictionary<DataType, object>> data_table = null;
        public Dictionary<int, UnitData> u_data = new Dictionary<int, UnitData>();
        public TableWWW val;

        private void Start()
        {
            load_data();
        }

        public void load_data()
        {
            if (data_table == null)
            {
                data_table = new List<EnumDictionary<DataType, object>>();
            }
            data_table?.Clear();

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                val.Req<UnitData>(UnitData.GetGoogleSheetGID(), u_data, (a_bSuccess) =>
                 {
                     if (a_bSuccess)
                     {
                         for (int i = 1; i <= u_data.Count; i++)
                         {
                             data_table.Add(new EnumDictionary<DataType, object> {
                            { DataType.Id,          u_data[i].id },
                            { DataType.Nickname,    u_data[i].nickname },
                            { DataType.Hp,          u_data[i].hp},
                            { DataType.Mspeed,      u_data[i].mspeed },
                            { DataType.Aspeed,      u_data[i].aspeed },
                            { DataType.Rspeed,      u_data[i].rspeed },
                            { DataType.Damage,      u_data[i].damage },
                            { DataType.Armor,       u_data[i].armor },
                            { DataType.Atime,       u_data[i].atime },
                            });
                         }
                     }
                     else
                     {
                         List<Dictionary<string, object>> table = CSVReader.read("DataTable/UnitDataTable");
                         for (int i = 0; i < table.Count; i++)
                         {
                             data_table.Add(new EnumDictionary<DataType, object> {
                            { DataType.Id,          Convert.ToUInt32 ( table[i]["id"] ) },
                            { DataType.Nickname,    table[i]["nickname"] },
                            { DataType.Hp,          Convert.ToSingle ( table[i]["hp"] ) },
                            { DataType.Mspeed,      Convert.ToSingle ( table[i]["mspeed"] ) },
                            { DataType.Aspeed,      Convert.ToSingle ( table[i]["aspeed"] ) },
                            { DataType.Rspeed,      Convert.ToSingle ( table[i]["rspeed"] ) },
                            { DataType.Damage,      Convert.ToSingle ( table[i]["damage"] ) },
                            { DataType.Armor,       Convert.ToSingle ( table[i]["armor"] ) },
                            { DataType.Atime,       Convert.ToSingle ( table[i]["atime"] ) },
                            });
                         }
                     }
                     #if UNITY_EDITOR
                     for (int i = 0; i < data_table.Count; i++)
                     {
                         UnitData unit_data = Resources.Load<UnitData>("UnitData/U_" + data_table[i][DataType.Nickname]);
                         if (u_data == null) continue;
                         unit_data.id = Convert.ToUInt32(data_table[i][DataType.Id]);
                         unit_data.nickname = Convert.ToString(data_table[i][DataType.Nickname]);
                         unit_data.hp = Convert.ToSingle(data_table[i][DataType.Hp]);
                         unit_data.mspeed = Convert.ToUInt32(data_table[i][DataType.Mspeed]);
                         unit_data.aspeed = Convert.ToUInt32(data_table[i][DataType.Aspeed]);
                         unit_data.rspeed = Convert.ToUInt32(data_table[i][DataType.Rspeed]);
                         unit_data.damage = Convert.ToUInt32(data_table[i][DataType.Damage]);
                         unit_data.armor = Convert.ToUInt32(data_table[i][DataType.Armor]);
                         unit_data.atime = Convert.ToUInt32(data_table[i][DataType.Atime]);
                     }
                     #endif
                 });
            }
        }

        /// <summary>
        /// ID를 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<DataType, object> get_data(uint id)
        {
            return data_table?.Find(data => (uint)data[DataType.Id] == id);
        }

        /// <summary>
        /// Nickname을 검색하여 유닛 테이블을 찾습니다.
        /// </summary>
        /// <returns>Unit Data Table</returns>
        public EnumDictionary<DataType, object> get_data(string nickname)
        {
            return data_table?.Find(data => (string)data[DataType.Nickname] == nickname);
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////
        
        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }

            if (instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
        }


        private void OnDestroy()
        {
            if (instance == this) { _app_is_close = true; }
        }

        private void OnApplicationQuit()
        {
            if (instance == this) { _app_is_close = true; }
        }
    }
}
