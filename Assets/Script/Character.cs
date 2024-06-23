using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public struct Stats
{
    float PhysDMG;
    float MagDmg;
    float BaseHP;
    float HPMod;
    float PhysRes;
    float MagRes;
    float MaxEnergy;
}


public class Character : MonoBehaviour
{
    bool left = false;
    [SerializeField] private Grid grid;
    [SerializeField] private Vector2 StartTile;
    public Tile currentTile;
    private Coroutine CurrentMovement;
    private List<Tile> path;
    private NavMeshAgent ai;
    private bool grabbed;

    private void Awake()
    {
        TriggerWatcher.Instance().StartListening(ETriggers.ONGRIDLOADED, Init);
        ai = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (grabbed)
        {
            CheckUnder();
        }
    }

    private void Init(Dictionary<string, object> eventParams)
    {
        currentTile = grid.grid[StartTile];
        transform.position = currentTile.transform.position;
        Vector2 baseTile = currentTile.GetCoords();
        Vector2 furthestTile = currentTile.GetCoords();
        for(int i = 0; i <= grid.grid.Count; i++)
        {
            Vector2 newTile = baseTile + new Vector2(i, 0);
            if (grid.grid.TryGetValue(newTile, out Tile value))
            {
                furthestTile = newTile;
            }
            else
            {
                break;
            }
        }
        List<Tile> path = GetComponent<Pathfinding>().FindPath(currentTile.GetCoords(), furthestTile);
        //Move(path);

    }

    public void Move(List<Tile> _path)
    {
        if(CurrentMovement != null)
        {
            StopCoroutine(CurrentMovement);

        }
        path = new List<Tile>();
        path = _path;

        CurrentMovement = StartCoroutine("followPath");
    }

    public void BeGrabbed()
    {
        
        grabbed = true;
        gameObject.layer = 2;
    }
    public void BeUnGrabbed()
    {

        grabbed = false;
        gameObject.layer = 6;
        transform.position = currentTile.transform.position;
    }

    private void CheckUnder()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Tile>())
            {
                hit.collider.GetComponent<Tile>().Hover();
                currentTile = hit.collider.GetComponent<Tile>();
            }
        }
    }

    private void ReversePath()
    {
        Vector2 baseTile = currentTile.GetCoords();
        Vector2 furthestTile = currentTile.GetCoords();
        if (left)
        {
            for (int i = 0; i <= grid.grid.Count; i--)
            {
                Vector2 newTile = baseTile + new Vector2(i, 0);
                if (grid.grid.TryGetValue(newTile, out Tile value))
                {
                    furthestTile = newTile;
                    print(newTile);
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i <= grid.grid.Count; i++)
            {
                Vector2 newTile = baseTile + new Vector2(i, 0);
                if (grid.grid.TryGetValue(newTile, out Tile value))
                {
                    furthestTile = newTile;
                    print(newTile);
                }
                else
                {
                    break;
                }
            }
        }

        List<Tile> path = GetComponent<Pathfinding>().FindPath(currentTile.GetCoords(), furthestTile);
        Move(path);
    }

    private IEnumerator followPath()
    {
        while (path.Count > 0)
        {
            ai.SetDestination(path[0].transform.position);
            currentTile.Hover();
            while (!ai.isStopped && ai.remainingDistance > ai.stoppingDistance)
            {
                yield return null;
            }
            currentTile.Unhover();
            currentTile = path[0];
            
            path.RemoveAt(0);
        }
        yield return null;
    }

}
