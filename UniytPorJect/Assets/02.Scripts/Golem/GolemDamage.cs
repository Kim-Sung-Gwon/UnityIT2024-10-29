using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GolemDamage : MonoBehaviour
{
    private Transform tr;
    private BoxCollider boxCol;
    private Image GolemHpBar;
    private GameObject BloodEffect;
    private Text damageText;

    int MaxHp = 120;
    int CurHp = 0;

    private void Start()
    {
        boxCol = tr.GetChild(3).GetComponent<BoxCollider>();
        BloodEffect = Resources.Load<GameObject>("Effects/BloodEffect");
    }

    void OnEnable()
    {
        tr = GetComponent<Transform>();
        GolemHpBar = tr.Find("GolemCanvas").transform.GetChild(1).GetComponent<Image>();
        damageText = tr.Find("GolemCanvas").transform.GetChild(2).GetComponent<Text>();
        damageText.gameObject.SetActive(false);
        GolemHpBar.color = Color.green;
        CurHp = MaxHp;
        if (GolemHpBar.fillAmount == 0)
            GolemHpBar.fillAmount = 1;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            col.gameObject.SetActive(false);
            ShowBlood(col);

            int _damage = Random.Range(10, 25);
            StartCoroutine(OndamagetText(_damage));
            EnemyDamage(_damage);
        }
    }

    void EnemyDamage(int damage)
    {
        EnemyCurHp(damage);

        if (CurHp <= 0)
            Die();
    }

    private void EnemyCurHp(int damage)
    {
        CurHp = Mathf.Clamp(CurHp - damage, 0, MaxHp);
        GolemHpBar.fillAmount = (float)CurHp / (float)MaxHp;
        GolemHpBar.color = GolemHpBar.fillAmount <= 0.7f ? Color.yellow :
                           GolemHpBar.fillAmount <= 0.4f ? Color.red :
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

    public void BoxColEnable()
    {
        boxCol.enabled = true;
    }

    public void BoxColDiavie()
    {
        boxCol.enabled = false;
    }
}
