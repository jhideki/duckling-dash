using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    public float visionRange = 3f;
    public float attackSpeed = 7f;

    private float lastAttackTime;

    private bool isAttacking;
    private bool isPatrolling = true;

    public Bush bush;
    private Rigidbody2D rb;
    private Transform currentTarget;

    private float currentAngle = 0f;
    private bool isClockwise;
    private Vector2 spawnPoint;

    private Coroutine returnToPatrolCoroutine;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAngle = UnityEngine.Random.Range(0f, 360f);
        isClockwise = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (bush.isHiding && isAttacking)
        {
            currentTarget = null;
            //returnToPatrolCoroutine = StartCoroutine(ReturnToPath());
            isPatrolling = true;
            isAttacking = false;
        }

        if (!isAttacking && isPatrolling)
        {
            PatrolInCircle();
        }
        else if (isAttacking && !bush.isHiding)
        {
            isPatrolling = false;
            AttackTarget();
        }

        CheckAndDeleteIfFar();
    }

    void OnTriggerEnter2D(Collider2D target)
    {        
        // Check if the collider belongs to the target
        if (target.CompareTag("Player") || target.CompareTag("Duck"))
        {
            // Destroy the target
            Destroy(target.gameObject);
            currentTarget = null;
            //StartCoroutine(ReturnToPath());
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
    
    /*IEnumerator ReturnToPath()
    {
        Vector2 targetPosition = GetNextPosition();

        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPosition, attackSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset coroutine reference
        returnToPatrolCoroutine = null;
    }

    Vector2 GetNextPosition()
    {
        float x = spawnPoint.x + Mathf.Cos(currentAngle) * patrolRadius;
        float y = spawnPoint.y + Mathf.Sin(currentAngle) * patrolRadius;
        return new Vector2(x, y);
    }*/

    void CheckForTargets()
    {
        // Check for targets within the vision range
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, visionRange);

        // Iterate through detected targets
        foreach (Collider2D target in targets)
        {   
            if ((target.CompareTag("Player") || target.CompareTag("Duck")) && !bush.isHiding)
            {
                isAttacking = true;
                currentTarget = target.transform;
            }
        }
    }

    void AttackTarget()
    {
        if (currentTarget != null)
        {
            // Calculate the direction towards the target
            Vector2 direction = (currentTarget.position - transform.position).normalized;

            // Move the Hawk towards the target using a force
            rb.AddForce(direction * attackSpeed, ForceMode2D.Force);
        }   
    }

        void CheckAndDeleteIfFar()
    {
        if (Vector2.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) > hawkDistance)
        {
            Destroy(gameObject);
        }
    }
}