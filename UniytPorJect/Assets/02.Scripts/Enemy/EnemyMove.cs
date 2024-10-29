using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private EnemyAI EnemyAI;
    private NavMeshAgent agent;
    private Transform enemyTr;
    private Transform PlayerTr;

    public List<Transform> wayPoint;

    float damping = 1.0f;

    public int Idx = 0;

    private bool _patrolling;
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            damping = 1.0f;
            if (_patrolling)
                MovePoint();
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    void Start()
    {
        EnemyAI = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        enemyTr = GetComponent<Transform>();
        PlayerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent.autoBraking = false;
        agent.updateRotation = false;

        var group = GameObject.Find("WayPoint");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoint);
            wayPoint.RemoveAt(0);
        }
    }

    void Update()
    {
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (_patrolling == false) return;

        if (agent.remainingDistance <= 0.5f)
        {
            Idx++;
            if (Idx == wayPoint.Count) Idx = 0;
            MovePoint();
        }
    }

    void MovePoint()
    {
        if (agent.isPathStale) return;
        agent.destination = wayPoint[Idx].position;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
}
