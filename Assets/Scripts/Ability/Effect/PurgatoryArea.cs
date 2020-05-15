using UnityEngine;
using System.Collections.Generic;

namespace Game.Ability.Effect {
    using Management;
    using Unit;

    public class PurgatoryArea : MonoBehaviour {

        public UUnit source;
        [Range(0f, 1f)]
        public float slowly_value = 0.8f;

        public bool is_active = false;

        private List<UUnit> unit_pool = new List<UUnit> ( );
        private new Collider collider;

        public void active ( bool value) {
            is_active = value;
            collider.enabled = value;
        }


        public void confirm ( ) {
            collider = GetComponent<Collider> ( );
        }
        

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }


        private void FixedUpdate ( ) {
            if(source == null) {
                return;
            }

            transform.position = source.transform.position;
            ShaderBlackBoard.instance.world_position = source.get_position ( );
        }


        private void OnDestroy ( ) {
            unit_pool.ForEach ( u => u.unit_status.rhythm = 1.0f );
        }


        private void OnTriggerEnter ( Collider other ) {
            UUnit unit = other.GetComponent<UUnit> ( );
            if ( unit == null || unit == source) {
                return;    
            }

            unit_pool.Add ( unit );
            unit.unit_status.rhythm = slowly_value;
        }

        private void OnTriggerExit ( Collider other ) {
            UUnit unit = other.GetComponent<UUnit> ( );
            if ( unit == null || !unit_pool.Contains(unit) ) {
                return;
            }

            unit_pool.Remove ( unit );
            unit.unit_status.rhythm = 1.0f;
        }
    }
}
