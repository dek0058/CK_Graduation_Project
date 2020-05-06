using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JToolkit.Utility {
    public class BitLayer {

        private int layer = 0;
        public int current {
            get => layer;
        }

        /// <summary>
        /// 속성을 설정합니다.
        /// </summary>
        public void set ( int properties, bool value ) {
            if ( value ) {
                layer |= 1 << properties;
            } else {
                layer &= ~(1 << properties);
            }
        }


        /// <summary>
        /// 선택한 속성의 상태를 확인합니다.
        /// </summary>
        public bool get ( int properties ) {
            int bit = (layer >> properties) & 1;
            return bit == 1 ? true : false;
        }


        public void clear ( ) {
            layer = 0;
        }


        public void full ( ) {
            layer = int.MaxValue;
        }
    }
}
