using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendInNewDirection : MonoBehaviour
{
    public Vector2 randomDirection;
    private HawkAttk hawkAttk;
    void Start()
    {
        hawkAttk = transform.parent.GetComponent<HawkAttk>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the parent object
        if (other.transform == transform.parent)
        {
            // Get a random direction facing inside the BoxCollider2D
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

        }
    }
}
