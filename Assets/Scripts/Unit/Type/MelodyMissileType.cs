using UnityEngine;

namespace Game.Unit.Type {
    using Game.Unit;
    using Game.Management;

    public class MelodyMissileType : UnitType {

        private void OnCollisionEnter2D ( Collision2D collision ) {
            Debug.Log ( collision.gameObject.name );
        }

        private void OnTriggerEnter2D ( Collider2D collision ) {
            // TODO : 충돌처리
            if ( collision.gameObject.layer == (int)GameLayer.Map_Border || 
                collision.gameObject.layer == (int)GameLayer.Door) {
                Destroy ( transform.parent.parent.gameObject );
                return;
            }

            /// HACK : 따로 맵 콜리더에 뺴두던가 Missile 컴포넌트를 정규화 해야할듯?
            if ( collision.gameObject.layer == (int)GameLayer.Unit_Collider) {
                // TODO : 삭제

                Unit source = GetComponentInParent<Unit> ( );
                Unit target = collision.GetComponentInParent<Unit> ( );
                if(source.player == target.player) {
                    return;
                }

                // HACK
                Destroy ( transform.parent.parent.gameObject );
            }
        }
    }
}