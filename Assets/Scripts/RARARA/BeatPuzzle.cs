using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Game.Audio;

public enum BeatType
{
    Beat_Drum,
    Beat_Guitar,
    Beat_Piano,
    Beat_Violin,
}

public class BeatPuzzle : MonoBehaviour
{
    public BeatType type;
    public PathCreator pathCreator;
    public FollowPath follow;
    public SfxAudio sfx;
    public GameObject boardList;

    [HideInInspector]
    public List<BeatBoard> beatBoards = new List<BeatBoard>();

    private void Start()
    {
        for (int i = 0; i < boardList.transform.childCount; i++)
        {
            beatBoards.Add(boardList.transform.GetChild(i).GetComponent<BeatBoard>());
        }
    }

    public void ResetBoard()
    {
        follow.isActive = false;
        follow.current_time = 0;
        follow.transform.position = pathCreator.path.GetPointAtTime(0f, EndOfPathInstruction.Stop);

        for (int i = 0; i < beatBoards.Count; i++)
        {
            beatBoards[i].StopAllCoroutines();
        }
    }
}