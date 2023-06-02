using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] GameObject manager;
    private SpawnManager managerScript;

    private void Start()
    {
        managerScript = manager.GetComponent<SpawnManager>();
    }

    public void Collect()
    {
        if (isActive)
        {
            managerScript.itemCollected(gameObject);
            Destroy(gameObject);
        }
    }
}
