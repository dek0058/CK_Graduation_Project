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

            // HACK : 테스트 기능
            SoundManager.instance.off_music ( );
            sfx.play ( ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Stage1_Boss_Music ) as AudioClip, 1f, 0f, 0f );
            ai.start ( );
        }


        protected override void inactive ( ) {
            base.inactive ( );

            SoundManager.instance.on_music ( );
            sfx.stop ( );
            ai.reset ( );
        }


        protected override void update ( ) {
            base.update ( );
 
            // HACK
            if(is_active) {

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
