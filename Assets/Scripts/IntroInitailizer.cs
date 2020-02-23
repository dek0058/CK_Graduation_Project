using System.Collections;
using UnityEngine;

using Game.Management;

public class IntroInitailizer : MonoBehaviour {

    [SerializeField]
    [Range(1f, 5f)]
    private float scene_duartion = 2f;


    private IEnumerator Escene_transition ( float duration ) {
        bool loop = true;
        float time = 0f;
        while ( loop ) {
            time += Time.deltaTime;
            if ( time >= duration ) {
                loop = false;
            }
            yield return new WaitForEndOfFrame ( );
        }

        TransitionManager.instance.load ( TransitionManager.SceneType.Menu );
    }


    ////////////////////////////////////////////////////////////////////////////
    ///                               Unity                                  ///
    ////////////////////////////////////////////////////////////////////////////


    private void Start ( ) {
        StartCoroutine ( Escene_transition ( scene_duartion ) );
    }

}
