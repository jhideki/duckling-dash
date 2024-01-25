using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public int radius;
    public float angle = 25f;

    private float timer;
    private Hiding hiding;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        GameObject closestTarget = FindClosestTarget();
        hiding = closestTarget.GetComponent<Hiding>();
        if (closestTarget != null && !hiding.GetHiding())
        {
            Debug.Log(hiding);
            float distance = Vector2.Distance(transform.position, closestTarget.transform.position);

            if (distance < radius)
            {
                if (timer > 2)
                {
                    timer = 0;
                    shoot(closestTarget);
                }
            }
        }
    }

    void shoot(GameObject target)
    {
        // Instantiate the middle bullet
        InstantiateBullet(Vector3.zero, target.transform.position - bulletPos.position);

        // Instantiate the two side bullets with a 25-degree angle

        // Calculate the direction for the side bullets
        Vector3 sideBulletDirection1 = Quaternion.Euler(0, 0, angle) * (target.transform.position - bulletPos.position);
        Vector3 sideBulletDirection2 = Quaternion.Euler(0, 0, -angle) * (target.transform.position - bulletPos.position);

        // Offset the side bullets to match the middle bullet's position
        InstantiateBullet(Vector3.zero, sideBulletDirection1);
        InstantiateBullet(Vector3.zero, sideBulletDirection2);
    }

    void InstantiateBullet(Vector3 offset, Vector3 direction)
    {
        // Instantiate the bullet with an offset
        GameObject newBullet = Instantiate(bullet, bulletPos.position + offset, Quaternion.identity);
        // Set the direction and rotation for the bullet
        newBullet.GetComponent<EnemyBullet>().SetDirectionAndRotation(direction);
    }

    GameObject FindClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] ducks = GameObject.FindGameObjectsWithTag("Duck");

        if (player == null && ducks.Length == 0)
        {
            return null;
        }

        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

        if (player != null)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            if (playerDistance < closestDistance)
            {
                closestTarget = player;
                closestDistance = playerDistance;
            }
        }

        foreach (var duck in ducks)
        {
            float duckDistance = Vector2.Distance(transform.position, duck.transform.position);
            if (duckDistance < closestDistance)
            {
                closestTarget = duck;
                closestDistance = duckDistance;
            }
        }

        return closestTarget;
    }

}
