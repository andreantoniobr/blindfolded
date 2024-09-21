using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioStepsController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> grassSteps;
    [SerializeField] private List<AudioClip> stoneSteps;
    [SerializeField] private List<AudioClip> waterSteps;

    private List<AudioClip> currentList;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentList = grassSteps;
    }

    public void PlayStep()
    {
        if (currentList != null && currentList.Count > 0)
        {
            AudioClip audioClip = currentList[Random.Range(0, currentList.Count)];
            audioSource.PlayOneShot(audioClip);
        }        
    }
}
