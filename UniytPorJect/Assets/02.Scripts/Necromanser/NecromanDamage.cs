using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NecromanDamage : MonoBehaviour
{
    private Transform Tr;
    private Image Hpbar;
    private BoxCollider BoxCol;
    private GameObject BloodEffect;
    private Text damageText;

    int MaxHp = 100;
    int CurHp;

    void Start()
    {
        BoxCol = transform.GetChild(8).GetComponent<BoxCollider>();
        BloodEffect = Resources.Load<GameObject>("Effects/BloodEffect");
    }

    private void OnEnable()
    {
        Tr = GetComponent<Transform>();
        Hpbar = Tr.Find("NecremCanvas").transform.GetChild(1).GetComponent<Image>();
        damageText = Tr.Find("NecremCanvas").transform.GetChild(2).GetComponent<Text>();
        ResetHealth();
    }

    private void ResetHealth()
    {
        damageText.gameObject.SetActive(false);
        CurHp = MaxHp;
        Hpbar.fillAmount = 1;
        Hpbar.color = Color.green;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            col.gameObject.SetActive(false);
            ShowBlood(col);

            int _damage = Random.Range(15, 30);
            StartCoroutine(OndamagetText(_damage));
            EnemyDamage(_damage);
        }
    }

    void EnemyDamage(int damage)
    {
        CurHpbar(damage);

        if (CurHp <= 0)
            Die();
    }

    private void CurHpbar(int damage)
    {
        CurHp = Mathf.Clamp(CurHp - damage, 0, MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        Hpbar.color = Hpbar.fillAmount <= 0.7f ? Color.yellow :
                      Hpbar.fillAmount <= 0.4f ? Color.red :
                      Color.green;
    }

    IEnumerator OndamagetText(int damage)
    {
        damageText.gameObject.SetActive(true);
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.5f);
        damageText.gameObject.SetActive(false);
    }

    private void ShowBlood(Collision col)
    {
        Vector3 pos = col.contacts[0].point;
        Quaternion rot = Quaternion.LookRotation(col.contacts[0].normal);
        GameObject blood = Instantiate(BloodEffect, pos, rot);
        Destroy(blood, 0.5f);
    }

    void Die()
    {
        GetComponent<EnemyAI>().state = EnemyAI.State.die;
        GameManager.G_Manager.KillScore();
    }

    public void OnBoxColEnable()
    {
        BoxCol.enabled = true;
    }

    public void OnBoxColDiavie()
    {
        BoxCol.enabled = false;
    }
}
