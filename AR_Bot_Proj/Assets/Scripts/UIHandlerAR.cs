using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerAR : MonoBehaviour
{
    public GameObject modalWindow;

    private void Awake()
    {
        modalWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene("MainGame");
    }
}
