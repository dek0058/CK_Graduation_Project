using UnityEngine;
using Game.Unit;

public class TestNoteAI : MonoBehaviour
{
    public Transform target;
    public UUnit unit;

    void FixedUpdate()
    {
        Vector3 heading = target.position - transform.position;
       
        if (heading.sqrMagnitude > 1f)
        {
            transform.LookAt(target);
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);
        }
    }
}