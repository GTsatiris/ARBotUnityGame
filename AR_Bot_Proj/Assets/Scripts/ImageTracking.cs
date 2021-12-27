using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject placeablePrefab;
    [SerializeField]
    private GameObject levelPrefab;

    private GameObject Level;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private List<string> processedMarkers = new List<string>();
    private ARTrackedImageManager trackedImageManager;

    private int[] CODES_FW = { 1, 2, 7, 8, 9, 10 };
    private int[] CODES_TL = { 3, 4 };
    private int[] CODES_TR = { 5, 6 };

    private int[] levelCodes = { 11, 12, 13 };

    private const int MAX_MARKERS = 10;

    private void Awake()
    {
        GlobalVars.COMMANDS = new Queue<int>();

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        for(int i = 1; i <= MAX_MARKERS; i++)
        {
            GameObject newPrefab = Instantiate(placeablePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            newPrefab.name = "marker" + i;
            newPrefab.SetActive(false);
            spawnedPrefabs.Add("marker" + i, newPrefab);
        }

        Level = Instantiate(levelPrefab, Vector3.zero, Quaternion.Euler(new Vector3(0, 90, 0)));
        Level.name = "marker" + levelCodes[0];
        Level.transform.localScale = new Vector3(GlobalVars.SCALE_FACTOR, GlobalVars.SCALE_FACTOR, GlobalVars.SCALE_FACTOR);
        Level.SetActive(false);
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            string name = trackedImage.name;
            if (spawnedPrefabs.ContainsKey(name))
                spawnedPrefabs[name].SetActive(false);
            else
            { 
                foreach(int lvlCode in levelCodes)
                {
                    if (name == "marker" + lvlCode)
                        Level.SetActive(false);
                }
            }
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        if (spawnedPrefabs.ContainsKey(name))
        {
            if (GlobalVars.LEVEL_VISIBLE)
            {
                if (!processedMarkers.Contains(name))
                {
                    processedMarkers.Add(name);
                    int command = GetCommandCode(name);
                    if (command != -1)
                        GlobalVars.COMMANDS.Enqueue(command);
                }
                GameObject prefab = spawnedPrefabs[name];
                prefab.transform.position = position;
                prefab.SetActive(true);
            }
        }
        else
        {
            //Level = spawnedPrefabs["marker1"];
            Level.transform.position = position;
            Level.SetActive(true);
            GlobalVars.LEVEL_VISIBLE = true;
        }
    }

    private int GetCommandCode(string marker)
    {
        foreach( int code in CODES_FW)
        {
            if (marker == "marker" + code)
                return 0;
        }

        foreach (int code in CODES_TL)
        {
            if (marker == "marker" + code)
                return 1;
        }

        foreach (int code in CODES_TR)
        {
            if (marker == "marker" + code)
                return 2;
        }

        return -1;
    }

}


