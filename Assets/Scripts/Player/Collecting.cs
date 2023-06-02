using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecting : MonoBehaviour
{
    public Camera playerCamera;
    [SerializeField] float raycastDistance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastDistance))
            {
                Collectable collectible = hit.collider.GetComponent<Collectable>();
                if (collectible != null)
                {
                    collectible.Collect();
                }
            }
        }
    }

}
