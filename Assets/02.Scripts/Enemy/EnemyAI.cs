using System.Collections;
using System.Collections.Generic;
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

    private WaitForSeconds ws;

    public float traceDist = 10.0f;
    public float attackDist = 2f;
    public float LastAttackTime;

    public bool isDie = false;
    public bool isAttack = false;

    void Start()
    {
        EnemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (playerTr != null)
            playerTr = playerTr.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAni = GetComponent<EnemyAnimation>();
        ws = new WaitForSeconds(0.3f);
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
            if (state == State.die) yield break;

            float dist = (playerTr.position - EnemyTr.position).magnitude;

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
                    agent.isStopped = false;
                    enemyMove.patrolling = true;
                    enemyAni.Enemymove();
                    isAttack = false;
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

    private void PlayerTrace()
    {
        enemyMove.traceTarget = playerTr.position;
        agent.isStopped = false;
        isAttack = false;
        enemyAni.Enemymove();
    }

    void Attacking()
    {
        isAttack = true;
        enemyMove.Stop();
        enemyAni.Enemyattack();
        isAttack = false;
    }

    public void EnemyDie()
    {
        isDie = true;
        isAttack = false;
        enemyMove.Stop();
        enemyAni.Enemydie();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";
        state = State.die;
        StartCoroutine(ObjectPoolPush());
    }

    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3.0f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        gameObject.tag = "Enemy";
        gameObject.SetActive(false);
        state = State.ptrol;
    }
}
