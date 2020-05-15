using UnityEngine;

namespace Game.Unit.Product
{
    using Obj.Puzzle;
    using Management;

    public class UBeatboxPiece : UUnit
    {
        public BeatboxPuzzle.Clip beat_clip;
        public bool isActive = true;

        public void Play()
        {
            if (sfx == null) return;

            AudioClip clip = ResourceLoader.instance.get_prefab((int)beat_clip) as AudioClip;
            sfx.play(clip, 1f, 0f, 1f);
        }
    }
}