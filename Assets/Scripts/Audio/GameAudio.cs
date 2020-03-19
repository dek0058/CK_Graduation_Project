using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

namespace Game.Audio {
    using Management;

    public class GameAudio : MonoBehaviour {

        [HideInInspector]
        public AudioSource source;

        public AudioMixerGroup mixer_group {
            get => source.outputAudioMixerGroup; set => source.outputAudioMixerGroup = value;
        }


        private readonly ASREnvelope envelope = new ASREnvelope ( );

        public ASREnvelope.State state {
            get => envelope.state;
        }

        private uint until_envelope_trigger;


        public void play ( double start_time, double attack_time, double release_time ) {
            int sample_rate = AudioSettings.outputSampleRate;
            float total = (float)(attack_time + release_time);
            double sustain_time = source.clip.length - total > 0 ? source.clip.length - total : 0f;

            sustain_time = sustain_time > attack_time ? sustain_time - attack_time : 0.0;
            envelope.reset ( attack_time, sustain_time, release_time, sample_rate );

            double time_util_trigger = start_time > AudioSettings.dspTime ? start_time - AudioSettings.dspTime : 0.0;
            until_envelope_trigger = (uint)(time_util_trigger * sample_rate);

            source.PlayScheduled ( start_time );
        }


        public void play ( AudioClip clip, float pitch, double start_time, double attack_time, double release_time, bool destroy = false ) {
            int sample_rate = AudioSettings.outputSampleRate;
            float total = (float)(attack_time + release_time);
            double sustain_time = clip.length - total > 0 ? clip.length - total : 0f;

            sustain_time = sustain_time > attack_time ? sustain_time - attack_time : 0.0;
            envelope.reset ( attack_time, sustain_time, release_time, sample_rate );

            double time_util_trigger = start_time > AudioSettings.dspTime ? start_time - AudioSettings.dspTime : 0.0;
            until_envelope_trigger = (uint)(time_util_trigger * sample_rate);

            source.clip = clip;
            source.pitch = pitch;

            source.PlayScheduled ( start_time );
            if(destroy) {
                StartCoroutine ( destroy_object ( ) );
            }
        }


        public void confirm ( ) {
            if(source == null) {
                source = GetComponent<AudioSource> ( );
            }

            if( mixer_group == null) {
                mixer_group = SoundManager.instance.sfx_group;
            }
        }


        private IEnumerator destroy_object ( ) {
            yield return new WaitUntil ( ( ) => envelope.state == ASREnvelope.State.Idle );
            Destroy ( gameObject );
        }

        ////////////////////////////////////////////////////////////////////////////
        ///                               Unity                                  ///
        ////////////////////////////////////////////////////////////////////////////

        private void Awake ( ) {
            confirm ( );
        }
        
        private void OnAudioFilterRead ( float[] data, int channels ) {

            for(int i = 0; i < data.Length; i += channels ) {
                double volum = 0;
                
                if(until_envelope_trigger == 0) {
                    volum = envelope.get_level ( );
                } else {
                    --until_envelope_trigger;
                }

                for(int j = 0; j < channels; ++j ) {
                    data[i + j] *= (float)volum;
                }
            }
        }

        private void OnApplicationFocus ( bool focus ) {
            if ( focus ) {
                source.UnPause ( );
            } else {
                source.Pause ( );
            }
        }
    }
}
