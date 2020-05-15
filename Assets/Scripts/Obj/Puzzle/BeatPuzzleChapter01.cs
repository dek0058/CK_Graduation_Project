using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Obj.Puzzle
{
    using Unit;
    using Unit.Product;

    public class BeatPuzzleChapter01 : MonoBehaviour
    {
        public UUnit unit = null;
        public UBeatboxPiece[] beats;
        private int beat_cnt = 0;
        private Material mat;

        private void Start()
        {
            mat = Resources.Load<Material>("Tile/Material/Green");
            beats = transform.GetComponentsInChildren<UBeatboxPiece>();
            StartCoroutine(MoveToPosition(unit.transform, beats[beat_cnt].transform.position, 0f));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < beats.Length; i++)
                {
                    beats[i].transform.GetComponent<MeshRenderer>().material = mat;
                    beats[i].isActive = true;
                }
            }
        }

        private IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
        {
            Vector3 currentPos = transform.position;
            Vector3 deltaPos = transform.position - position;
            float speed = deltaPos.magnitude / Time.deltaTime;
            float t = 0f;

            while (!beats[beat_cnt].isActive)
            {
                yield return null;
            }

            unit.move(Vector2.right);
            unit.rotate(90f);

            while (t < 1)
            {
                unit.unit_movemt_order.set(OrderId.Move);

                t += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(currentPos, position, t);
                yield return null;
            }

            unit.unit_movemt_order.set(OrderId.Stop);
            unit.move(Vector2.zero);
            beats[beat_cnt].Play();
            beat_cnt++;

            if (beat_cnt < beats.Length)
            {
                StartCoroutine(MoveToPosition(unit.transform, beats[beat_cnt].transform.position, 1.7f));
            }
        }
    }
}