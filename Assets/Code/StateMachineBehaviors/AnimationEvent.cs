using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class AnimationEvent : StateMachineBehaviour
{
    public List<TimeEvent> Events = new List<TimeEvent>();

    [Serializable]
    public class TimeEvent
    {
        public string Name;
        [HideInInspector]
        public bool hasBeenFired = false;
        [Range(0, 1)]
        public float eventTime;

        public virtual void Reset()
        {

        }

        public virtual void Fire()
        {
            hasBeenFired = true;
        }
    }

    public void OnEnable()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int j = 0; j < Events.Count; j++)
        {
            if (!Events[j].hasBeenFired && stateInfo.normalizedTime >= Events[j].eventTime)
            {
                Events[j].Fire();
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Reset();
    }

    public virtual void Reset()
    {

    }
}
