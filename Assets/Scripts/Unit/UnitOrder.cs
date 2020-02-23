using System;

namespace Game.Unit {
    public class UnitOrder {

        public enum Active {
            None = 0,
            Move,
            Rotate,
        }

        public int layer = 0;
        public int dissable = 0;



        public void set_order ( Order_Id order, bool value ) {
            if ( value ) {
                layer |= 1 << (int)order;
            } else {
                layer &= ~(1 << (int)order);
            }
        }

        /// <summary>
        /// 선택한 명령이 입력되어있는지 확인합니다.
        /// </summary>
        public bool get_order ( Order_Id order ) {
            int bit = (layer >> (int)order) & 1;
            return bit == 1 ? true : false;
        }


        public void set_active ( Active active, bool value ) {
            if ( value ) {
                dissable |= 1 << (int)active;
            } else {
                dissable &= ~(1 << (int)active);
            }
        }


        /// <summary>
        /// 선택한 행동이 비활성화 상태인지 확인합니다.
        /// </summary>
        public bool get_active ( Active active ) {
            int bit = (dissable >> (int)active) & 1;
            return bit == 1 ? true : false;
        }
    }


    public enum Order_Id {
        None = 0,
        Stop,
        Move,
        Attack,

    }
}