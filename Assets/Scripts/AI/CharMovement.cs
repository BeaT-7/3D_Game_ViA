using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharMovement : MonoBehaviour
{
    public Transform patrolCenter;
    [SerializeField] float patrolDistance; 

    private NavMeshAgent agent; 
    private Animator animator;
    private Transform player;

    public Rigidbody playerRB;
    public CameraMovement cameraScript;

    private Vector3 currentDestination;
    private float distanceToPlayer;

    [SerializeField] float patrolDurationMin;
    [SerializeField] float patrolDurationMax;

    private float randPatrolDuration;
    private float patrolTimer = 0f;
    private float startMovementTimer = 0f;
    private float startChaseTimer = 0f;

    private bool isAnimationSet = false;
    private bool playerDetected = false;
    private bool isWalkpointSet = false;
    private bool isRandomTimePicked = false;
    private bool isLooking = false;

    private bool inChaseRoutine = false;

    private bool startUpdate = false;
    private enum State
    {
        Idle,
        Patrolling,
        Chasing,
        Detecting,
        End
    }
    private State currentState;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    void Update()
    {
        if (startUpdate)
        {
            if (currentState == State.Idle) Idle();
            if (currentState == State.Patrolling) Patrolling();
            if (currentState == State.Chasing)
            {
                agent.SetDestination(player.position);
                if (!inChaseRoutine) StartCoroutine(ChaseRoutine());
            }
            if (currentState == State.Detecting) Detecting();
            if (currentState == State.End) LostGame();


            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (currentState == State.Chasing && distanceToPlayer < 2.5f) currentState = State.End;
        }
    }

    private void Patrolling()
    {
        startMovementTimer += Time.deltaTime;
        if (!isAnimationSet) SetAnimation();
        if (!isWalkpointSet)
        {
            currentDestination = GetRandomPatrolDestination();
            agent.SetDestination(currentDestination);
            isWalkpointSet = true;
        }
        if (agent.remainingDistance <= 1f && startMovementTimer >= 2f)
        {
            startMovementTimer = 0f;
            isAnimationSet = false;
            isWalkpointSet = false;
            currentState = State.Idle;
        }
    }

    private void Idle()
    {
        if (!isAnimationSet) SetAnimation();
        if (!isRandomTimePicked)
        {
            randPatrolDuration = GetRandomPatrolDuration();
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= randPatrolDuration)
        {
            currentState = State.Patrolling;
            patrolTimer = 0f;
            isAnimationSet = false;
            isRandomTimePicked = false;
            
        }
    }

    private void Detecting()
    {
        if (!isAnimationSet) SetAnimation();
        if (!isLooking)
        {
            startChaseTimer = 0f;
            agent.isStopped = true;
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = lookRotation;
            isLooking = true;
        }

        startChaseTimer += Time.deltaTime;
        if (startChaseTimer >= 2.5f)
        {
            isAnimationSet = false;
            isLooking = false;
            isWalkpointSet = false;
            agent.isStopped = false;
            currentState = State.Chasing;
        }

        if (!playerDetected)
        {
            agent.ResetPath();
            currentState = State.Idle;
            isAnimationSet = false;
            isLooking = false;
            isWalkpointSet = false;
            agent.isStopped = false;
            startMovementTimer = 0f;
            patrolTimer = 0f;
        }
    }

    private void LostGame()
    {
        isAnimationSet = false;
        SetAnimation();

        playerRB.isKinematic = true;
        cameraScript.enabled = false;

        // UI stuff Here - Text "You lost" / Button to restart or quit / ?time you played?
        // placeholder console output
        Debug.Log("You Lost");

        enabled = false;
    }

    private Vector3 GetRandomPatrolDestination()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * patrolDistance;
        Vector3 randomPosition = patrolCenter.position + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        return randomPosition;
    }

    private float GetRandomPatrolDuration()
    {
        var randValue = Random.Range(patrolDurationMin, patrolDurationMax);
        isRandomTimePicked = true;
        return randValue;
    }
    private void SetAnimation()
    {
        animator.SetBool("SprintTrigger", false);
        if (currentState == State.Patrolling)
        {
            animator.SetBool("WalkTrigger", true);
            animator.SetBool("IdleTrigger", false);
            isAnimationSet = true;
        }
        if (currentState != State.Patrolling)
        {
            animator.SetBool("IdleTrigger", true);
            animator.SetBool("WalkTrigger", false);
            isAnimationSet = true;
        }
    }

    private IEnumerator ChaseRoutine()
    {
        inChaseRoutine = true;
        animator.SetBool("IdleTrigger", false);
        animator.SetBool("SprintTrigger", true);
        agent.speed = 6f;
        yield return null;

        yield return new WaitForSeconds(2f);
        
        for (int i = 0; i < 10; i++)
        {
            if (playerDetected)
            {
                inChaseRoutine = false;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        inChaseRoutine = false;
        isAnimationSet = false;
        agent.ResetPath();
        agent.speed = 2.5f;
        currentState = State.Idle;
        
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentState = State.Patrolling;

        startUpdate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAnimationSet = false;
            playerDetected = true;
            currentState = State.Detecting;
            Debug.Log("Detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            startChaseTimer = 0f;
        }
    }
}
