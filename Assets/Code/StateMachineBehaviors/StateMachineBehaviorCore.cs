using UnityEngine;
using System.Collections;

/// <summary>
/// This class is for general variables that we are going to want in all StateMachineBehaviors
/// </summary>
public class StateMachineBehaviorCore : StateMachineBehaviour
{
  private bool hasInitialized = false;
  public Actor actor;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(!hasInitialized)
    {
      actor = animator.GetComponent<Actor>();
      hasInitialized = true;
    }
    base.OnStateEnter(animator, stateInfo, layerIndex);
  }
}
