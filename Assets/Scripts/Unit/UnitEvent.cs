namespace Game.Unit {
    /// <summary>
    /// 소생하였을 때 이벤트
    /// </summary>
    /// <param name="unit">살아난 유닛</param>
    public delegate void on_revive ( UUnit unit );

    /// <summary>
    /// 죽었을 때 이벤트
    /// </summary>
    /// <param name="unit">죽은 유닛</param>
    public delegate void on_dead ( UUnit unit );

    /// <summary>
    /// 유닛이 공격을 시작했을 때 이벤트
    /// </summary>
    /// <param name="source">공격한 유닛</param>
    public delegate void on_attack ( UUnit source );

    /// <summary>
    /// 유닛이 피해를 받는 중일 때 이벤트
    /// </summary>
    /// <param name="source">피해를 입힌 유닛</param>
    /// <param name="target">피해를 받는 유닛</param>
    public delegate void on_damaged ( UUnit source, UUnit target );
}