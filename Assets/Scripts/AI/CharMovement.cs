using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharMovement : MonoBehaviour
{
    public Transform patrolCenter; // Reference to the patrol center point
    public float patrolDistance = 5f; // Maximum distance from the patrol center

    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Vector3 currentDestination; // Current patrol destination
    private float patrolDuration = 2f; // Duration to stop at each patrol destination
    private float patrolTimer = 0f; // Timer for patrol duration


    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get reference to the NavMeshAgent component

        if (agent != null && agent.isActiveAndEnabled)
        {
            // Set initial destination within the patrol distance from the patrol center
            currentDestination = GetRandomPatrolDestination();
            agent.SetDestination(currentDestination);
        }
        else
        {
            Debug.LogError("NavMeshAgent component is missing or disabled on the character object.");
        }
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolDuration)
            {
                currentDestination = GetRandomPatrolDestination();
                agent.SetDestination(currentDestination);
                patrolTimer = 0f;
            }
        }
    }

    private Vector3 GetRandomPatrolDestination()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * patrolDistance;
        Vector3 randomPosition = patrolCenter.position + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        return randomPosition;
    }

}
