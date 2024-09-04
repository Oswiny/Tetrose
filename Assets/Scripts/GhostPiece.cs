using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GhostPiece : MonoBehaviour
{
    public Color ghostColor;


    // Start is called before the first frame update



    List<GameObject> pieceObjects;



    void Start()
    {
        Movement movement = new Movement();
        pieceObjects = movement.getPieceObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject oldGhostPiece;
    public void updateGhostPiece(GameObject ghostPieceR, Transform ghostPieceO)
    {
        Destroy(oldGhostPiece);
        GameObject ghostPiece = Instantiate(ghostPieceR, ghostPieceO.position, ghostPieceO.rotation);
        ghostPiece.GetComponent<Movement>().enabled = false;
        foreach (Transform ghostCubeTransform in ghostPiece.transform)
        {
            ghostCubeTransform.GetComponent<SpriteRenderer>().color = ghostColor;
        }



        pushGhost(ghostPiece);
        oldGhostPiece = ghostPiece;
    }

    void pushGhost(GameObject ghostPiece)
    {

        List<Vector3> posList = getPosListOfCubes(ghostPiece.transform);
        float minY = posList.Select(item => item.y).Min();

        while (!(((-10.5f) >= (minY - 1f)) || isInContactWithOtherCubes(ghostPiece.transform)))
        {
            ghostPiece.transform.position += Vector3.down;
            posList = getPosListOfCubes(ghostPiece.transform);
            minY = posList.Select(item => item.y).Min();
        }
    }




    bool isInContactWithOtherCubes(Transform ghostPiece)
    {
        List<Vector3> posList = getPosListOfCubes(ghostPiece);

        for (int i = 0; i < posList.Count; i++)
        {
            posList[i] += Vector3.down;
        }

        for (int i = 0; i < pieceObjects.Count; i++)
        {
            for (int j = 0; j < posList.Count; j++)
            {
                if (pieceObjects[i].transform.position == posList[j])
                {
                    //Debug.Log("TOUCHING ANOTHER CUBE");
                    return true;
                }
            }
        }

        return false;
    }


    List<Vector3> getPosListOfCubes(Transform ghostPiece)
    {
        List<Vector3> posOfCubes = new List<Vector3>();
        foreach (Transform cube in ghostPiece)
        {
            posOfCubes.Add(cube.position);
        }

        // returns a list of positions of child objects
        return posOfCubes;
    }
}
