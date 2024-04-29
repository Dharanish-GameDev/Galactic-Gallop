using UnityEngine;

public abstract class P_BaseState
{
    /// <summary>
    /// Runs Only Once During Switching State from Another to it
    /// </summary>
    /// <param name="context"></param>
    public virtual void Enter(PlayerStateContext context) { }

    /// <summary>
    /// Runs Every Single Frame While this state is active
    /// </summary>
    /// <param name="context"></param>
    public virtual void Update(PlayerStateContext context) { }

    /// <summary>
    /// Runs Only Once Before Switching State from it to Another
    /// </summary>
    /// <param name="context"></param>
    public virtual void Exit(PlayerStateContext context) { }
}
