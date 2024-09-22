using JimmysUnityUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private TerrainTextureDetector terrainTextureDetector;
    [SerializeField] private List<AudioClip> dirtSteps;
    [SerializeField] private List<AudioClip> grassSteps;
    [SerializeField] private List<AudioClip> stoneSteps;
    [SerializeField] private List<AudioClip> waterSteps;
    [SerializeField] private List<AudioClip> jumps;

    private List<AudioClip> currentList;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerAnimationEventController.PlayStepEvent += PlayStep;
    }

    private void OnDestroy()
    {
        PlayerAnimationEventController.PlayStepEvent -= PlayStep;
    }

    private bool TryGetPlayerTerrainType(Vector3 playerPosition, out int terrainTextureIndex)
    {
        bool canGetTerrainTextureIndex = false;
        terrainTextureIndex = -1;
        if (terrainTextureDetector)
        {
            canGetTerrainTextureIndex = true;
            terrainTextureIndex = terrainTextureDetector.GetDominantTextureIndexAt(playerPosition);
        }
        return canGetTerrainTextureIndex;
    }

    private List<AudioClip> GetAudioList(int terrainTextureIndex)
    {
        List<AudioClip> audioList = null;
        switch (terrainTextureIndex)
        {
            case 0:
                audioList = dirtSteps;
                break;
            case 1:
                audioList = grassSteps;
                break;
            case 2:
                audioList = stoneSteps;
                break;
        }
        return audioList;
    }

    private void PlayStep(Vector3 playerPosition)
    {
        if (TryGetPlayerTerrainType(playerPosition, out int terrainTextureIndex))
        {
            currentList = GetAudioList(terrainTextureIndex);
            if (currentList != null && currentList.Count > 0)
            {
                AudioClip audioClip = currentList[Random.Range(0, currentList.Count)];
                audioSource.PlayOneShot(audioClip);
            }
        }        
    }

    private void PlayJump()
    {
        if (jumps != null && jumps.Count > 0)
        {
            AudioClip audioClip = jumps[Random.Range(0, jumps.Count)];
            audioSource.PlayOneShot(audioClip);
        }
    }
}
