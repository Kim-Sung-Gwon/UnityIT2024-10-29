using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemDamage : MonoBehaviour
{
    private Transform tr;
    private BoxCollider boxCol;
    private Image GolemHpBar;
    private GameObject BloodEffect;
    private Text damageText;

    public int MaxHp = 120;
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
            ShowBlood(col);
            float _damage = col.gameObject.GetComponent<Bullet>().Damage;
            _damage = Random.Range(10, 25);
            StartCoroutine(OndamagetText(_damage));
            col.gameObject.SetActive(false);
            CurHp -= (int)_damage;
            EnemyCurHp();
            if (CurHp <= 0)
                Die();
        }
    }

    IEnumerator OndamagetText(float damage)
    {
        damageText.gameObject.SetActive(true);
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.5f);
        damageText.gameObject.SetActive(false);
    }

    private void ShowBlood(Collision col)
    {
        Vector3 pos = col.contacts[0].point;
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate(BloodEffect, pos, rot);
        Destroy(blood, 0.5f);
    }

    public void EnemyCurHp()
    {
        CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        GolemHpBar.fillAmount = (float)CurHp / (float)MaxHp;
        if (GolemHpBar.fillAmount <= 0.7f)
            GolemHpBar.color = Color.yellow;
        if (GolemHpBar.fillAmount <= 0.4f)
            GolemHpBar.color = Color.red;
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
