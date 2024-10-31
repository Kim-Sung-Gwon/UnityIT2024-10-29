using System.Collections;
using UnityEngine;

public class BulletPack : MonoBehaviour, IIitem
{
    public int bullet = 25;

    private void OnEnable()
    {
        StartCoroutine(BulletPakcDisable(4f));
    }

    IEnumerator BulletPakcDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }

    // 인터 페이스
    public void Use(GameObject target)
    {
        FireCtrl fireCtrl = target.GetComponent<FireCtrl>();
        if (fireCtrl != null)
        {
            fireCtrl.totalBullet += bullet;
            this.gameObject.SetActive(false);
        }
    }
}
