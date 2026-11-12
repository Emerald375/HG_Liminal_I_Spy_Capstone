using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject thisObject;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        RandomiseObjectPosition();
    }

    public void RandomiseObjectPosition()
    {
        int randomSP = Random.Range(0, spawnPoints.Length); //Get random spawn point loaction
        Transform spawnPoint = spawnPoints[randomSP];

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        //Debug.Log("Object Spawned: " + thisObject);
    }
}
