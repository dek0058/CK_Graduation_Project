using UnityEngine;
using PathCreation;
using Game.AI;

public class FollowPath : UnitAI
{
    [Header("FollowPath")]
    public BeatPuzzle puzzle;
    public EndOfPathInstruction endOfPathInstruction;
    public float time;
    public bool reverseLoop;

    [HideInInspector]
    public bool isActive = false;
    [HideInInspector]
    public float current_time;

    private float distanceTravelled;
    private bool isReverse = false;

    void Start()
    {
        if (puzzle.pathCreator != null)
        {
            puzzle.pathCreator.pathUpdated += OnPathChanged;
            transform.position = puzzle.pathCreator.path.GetPointAtTime(0f, endOfPathInstruction);
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) // HACK : 임시로 키로 해두었고 때리면 실행하게 해야 함
        {
            if (!isActive)
            {
                for (int i = 0; i < puzzle.beatBoards.Count; i++)
                {
                    StartCoroutine(puzzle.beatBoards[i].Ebeat_puzzle_distance());
                }
                isActive = true;
            }
        }

        if (!isActive) return;

        if (puzzle.pathCreator != null)
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
            transform.position = puzzle.pathCreator.path.GetPointAtTime(percentComplete, endOfPathInstruction);
            
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
        distanceTravelled = puzzle.pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}