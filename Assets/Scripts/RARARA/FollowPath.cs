using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float time = 5;
    public bool reverseLoop = false;

    private float distanceTravelled;
    private float current_time;
    private bool isReverse = false;

    void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            if (isReverse)
            {
                current_time -= Time.deltaTime;
            }
            else
            {
                current_time += Time.deltaTime;
            }

            float percentComplete = current_time / time;
            percentComplete = Mathf.Clamp01(percentComplete);
            transform.position = pathCreator.path.GetPointAtTime(percentComplete, endOfPathInstruction);
            
            if (Mathf.Approximately(percentComplete, 1f))
            {
                if (endOfPathInstruction == EndOfPathInstruction.Loop)
                {
                    current_time = 0;
                }
                else if(endOfPathInstruction == EndOfPathInstruction.Reverse)
                {
                    isReverse = true;
                }
            }
            else if (reverseLoop && Mathf.Approximately(percentComplete, 0f))
            {
                isReverse = false;
            }
        }
    }
    
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
