using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject finish;

    private GameObject[] campLocations;
    private GameObject[] campSites;
    private GameObject[] itemSpawns;
    private GameObject[] items;
    private GameObject[] aiSpawns;
    private GameObject[] ai;

    [SerializeField] int itemsToCollect;
    private GameObject[] activeItems;

    void Start()
    {
        campLocations = GameObject.FindGameObjectsWithTag("CampLoc");
        campSites = GameObject.FindGameObjectsWithTag("CampSite");

        itemSpawns = GameObject.FindGameObjectsWithTag("ColSpawnPoint");
        items = GameObject.FindGameObjectsWithTag("Collectible");

        aiSpawns = GameObject.FindGameObjectsWithTag("AI_Spawnpoint");
        ai = GameObject.FindGameObjectsWithTag("CampPatrolAI");

        activeItems = new GameObject[itemsToCollect];

        SpawnCamps();
        SpawnItems();
        SpawnAi();

        SelectItems();
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
    void SpawnAi()
    {
        for (int i = 0; i < aiSpawns.Length - 1; i++)
        {
            int randomIndex = Random.Range(i, aiSpawns.Length);
            GameObject temp = aiSpawns[i];
            aiSpawns[i] = aiSpawns[randomIndex];
            aiSpawns[randomIndex] = temp;
        }
        for (int i = 0; i < ai.Length; i++)
        {
            Vector3 position = aiSpawns[i].transform.position;
            RaycastHit hit;
            if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity))
            {
                CharMovement aiScript = ai[i].GetComponent<CharMovement>();
                aiScript.patrolCenter = aiSpawns[i].GetComponent<Transform>();
                ai[i].transform.position = hit.point;
                

                NavMeshAgent navMeshAgent = ai[i].GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = true;
                aiScript.enabled = true;
            }

        }
    }

    void SelectItems()
    {
        int i = 0;
        while (true)
        {
            int rand = Random.Range(0, items.Length);
            if (System.Array.IndexOf(activeItems, items[rand]) == -1)
            {
                activeItems[i] = items[rand];
                Collectable col = activeItems[i].GetComponent<Collectable>();
                col.isActive = true;
                Debug.Log(activeItems[i].name);
                i++;
                if (i >= itemsToCollect) break;
            }
        }
    }

    public void itemCollected(GameObject item)
    {
        int index = System.Array.IndexOf(activeItems, item);
        activeItems[index] = null;
        bool won = true;
        foreach (GameObject activeItem in activeItems)
        {
            if (activeItem != null) won = false;
        }
        if (won) gameWon();
    }

    public void gameWon()
    {
        Debug.Log("All items collected, now escape!");
        BoxCollider finishBox = finish.GetComponent<BoxCollider>();
        finishBox.enabled = true;
    }

    private void restartGame()
    {

    }
}
