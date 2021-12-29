using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimation : MonoBehaviour
{
    public float BoingSpeed;
    public float RotationSpeed;
    public Transform Lerp1;
    public Transform Lerp2;

    private float orig_y;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float delta = 0.5f * Mathf.Sin(Time.time * BoingSpeed) + 0.5f;
        transform.position = Vector3.Lerp(Lerp1.position, Lerp2.position, delta);
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime, Space.Self);
    }
}
