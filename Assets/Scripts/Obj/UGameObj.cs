using UnityEngine;

namespace Game.Obj {
    using Management;

    public class UGameObj : MonoBehaviour, IGameSpace {

        public SpriteRenderer renderer2D;

        [SerializeField]
        private GameSpace gp;
        public GameSpace game_space {
            get => gp;
            set {
                gp = value;
                GameLayer layer = GameLayer.Origin_Unit_Collider;

                switch ( gp ) {
                    case GameSpace.Origin:      layer = GameLayer.Origin_Unit_Collider;     break;
                    case GameSpace.Purgatory:   layer = GameLayer.Purgatory_Unit_Collider;  break;
                    case GameSpace.Both:        layer = GameLayer.Unit_Collider;            break;
                }
                gameObject.layer = (int)layer;
            }
        }


        protected virtual void collision_event ( Collider2D collision ) {
            
        }


        public virtual void confirm ( ) {

            if ( renderer2D == null ) {
                renderer2D = GetComponent<SpriteRenderer> ( );
            }

            game_space = gp;
        }


        public virtual void initialize ( ) {
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        public void Awake ( ) {
            confirm ( );
        }


        private void Start ( ) {
            initialize ( );
        }


        private void OnTriggerEnter2D ( Collider2D collision ) {
            collision_event ( collision );
        }
    }
}
