using System;

namespace JToolkit.Math {
    public class Music {
        public const int Midi_Note_C4 = 60;

        public static float midi_note_to_pitch ( int midi_note, int base_note ) {
            int semitone_offset = midi_note - base_note;
            return (float)System.Math.Pow ( 2.0, semitone_offset / 12.0 );
        }

    }
}
