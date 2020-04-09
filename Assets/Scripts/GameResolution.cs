using UnityEngine;

namespace Game {
    using JToolkit.Utility;

    public class GameResolution {

        readonly private EnumDictionary<GameResolutionType, Vector2> resolution_data = new EnumDictionary<GameResolutionType, Vector2> ( ) {
            { GameResolutionType.Res1920x1080, new Vector2(1920, 1080) },
            { GameResolutionType.Res1680x1050, new Vector2(1680, 1050) },
            { GameResolutionType.Res1600x1024, new Vector2(1600, 1024) },
            { GameResolutionType.Res1440x900, new Vector2(1440, 900) },
            { GameResolutionType.Res1366x768, new Vector2(1366, 768) },
            { GameResolutionType.Res1360x768, new Vector2(1360, 768) },
            { GameResolutionType.Res1280x1024, new Vector2(1280, 1024) },
            { GameResolutionType.Res1280x960, new Vector2(1280, 960) },
            { GameResolutionType.Res1280x800, new Vector2(1280, 800) },
            { GameResolutionType.Res1280x768, new Vector2(1280, 768) },
            { GameResolutionType.Res1280x720, new Vector2(1280, 720) },
            { GameResolutionType.Res1152x864, new Vector2(1152, 864) },
            { GameResolutionType.Res1024x768, new Vector2(1024, 768) },
        };

        private bool is_fullscreen;
        public GameResolutionType type;

        public void set ( GameResolutionType type ) {
            this.type = type;
            Vector2 size = resolution_data[this.type];
            Screen.SetResolution ( (int)size.x, (int)size.y, is_fullscreen );
        }

        public void fullscreen ( bool value ) {
            is_fullscreen = value;
            set ( type );
        }

        
    }


    public enum GameResolutionType {
        Res1920x1080,
        Res1680x1050,
        Res1600x1024,
        Res1440x900,
        Res1366x768,
        Res1360x768,
        Res1280x1024,
        Res1280x960,
        Res1280x800,
        Res1280x768,
        Res1280x720,
        Res1152x864,
        Res1024x768
    }
}
