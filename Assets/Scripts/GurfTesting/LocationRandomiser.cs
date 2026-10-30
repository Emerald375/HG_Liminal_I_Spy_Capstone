using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRandomiser : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToMove;
    [SerializeField] private Transform[] spawnPointGroups;


    private void Start()
    {
        for(int objectIndex = 0; objectIndex < objectsToMove.Length; objectIndex += 1)
        {
            Transform spawnPointGroup = spawnPointGroups[objectIndex];

            int randomIndex = Random.Range(0, spawnPointGroup.childCount);

            Transform spawnPoint = spawnPointGroup.GetChild(randomIndex);
            
            objectsToMove[objectIndex].position = spawnPoint.position;
        }
    }
}
