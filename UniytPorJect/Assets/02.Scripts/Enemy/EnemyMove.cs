using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private EnemyAI EnemyAI;
    private NavMeshAgent agent;
    private Transform enemyTr;
    private Transform PlayerTr;

    public List<Transform> wayPoint = new List<Transform>();

    float damping;

    const float defaultDamping = 1.0f;
    const float traceDamping = 7.0f;
    public int waypointIndex = 0;

    private bool _patrolling;
    public bool patrolling
    {
        get =>  _patrolling; 
        set
        {
            _patrolling = value;
            damping = defaultDamping;
            if (_patrolling)
                MoveToNextWayPoint();
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get => _traceTarget;
        set
        {
            _traceTarget = value;
            damping = traceDamping;
            MoveToTarget(_traceTarget);
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
        #region 최적화 하기전 코드
        //if (!agent.isStopped)
        //{
        //    Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
        //    enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        //}

        //if (_patrolling == false) return;

        //if (agent.remainingDistance <= 0.5f)
        //{
        //    waypointIndex++;
        //    if (waypointIndex == wayPoint.Count) waypointIndex = 0;
        //    MovePoint();
        //}
        #endregion

        if (!agent.isStopped)
        {
            RotateTowards(agent.desiredVelocity);

            // 패트롤 상태에서 목적지에 도달 했는지 확인
            if (_patrolling && agent.remainingDistance <= 0.5f)
            {
                waypointIndex = (waypointIndex + 1) % wayPoint.Count;
                MoveToNextWayPoint();
            }
        }
    }

    void RotateTowards(Vector3 velocity)
    {
        if (velocity == Vector3.zero) return;
        {
            Quaternion rot = Quaternion.LookRotation(velocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void MoveToNextWayPoint()
    {
        if (wayPoint.Count == 0 || agent.isPathStale) return;

        agent.destination = wayPoint[waypointIndex].position;
        agent.isStopped = false;
    }

    void MoveToTarget(Vector3 position)
    {
        if (agent.isPathStale) return;

        agent.destination = position;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    #region 최적화 하기전 코드
    //void MovePoint()
    //{
    //    if (agent.isPathStale) return;
    //    agent.destination = wayPoint[waypointIndex].position;
    //    agent.isStopped = false;
    //}

    //void TraceTarget(Vector3 pos)
    //{
    //    if (agent.isPathStale) return;
    //    agent.destination = pos;
    //    agent.isStopped = false;
    //}
    #endregion
}
