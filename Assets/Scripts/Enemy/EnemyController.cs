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

    private Transform playerTarget;
    public float chaseDistance = 7f;
    public float attackDistance = 1f;
    public float chasePlayerAfterAttackDistance = 3f;

    private float waitBeforeAttackTime = 3f;
    private float attackTimmer;


    private bool enemyDied;

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

        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        attackTimmer = waitBeforeAttackTime;

    }

    // Update is called once per frame
    void Update()
    {

        if (enemyDied)
        {
            return;
        }

        if (enemyState == EnemyState.PATROL)
        {
            Patrol();
        }

        if(enemyState != EnemyState.CHASE && enemyState != EnemyState.ATTACK)
        {
            if(Vector3.Distance(transform.position, playerTarget.transform.position) <= chaseDistance)
            {
                enemyState = EnemyState.CHASE;

                anim.StopPlayback();
            }
        }

        if (enemyState == EnemyState.CHASE)
        {
            ChasePlayer();
        }

        if (enemyState == EnemyState.ATTACK)
        {
           AttackPlayer();
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

    void ChasePlayer()
    {
        navAgent.SetDestination(playerTarget.position);
        navAgent.speed = runSpeed;

        if (navAgent.velocity.sqrMagnitude == 0) 
        { 
          anim.SetBool("Run",false);
        }

        else
        {
            anim.SetBool("Run",true);
        }

        if(Vector3.Distance(transform.position, playerTarget.position) <= attackDistance)
        {
            enemyState = EnemyState.ATTACK;
        }
        else if(Vector3.Distance(transform.position,playerTarget.position) > chaseDistance)
        {
            timmerCount = patrolTimmer;
            enemyState = EnemyState.PATROL;
            anim.SetBool("Run", false);
        }
    }

    void AttackPlayer()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);

        attackTimmer += Time.deltaTime;
        
        if(attackTimmer > waitBeforeAttackTime)
        {
            anim.SetTrigger("Attack1");

            attackTimmer = 0f;
        }

        if(Vector3.Distance(transform.position, playerTarget.position) >
            attackDistance + chasePlayerAfterAttackDistance)
        {
            navAgent.isStopped = false;
            enemyState = EnemyState.CHASE;
        }

    }

    void DeactivateScript()
    {

       // GameplayController.instance.EnemyDied();

        enemyDied = true;

        StartCoroutine(DeactivateEnemyGameObject());

    }

    IEnumerator DeactivateEnemyGameObject()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void SetInitialState(EnemyState initialState)
    {
        enemyState = initialState;
    }


}
