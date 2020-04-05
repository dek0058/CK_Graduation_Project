using UnityEngine;
using System.Collections.Generic;

using Game.Unit;

class LocationSlow : MonoBehaviour {

    private List<UUnit> units = new List<UUnit> ( );


    private void OnTriggerEnter2D ( Collider2D collision ) {
        UUnit unit = collision.GetComponentInParent<UUnit> ( );
        if(unit == null) {
            return;
        }

        units.Add ( unit );
        unit.unit_status.rhythm = 0.33f;
    }

    private void OnTriggerExit2D ( Collider2D collision ) {
        UUnit unit = collision.GetComponentInParent<UUnit> ( );
        if ( unit == null ) {
            return;
        }

        if(!units.Contains(unit)) {
            return;
        }

        units.Remove ( unit );
        unit.unit_status.rhythm = 1f;
    }
}