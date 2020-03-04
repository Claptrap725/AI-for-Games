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
    [HideInInspector]
    public Vector3 pathTarget = Vector3.zero;
    public float pathTimer = 0;

    private void Awake()
    {
        aISteering = GetComponent<AISteering>();
        pathFollower = GetComponent<PathFollower>();
    }

    void Update()
    {
        pathTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        if (pathFollower.pathFinished) pathfindReady = true;
        //if (pathTimer <= 0) pathfindReady = true;

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
        result = Vector3.Distance(other.transform.position, Steve.instance.transform.position) < 3;
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
            Steve.instance.pathTimer = 5f;
            swordsman.pathfindReady = false;
            if (Steve.instance.atHome)
            {
                if (swordsman.pathTarget == Vector3.zero || swordsman.pathFollower.pathFinished)
                {
                    swordsman.pathTarget = new Vector3(Random.Range(-474f, -201f), 28, Random.Range(-208f, 68f));
                }

                swordsman.NewPath(AStar.instance.GetPath(swordsman.transform.position, swordsman.pathTarget));
                
                if (swordsman.pathFollower.pathIterator >= swordsman.pathFollower.path.points.Count - 3)
                {
                    swordsman.pathTarget = Vector3.zero;
                }
                //Collider[] walls = Physics.OverlapSphere(swordsman.aISteering.agent.heading.normalized * 0.4f + swordsman.transform.position + Vector3.up * 0.2f, 1.8f);
                //for (int i = 0; i < walls.Length; i++)
                //{
                //    if (walls[i].tag == "Wall")
                //    {
                //        swordsman.pathTarget = Vector3.zero;
                //        swordsman.aISteering.agent.ApplyForce(swordsman.aISteering.agent.heading * -10);
                //        break;
                //    }
                //}
                
            }
            else
            {
                swordsman.NewPath(AStar.instance.GetPath(swordsman.transform.position, Steve.instance.transform.position));
            }
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