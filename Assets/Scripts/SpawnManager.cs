using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] campLocations = GameObject.FindGameObjectsWithTag("CampLoc");
        foreach (GameObject campLocation in campLocations)
        {
            Debug.Log("Camp Location: " + campLocation.name);
        }

        GameObject[] campSites = GameObject.FindGameObjectsWithTag("CampSite");
        foreach (GameObject campSite in campSites)
        {
            Debug.Log("Camp Site: " + campSite.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
