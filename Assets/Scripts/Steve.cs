using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    public GameObject berryDropOff;
    public GameObject home;
    public List<GameObject> berryBushes;

    [HideInInspector]
    public AISteering aISteering;
    [HideInInspector]
    public PathFollower pathFollower;

    public static Steve instance;

    public bool seeEnemy { get; private set; }
    public bool enemyClose { get { return swordsmen.Count > 0; } }
    public bool inMaze { get; private set; }
    public bool basketFull { get { return berries == maxBerries; } }
    private int maxBerries = 10;
    public int health = 10;
    public List<Swordsman> swordsmen = new List<Swordsman>();

    //[HideInInspector]
    public bool pathfindReady = true;
    //[HideInInspector]
    public bool collectingBerries = false;
    [HideInInspector]
    public float berryTimer = 0.5f;
    //[HideInInspector]
    public int berries = 0;

    private void Awake()
    {
        instance = this;
        aISteering = GetComponent<AISteering>();
        pathFollower = GetComponent<PathFollower>();
    }

    private void Start()
    {
        seeEnemy = false;
        inMaze = false;
    }

    private void Update()
    {
        if (pathFollower.pathFinished) pathfindReady = true;
        Decide();
    }

    void Decide()
    {
        IDecision decision = new SeeEnemyDecision(
                                new EnemyCloseDecision(
                                    new FleeAnswer(),
                                    new RunHomeAnswer()),
                                new BasketFullDecision(
                                    new DropOffBerriesAnswer(),
                                    new CollectBerriesAnswer()));
        
        while (decision != null)
        {
            decision = decision.MakeDecision();
        }

    }
}


// Steve Decisions
public class SeeEnemyDecision : BianaryDecision, IDecision
{
    public SeeEnemyDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
        result = Steve.instance.seeEnemy;
    }
}

public class EnemyCloseDecision : BianaryDecision, IDecision
{
    public EnemyCloseDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
        result = Steve.instance.enemyClose;
    }
}

public class BasketFullDecision : BianaryDecision, IDecision
{
    public BasketFullDecision(IDecision _trueBranch, IDecision _falseBranch)
    {
        trueBrach = _trueBranch;
        falseBrach = _falseBranch;
        result = Steve.instance.basketFull;
    }
}

// Steve Answers
public class CollectBerriesAnswer : BianaryAnswer, IDecision
{
    public override void Do()
    {
        if (!Steve.instance.collectingBerries)
        {
            if (Steve.instance.pathfindReady)
            {
                Steve.instance.pathfindReady = false;
                Steve.instance.pathFollower.NewPath(AStar.instance.GetPath(Steve.instance.transform.position, Steve.instance.berryBushes[Random.Range(0, Steve.instance.berryBushes.Count)].transform.position));
            }

            for (int i = 0; i < Steve.instance.berryBushes.Count; i++)
            {
                if (Vector3.Distance(Steve.instance.berryBushes[i].transform.position, Steve.instance.transform.position) < 6)
                {
                    Steve.instance.collectingBerries = true;
                }
            }
        }
        else
        {
            if (Steve.instance.berryTimer <= 0)
            {
                Steve.instance.berryTimer = 0.5f;
                Steve.instance.berries++;
                if (Steve.instance.basketFull)
                {
                    Steve.instance.collectingBerries = false;
                    Steve.instance.pathfindReady = true;
                }
            }
            else
            {
                Steve.instance.berryTimer -= Time.deltaTime;
            }
        }
    }
}

public class RunHomeAnswer : BianaryAnswer, IDecision
{
    public override void Do()
    {
        Steve.instance.collectingBerries = false;
        if (Steve.instance.pathfindReady)
        {
            Steve.instance.pathfindReady = false;
            Steve.instance.pathFollower.NewPath(AStar.instance.GetPath(Steve.instance.transform.position, Steve.instance.home.transform.position));
        }
    }
}

public class DropOffBerriesAnswer : BianaryAnswer, IDecision
{
    public override void Do()
    {
        Steve.instance.collectingBerries = false;
        if (Vector3.Distance(Steve.instance.berryDropOff.transform.position, Steve.instance.transform.position) < 6)
        {
            Steve.instance.berries = 0;
            Steve.instance.pathfindReady = true;
        }
        else if (Steve.instance.pathfindReady)
        {
            Steve.instance.pathfindReady = false;
            Steve.instance.pathFollower.NewPath(AStar.instance.GetPath(Steve.instance.transform.position, Steve.instance.berryDropOff.transform.position));
        }
    }
}

public class FleeAnswer : BianaryAnswer, IDecision
{
    public override void Do()
    {
        Steve.instance.collectingBerries = false;
        Steve.instance.pathfindReady = true;
        for (int i = 0; i < Steve.instance.swordsmen.Count; i++)
        {
            if (!Steve.instance.aISteering.seekTargets.Contains(Steve.instance.swordsmen[i].gameObject))
                Steve.instance.aISteering.seekTargets.Add(Steve.instance.swordsmen[i].gameObject);
        }
    }
}