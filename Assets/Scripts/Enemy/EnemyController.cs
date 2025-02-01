using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{

    private Animator anim;
    private NavMeshAgent navAgent;

    private EnemyState enemyState;

    private float patrolRadius = 30f;
    private float patrolTimmer = 10f;
    private float timmerCount;

    public float moveSpeed = 3.5f;
    public float runSpeed = 5f;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        timmerCount = patrolTimmer;
        enemyState = EnemyState.PATROL;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        timmerCount += Time.deltaTime;
        navAgent.speed = moveSpeed;

        if(timmerCount > patrolTimmer)
        {
            SetNewRandomPosition();
            timmerCount = 0f;
        }

        if(navAgent.remainingDistance <= 0.5f)
        {
            navAgent.velocity = Vector3.zero;
        }

        if (navAgent.velocity.sqrMagnitude == 0)
        {
            anim.SetBool("Walk", false);
        }
        else 
        {

            anim.SetBool("Walk", true);
        }

    }

    void SetNewRandomPosition()
    {
        Vector3 newDestination = RandomNavSphere(transform.position,patrolRadius,-1);
        navAgent.SetDestination(newDestination);
    }

    Vector3 RandomNavSphere(Vector3 originPos, float dist, int layerMask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;
        randDir += originPos;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit,dist,layerMask);

        return navHit.position;
    }

}
