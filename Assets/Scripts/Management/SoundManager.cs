using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace Game.Management {
    using JToolkit.Utility;
    using Audio;

    [RequireComponent ( typeof ( AudioSource ) )]
    public class SoundManager : Singleton<SoundManager> {

        public GameAudio main_audio;
        public AudioMixer mixer;

        public AudioMixerGroup master_group;
        public AudioMixerGroup sfx_group;
        public AudioMixerGroup music_group;

        [Range(0f, 1f)]
        public float sfx_volum = 1f;
        [Range(0f, 1f)]
        public float music_volum = 1f;

        public bool sfx_mute = false;
        public bool music_mute = false;

        [SerializeField, Range(1, 10)]
        private int sfx_index = 5;
        private List<GameAudio> sfx_audio_list = new List<GameAudio> ( );
        private int next_index = 0;

        private enum MixerParameter {
            Sfx_Vol,
            Music_Vol,
        }
        private EnumDictionary<MixerParameter, string> parameter_names = new EnumDictionary<MixerParameter, string> ( ) {
            {MixerParameter.Sfx_Vol, "Sfx_Vol" },
            {MixerParameter.Music_Vol, "Music_Vol" },
        };

        public bool is_running = false;


        public void set_sfx_volum ( float volum ) {
            if(volum > 1f) {
                volum = 1f;
            } else if(volum < 0f) {
                volum = 0f;
            }
            sfx_volum = volum;
            float result = -80 + (sfx_volum * 80f);
            if ( !sfx_mute ) {
                mixer.SetFloat ( parameter_names[MixerParameter.Sfx_Vol], result );
            }
        }

        public void set_music_volum ( float volum ) {
            if ( volum > 1f ) {
                volum = 1f;
            } else if ( volum < 0f ) {
                volum = 0f;
            }
            music_volum = volum;
            float result = -80 + (music_volum * 80f);
            if ( !music_mute ) {
                mixer.SetFloat ( parameter_names[MixerParameter.Music_Vol], result );
            }
        }


        public void set_sfx_mute ( bool value ) {
            sfx_mute = value;
            if ( value ) {
                mixer.SetFloat ( parameter_names[MixerParameter.Sfx_Vol], -80f );
            } else {
                float result = -80 + (sfx_volum * 80f);
                mixer.SetFloat ( parameter_names[MixerParameter.Sfx_Vol], result );
            }
        }

        public void set_music_mute ( bool value ) {
            music_mute = value;
            if(value) {
                mixer.SetFloat ( parameter_names[MixerParameter.Music_Vol], -80f );
            } else {
                float result = -80 + (music_volum * 80f);
                mixer.SetFloat ( parameter_names[MixerParameter.Music_Vol], result );
            }
        }


        public void play ( AudioClip clip, double tick_time, double attack_time, double release_time, int midi_note_number = 60 ) {
            float pitch = JToolkit.Math.Music.midi_note_to_pitch ( midi_note_number, JToolkit.Math.Music.Midi_Note_C4 );

            int cnt = 0;
            while(sfx_audio_list[next_index].state != ASREnvelope.State.Idle) {
                if(cnt >= sfx_index) {
                    GameObject prefab = ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Game_Audio ) as GameObject;
                    GameObject audio_obj = Instantiate ( prefab, transform );
                    GameAudio game_audio = audio_obj.GetComponent<GameAudio> ( );
                    audio_obj.name = "Sfx Game Audio - Destroy";
                    game_audio.play ( clip, pitch, tick_time, attack_time, release_time, true );
                    next_index = (next_index + 1) % sfx_audio_list.Count;
                    return;
                }
                cnt++;
                next_index = (next_index + 1) % sfx_audio_list.Count;
            }

            sfx_audio_list[next_index].play ( clip, pitch, tick_time, attack_time, release_time );
            next_index = (next_index + 1) % sfx_audio_list.Count;
        }


        public void set_music ( AudioClip clip ) {
            main_audio.source.clip = clip;
        }


        public void on_music ( ) {
            is_running = true;
        }


        public void confirm ( ) {

            if( main_audio == null) {
                main_audio = GetComponent<GameAudio> ( );
            }

            if( main_audio.source == null) {
                main_audio.source = GetComponent<AudioSource> ( );
            }

            if(mixer == null) {
                mixer = Resources.Load<AudioMixer> ( "Audio/AudioMixer" );
            }

            if ( master_group == null ) {
                master_group = mixer.FindMatchingGroups ( "Master" )[0];
            }

            if ( sfx_group == null ) {
                sfx_group = mixer.FindMatchingGroups ( "SFX" )[0];
            }

            if ( music_group == null ) {
                music_group = mixer.FindMatchingGroups ( "Music" )[0];
            }

            if( main_audio.source.outputAudioMixerGroup == null) {
                main_audio.source.outputAudioMixerGroup = music_group;
            }


            if ( sfx_audio_list.Count >= sfx_index ) {
                return;
            } else {
                GameObject prefab = ResourceLoader.instance.get_prefab ( ResourceLoader.Resource.Game_Audio ) as GameObject;
                while ( sfx_audio_list.Count < sfx_index ) {
                    GameObject audio_obj = Instantiate ( prefab, transform );
                    sfx_audio_list.Add ( audio_obj.GetComponent<GameAudio> ( ) );
                    audio_obj.name = "Sfx Game Audio - " + sfx_audio_list.Count;
                }
            }

            set_sfx_volum ( sfx_volum );
            set_music_volum ( music_volum );

            set_sfx_mute ( sfx_mute );
            set_music_mute ( music_mute );

            // HACK
            on_music ( );
        }


        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            AudioSource s = GetComponent<AudioSource> ( );
            if ( s == null ) {
                s = gameObject.AddComponent<AudioSource> ( );
            }

            GameAudio a = GetComponent<GameAudio> ( );
            if ( a == null ) {
                gameObject.AddComponent<AudioSource> ( );
            }
        }


        private void LateUpdate ( ) {
            if(!is_running) {
                return;
            }

            if(main_audio.state == ASREnvelope.State.Idle) {
                main_audio.play ( AudioSettings.dspTime + Time.deltaTime, 0f, 1f );
            }
        }


        private void OnValidate ( ) {
#if UNITY_EDITOR
            if ( Application.isPlaying ) {
                set_sfx_volum ( sfx_volum );
                set_music_volum ( music_volum );

                set_sfx_mute ( sfx_mute );
                set_music_mute ( music_mute );
            }
#endif
        }


        private void OnEnable ( ) {
            if ( instance == null ) {
                instance = this;
            } else {
                if ( instance != this ) {
                    Destroy ( gameObject );
                }
            }

            if ( instance == this ) {
                DontDestroyOnLoad ( gameObject );
                confirm ( );
            }
        }


        private void OnDestroy ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }

        private void OnApplicationQuit ( ) {
            if ( instance == this ) { _app_is_close = true; }
        }
    }
}
