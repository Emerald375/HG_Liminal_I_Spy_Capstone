using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{

    public Text checklistText;


    public void IsFound(string propName)
    {
        if (propName == "Cube")
        {
            checklistText.text = checklistText.text.Replace("[?] Cube", "[X] Cube");
        }

        if (propName == "Sphere")
        {
            checklistText.text = checklistText.text.Replace("[?] Sphere", "[X] Sphere");
        }

        if (propName == "Cylinder")
        {
            checklistText.text = checklistText.text.Replace("[?] Cylinder", "[X] Cylinder");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
