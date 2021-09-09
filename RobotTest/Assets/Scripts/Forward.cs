using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : Command
{
    private GameObject robot;
    private Vector3 start;
    private Vector3 finish;
    private float delta;
    private float speed;

    public Forward(int ID)
    {
        this.ID = ID;
        this.delta = 1.0f;
        this.speed = 0.1f;
    }

    public override bool Init(GameObject obj, Vector3 lookAt)
    {
        robot = obj;
        start = robot.transform.position;
        finish = start + 1.0f * lookAt;
        return canMove();
    }

    public override bool Execute()
    {
        robot.transform.position = delta * start + (1 - delta) * finish;
        delta -= speed * Time.deltaTime;
        return false;
    }

    private bool canMove()
    {
        //TODO
        return false;
    }

}
