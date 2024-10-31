using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPack : MonoBehaviour, IIitem
{
    public int bullet = 25;

    private void OnEnable()
    {
        Invoke("BulletPakcDisable", 4);
    }

    private void BulletPakcDisable()
    {
        this.gameObject.SetActive(false);
    }

    // 인터 페이스
    public void Use(GameObject target)
    {
        FireCtrl fireCtrl = target.GetComponent<FireCtrl>();
        if (fireCtrl != null)
        {
            fireCtrl.MaxBullet += bullet;
        }
        this.gameObject.SetActive(false);
    }
}
