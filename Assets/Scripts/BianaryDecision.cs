using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecision
{
    IDecision MakeDecision();
}

// Parent classes
public class BianaryDecision : IDecision
{
    protected bool result;
    protected IDecision trueBrach;
    protected IDecision falseBrach;

    public BianaryDecision() { }

    public BianaryDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
    }

    public IDecision MakeDecision()
    {
        if (result == true)
        {
            return trueBrach;
        }
        else
        {
            return falseBrach;
        }
    }
}

public class BianaryAnswer : IDecision
{
    public IDecision MakeDecision()
    {
        Do();
        return null;
    }

    public virtual void Do()
    {

    }
}
