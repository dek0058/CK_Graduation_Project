using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Game.Unit {
    public abstract class UnitType : MonoBehaviour {

        public UUnit unit;
        public Animator animator;

        public Dictionary<string, UnityEvent> schedules = new Dictionary<string, UnityEvent> ( );
        private List<DamageInfo> damage_list = new List<DamageInfo> ( );

        /// <summary>
        /// 데미지를 축적합니다.
        /// </summary>
        /// <param name="type">데미지 종류</param>
        /// <param name="amount">데미지 양</param>
        public void add_damage ( DamageInfo.Type type, float amount, bool is_real = false ) {
            if( amount > 0f) {
                damage_list.Add ( new DamageInfo ( type, amount, is_real ) );
            }
        }

        public DamageInfo[] get_damage ( ) {
            return damage_list.ToArray ( );
        }

        public DamageInfo[] get_damage ( DamageInfo.Type type ) {
            if(damage_list.Count == 0) {
                return null;
            }
            return damage_list.Where ( dmg => dmg.type == type ).ToArray ( );
        }

        public DamageInfo get_real_damage ( ) {
            return damage_list.Find ( dmg => dmg.is_real == true );
        }

        public float get_damage_to_value ( DamageInfo.Type type ) {
            float dmg = 0f;
            DamageInfo[] info = get_damage ( type );
            if(info == null) {
                return dmg;
            }
            for ( int i = 0; i < info.Length; ++i ) {
                dmg += info[i].amount;
            }
            return dmg;
        }

        public void damage_clear ( ) {
            damage_list.Clear ( );
        }


        public void add ( string key, UnityAction callback ) {
            if(schedules.ContainsKey(key)) {
                return;
            }
            UnityEvent uevent = new UnityEvent ( );
            uevent.AddListener ( callback );
            
            schedules.Add ( key, uevent );
        }



        public void confirm ( ) {
            if(animator == null) {
                animator = GetComponent<Animator> ( );
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
    }


    public struct DamageInfo {
        public enum Type {  // 데미지 종류
            Melee,      // 일반
            Spell,      // 마법
            Universal,  // 트루 (고정)
        }
        public Type type {
            get; set;
        }
        public float amount {
            get; set;
        }
        public bool is_real {
            get; set;
        }

        public DamageInfo ( Type type, float amount, bool is_real = false ) => 
            (this.type, this.amount, this.is_real) = (type, amount, is_real);

        public static float damage ( Type type, float damage, float armor ) {
            float dmg = 0f;
            switch ( type ) {
                case Type.Melee:
                    dmg = damage - armor;
                    if(dmg < 0f) {
                        dmg = 0f;
                    }
                    break;
                case Type.Spell:
                    dmg = damage - armor;
                    if(dmg < 0f) {
                        dmg = 0f;
                    }
                    break;
                case Type.Universal:
                    dmg = damage;
                    break;
            }
            return dmg;
        }
    }
}
