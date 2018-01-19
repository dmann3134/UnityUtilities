using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioPooledObject : PooledObject
{
  public AudioSource source;
  public Actor pairedActor;
  public PriorityData priority;
  public float volume = .05f;

  public void Play(Transform parent = null)
  {
    transform.parent = parent;
    transform.localPosition = new Vector3(0, 0, 0);
    OnStartClip();
  }

  public void OnFinishedClip()
  {
    source.Stop();
    ReturnToPool();
  }

  public void OnStartClip(float vol = -1)
  {
    source.volume = volume;
    if (vol > 0)
    {
      source.volume *= vol;
    }
    source.gameObject.SetActive(true);
    source.Play();
    StartCoroutine(CleanupClipAudio(source.clip.length));
  }

  public IEnumerator CleanupClipAudio(float clipLength)
  {
    yield return new WaitForSeconds(clipLength);
    OnFinishedClip();
  }

  // will probably need to add more audiocontroller shit in here
  [System.Serializable]
  public class PriorityData
  {
    public string PriorityOneMixerName;
    public string PriorityTwoMixerName;
    public string PriorityThreeMixerName;
  }
}