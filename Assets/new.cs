using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StashDuck : MonoBehaviour
{

    private PickupDuck pickupDuck;
    private FollowParent followParent;
    private DuckCounter duckCounter;
    public int numCollider = 2;
    private int Count = 1;
    private bool Isreset = false;

    void Start()
    {
        duckCounter = GameObject.Find("DuckCounter").GetComponent<DuckCounter>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            MultiCollide(other.gameObject);
            duckCounter.SetScore();
        }

    }

    private void MultiCollide(GameObject playr)
    {
        // Player entered the trigger, try to find the FollowParent script dynamically
        pickupDuck = playr.GetComponent<PickupDuck>();
        followParent = pickupDuck.lastDuck;

        if (followParent != null)
        {
            followParent.FreeAndDeleteAllDucks(followParent.GetPreviousDuck());
            Destroy(followParent.gameObject);
        }

    }
}
