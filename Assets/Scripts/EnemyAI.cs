using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    public float attackDistance = 5;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public GameObject player;
    public GameObject[] wanderPoints;
    Vector3 nextDestination;
    Animator anim;
    float distnaceToPlayer;
    public float idleTime = 2.5f;
    int currentDestinationIndex = 0;
    Transform deadTransform;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();

        Initialize();
    }

    void Update()
    {
        distnaceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Idle:
                UpdateIdleState();
                break;
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

    }

    void UpdateIdleState()
    {
        print("idle");
        anim.SetInteger("animState", 0);

        if (distnaceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        else if (idleTime <= 0f)
        {
            if (wanderPoints.Length > 1)
            {
                FindNextPoint();
            }
            currentState = FSMStates.Patrol;
        }
        else
        {
            idleTime -= Time.deltaTime;
        }
    }

    void UpdatePatrolState()
    {
        print("Patrolling!");

        anim.SetInteger("animState", 1);

        if (Vector3.Distance(transform.position, nextDestination) < 1)
        {
            idleTime = 2.5f;
            currentState = FSMStates.Idle;
        }
        else if (distnaceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        //transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }
    void UpdateChaseState()
    {
        print("Chasing!");

        anim.SetInteger("animState", 2);

        nextDestination = player.transform.position;

        if (distnaceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distnaceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        //transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }
    void UpdateAttackState()
    {
        print("attack");

        nextDestination = player.transform.position;
        if (distnaceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distnaceToPlayer > attackDistance && distnaceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distnaceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        anim.SetInteger("animState", 3);
    }
    void UpdateDeadState()
    {
        print("dead");
        anim.SetInteger("animState", 4);
        deadTransform = gameObject.transform;

        Destroy(gameObject, 3);
    }

    private void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmoPosition = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
        //attack
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoPosition, attackDistance);

        //chase
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gizmoPosition, chaseDistance);
    }
}
