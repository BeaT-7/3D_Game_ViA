using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] campLocations;
    public GameObject[] campSites;
    public GameObject[] itemSpawns;
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        campLocations = GameObject.FindGameObjectsWithTag("CampLoc");
        campSites = GameObject.FindGameObjectsWithTag("CampSite");
        itemSpawns = GameObject.FindGameObjectsWithTag("ColSpawnPoint");
        items = GameObject.FindGameObjectsWithTag("Collectible");

        SpawnCamps();
        SpawnItems();

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    void SpawnCamps()
    {
        // Shuffles the camp site locations
        for (int i = 0; i < campLocations.Length-1; i++)
        {
            int randomIndex = Random.Range(i, campLocations.Length);
            GameObject temp = campLocations[i];
            campLocations[i] = campLocations[randomIndex];
            campLocations[randomIndex] = temp;
        }

        // Move camps to the shuffled locations
        for (int i = 0; i < campSites.Length; i++)
        {
            campSites[i].transform.position = campLocations[i].transform.position;
            campSites[i].transform.rotation = campLocations[i].transform.rotation;

            Debug.Log("Camp Site: " + campSites[i].name + "; Camp Site Location: " + campLocations[i].name);
        }
    }

    void SpawnItems()
    {
        for (int i = 0; i < itemSpawns.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, itemSpawns.Length);
            GameObject temp = itemSpawns[i];
            itemSpawns[i] = itemSpawns[randomIndex];
            itemSpawns[randomIndex] = temp;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.position = itemSpawns[i].transform.position;
        }

    }
}
