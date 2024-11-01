using System.Collections;
using UnityEngine;

public class BulletPack : MonoBehaviour, IIitem
{
    private int bullet = 25;

    private void OnEnable()
    {
        StartCoroutine(BulletPakcDisable(4f));
    }

    IEnumerator BulletPakcDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    // 인터 페이스
    public void Use(GameObject target)
    {
        if (target.TryGetComponent<FireCtrl>(out FireCtrl fireCtrl))
        {
            fireCtrl.totalBullet += bullet;
            gameObject.SetActive(false);
        }

        #region 최적화 작업전 코드
        //FireCtrl fireCtrl = target.GetComponent<FireCtrl>();
        //if (fireCtrl != null)
        //{
        //    fireCtrl.totalBullet += bullet;
        //    this.gameObject.SetActive(false);
        //}
        #endregion
    }
}
