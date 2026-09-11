using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public Transform teleportPoint;
    public Transform playerTransform;

    public void Teleport()
    {
        playerTransform.position = teleportPoint.position;
    }
}    

