using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private bool forceEndgame;
    private GameObject target;
    private GameObject ResUI;

    private Animator anim;

    public float speed;

    void Awake()
    {
        anim = GetComponent<Animator>();
        //FOR THE DEBUGGINGS
        /*GlobalVars.COMMANDS = new Queue<int>();
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(1);
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(1);
        GlobalVars.COMMANDS.Enqueue(0);
        GlobalVars.COMMANDS.Enqueue(0);*/
        startCommand = false;
        commandUnderway = false;
        forceEndgame = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("target");
        ResUI = GameObject.FindWithTag("ResultUI");
        ResUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(forceEndgame)
        {
            anim.SetBool("Walk_Anim", false);
            if(Vector3.Distance(transform.position, target.transform.position) < 0.3 * GlobalVars.SCALE_FACTOR)
            {
                Debug.Log("WIN!!!!!");
                ResUI.transform.Find("Prompt").GetComponent<TMP_Text>().text = "WIN!!";
                ResUI.SetActive(true);
            }
            else
            {
                Debug.Log("LOUTSOS");
                ResUI.transform.Find("Prompt").GetComponent<TMP_Text>().text = "TRY AGAIN!!";
                ResUI.SetActive(true);
            }
        }
        else
        { 
            if (GlobalVars.CAN_EXECUTE)
            {
                anim.SetBool("Walk_Anim", true);
                ExecuteAlgorithm();
            }
            else
            {
                anim.SetBool("Walk_Anim", false);
            }
        }
    }

    private void ExecuteAlgorithm()
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
                forceEndgame = true;
            }
        }
    }

    private bool checkMove()
    {
        RaycastHit hit;
        Vector3 raisedPoint = new Vector3(finishPos.x, finishPos.y + GlobalVars.SCALE_FACTOR * 0.2f, finishPos.z);
        if (Physics.Raycast(raisedPoint, Vector3.down, out hit, GlobalVars.SQUARE_W * GlobalVars.SCALE_FACTOR))
        {
            if (hit.collider.gameObject.CompareTag("tile"))
            {
                return true;
            }
            else
            {
                Debug.Log("HIT, NO TILE");
                forceEndgame = true;
                return false;
            }
        }
        else
        {
            Debug.Log("NO HIT");
            forceEndgame = true;
            return false;
        }
    }

    private void initComm_0()
    {
        startPos = transform.position;
        finishPos = startPos + (GlobalVars.SQUARE_W * GlobalVars.SCALE_FACTOR) * transform.forward;

        delta = 1.0f;

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

        transform.eulerAngles = new Vector3(0.0f, yRotOrig + (-90.0f * delta), 0.0f);

        delta += speed * Time.deltaTime;
    }

    private void initComm_2()
    {
        delta = 0.0f;

        yRotOrig = transform.rotation.eulerAngles.y;

        transform.eulerAngles = new Vector3(0.0f, yRotOrig + (90.0f * delta), 0.0f);

        delta += speed * Time.deltaTime;
    }

    private void initComm_3()
    {
        //TODO
        return;
    }

    private bool execComm_0()
    {
        if (canMove)
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
            transform.eulerAngles = new Vector3(0.0f, yRotOrig + (-90.0f * delta), 0.0f);
        else
            transform.eulerAngles = new Vector3(0.0f, yRotOrig - 90.0f, 0.0f);

        delta += speed * Time.deltaTime;

        if (delta < 1.0f)
            return true;
        else
            return false;
    }

    private bool execComm_2()
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

    private bool execComm_3()
    {
        //TODO
        return false;
    }
}
