using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit {
    [Serializable]
    public class UnitOrder {

        private OrderId previous = OrderId.None;

        private OrderId current = OrderId.None;
        public OrderId order {
            get => current;
        }

       


        public void clear ( ) {
            previous = OrderId.None;
        }


        public void set ( OrderId id ) {
            if(previous == id) {
                return;
            }
            previous = id;
        }


        public OrderId execute ( ) {
            OrderId id = current;
            current = OrderId.None;
            return id;
        }


        private float releas_time = 0f;
        private const float push_time = 0.033f;
        public void update ( ) {
            releas_time += Time.unscaledDeltaTime;
            if ( releas_time >= push_time ) {
                current = previous;
                previous = OrderId.None;
                releas_time = 0f;
            }
        }
    }


    /// <summary>
    /// 명령 ID
    /// </summary>
    public enum OrderId {
        // 기본 유닛 속성
        None = 0,
        Stop,
        Move,
        Attack,

        // 어빌리티
        PurgatoryArea,
    }
}