using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Grid : MonoBehaviour
{


    [SerializeField] private int GridWidth;
    [SerializeField] private int GridHeight;
    [SerializeField] private GameObject TilePrefab;

    public Dictionary<Vector2, Tile> grid;
    private float CellSizeX;
    private float CellSizeY;

    void Start()
    {
        grid = new Dictionary<Vector2, Tile>();
        CellSizeX = TilePrefab.GetComponent<Renderer>().bounds.size.x;
        CellSizeY = TilePrefab.GetComponent<Renderer>().bounds.size.z;
        float CurrentX = 0; float CurrentY = 0;
        float xOffset = 0;

        #region Grid creation

        for (int j = 0; j < GridWidth; j++) {
            xOffset = j % 2 == 0 ? (CellSizeX / 2) : 0;
            CurrentX = 0;
            for (int i = 0; i < GridHeight; i++)
            {
                GameObject NewTile = Instantiate(TilePrefab, transform);
                NewTile.transform.position = new Vector3(CurrentX + xOffset,0, CurrentY);
                NewTile.GetComponent<Tile>().ChangeText(i + ", " + j);
                NewTile.GetComponent<Tile>().SetCoords(new Vector2(i, j));
                grid.Add(new Vector2(i, j), NewTile.GetComponent<Tile>());
                CurrentX += CellSizeX;
            }
            CurrentY += (CellSizeY / 2 + CellSizeY / 4);
        }

        #endregion

        FindNeighbors();
        TriggerWatcher.Instance().TriggerEvent(ETriggers.ONGRIDLOADED, new Dictionary<string, object>());

    }

    private void FindNeighbors()
    {
        foreach(var t in grid)
        {
            Vector2 coord = t.Key;
            bool isOdd = coord.y % 2==0;
            //Side 1
            Vector2 newPos = isOdd ? new Vector2(0, -1) : new Vector2(-1,-1);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }

            //Side 2
            newPos = new Vector2(-1, 0);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }

            //Side 3
            newPos = isOdd ? new Vector2(0, 1) : new Vector2(-1, 1);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }

            //Side 4
            newPos = isOdd ? new Vector2(1, 1) : new Vector2(0, 1);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }

            //Side 5
            newPos = new Vector2(1, 0);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }

            //Side 6
            newPos = isOdd ? new Vector2(1, -1) : new Vector2(0, -1);
            if (grid.ContainsKey(coord + newPos)) { t.Value.AddNeighbor(grid[coord + newPos]); }
        }
    }

}
