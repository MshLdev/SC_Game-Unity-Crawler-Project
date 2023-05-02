using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public string message;
    public string question;
    public List<string> dialogOptions = new List<string>();
    

    // Start is called before the first frame update
    void Start()
    {
        dialogOptions.Add("Option 1");
        dialogOptions.Add("Option 2");
        dialogOptions.Add("Option 3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
