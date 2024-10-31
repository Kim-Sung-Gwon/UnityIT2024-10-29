using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    public static FireCtrl F_Ctrl;

    public enum State { Ready, Empty, Reload }
    public State state { get; private set; }

    private Transform firePos;
    private Text bulletText;
    private ParticleSystem muzzFalsh;
    private PlayerSound playersound;

    float LastFireTime;

    int CurBullet;
    public int maxBullet = 25;
    public int totalBullet = 100;

    bool isReload = false;

    void Start()
    {
        firePos = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).GetChild(0).
            GetChild(1).GetChild(0).GetChild(0);

        bulletText = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(3).
            GetChild(0).GetComponent<Text>();

        playersound = GetComponent<PlayerSound>();

        CurBullet = maxBullet;
        state = State.Ready;
        LastFireTime = 0;
        BulletUpdata();
    }

    private void OnEnable()
    {
        muzzFalsh = transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).
            GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        muzzFalsh.Stop();
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= LastFireTime && !isReload)
        {
            LastFireTime = Time.time;
            BulletFire();
        }
    }

    public void BulletFire()
    {
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

        #region 최적화 전 코드
        //int bulletRemain = maxBullet - CurBullet;
        //if (totalBullet <= bulletRemain)
        //{
        //    bulletRemain = totalBullet;
        //}
        //CurBullet += bulletRemain;
        //totalBullet -= bulletRemain;
        #endregion

        int bulletsToReload = Mathf.Min(maxBullet - CurBullet, totalBullet);
        CurBullet += bulletsToReload;
        totalBullet -= bulletsToReload;

        isReload = false;
        state = State.Ready;
        BulletUpdata();
    }

    public bool ReloadPlay()
    {
        if (state == State.Reload || totalBullet <= 0 || CurBullet >= maxBullet)
        {
            return false;
        }
        StartCoroutine(ReloadBullet());
        return true;
    }

    public void BulletUpdata()
    {
        bulletText.text = string.Format($"{CurBullet} / {totalBullet}");
    }
}
