using System;
using UnityEngine;

public abstract class FSMState
{
    protected GameObject gameObject;

    protected FSMState(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public abstract void Init();
    public abstract Type Execute();
    public abstract void Exit();
}
