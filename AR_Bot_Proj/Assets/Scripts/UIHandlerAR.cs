using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerAR : MonoBehaviour
{
    public GameObject modalWindow;
    public GameObject promptWindow;

    private void Awake()
    {
        modalWindow.SetActive(false);
        promptWindow.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVars.LEVEL_VISIBLE)
            promptWindow.SetActive(false);
    }

    public void OnPlayButtonClick()
    {
        if (modalWindow.activeInHierarchy)
            modalWindow.SetActive(false);
        else
            modalWindow.SetActive(true);
    }

    public void OnFinalPlayButtonClick()
    {
        //SceneManager.LoadScene("MainGame");
        modalWindow.SetActive(false);
        GlobalVars.CAN_EXECUTE = true;
    }
}
