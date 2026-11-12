using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListRandomiser : MonoBehaviour
{

    [SerializeField] private Transform objectsToFind;
    [SerializeField] private int objectsToSelect = 3;
    private List<FindableObject> findableObjectList = new List<FindableObject>();
    private List<FindableObject> activeObjects = new List<FindableObject>();
    private List<FindableObject> waitingObjects = new List<FindableObject>();
    
    [SerializeField] private Text[] checklistTexts;


    private void Start()
    {
        Debug.Log("ListRandomiser has started");

        for(int checklistIndex = 0; checklistIndex < checklistTexts.Length; checklistIndex += 1)
        {
            checklistTexts[checklistIndex].gameObject.SetActive(false);
        }

        for(int childIndex = 0; childIndex < objectsToFind.childCount; childIndex += 1)
        {
            FindableObject findableObject = objectsToFind.GetChild(childIndex).GetComponent<FindableObject>();
            findableObjectList.Add(findableObject);
        }
        Debug.Log("Objects found: " + findableObjectList.Count);
        
        for(int objectIndex = 0; objectIndex < findableObjectList.Count; objectIndex += 1)
        {
            findableObjectList[objectIndex].SetFindable(false);
        }

        List<FindableObject> availableObjects = new List<FindableObject>(findableObjectList);

        Debug.Log("Available objects: " + availableObjects.Count);

        for(int selectionIndex = 0; selectionIndex < objectsToSelect; selectionIndex += 1)
        {
            int randomIndex = Random.Range(0, availableObjects.Count);
            FindableObject selectedObject = availableObjects[randomIndex]; 

            
            Debug.Log("Selected Object: " + selectedObject.name);

            if(selectionIndex < checklistTexts.Length)
            {
                activeObjects.Add(selectedObject);
                selectedObject.SetFindable(true);
                
            }
            else
            {
                waitingObjects.Add(selectedObject);
                selectedObject.SetFindable(false);
            }

            availableObjects.Remove(selectedObject);
        }

        UpdateChecklist();
    }

    private void UpdateChecklist()
    {
        for(int objectIndex = 0; objectIndex < activeObjects.Count; objectIndex += 1)
        {
            FindableObject selectedObject = activeObjects[objectIndex];

            if(selectedObject == null) continue;

            ChecklistItem checklistItem = selectedObject.GetComponent<ChecklistItem>();

            checklistItem.listRandomiser = this;
            checklistItem.checklistText = checklistTexts[objectIndex];
            checklistTexts[objectIndex].text = checklistItem.propName;
            checklistTexts[objectIndex].gameObject.SetActive(true);
        }
    }

    public void ObjectFound(FindableObject foundObject)
    {
        int foundIndex = activeObjects.IndexOf(foundObject);

        Debug.Log("Object found: " + foundObject.name);
        Debug.Log("Found index: " + foundIndex);
        Debug.Log("Waiting objects: " + waitingObjects.Count);

        if(waitingObjects.Count > 0)
        {
            FindableObject nextObject = waitingObjects[0];

            activeObjects[foundIndex] = nextObject;
            waitingObjects.RemoveAt(0);

            nextObject.SetFindable(true);

            UpdateChecklist();
        }
        else
        {
            activeObjects[foundIndex] = null;
            checklistTexts[foundIndex].text = "";
            checklistTexts[foundIndex].gameObject.SetActive(false);
        }
    }
    


}
