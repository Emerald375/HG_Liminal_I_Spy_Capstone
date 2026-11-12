using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindableObject : MonoBehaviour
{

    /*[SerializeField] private HighlightObject highlightObject;
    [SerializeField] private SelectObject selectObject;
    [SerializeField] private ChecklistItem checklistItem;
    [SerializeField] private Outline outline;*/

    [SerializeField] private ObjectSpawner spawner;
    public void SetFindable(bool findable)
    {
        /*highlightObject.enabled = findable;
        selectObject.enabled = findable;
        checklistItem.enabled = findable;
        outline.enabled = findable;*/
        
        GetComponent<Collider>().enabled = findable;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void MoveObject()
    {
        spawner.SpawnObject();
        Debug.Log("Findable Objects Script");
    }
}
