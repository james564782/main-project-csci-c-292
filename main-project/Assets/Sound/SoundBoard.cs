using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : MonoBehaviour
{
    public static SoundBoard instance;
    private AudioSource[] audioSource; //0 for music
    [SerializeField] AudioClip[] uiSelectionSounds;
    [SerializeField] AudioClip[] music;
    int selectedMusic = 0;

    public void Awake() {
        instance = this;
        audioSource = GetComponentsInChildren<AudioSource>();
        if (audioSource.Length < 1) {
            Debug.Log("Something do be wrong with the sound");
            Debug.Break();
        }
        audioSource[0].clip = music[0];   
        audioSource[0].Play();
    }

    public void PlayAudio(int type, int selection) {

    }
    public void PlayAudio(AudioClip clip) {
        audioSource[6].PlayOneShot(clip);
    }

    public void PlayAudio(AudioClip clip, float volumeScale) {
        audioSource[6].PlayOneShot(clip, volumeScale);
    }

}
