using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    private Image HpImage;
    private GameManager gameManager;
    private PlayerAnimator playerAnimator;
    private PlayerSound playerSound;
    private CharacterController cc;

    public int MaxHp = 100;
    public int CurHp = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        playerSound = GetComponent<PlayerSound>();

        CurHp = MaxHp;
        HpImage = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        CurHpImage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageZone"))
        {
            OnPlayerDamage(Random.Range(5, 13));
        }

        if (other.gameObject.CompareTag("NecromDamage"))
        {
            OnPlayerDamage(Random.Range(15, 24));
        }
    }

    void OnPlayerDamage(int damage)
    {
        playerSound.HitSound();
        CurHp -= damage;
        CurHpImage();
        if (CurHp <= 0)
            PlayerDie();
    }

    public void CurHpImage()
    {
        CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        HpImage.fillAmount = (float)CurHp / (float)MaxHp;
        if (HpImage.fillAmount <= 0.7f)
            HpImage.color = Color.yellow;
        else if (HpImage.fillAmount <= 0.4f)
            HpImage.color = Color.red;
        else
            HpImage.color = Color.green;
    }

    public void PlayerDie()
    {
        cc.enabled = false;
        gameObject.tag = "Untagged";
        playerSound.DieSound();
        playerAnimator.DieAnimation();
        StartCoroutine(LodGameOverUi());
    }

    IEnumerator LodGameOverUi()
    {
        yield return new WaitForSeconds(3.0f);
        gameManager.GameOver();
    }
}
