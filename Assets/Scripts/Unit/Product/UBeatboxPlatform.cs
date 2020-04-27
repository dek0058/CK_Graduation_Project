using UnityEngine;

namespace Game.Unit.Product {
    using Obj.Puzzle;
    using Management;


    public class UBeatboxPlatform : UUnit {

        public UBeatboxPiece piece;

        public bool play ( ) {
            if(piece == null) {
                return false;
            }
            AudioClip clip = ResourceLoader.instance.get_prefab ( (int)piece.clip ) as AudioClip;
            sfx.play ( clip, 1f, 0f, 1f );
            return true;
        }
    }
}
