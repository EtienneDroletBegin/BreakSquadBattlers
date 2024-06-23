using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color baseMat;
    private Vector2 Coords;
    private List<Tile> neighbors = new List<Tile>();

    public int belongsTo;
    public bool lit = false;

    public Tile PreviousTile;

    public float gCost;
    public float hCost;
    public float fCost;


    public List<Tile> GetNeighbors() { return neighbors; }
    public Vector2 GetCoords() { return Coords; }

    private void Start()
    {
        baseMat = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        Unhover();
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }


    public void AddNeighbor(Tile newNeighbor)
    {
        neighbors.Add(newNeighbor);
    }
    public void SetCoords(Vector2 _coords)
    {
        Coords = _coords;
    }
    public void Hover()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
    public void Unhover()
    {
        GetComponent<Renderer>().material.color = baseMat;
    }
    public void ChangeText(string _String)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = _String;
    }
}
