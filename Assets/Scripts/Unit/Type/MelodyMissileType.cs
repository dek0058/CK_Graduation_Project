using UnityEngine;

namespace Game.Unit.Type {
    using Game.Unit;
    using Game.Management;

    public class MelodyMissileType : UnitType {



        private void OnTriggerEnter2D ( Collider2D collision ) {
            // TODO : 충돌처리

            /// HACK : 따로 맵 콜리더에 뺴두던가 Missile 컴포넌트를 정규화 해야할듯?
            if ( collision.gameObject.layer == (int)GameLayer.Map_Collider ) {
                // TODO : 삭제

                // HACK
                Destroy ( transform.parent.parent.gameObject );
            }
        }
    }
}