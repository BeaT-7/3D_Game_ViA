using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isActive = false;

    public void Collect()
    {
        if (isActive)
        {
            Destroy(gameObject);
        }
    }
}
