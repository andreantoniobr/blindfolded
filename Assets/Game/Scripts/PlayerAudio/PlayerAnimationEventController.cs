using JimmysUnityUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAnimationEventController : MonoBehaviour
{ 
    [SerializeField] private List<AudioClip> jumps;

    private List<AudioClip> currentList;

    private AudioSource audioSource;

    public static event Action<Vector3> PlayStepEvent;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();        
    }

    public void PlayStep()
    {
        PlayStepEvent?.Invoke(transform.position);
    }

    public void PlayJump()
    {
        if (jumps != null && jumps.Count > 0)
        {
            AudioClip audioClip = jumps[UnityEngine.Random.Range(0, jumps.Count)];
            audioSource.PlayOneShot(audioClip);
        }
    }
}
