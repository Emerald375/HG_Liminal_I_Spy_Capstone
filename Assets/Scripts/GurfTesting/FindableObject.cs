using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindableObject : MonoBehaviour
{

    /*[SerializeField] private HighlightObject highlightObject;
    [SerializeField] private SelectObject selectObject;
    [SerializeField] private ChecklistItem checklistItem;
    [SerializeField] private Outline outline;*/
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
}
