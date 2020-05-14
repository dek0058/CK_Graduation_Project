using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Ability{
    using Unit;
    using Management;

    [System.Serializable]
    public class AbilityCaster {

        public UUnit unit;
        public Ability.Info info = new Ability.Info();
        public List<AbilityInfo> abilitys = new List<AbilityInfo> ( );

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
            if ( AbilityManager.instance.ability_list.ContainsKey ( id ) ) {
                // 중복체크
                for(int i = 0; i < unit.ability_caster.abilitys.Count; ++i ) {
                    if (unit.ability_caster.abilitys[i].id == id) {
                        return;
                    }
                }

                Ability a = AbilityManager.instance.ability_list[id];
                unit.ability_caster.abilitys.Add ( new AbilityInfo ( a ) );
            }
        }

        public static void remove(UUnit unit, int id) {
            if ( AbilityManager.instance.ability_list.ContainsKey ( id ) ) {
                AbilityInfo info = null;
                for ( int i = 0; i < unit.ability_caster.abilitys.Count; ++i ) {
                    if ( unit.ability_caster.abilitys[i].id == id ) {
                        info = unit.ability_caster.abilitys[i];
                        break;
                    }
                }

                if(info != null) {
                    unit.ability_caster.abilitys.Remove ( info );
                }
            }
        }

        /// <summary>
        /// 미사용
        /// </summary>
        public void update ( ) {
            if(do_use) {
                return;
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


        public IEnumerator Ecooltime_update ( AbilityInfo info ) {
            bool loop = true;
            info.cooltime = info.ability.cooltime;

            while(loop) {
                if(info == null) {
                    yield break;
                }

                info.cooltime -= Time.deltaTime;
                if(info.cooltime <= 0f) {
                    info.cooltime = 0f;
                    loop = false;
                }

                yield return null;
            }
        }
    }
}