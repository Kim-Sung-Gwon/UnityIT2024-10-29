using UnityEngine;
using UnityEngine.UI;

public class CrackDamage : MonoBehaviour
{
    private Image Hpbar;

    int MaxHp = 500;
    int CurHp;

    void Start()
    {
        Hpbar = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(9).GetComponent<Image>();
        CurHp = MaxHp;
        HpCur();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = col.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                int damage = Random.Range(10, 23);
                CurHp -= damage;
                HpCur();
                if (CurHp <= 0)
                    GameSet();
                col.gameObject.SetActive(false);
            }
        }
    }

    void HpCur()
    {
        CurHp = Mathf.Clamp(CurHp, 0 ,MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        Hpbar.color = Hpbar.fillAmount <= 0.7f ? Color.yellow :
                      Hpbar.fillAmount <= 0.4f ? Color.red :
                      Color.green;
    }

    public void GameSet()
    {
        GameManager.G_Manager.GameOver();
    }
}
