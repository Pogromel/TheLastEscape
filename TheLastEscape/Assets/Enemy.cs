using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Enemy : MonoBehaviour
{
    public Transform[] points;
    public Transform player;
    public float chaseRadius = 10f;
    public float fieldOfViewAngle = 45f;

    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (points.Length > 0)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        DrawFieldOfView();

        if (IsPlayerInFieldOfView())
        {
            // Chase the player
            agent.destination = player.position;
        }
        else
        {
            // Resume patrolling
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < chaseRadius)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle < fieldOfViewAngle / 2)
            {
                return true;
            }
        }

        return false;
    }

    void DrawFieldOfView()
    {
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * chaseRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward * chaseRadius;

        Debug.DrawLine(transform.position, transform.position + leftBoundary, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + rightBoundary, Color.yellow);

        // Draw the field of view arc
        int segments = 20; // Number of segments to make the arc
        float angle = -fieldOfViewAngle / 2;
        float angleIncrement = fieldOfViewAngle / segments;

        Vector3 prevPoint = transform.position + (Quaternion.Euler(0, angle, 0) * transform.forward * chaseRadius);

        for (int i = 0; i <= segments; i++)
        {
            angle += angleIncrement;
            Vector3 nextPoint = transform.position + (Quaternion.Euler(0, angle, 0) * transform.forward * chaseRadius);
            Debug.DrawLine(prevPoint, nextPoint, Color.yellow);
            prevPoint = nextPoint;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // Display the field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * chaseRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward * chaseRadius;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Draw the field of view arc
        Gizmos.color = new Color(1, 1, 0, 0.1f); // Semi-transparent yellow
        int segments = 20; // Number of segments to make the arc
        float angle = -fieldOfViewAngle / 2;
        float angleIncrement = fieldOfViewAngle / segments;

        Vector3 prevPoint = transform.position + (Quaternion.Euler(0, angle, 0) * transform.forward * chaseRadius);

        for (int i = 0; i <= segments; i++)
        {
            angle += angleIncrement;
            Vector3 nextPoint = transform.position + (Quaternion.Euler(0, angle, 0) * transform.forward * chaseRadius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
