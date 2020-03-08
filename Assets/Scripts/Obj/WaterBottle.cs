using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Obj {

    public class WaterBottle : MonoBehaviour {

        public enum PaintColor {
            Red,
            Blue,
            Green,
            Yellow,
        }
        public PaintColor paint_color;
        public bool switch_on;


        public void trigger_on ( ) {
            //HACK
            if(transform.childCount > 0) {
                return;
            }

            GameObject prefab = null;
            switch ( paint_color ) {
                case PaintColor.Red:
                    prefab = Resources.Load<GameObject> ( "Object/Paint_Tile/Prefab/paint_tile_red" );
                    break;
                case PaintColor.Blue:
                    prefab = Resources.Load<GameObject> ( "Object/Paint_Tile/Prefab/paint_tile_blue" );
                    break;
                case PaintColor.Green:
                    prefab = Resources.Load<GameObject> ( "Object/Paint_Tile/Prefab/paint_tile_green" );
                    break;
                case PaintColor.Yellow:
                    prefab = Resources.Load<GameObject> ( "Object/Paint_Tile/Prefab/paint_tile_yellow" );
                    break;
            }

            if(prefab != null) {
                Instantiate<GameObject> ( prefab, transform.position, Quaternion.identity, transform );
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Update ( ) {
            if(switch_on) {
                trigger_on ( );
            }
        }
    }
}