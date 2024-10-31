using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private readonly int PosX = Animator.StringToHash("PosX");
    private readonly int PosY = Animator.StringToHash("PosY");
    private readonly int Run = Animator.StringToHash("RunBool");
    private readonly int Jump = Animator.StringToHash("JumpTrigger");
    private readonly int Fire = Animator.StringToHash("FireTrigger");
    private readonly int Reload = Animator.StringToHash("ReloadTrigger");
    private readonly int Die = Animator.StringToHash("DieTrigger");

    public bool isJump;

    private bool _IsRun;
    public bool IsRun
    {
        get => _IsRun;
        set
        {
            _IsRun = value;
            animator.SetBool(Run, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveAnimation(Vector2 moveInput)
    {
        animator.SetFloat(PosX, moveInput.x);
        animator.SetFloat(PosY, moveInput.y);
    }

    public void JumpAnimation()
    {
        animator.SetTrigger(Jump);
    }

    public void FireAnimation()
    {
        animator.SetLayerWeight(1, 1f);
        animator.SetTrigger(Fire);
    }

    public void ReloadAnimation()
    {
        animator.SetLayerWeight(2, 1f);
        animator.SetTrigger(Reload);
    }

    public void RunAnimation(bool running)
    {
        IsRun = running;
    }

    public void DieAnimation()
    {
        animator.SetTrigger(Die);
    }
}
