using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandlerMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalVars.CAN_EXECUTE = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExecuteButtonClick()
    {
        GlobalVars.CAN_EXECUTE = true;
    }
}
