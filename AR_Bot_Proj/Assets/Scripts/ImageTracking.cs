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
    private Dictionary<string, Vector3> markerPositions = new Dictionary<string, Vector3>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        for(int i = 1; i <= 8; i++)
        {
            markerPositions.Add("marker" + i, new Vector3(0.0f, 0.0f, 0.0f));

            GameObject newPrefab = Instantiate(placeablePrefab, Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
            newPrefab.name = "marker" + i;
            newPrefab.SetActive(false);
            spawnedPrefabs.Add("marker" + i, newPrefab);
        }
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
            markerPositions[name] = new Vector3(0.0f, 0.0f, 0.0f);
            spawnedPrefabs[name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        markerPositions[name] = position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        //foreach (GameObject go in spawnedPrefabs.Values)
        //{
        //    if (go.name != name)
        //    {
        //        go.SetActive(false);
        //    }
        //}
    }

}


