using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : MonoBehaviour
{
    [HideInInspector]
    public AISteering aISteering;
    [HideInInspector]
    public PathFollower pathFollower;


    public bool attackReady { get { return attackTimer <= 0; } set { if (value) attackTimer = 0; else attackTimer = attackCoolDown; } }
    float attackTimer = 0;
    readonly float attackCoolDown = 0.5f;

    [HideInInspector]
    public bool pathfindReady = true;

    private void Awake()
    {
        aISteering = GetComponent<AISteering>();
        pathFollower = GetComponent<PathFollower>();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (pathFollower.pathFinished) pathfindReady = true;
        Decide();
    }

    public void NewPath(Path newPath)
    {
        pathFollower.NewPath(newPath);
    }

    void Decide()
    {
        IDecision decision = new SteveInMazeDecision(
                                new SteveCloseDecision(gameObject,
                                    new AttackAnswer(this),
                                    new PathfindAnswer(this)),
                                new SteveCloseDecision(gameObject,
                                    new AttackAnswer(this),
                                    new PursueAnswer(this)));

        while (decision != null)
        {
            decision = decision.MakeDecision();
        }
    }
}


// Enemy Decisions
public class SteveCloseDecision : BianaryDecision, IDecision
{
    public SteveCloseDecision(GameObject other, IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
        result = Vector3.Distance(other.transform.position, Steve.instance.transform.position) < 1;
    }
}

public class SteveInMazeDecision : BianaryDecision, IDecision
{
    public SteveInMazeDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
        result = Steve.instance.inMaze;
    }
}

// Enemy Answers
public class PursueAnswer : BianaryAnswer, IDecision
{
    Swordsman swordsman;
    public PursueAnswer(Swordsman _swordsman)
    {
        swordsman = _swordsman;
    }

    public override void Do()
    {
        swordsman.pathfindReady = true;
        if (!swordsman.aISteering.seekTargets.Contains(Steve.instance.gameObject))
            swordsman.aISteering.seekTargets.Add(Steve.instance.gameObject);
    }
}

public class PathfindAnswer : BianaryAnswer, IDecision
{
    Swordsman swordsman;
    public PathfindAnswer(Swordsman _swordsman)
    {
        swordsman = _swordsman;
    }

    public override void Do()
    {
        swordsman.aISteering.seekTargets.Clear();
        if (swordsman.pathfindReady)
        {
            swordsman.pathfindReady = false;
            swordsman.NewPath(AStar.instance.GetPath(swordsman.transform.position, Steve.instance.transform.position));
        }
    }
}

public class AttackAnswer : BianaryAnswer, IDecision
{
    Swordsman swordsman;
    public AttackAnswer(Swordsman _swordsman)
    {
        swordsman = _swordsman;
    }

    public override void Do()
    {
        if (swordsman.attackReady)
        {
            swordsman.attackReady = false;
            Steve.instance.health--;
        }
        else
        {
            new PursueAnswer(swordsman);
        }
    }
}