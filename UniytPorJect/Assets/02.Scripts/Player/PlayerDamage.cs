using System.Collections;
using System.Collections.Generic;
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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerSound = GetComponent<PlayerSound>();
        CurHp = MaxHp;
        HpImage = GameObject.Find("UI_Canvas").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        HpImage.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageZone"))
        {
            playerSound.HitSound();
            CurHp -= 5;
            CurHpImage();
            if (CurHp <= 0)
                PlayerDie();
        }

        if (other.gameObject.CompareTag("NecromDamage"))
        {
            playerSound.HitSound();
            CurHp -= 15;
            CurHpImage();
            if (CurHp <= 0)
                PlayerDie();
        }
    }

    public void CurHpImage()
    {
        CurHp = Mathf.Clamp(CurHp, 0, MaxHp);
        HpImage.fillAmount = (float)CurHp / (float)MaxHp;
        if (HpImage.fillAmount <= 0.7f)
            HpImage.color = Color.yellow;
        if (HpImage.fillAmount <= 0.4f)
            HpImage.color = Color.red;
    }

    public void PlayerDie()
    {
        cc.enabled = false;
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
