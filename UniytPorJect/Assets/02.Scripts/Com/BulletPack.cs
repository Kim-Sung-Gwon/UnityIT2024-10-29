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

    // ���� ���̽�
    public void Use(GameObject target)
    {
        if (target.TryGetComponent<FireCtrl>(out FireCtrl fireCtrl))
        {
            fireCtrl.totalBullet += bullet;
            gameObject.SetActive(false);
        }

        #region ����ȭ �۾��� �ڵ�
        //FireCtrl fireCtrl = target.GetComponent<FireCtrl>();
        //if (fireCtrl != null)
        //{
        //    fireCtrl.totalBullet += bullet;
        //    this.gameObject.SetActive(false);
        //}
        #endregion
    }
}
