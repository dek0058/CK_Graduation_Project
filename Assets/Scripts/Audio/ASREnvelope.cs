using System;

namespace Game.Audio {
    [Serializable]
    public class ASREnvelope {

        public enum State {
            Idle,
            Attack,
            Sustain,
            Release,
        }

        public State state;
        private double attack_increment;
        private uint sustain_samples;
        private double release_increment;
        private double output_level;


        public void reset ( double attack_time, double sustain_time, double release_time, int sample_rate ) {
            state = State.Attack;
            attack_increment = attack_time > 0.0 ? 1.0 / (attack_time * sample_rate) : 1.0;
            sustain_samples = (uint)(sustain_time * sample_rate);
            release_increment = release_time > 0.0 ? 1.0 / (release_time * sample_rate) : 1.0;
            output_level = 0.0;
        }


        public double get_level ( ) {
            switch ( state ) {
                case State.Idle:
                    output_level = 0.0;
                    break;
                case State.Attack:
                    output_level += attack_increment;
                    if(output_level > 1.0) {
                        output_level = 1.0;
                        state = State.Sustain;
                    }
                    break;
                case State.Sustain:
                    if(sustain_samples == 0 || --sustain_samples == 0) {
                        state = State.Release;
                    }
                    break;
                case State.Release:
                    output_level -= release_increment;
                    if(output_level < 0.0) {
                        output_level = 0.0;
                        state = State.Idle;
                    }
                    break;
            }
            return output_level;
        }
    }
}
