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

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private List<string> processedMarkers = new List<string>();
    private ARTrackedImageManager trackedImageManager;

    private int counter;

    private void Awake()
    {
        GlobalVars.COMMANDS = new Queue<int>();

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        for(int i = 1; i <= 8; i++)
        {
            GameObject newPrefab = Instantiate(placeablePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            newPrefab.name = "marker" + i;
            newPrefab.SetActive(false);
            spawnedPrefabs.Add("marker" + i, newPrefab);
        }

        counter = 0;
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
            spawnedPrefabs[name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        if(!processedMarkers.Contains(name))
        {
            processedMarkers.Add(name);
            int command = GetCommandCode(name);
            if(command != -1)
                GlobalVars.COMMANDS.Enqueue(command);
        }

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);
    }

    private int GetCommandCode(string marker)
    {
        if ((marker == "marker1") || (marker == "marker2"))
            return 0;
        if ((marker == "marker3") || (marker == "marker4"))
            return 1;
        if ((marker == "marker5") || (marker == "marker6"))
            return 2;
        else
            return -1;
    }

}


