using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField]
    private AudioSourcesList audioSourcesList;

    [Header("AudioClips")]
    [SerializeField] private AudioClipsSO audioClips;

    public AudioClipsSO AudioClips => audioClips;
    public AudioSourcesList AudioSourcesList => audioSourcesList;

    private void Awake()
    {
        instance = this;

    }
    public void PlaySfx(AudioClip clip,AudioSource audioSource, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
    public void PlaySfxOneTime(AudioClip clip,AudioSource audioSource,float volume)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
    public void FadeGameBgMusic()
    {
        audioSourcesList.gameBgSource.volume = 0.035f;
    }

}
[System.Serializable]
public class AudioSourcesList
{
    public AudioSource collectingSfxSource;
    public AudioSource meteorBerakSfxSource;
    public AudioSource gameBgSource;
    public AudioSource powerUpAudioSource;
}
