using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class AnimationUtilities
{
    public static string GetCurrentlyPlayingClips(Animator animator, int layer = 0)
    {
        if (!animator) return null;

        string message = "";
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfos.Length == 0) { message += "No clips were found"; }

        //iterate and generateMessage
        for (int i = 0; i < clipInfos.Length; i++)
        {
            if (i > 0) { message += ", "; }
            message += clipInfos[i].clip.name;
        }
        return message;
    }
}

