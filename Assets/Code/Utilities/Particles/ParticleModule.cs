using UnityEngine;
using System.Collections;

public class ParticleModule
{
    public ParticleUtilities particleUtility;

    // Use this for initialization
    public virtual void Start(ParticleUtilities utility)
    {
        particleUtility = utility;
    }
}
