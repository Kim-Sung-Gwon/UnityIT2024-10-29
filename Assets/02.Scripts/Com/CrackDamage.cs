using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrackDamage : MonoBehaviour
{
    [SerializeField] private Transform tr;
    [SerializeField] private Image Hpbar;

    int MaxHp = 500;
    int CurHp;

    void Start()
    {
        tr = GetComponent<Transform>();
        Hpbar = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(9).GetComponent<Image>();
        CurHp = MaxHp;
        Hpbar.color = Color.green;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            float damage = col.gameObject.GetComponent<Bullet>().Damage;
            col.gameObject.SetActive(false);
            CurHp -= (int)damage;
            HpCur();
            if (CurHp <= 0)
                GameSet();
        }
    }

    void HpCur()
    {
        CurHp = Mathf.Clamp(CurHp, 0 ,MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        if (Hpbar.fillAmount <= 0.7f)
            Hpbar.color = Color.yellow;
        if (Hpbar.fillAmount <= 0.4f)
            Hpbar.color = Color.red;
    }

    public void GameSet()
    {
        GameManager.G_Manager.GameOver();
    }
}
