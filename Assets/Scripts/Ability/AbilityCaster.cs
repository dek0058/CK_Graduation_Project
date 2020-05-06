using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Ability{
    using Unit;

    [System.Serializable]
    public class AbilityCaster {

        public UUnit unit;
        public List<Ability> abilitys = new List<Ability> ( );

        /// <summary> 능력의 상태 </summary>
        public enum Condition { 
            None = 0,
            Begin,          // 시작
            Execute,        // 시전
            Channeling,     // 채널링 중
            Effect,         // 효과 발동
            Cancel,         // 취소
            End,            // 끝
        }
        private Condition condition = Condition.None;

        private bool do_use = false;    // 어빌리티를 사용중인가 아닌가?


        public static void add(UUnit unit, int id) {
            if(Ability.ability_list.ContainsKey(id)) {
                Ability a = Ability.ability_list[id];
                if ( !unit.ability_caster.abilitys.Contains ( a ) ) {
                    unit.ability_caster.abilitys.Add ( a );
                }
            }
        }

        public static void remove(UUnit unit, int id) {
            if ( Ability.ability_list.ContainsKey ( id ) ) {
                Ability a = Ability.ability_list[id];
                if ( unit.ability_caster.abilitys.Contains ( a ) ) {
                    unit.ability_caster.abilitys.Remove ( a );
                }
            }
        }


        public void update ( ) {
            if(do_use) {
                return;
            }

            for(int i = 0; i < abilitys.Count; ++i ) {
                if(abilitys[i].is_passive) {
                    continue;
                }
                if(unit.unit_order.order == abilitys[i].order_id) {
                    // TODO : 어빌리티 사용
                    break;
                }
            }

        }


        



        /// <summary>
        /// 어빌리티를 가지고 있는지 확인합니다.
        /// </summary>
        public bool have_ability ( int id ) {
            bool value = false;
            for(int i = 0; i  < abilitys.Count; ++i ) {
                if(abilitys[i].id == id) {
                    value = true; break;
                }
            }
            return value;
        }
    }
}