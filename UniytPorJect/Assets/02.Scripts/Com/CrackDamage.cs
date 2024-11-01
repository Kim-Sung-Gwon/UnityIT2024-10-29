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
        Hpbar.color = Color.green;
        CurHp = MaxHp;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = col.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                HpCur(Random.Range(10, 23));
                col.gameObject.SetActive(false);
                if (CurHp <= 0)
                    GameManager.G_Manager.GameOver();
            }
        }
    }

    void HpCur(int damage)
    {
        CurHp = Mathf.Clamp(CurHp - damage, 0 ,MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        Hpbar.color = Hpbar.fillAmount <= 0.7f ? Color.yellow :
                      Hpbar.fillAmount <= 0.4f ? Color.red :
                      Color.green;
    }
}
