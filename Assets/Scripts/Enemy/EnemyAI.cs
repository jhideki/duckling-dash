using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = .5f;
    public float patrolRadius = 7f;
    public float visionRange = 3f;
    public float attackCooldown = 5f;
    public float currentCooldown;
    public float attackSpeed = 5.5f;
    private float currentAngle = 0f;
    public float distance = 0.1f;

    private bool isClockwise;
    private bool isAttacking;
    private bool isPatrolling = true;
    private bool onCooldown = false;
    private Hiding hiding;

    private bool isHiding;

    private Transform currentTarget;

    private Vector2 spawnPoint;

    private Coroutine returnToPatrolCoroutine;


    void Start()
    {
        currentAngle = UnityEngine.Random.Range(0f, 360f);
        isClockwise = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (isHiding && isAttacking)
        {
            currentTarget = null;
            isAttacking = false;
            returnToPatrolCoroutine = StartCoroutine(ReturnToPath());
        }

        if ((!isAttacking && isPatrolling) || onCooldown)
        {
            PatrolInCircle();
        }
        else if (isAttacking && !isHiding && !onCooldown)
        {
            isPatrolling = false;
            AttackTarget();
        }

        if (!IsOnPatrolPath() && onCooldown)
        {
            // Perform an action when not on patrol path
            StartCoroutine(ReturnToPath());
        }
    }

    bool IsOnPatrolPath()
    {
        // Calculate the distance between the Hawk and its patrol path
        float distanceToPatrolPath = Vector2.Distance(transform.position, GetNextPosition());

        // Check if the distance is within a threshold
        return distanceToPatrolPath < distance;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        // Check if the collider belongs to the target
        if ((target.CompareTag("Player") || target.CompareTag("Duck")) && !onCooldown)
        {
            // Destroy the target
            isAttacking = false;
            onCooldown = true;
            hiding = target.gameObject.GetComponent<Hiding>();
            isHiding = hiding.GetHiding();
            StartCoroutine(StartCountdown());
        }
    }

    void PatrolInCircle()
    {
        float x = spawnPoint.x + Mathf.Cos(currentAngle) * patrolRadius;
        float y = spawnPoint.y + Mathf.Sin(currentAngle) * patrolRadius;
        transform.position = new Vector3(x, y, transform.position.z);

        currentAngle += isClockwise ? Time.deltaTime * patrolSpeed : -Time.deltaTime * patrolSpeed;

        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }
        else if (currentAngle < 0f)
        {
            currentAngle += 360f;
        }

        CheckForTargets();
    }

    void CheckForTargets()
    {
        // Check for targets within the vision range
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, visionRange);

        // Iterate through detected targets
        foreach (Collider2D target in targets)
        {
            if ((target.CompareTag("Player") || target.CompareTag("Duck")) && !isHiding)
            {
                isAttacking = true;
                currentTarget = target.transform;
            }
        }
    }

    void AttackTarget()
    {
        if (currentTarget != null && !onCooldown)
        {
            // Calculate the direction towards the target
            Vector2 direction = (currentTarget.position - transform.position).normalized;

            // Move the Hawk towards the target
            transform.position += new Vector3(direction.x, direction.y, 0) * attackSpeed * Time.deltaTime;
        }
    }

    IEnumerator StartCountdown()
    {
        float timer = attackCooldown;

        while (timer > 0)
        {
            // Decrease the timer by deltaTime
            timer -= Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
        onCooldown = false;
        currentTarget = null;
        isAttacking = false;
    }

    IEnumerator ReturnToPath()
    {
        Vector2 targetPosition = GetNextPosition();

        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPosition, attackSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset coroutine reference
        returnToPatrolCoroutine = null;

        // Reset patrolling
        isPatrolling = true;
        isAttacking = false;
    }

    Vector2 GetNextPosition()
    {
        float x = spawnPoint.x + Mathf.Cos(currentAngle) * patrolRadius;
        float y = spawnPoint.y + Mathf.Sin(currentAngle) * patrolRadius;
        return new Vector2(x, y);
    }
}