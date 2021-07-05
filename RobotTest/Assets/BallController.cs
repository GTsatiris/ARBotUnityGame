using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject start;
    public GameObject finish;

    private float delta;
    public float speed;

    private Vector3 rot = Vector3.zero;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        delta = 0.0f;
        gameObject.transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            if(delta < 1.0)
                delta += speed;

            Vector3 direction = Vector3.Normalize(start.transform.position - gameObject.transform.position);
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rot = lookRotation.eulerAngles;
            Debug.Log(rot);
            anim.SetBool("Walk_Anim", true);
        }
        if (Input.GetKey("s"))
        {
            if (delta > 0.0)
                delta -= speed;

            Vector3 direction = Vector3.Normalize(finish.transform.position - gameObject.transform.position);
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rot = lookRotation.eulerAngles;
            Debug.Log(rot);
            anim.SetBool("Walk_Anim", true);
        }
        gameObject.transform.eulerAngles = rot;
        Debug.Log("CURR ROT: " + gameObject.transform.rotation.eulerAngles);
        gameObject.transform.position = delta * start.transform.position + (1.0f - delta) * finish.transform.position;
    }
}
