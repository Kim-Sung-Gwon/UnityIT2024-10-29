using System.Collections;
using System.Collections.Generic;
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
        CurHp = MaxHp;
        Hpbar.color = Color.green;
        if (Hpbar.fillAmount == 0)
            Hpbar.fillAmount = 1;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            ShowBlood(col);
            float damage = col.gameObject.GetComponent<Bullet>().Damage;
            damage = Random.Range(15, 30);
            StartCoroutine(OndamagetText(damage));
            col.gameObject.SetActive(false);
            CurHp -= (int)damage;
            NecromHpBar();
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
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _normal);
        GameObject blood = Instantiate(BloodEffect, pos, rot);
        Destroy(blood, 0.5f);
    }

    private void NecromHpBar()
    {
        CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        Hpbar.fillAmount = (float)CurHp / (float)MaxHp;
        if (Hpbar.fillAmount <= 0.7f)
            Hpbar.color = Color.yellow;
        if (Hpbar.fillAmount <= 0.4f)
            Hpbar.color = Color.red;
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
