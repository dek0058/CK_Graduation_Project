using System.Collections;
using UnityEngine;
using Game.Management;

public class BeatBoard : MonoBehaviour
{
    public BeatPuzzle puzzle;
    public BeatType type;
    public ResourceLoader.Resource resource;
    public float time;
    
    [HideInInspector]
    public bool isHold = false;

    private float distanceTravelled;

    void Start()
    {
        transform.position = puzzle.pathCreator.path.GetPointAtTime(time / puzzle.follow.time, PathCreation.EndOfPathInstruction.Stop);
    }

    public IEnumerator Ebeat_puzzle_distance()
    {
        while (!isHold)
        {
            if (gameObject.activeSelf && Vector2.Distance(puzzle.follow.transform.position, transform.position) < 0.05f)
            {
                puzzle.sfx.play(ResourceLoader.instance.get_prefab(resource) as AudioClip, 1f, 0f, 0f);
                if (type != puzzle.type) {
                    puzzle.ResetBoard();
                }
                yield break;
            }
            yield return null;
        }
    }
}