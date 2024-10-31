using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private readonly int move = Animator.StringToHash("MoveBool");
    private readonly int attack = Animator.StringToHash("AttackTrigger");
    private readonly int die = Animator.StringToHash("DieTrigger");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Enemymove()
    {
        animator.SetBool(move, true);
    }

    public void Enemyattack()
    {
        animator.SetTrigger(attack);
    }

    public void Enemydie()
    {
        animator.SetTrigger(die);
    }
}
