using System;
using System.Collections.Generic;

namespace Game.Unit {
    [Serializable]
    public class UnitOrder {

        private OrderId previous = OrderId.None;
        private OrderId next = OrderId.None;

        public OrderId order {
            get => previous;
        }


        public void clear ( ) {
            previous = OrderId.None;
            next = OrderId.None;
        }


        public void set ( OrderId id ) {
            next = id;
        }


        public OrderId execute ( ) {
            OrderId id = previous;
            previous = next;
            return id;
        }


        public void update ( ) {
            if(previous == OrderId.None) {
                previous = next;
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

    }
}