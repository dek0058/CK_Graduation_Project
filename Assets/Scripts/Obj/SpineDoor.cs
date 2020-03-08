using UnityEngine;
using Spine.Unity;

namespace Game.Obj {
    using Game.Management;
    using Game.Unit;
    using Game.User;

    public class SpineDoor : GameDoor {

        [HideInInspector]
        public SkeletonAnimation sk_animation = null;

        [Header("Animation Referance")]
        public AnimationReferenceAsset close_animation;
        public AnimationReferenceAsset open_animation;

        private State previous_state;


        private void play_new_animation ( ) {
            Spine.Animation next = null;
            float timescale = 1f;

            switch ( state ) {
                case State.Close:
                    next = close_animation;
                    timescale = -1f;
                    close ( );
                    break;
                case State.Open:
                case State.Broken:
                    next = open_animation;
                    open ( );
                    break;
            }

            set_animation ( next, false, timescale );
        }


        protected override void transition ( Collider2D collision ) {
            LayerMask mask = collision.gameObject.layer;
            if( mask != (int)GameLayer.Unit) {
                return;
            }
            Unit unit = collision.GetComponent<Unit> ( );
            Player player = PlayerManager.instance.get_player ( unit );

            if(player != null) {
                Debug.Log ( StageManager.instance );
                StageManager.instance.current_stage.transition_room ( next_room, unit, teleport_transform.position );

            } else {
                unit.movement_system.set_position ( teleport_transform.position );
                // 이동 효과
            }
        }


        protected override void confirm ( ) {
            base.confirm ( );

            if(sk_animation == null) {
                sk_animation = GetComponent<SkeletonAnimation> ( );
            }

            previous_state = state;
        }



        public void set_animation ( AnimationReferenceAsset animation, bool loop, float timescale ) {
            sk_animation.state.SetAnimation ( 0, animation, loop ).TimeScale = timescale;
        }

        public void set_animation ( Spine.Animation animation, bool loop, float timescale ) {
            sk_animation.state.SetAnimation ( 0, animation, loop ).TimeScale = timescale;
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );

        }


        private void Update ( ) {

            switch ( my_room.state ) {
                case Stage.GameRoom.State.Inactive:

                    break;
                case Stage.GameRoom.State.Active: {
                    if ( state != State.Close ) {
                        set_state ( State.Close );
                    }
                } break;
                case Stage.GameRoom.State.Clear: {
                    if ( state != State.Open ) {
                        set_state ( State.Open );
                    }
                } break;
            }


            if(previous_state != state) {
                play_new_animation ( );
            }
            previous_state = state;
        }
    }
}
