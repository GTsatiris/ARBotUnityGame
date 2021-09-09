using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public int ID;
    public abstract bool Init(GameObject obj, Vector3 lookAt);
    public abstract bool Execute();  
}
