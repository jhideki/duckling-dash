using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    private bool isHiding;
    public void SetHiding(bool hiding)
    {
        isHiding = hiding;
    }

    public bool GetHiding()
    {
        return isHiding;
    }
}
