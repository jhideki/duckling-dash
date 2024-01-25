using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
