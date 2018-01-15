using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class StateMachineEventManager : StateMachineBehaviour
{
    public List<StateMachineEvent> Events = new List<StateMachineEvent>();

    public void OnEnable()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
