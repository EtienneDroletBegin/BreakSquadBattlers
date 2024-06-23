using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Grid grid;
    float G = 1;

    private List<Tile> openList;
    private List<Tile> closedList;

    public List<Tile> FindPath(Vector2 start, Vector2 end)
    {
        Tile Startpoint = grid.grid[start];
        openList = new List<Tile> { Startpoint };
        closedList = new List<Tile>();

        foreach(Tile t in grid.grid.Values)
        {
            t.gCost = int.MaxValue;
            t.CalculateFCost();
            t.PreviousTile = null;
        }

        grid.grid[start].gCost = 0;
        grid.grid[start].hCost = CalculateDist(grid.grid[start], grid.grid[end]);
        grid.grid[start].CalculateFCost();

        while(openList.Count > 0)
        {
            Tile currentTile = getLowestFCostTile(openList);
            if (currentTile == grid.grid[end])
            {
                //Done   
                return CalculatePath(grid.grid[end]);
            }
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach(Tile t in currentTile.GetNeighbors())
            {
                if (closedList.Contains(t)) { continue; }

                float TentativeGCost = currentTile.gCost + CalculateDist(currentTile, t);
                if(TentativeGCost < t.gCost)
                {
                    t.PreviousTile = currentTile;
                    t.gCost = TentativeGCost;
                    t.hCost = CalculateDist(t, grid.grid[end]);
                    t.CalculateFCost();

                    if (!openList.Contains(t))
                    {
                        openList.Add(t);
                    }
                }
            }
        }

        return null;
    }

    private List<Tile> CalculatePath(Tile end)
    {
        List<Tile> path = new List<Tile>();
        path.Add(end);
        Tile currentTile = end;
        while(currentTile.PreviousTile != null)
        {
            path.Add(currentTile.PreviousTile);
            currentTile = currentTile.PreviousTile;
        }
        path.Reverse();
        return path;
    }


    private float CalculateDist(Tile a, Tile b)
    {
        float xDist = Mathf.Abs(a.GetCoords().x - b.GetCoords().x);
        float yDist = Mathf.Abs(a.GetCoords().y - b.GetCoords().y);
        float remaining = Mathf.Abs(xDist - yDist);

        return 10*Mathf.Min(xDist, yDist) + 10*remaining;
    }

    private Tile getLowestFCostTile(List<Tile>TileList)
    {
        Tile lowestCost = TileList[0];
        for (int i = 1; i<TileList.Count; i++)
        {
            if (TileList[i].fCost < lowestCost.fCost)
            {
                lowestCost = TileList[i];
            }
        }
        return lowestCost;
    }
}
