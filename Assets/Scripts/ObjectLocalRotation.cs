using UnityEngine;

public class ObjectLocalRotation : MonoBehaviour{

    public float speed = 1f;

    private void Update ( ) {

        transform.Rotate ( 0f, speed * Time.deltaTime, 0f);

    }

}