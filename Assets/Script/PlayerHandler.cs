using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    private Character GrabbedCharacter;
    [SerializeField] private Character chara;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Character>())
                {
                    GrabbedCharacter = hit.collider.GetComponent<Character>();
                    GrabbedCharacter.BeGrabbed();
                }
                else
                {
                    if (hit.collider.GetComponent<Tile>())
                    {
                        List<Tile> path = chara.GetComponent<Pathfinding>().FindPath(chara.currentTile.GetCoords(), hit.collider.GetComponent<Tile>().GetCoords());
                        chara.Move(path);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(GrabbedCharacter != null)
            {
                GrabbedCharacter.BeUnGrabbed();
            }
            GrabbedCharacter = null;
        }


        if (GrabbedCharacter != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GrabbedCharacter.transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
            }
        }
    }
}
