using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stage.Room {
    using Audio;
    using Management;
    using Unit.Character;
    using AI;

    public class PianoBossRoom : GameRoom {

        public SfxAudio sfx;

        public UPianoMan boss;

        // HACK
        public TemporaryBossAI ai;

        protected override void active ( ) {
            base.active ( );

            if(state == State.Inactive) {
                state = State.Active;
            }

            if(state == State.Active) {
                SoundManager.instance.off_music ( );
                sfx.play ( ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Stage1_Boss_Music ) as AudioClip, 1f, 0f, 0f );
                ai.start ( );

                CameraManager.instance.game_camera.cv_camera.Follow =
                    CameraManager.instance.game_camera.camera_point;
            }
        }


        protected override void inactive ( ) {
            base.inactive ( );

            SoundManager.instance.on_music ( );
            sfx.stop ( );
            ai.reset ( );

            CameraManager.instance.game_camera.cv_camera.Follow =
                    CameraManager.instance.game_camera.source.transform;
        }


        protected override void update ( ) {
            base.update ( );
 
            // HACK
            if(state == State.Active) {

                float dist = Vector2.Distance ( boss.get_position(),
                    CameraManager.instance.game_camera.source.get_position ( ) ) * 0.5f;
                float angle = JToolkit.Math.Angle.target_to_angle ( boss.get_position ( ),
                    CameraManager.instance.game_camera.source.get_position ( ) ) * Mathf.Rad2Deg - 90f;
                Vector2 location = JToolkit.Math.Polar.location ( dist, angle );

                CameraManager.instance.game_camera.camera_point.transform.position =
                    (Vector2)boss.get_position ( ) + location;

                if ( !sfx.audio_list[0].source.isPlaying ) {
                    return;
                }
                float time = ai.p_queue.Peek ( ).tick;
                if(sfx.audio_list[0].source.time > time) {
                    TemporaryData d = ai.p_queue.Dequeue ( );
                    switch ( d.pattern ) {
                        case 1:
                            boss.attack ( );
                            break;
                        case 2:
                            boss.attack_randommissile ( );
                            break;
                        case 3:
                            boss.attack_tripplemissile ( );
                            break;
                        case 4:
                            boss.attack_cornermissile ( );
                            break;
                        case 5:
                            boss.attack_scattermissile ( );
                            break;
                    }
                }
            }
        }

    }
}
