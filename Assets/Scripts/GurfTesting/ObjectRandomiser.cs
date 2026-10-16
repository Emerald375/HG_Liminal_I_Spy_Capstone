using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectRandomiser : MonoBehaviour
{

    [SerializeField] private Transform objectsToFind;
    [SerializeField] private int objectsToSelect = 3;
    private List<FindableObject> findableObjectList = new List<FindableObject>();
    
    [SerializeField] private Text[] checklistTexts;


    private void Start()
    {
        for(int checklistIndex = 0; checklistIndex < checklistTexts.Length; checklistIndex += 1)
        {
            checklistTexts[checklistIndex].gameObject.SetActive(false);
        }

        for(int childIndex = 0; childIndex < objectsToFind.childCount; childIndex += 1)
        {
            FindableObject findableObject = objectsToFind.GetChild(childIndex).GetComponent<FindableObject>();
            findableObjectList.Add(findableObject);
        }
        
        for(int objectIndex = 0; objectIndex < findableObjectList.Count; objectIndex += 1)
        {
            findableObjectList[objectIndex].SetFindable(false);
        }

        List<FindableObject> availableObjects = new List<FindableObject>(findableObjectList);

        for(int selectionIndex = 0; selectionIndex < objectsToSelect; selectionIndex += 1)
        {
            int randomIndex = Random.Range(0, availableObjects.Count);
            FindableObject selectedObject = availableObjects[randomIndex]; 
            Debug.Log("Selected Object: " + selectedObject.name);
            selectedObject.SetFindable(true);

            ChecklistItem checklistItem = selectedObject.GetComponent<ChecklistItem>();
            checklistItem.checklistText = checklistTexts[selectionIndex];
            checklistTexts[selectionIndex].gameObject.SetActive(true);
            checklistTexts[selectionIndex].text = checklistItem.propName;

            availableObjects.Remove(selectedObject);
        }

    }


}
