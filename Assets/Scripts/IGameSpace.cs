

namespace Game {
    interface IGameSpace {
        GameSpace game_space {
            get; set;
        }
    }

    /// <summary>
    /// 어느 게임 공간에 관여하는지 확인하는 열거형
    /// </summary>
    public enum GameSpace {
        Origin = 0, // 원본만
        Purgatory,  // 연옥만
        Both,       // 양쪽 모두
    }
}
