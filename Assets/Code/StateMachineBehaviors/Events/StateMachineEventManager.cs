using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class fires off events defined in the StateMachineEvent class
/// </summary>
public class StateMachineEventManager : StateMachineBehaviorCore
{
  private bool haseBeenInitialized = false;
  public List<StateEvent> Events = new List<StateEvent>();

  public void OnEnable()
  {
    
  }

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(!haseBeenInitialized)
    {
      for (int i = 0; i < Events.Count; i++)
      {
        Events[i].Initialize(actor, animator);
      }
    }
    base.OnStateEnter(animator, stateInfo, layerIndex);
  }

  public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    for (int i = 0; i < Events.Count; i++)
    {
      if (!Events[i].hasBeenFired && stateInfo.normalizedTime >= Events[i].eventTime)
      {
        Events[i].Fire();
      }
    }
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Reset();
  }

  public virtual void Reset()
  {
    for (int i = 0; i < Events.Count; i++)
    {
      Events[i].Reset();
    }
  }
}
