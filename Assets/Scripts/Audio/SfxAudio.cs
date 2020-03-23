using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio {
    using Management;

    public class SfxAudio : MonoBehaviour {
        public List<GameAudio> audio_list = new List<GameAudio> ( );

        [SerializeField, Range(1, 10)]
        private int index = 1;

        // TODO : Audio Source 설정들


        public void play ( AudioClip clip, float pitch, float attack_time, float release_time ) {
            bool result = false;
            for(int i = 0; i < audio_list.Count; ++i ) {
                if(audio_list[i].state == ASREnvelope.State.Idle) {
                    result = true;
                    audio_list[i].play ( clip, pitch, AudioSettings.dspTime + Time.deltaTime, attack_time, release_time );
                    break;
                }
            }

            if(!result) {
                GameAudio a = create_audio ( "Sfx - Destroy" );
                a.play ( clip, pitch, AudioSettings.dspTime + Time.deltaTime, attack_time, release_time, true );
            }
        }

        public void stop ( ) {
            for ( int i = 0; i < audio_list.Count; ++i ) {
                audio_list[i].source.Stop ( );
            }
        }

        public void pause ( ) {
            for ( int i = 0; i < audio_list.Count; ++i ) {
                audio_list[i].source.Pause ( );
            }
        }

        public void unpuase ( ) {
            for ( int i = 0; i < audio_list.Count; ++i ) {
                audio_list[i].source.UnPause ( );
            }
        }

        public void confirm ( ) {
            while ( audio_list.Count < index ) {
                audio_list.Add ( create_audio ( "Sfx - " + (audio_list.Count + 1) ) );
            }
        }


        private GameAudio create_audio ( string name ) {
            GameObject audio = Instantiate ( ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Game_Audio ) as GameObject, transform );
            audio.name = name;
            return audio.GetComponent<GameAudio> ( );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }

        private void OnApplicationFocus ( bool focus ) {
            // HACK : Puase 기능을 따로 만들어야 함
            if ( focus ) {
                unpuase ( );
            } else {
                pause ( );
            }
        }
    }
}
