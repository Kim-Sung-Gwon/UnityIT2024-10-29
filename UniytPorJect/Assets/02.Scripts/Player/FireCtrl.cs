using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    public static FireCtrl F_Ctrl;

    public enum State { Ready, Empty, Reload }
    public State state { get; private set; }

    private Transform firePos;
    //private GameObject Bullet;
    private Text bulletText;
    private ParticleSystem muzzFalsh;
    private PlayerSound playersound;

    float LastFireTime;

    int CurBullet;
    public int NowBullet = 25;
    public int MaxBullet = 100;

    bool isReload = false;

    void Start()
    {
        //Bullet = Resources.Load("Bullet").GetComponent<GameObject>();
        firePos = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).GetChild(0).
            GetChild(1).GetChild(0).GetChild(0);

        bulletText = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(3).
            GetChild(0).GetComponent<Text>();

        playersound = GetComponent<PlayerSound>();

        CurBullet = NowBullet;
        state = State.Ready;
        LastFireTime = 0;
    }

    private void OnEnable()
    {
        muzzFalsh = transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).
            GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        muzzFalsh.Stop();
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= LastFireTime)
        {
            LastFireTime = Time.time;
            BulletFire();
        }
    }

    public void BulletFire()
    {
        if (isReload)
        {
            return;
        }
        var bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        if (bullet != null && !bullet.activeInHierarchy)
        {
            bullet.transform.position = firePos.position;
            bullet.transform.rotation = firePos.rotation;
            bullet.SetActive(true);
            muzzFalsh.Play();
            playersound.FireSound();
            Invoke("MuzzFalshFire", 0.1f);
        }
        --CurBullet;
        BulletUpdata();
        if (CurBullet <= 0)
        {
            state = State.Empty;
        }
    }

    public void MuzzFalshFire()
    {
        muzzFalsh.Stop();
    }

    IEnumerator ReloadBullet()
    {
        state = State.Reload;
        isReload = true;
        playersound.ReloadSound();
        GetComponent<PlayerAnimator>().ReloadAnimation();
        yield return new WaitForSeconds(playersound.reloadClip.length + 0.3f);
        int bulletRemain = NowBullet - CurBullet;
        if (MaxBullet <= bulletRemain)
        {
            bulletRemain = MaxBullet;
        }
        CurBullet += bulletRemain;
        MaxBullet -= bulletRemain;
        isReload = false;
        state = State.Ready;
        BulletUpdata();
    }

    public bool ReloadPlay()
    {
        if (state == State.Reload || MaxBullet <= 0 || CurBullet >= NowBullet)
        {
            return false;
        }
        StartCoroutine(ReloadBullet());
        return true;
    }

    public void BulletUpdata()
    {
        bulletText.text = string.Format($"{CurBullet} / {MaxBullet}");
    }
}
