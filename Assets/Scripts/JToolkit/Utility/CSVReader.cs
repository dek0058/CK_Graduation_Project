using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JToolkit.Utility {
    public class CSVReader {

        private static string _splite_return = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""";
        private static string _line_splite_return = @"\r\n|\n\r|\n|\r";
        private static char[] _trim_chars = { '\"' };

        public static List<Dictionary<string, object>> read ( string file ) {
            var list = new List<Dictionary<string, object>> ( );
            TextAsset data = Resources.Load<TextAsset> ( file );

            var lines = Regex.Split ( data.text, _line_splite_return );

            if ( lines.Length <= 1 ) return list;

            var header = Regex.Split ( lines[0], _splite_return );
            for(var i = 1; i < lines.Length; ++i) {
                var values = Regex.Split ( lines[i], _splite_return );

                if ( values.Length == 0 || values[0] == "" ) {
                    continue;
                }

                var entry = new Dictionary<string, object> ( );
                for(var j = 0; j < header.Length; i++ ) {
                    string value = values[j];
                    value = value.TrimStart ( _trim_chars ).TrimEnd ( _trim_chars ).Replace ( "\\", "" );
                    value = value.Replace ( "<br>", "\n" );
                    value = value.Replace ( "<c>", "," );
                    entry[header[j]] = value as object;
                }
                list.Add ( entry );
            }
            return list;
        }
    }
}
