using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource source;
    private AudioClip fireClip;
    public AudioClip reloadClip;
    private AudioClip dieClip;
    private AudioClip hitClip;

    void Start()
    {
        source = GetComponent<AudioSource>();
        fireClip = Resources.Load<AudioClip>("Sound/AutoGunFire");
        reloadClip = Resources.Load<AudioClip>("Sound/Roload");
        dieClip = Resources.Load<AudioClip>("Sound/DieSound");
        hitClip = Resources.Load<AudioClip>("Sound/HitSound");
    }

    public void FireSound()
    {
        source.PlayOneShot(fireClip, 1);
    }

    public void ReloadSound()
    {
        SoundManager.S_Manager.PlaySound(transform.position, reloadClip);
    }

    public void DieSound()
    {
        SoundManager.S_Manager.PlaySound(transform.position, dieClip);
    }

    public void HitSound()
    {
        SoundManager.S_Manager.PlaySound(transform.position, hitClip);
    }
}
