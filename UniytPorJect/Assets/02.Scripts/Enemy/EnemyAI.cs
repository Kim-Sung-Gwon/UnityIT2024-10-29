using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI  E_AI;

    public enum State { ptrol, trace, attack, die };
    public State state = State.ptrol;

    private Transform EnemyTr;
    private Transform playerTr;
    private NavMeshAgent agent;
    private EnemyMove enemyMove;
    private EnemyAnimation enemyAni;

    private WaitForSeconds ws = new WaitForSeconds(0.3f);

    public float traceDist = 10.0f;
    public float attackDist = 2f;

    public bool isDie = false;

    void Start()
    {
        EnemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player")?.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAni = GetComponent<EnemyAnimation>();

        if (E_AI == null) E_AI = this;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(CheckStateAnimtion());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1f);

        while (!isDie)
        {
            float dist = Vector3.Distance(playerTr.position, EnemyTr.position);

            if (dist <= attackDist)
                state = State.attack;

            else if (dist <= traceDist)
                state = State.trace;

            else
                state = State.ptrol;

            yield return ws;
        }
    }

    IEnumerator CheckStateAnimtion()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.ptrol:
                    StartPatrol();
                    break;

                case State.trace:
                    PlayerTrace();
                    break;

                case State.attack :
                    Attacking();
                    break;

                case State.die :
                    EnemyDie();
                    break;
            }
        }
    }

    private void StartPatrol()
    {
        agent.isStopped = false;
        enemyMove.patrolling = true;
        enemyAni.Enemymove();
    }

    private void PlayerTrace()
    {
        enemyMove.traceTarget = playerTr.position;
        agent.isStopped = false;
        enemyAni.Enemymove();
    }

    void Attacking()
    {
        enemyMove.Stop();
        enemyAni.Enemyattack();
    }

    public void EnemyDie()
    {
        isDie = true;
        enemyMove.Stop();
        enemyAni.Enemydie();

        Rigidbody rb = GetComponent<Rigidbody>();
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;

        gameObject.tag = "Untagged";
        state = State.die;
        StartCoroutine(ObjectPoolPush());
    }

    IEnumerator ObjectPoolPush() 
    {
        yield return new WaitForSeconds(3.0f);

        isDie = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;

        gameObject.tag = "Enemy";
        gameObject.SetActive(false);
        state = State.ptrol;
    }
}
