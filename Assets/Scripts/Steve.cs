using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    public GameObject berryDropOff;
    public GameObject home;
    public List<GameObject> berryBushes;
    GameObject healthBar;

    [HideInInspector]
    public AISteering aISteering;
    [HideInInspector]
    public PathFollower pathFollower;

    public static Steve instance;

    public bool seeEnemy;
    public bool enemyClose { get { return swordsmen.Count > 0; } }
    public bool inMaze;
    public bool basketFull { get { return berries == maxBerries; } }
    private int maxBerries = 10;
    public int health = 10;
    bool atHome = false;
    public List<Swordsman> swordsmen = new List<Swordsman>();
    public List<GameObject> spawnWhenAtHome = new List<GameObject>();

    [HideInInspector]
    public bool pathfindReady = true;
    [HideInInspector]
    public bool collectingBerries = false;
    [HideInInspector]
    public float berryTimer = 0.5f;
    [HideInInspector]
    public int berries = 0;

    public float pathTimer = 0;

    private void Awake()
    {
        instance = this;
        aISteering = GetComponent<AISteering>();
        pathFollower = GetComponent<PathFollower>();
        healthBar = transform.GetChild(3).gameObject;
    }

    private void Start()
    {
        seeEnemy = false;
        inMaze = false;
    }

    private void Update()
    {
        if (health > 0)
        {
            pathTimer -= Time.deltaTime;
            if (!pathfindReady && pathFollower.pathFinished) pathfindReady = true;
            if (pathTimer <= 0) pathfindReady = true;
            if (!inMaze && transform.position.x < -200) inMaze = true;

            aISteering.fleeTargets.Clear();
            swordsmen.Clear();
            Collider[] colls = Physics.OverlapSphere(transform.position, 20);
            for (int i = 0; i < colls.Length; i++)
            {
                Swordsman man = colls[i].GetComponent<Swordsman>();
                if (man != null)
                {
                    if (!seeEnemy)
                    {
                        seeEnemy = true;
                        pathfindReady = true;
                    }
                    swordsmen.Add(man);
                }
            }

            bool temp = atHome;

            if (Vector3.Distance(transform.position, home.transform.position) < 3)
                atHome = true;

            if (temp != atHome)
            {
                for (int i = 0; i < spawnWhenAtHome.Count; i++)
                {
                    spawnWhenAtHome[i].SetActive(true);
                }
            }

            healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x, healthBar.transform.localScale.y, health / 5f);

            if (!atHome)
                Decide();
        }
        else
        {
            aISteering.agent.alive = false;
            aISteering.rb.freezeRotation = false;
            healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x, healthBar.transform.localScale.y, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 20);
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
                Steve.instance.pathTimer = 10000f;
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
            Steve.instance.pathTimer = 5f;
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
            Steve.instance.pathTimer = 10000f;
            Steve.instance.pathfindReady = false;
            Steve.instance.pathFollower.NewPath(AStar.instance.GetPath(Steve.instance.transform.position, Steve.instance.berryDropOff.transform.position));
        }
    }
}

public class FleeAnswer : BianaryAnswer, IDecision
{
    public override void Do()
    {
        Steve.instance.pathfindReady = true;
        if (Steve.instance.collectingBerries)
        {
            RunHomeAnswer runHomeAnswer = new RunHomeAnswer();
            runHomeAnswer.Do();
        }
        Steve.instance.collectingBerries = false;
        for (int i = 0; i < Steve.instance.swordsmen.Count; i++)
        {
            if (!Steve.instance.aISteering.fleeTargets.Contains(Steve.instance.swordsmen[i].gameObject))
                Steve.instance.aISteering.fleeTargets.Add(Steve.instance.swordsmen[i].gameObject);
        }
    }
}