#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Obj.Puzzle {

    [CustomEditor(typeof(BeatboxPuzzle)), CanEditMultipleObjects]
    public class BeatboxPuzzleEditor : Editor {

        private BeatboxPuzzle[] components;

        private void OnEnable ( ) {
            components = new BeatboxPuzzle[targets.Length];
            for ( int i = 0; i < components.Length; i++ ) {
                components[i] = targets[i] as BeatboxPuzzle;
            }
        }

        public override void OnInspectorGUI ( ) {
            base.OnInspectorGUI ( );
            
            if(GUILayout.Button("Create Platform")) {
                foreach ( var com in components ) {
                    com.confirm ( );
                    com.time_caculate ( );
                    com.arrangement ( );
                }
            }
        }

    }
}
#endif