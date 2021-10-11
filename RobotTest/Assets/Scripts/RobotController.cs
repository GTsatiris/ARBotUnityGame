using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 finishPos;
    private float delta;
    private float yRotOrig;
    private bool canMove;

    private int currentCommand;
    private bool commandUnderway;
    private bool startCommand;
    private Animator anim;

    public float speed;

    void Awake()
    {
        anim = GetComponent<Animator>();
        GlobalVars.COMMANDS = new Queue<int>();
        GlobalVars.COMMANDS.Enqueue(1);
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(2);
        GlobalVars.COMMANDS.Enqueue(0);
        startCommand = false;
        commandUnderway = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (commandUnderway)
        {
            switch (currentCommand)
            {
                case 0:
                    if (startCommand)
                    {
                        initComm_0();
                        startCommand = false;
                    }
                    else
                    {
                        commandUnderway = execComm_0();
                    }
                    break;
                case 1:
                    if (startCommand)
                    {
                        initComm_1();
                        startCommand = false;
                    }
                    else
                    {
                        commandUnderway = execComm_1();
                    }
                    break;
                case 2:
                    if (startCommand)
                    {
                        initComm_2();
                        startCommand = false;
                    }
                    else
                    {
                        commandUnderway = execComm_2();
                    }
                    break;
                case 3:
                    if (startCommand)
                    {
                        initComm_3();
                        startCommand = false;
                    }
                    else
                    {
                        commandUnderway = execComm_3();
                    }
                    break;
            }
        }
        else
        {
            if (GlobalVars.COMMANDS.Count > 0)
            {
                currentCommand = GlobalVars.COMMANDS.Dequeue();
                startCommand = true;
                commandUnderway = true;
            }
            else
            {
                //Debug.Log("END GAME!");
                //TODO *************************************
            }
        }
    }

    private bool checkMove()
    {
        //float dice = Random.Range(0.0f, 1.0f);
        //Debug.Log(dice);
        //if (dice < 0.5f)
        //    return true;
        //else
        //    return false;
        return true;
    }

    private void initComm_0()
    {
        startPos = transform.position;
        finishPos = startPos + (GlobalVars.BOARD_WIDTH / GlobalVars.SQUARE_W) * transform.forward;
        delta = 1.0f;

        //TODO anim.SetBool("Walk_Anim", true);
        canMove = checkMove();
        if (canMove)
        {
            transform.position = delta * startPos + (1 - delta) * finishPos;
        }
        delta -= speed * Time.deltaTime;
    }

    private void initComm_1()
    {
        delta = 0.0f;

        yRotOrig = transform.rotation.eulerAngles.y;
        
        transform.eulerAngles = new Vector3(0.0f, yRotOrig + (90.0f * delta), 0.0f);

        delta += speed * Time.deltaTime;
    }
    private void initComm_2()
    {
        delta = 0.0f;

        yRotOrig = transform.rotation.eulerAngles.y;

        transform.eulerAngles = new Vector3(0.0f, yRotOrig + (-90.0f * delta), 0.0f);

        delta += speed * Time.deltaTime;
    }

    private void initComm_3()
    {
        return;
    }

    private bool execComm_0()
    {
        if(canMove)
        {
            if (delta >= 0.0f)
                transform.position = delta * startPos + (1 - delta) * finishPos;
            else
                transform.position = finishPos;
        }
        delta -= speed * Time.deltaTime;

        if (delta > 0.0f)
            return true;
        else
            return false;
    }

    private bool execComm_1()
    {
        if (delta <= 1.0f)
            transform.eulerAngles = new Vector3(0.0f, yRotOrig + (90.0f * delta), 0.0f);
        else
            transform.eulerAngles = new Vector3(0.0f, yRotOrig + 90.0f, 0.0f);

        delta += speed * Time.deltaTime;
        
        if (delta < 1.0f)
            return true;
        else
            return false;
    }
    private bool execComm_2()
    {

        if (delta <= 1.0f)
            transform.eulerAngles = new Vector3(0.0f, yRotOrig + (-90.0f * delta), 0.0f);
        else
            transform.eulerAngles = new Vector3(0.0f, yRotOrig - 90.0f, 0.0f);

        delta += speed * Time.deltaTime;

        if (delta < 1.0f)
            return true;
        else
            return false;
    }

    private bool execComm_3()
    {
        //TODO
        return false;
    }
}
