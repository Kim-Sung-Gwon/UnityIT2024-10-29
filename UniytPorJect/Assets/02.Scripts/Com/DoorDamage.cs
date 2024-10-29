using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorDamage : MonoBehaviour
{
    private Image Hpbar;

    public int CurHp;
    public int MaxHp = 200;

    void Start()
    {
        Hpbar = GameObject.Find("Panel_HpAndBullet").transform.GetChild(6).GetComponent<Image>();
        CurHp = MaxHp;
        Hpbar.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            CurHp -= 10;
            DoorCurHpBar();
            if (CurHp <= 0)
                GameManager.G_Manager.GameOver();
        }

        if (other.gameObject.CompareTag("Necroman"))
        {
            other.gameObject.SetActive(false);
            CurHp -= 15;
            DoorCurHpBar();
            if (CurHp <= 0)
                GameManager.G_Manager.GameOver();
        }
    }

    void DoorCurHpBar()
    {
        CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        if (Hpbar.fillAmount <= 0.7f)
            Hpbar.color = Color.yellow;
        if (Hpbar.fillAmount <= 0.4f)
            Hpbar.color = Color.red;
    }
}
