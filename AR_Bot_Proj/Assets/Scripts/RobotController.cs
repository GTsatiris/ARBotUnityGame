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
    private bool succeeded;
    private bool wokeUp;

    private int currentCommand;
    private bool commandUnderway;
    private bool startCommand;

    private bool forceEndgame;
    private GameObject ResUI;

    private Animator anim;
    private RAudioManager audioManager;

    public float speed;
    public GameObject Target;

    void Awake()
    {
        wokeUp = false;
        anim = GetComponent<Animator>();
        audioManager = GetComponent<RAudioManager>();
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
        succeeded = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResUI = GameObject.FindWithTag("ResultUI");
        ResUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(forceEndgame)
        {
            anim.ResetTrigger("IdleToWalk");
            anim.SetTrigger("WalkToIdle");
            //if(Vector3.Distance(transform.position, target.transform.position) < 0.3 * GlobalVars.SCALE_FACTOR)
            if(succeeded)
            {
                Debug.Log("WIN!!!!!");
                ResUI.transform.Find("Prompt").GetComponent<TMP_Text>().text = "WIN!!";
                ResUI.SetActive(true);
                anim.ResetTrigger("PowerOn");
                anim.SetTrigger("PowerOff");
            }
            else
            {
                Debug.Log("LOUTSOS");
                ResUI.transform.Find("Prompt").GetComponent<TMP_Text>().text = "TRY AGAIN!!";
                ResUI.SetActive(true);
                anim.ResetTrigger("PowerOn");
                anim.SetTrigger("Die");
                audioManager.Stop("PowerUp");
                audioManager.Play("Die");
            }
        }
        else
        {
            if((GlobalVars.COMMANDS.Count > 0) && !wokeUp)
            {
                anim.SetTrigger("PowerOn");
                audioManager.Play("PowerUp");
                wokeUp = true;
            }

            if (GlobalVars.CAN_EXECUTE)
            {
                anim.SetTrigger("IdleToWalk");
                ExecuteAlgorithm();
            }
            else
            {
                anim.SetTrigger("WalkToIdle");
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
                        anim.SetTrigger("LeftTurn");
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
                        anim.SetTrigger("RightTurn");
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

    /*private bool checkMove()
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
    }*/

    private bool checkMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position /*+ GlobalVars.SCALE_FACTOR * transform.up*/, transform.forward, out hit, GlobalVars.SQUARE_W))
        {
            if (hit.collider.gameObject.CompareTag("valid"))
            {
                finishPos = hit.collider.gameObject.transform.position;
                Debug.Log(hit.collider.gameObject.name);
                succeeded = false;
                return true;
            }
            else if (hit.collider.gameObject.CompareTag("target"))
            {
                finishPos = hit.collider.gameObject.transform.position;
                Debug.Log(hit.collider.gameObject.name);
                succeeded = true;
                return true;
            }
            else
            {
                Debug.Log("HIT, NO TILE");
                Debug.Log(hit.collider.gameObject.name);
                forceEndgame = true;
                succeeded = false;
                return false;
            }
        }
        else
        {
            Debug.Log("NO HIT");
            forceEndgame = true;
            succeeded = false;
            return false;
        }
    }

    private void initComm_0()
    {
        startPos = transform.position;
        finishPos = startPos + (GlobalVars.SQUARE_W * GlobalVars.SCALE_FACTOR) * transform.forward;

        delta = 1.0f;

        //Responsible to validate the mode and calculate target position
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
        {
            if (succeeded)
                Destroy(Target);
            return false;
        }
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
