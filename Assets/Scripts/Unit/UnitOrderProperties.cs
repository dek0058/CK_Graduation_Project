namespace Game.Unit {
    using JToolkit.Utility;

    public class UnitOrderProperties {

        public enum Properties {
            None = 0,
            Attack,
            Movement,
            Rotation,
            Spell,
        }

        private BitLayer layer = new BitLayer ( );


        /// <summary>
        /// 속성을 설정합니다.
        /// </summary>
        public void set ( Properties properties, bool value ) {
            layer.set ( (int)properties, value );
        }


        /// <summary>
        /// 선택한 속성의 상태를 확인합니다.
        /// </summary>
        public bool get ( Properties properties ) {
            return layer.get ( (int)properties );
        }


        public void clear ( ) {
            layer.clear ( );
        }

        public void full ( ) {
            layer.full ( );
        }
    }
}
