using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowParent : MonoBehaviour
{
    private Transform target;

    public float followSpeed;
    private FollowParent previousDuck;
    public float followDistance = 1.5f;
    public bool isFollowing = false;


    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - directionToPlayer * followDistance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

    }

    public void StartFollowing(Transform newTarget)
    {
        target = newTarget;
        isFollowing = true;
    }
    public void SetPreviousDuck(FollowParent duck)
    {
        previousDuck = duck;

    }

    public FollowParent GetPreviousDuck()
    {
        return previousDuck;
    }

}
