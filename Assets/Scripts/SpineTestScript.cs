using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineTestScript : MonoBehaviour
{

    public SkeletonAnimation sk_animation;


    public void on_event ( TrackEntry entry, Spine.Event e ) {
        Debug.Log ( e.Data.Name );
    }


    private void Awake ( ) {
        sk_animation = GetComponent<SkeletonAnimation> ( );
        sk_animation.state.Event += on_event;
    }


    private void Update ( ) {
        
        if(Input.GetKeyDown(KeyCode.Space)) {

        }
        
        
    }



}
