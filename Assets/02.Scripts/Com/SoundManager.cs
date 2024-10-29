using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S_Manager;
    public float SoundVolumn = 1.0f;
    public bool isSoundMute = false;

    void Awake()
    {
        if (S_Manager == null) S_Manager = this;
        else if (S_Manager != this) Destroy(gameObject);
    }

    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        if (isSoundMute) return;
        GameObject soundObj = new GameObject("Sound!");
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 20f;
        audioSource.volume = SoundVolumn;
        audioSource.Play();
        Destroy(soundObj, clip.length);
    }
}
