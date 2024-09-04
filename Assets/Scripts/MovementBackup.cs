/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class Movement : MonoBehaviour
{

    public static List<Vector3> pieceLocations = new List<Vector3>();
    public static List<GameObject> pieceObjects = new List<GameObject>();
    public bool isLocked;

    public ClearingAndPoints clearScript;
    // Start is called before the first frame update
    void Start()
    {
        clearScript = FindObjectOfType<ClearingAndPoints>();
        //pieceLocations.Add(new Vector3(3.5f, -3.5f, 0));
    }
    bool isAllowedToMove = true;
    // Update is called once per frame
    void Update()
    {
        if (!isLocked)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveToIfAllowed(Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveToIfAllowed(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveToIfAllowed(Vector3.down);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotateToIfAllowed("L");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                rotateToIfAllowed("R");
            }


            //debug
            if (Input.GetKeyDown(KeyCode.P))
            {
                foreach (Vector3 pieceLocation in pieceLocations)
                {
                    Debug.Log(pieceLocation);
                }
            }
        }
        else if (isLocked)
        {
            foreach (Transform cube in transform)
            {
                //CHANGE 1
                pieceLocations.Add(cube.position);


                if (gameObject.name == "DeleteBorderCube")
                {
                    Destroy(gameObject);
                }

                clearScript.checkClearableLine();

                GetComponent<Movement>().enabled = false;
            }
        }


    }

    public List<Vector3> getPieceLocations()
    {
        return pieceLocations;
    }

    void moveToIfAllowed(Vector3 move)
    {
        foreach (Transform cube in transform)
        {
            //Debug.Log(cube.localPosition + transform.position);
            //CHANGE 2
            bool result = compareAnyElementExistsOfAinB(new List<Vector3>() { cube.position + move }, pieceLocations);
            if (result)
            {
                isAllowedToMove = false;
            }
        }
        if (isAllowedToMove == true)
        {
            transform.position += move;
        }
        isAllowedToMove = true;
    }


    void rotateToIfAllowed(string rotation)
    {
        List<Vector3> kickTable = getKickTable(rotation);
        List<Vector3> localPosOfChildren = getPosAfterRotation(rotation);

        Vector3 origin = transform.position;

        for (int i = 0; i < kickTable.Count; i++)
        {
            List<Vector3> rotatedAndKicked = new List<Vector3>();
            for (int j = 0; j < transform.childCount; j++)
            {
                rotatedAndKicked.Add(localPosOfChildren[j] + kickTable[i]);
            }

            bool result = compareAnyElementExistsOfAinB(rotatedAndKicked, pieceLocations);




            if (!result)
            {
                Debug.Log(" INSIDE THE IF " + i);

                if (rotation == "L")
                {
                    transform.Rotate(new Vector3(0f, 0f, 90f));
                }
                else if (rotation == "R")
                {
                    transform.Rotate(new Vector3(0f, 0f, -90f));
                }

                transform.position += kickTable[i];
                break;
            }
        }


    }

    bool compareAnyElementExistsOfAinB(List<Vector3> listA, List<Vector3> listB)
    {
        foreach (Vector3 itemA in listA)
        {
            foreach (Vector3 itemB in listB)
            {
                if (itemA == itemB)
                {
                    return true;
                }
            }
        }
        return false;
    }


    List<Vector3> getPosAfterRotation(string rotation)
    {
        GameObject fakePiece = Instantiate(gameObject, transform.position, transform.rotation);
        fakePiece.gameObject.SetActive(false);
        fakePiece.name = "Fake Piece";
        if (rotation == "L")
        {
            fakePiece.transform.Rotate(new Vector3(0f, 0f, 90f));
        }
        else if (rotation == "R")
        {
            fakePiece.transform.Rotate(new Vector3(0f, 0f, -90f));
        }

        List<Vector3> postions = new List<Vector3>();
        foreach (Transform cube in fakePiece.transform)
        {
            postions.Add(cube.position);
        }

        Destroy(fakePiece);
        return postions;

    }


    List<Vector3> getKickTable(string desiredRotation)
    {
        string currentRotation = getCurrentRotation();
        if (currentRotation == "0")
        {
            if (desiredRotation == "L")
            {
                //0->L
                Debug.Log("0->L");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), new Vector3(0f, -2f, 0f), new Vector3(1f, -2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, 2f, 0f), new Vector3(+2f, -1f, 0f) };
                }


            }
            else if (desiredRotation == "R")
            {
                //0->R
                Debug.Log("0->R");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, +1f, 0f), new Vector3(0f, -2f, 0f), new Vector3(-1f, -2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-2f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(1f, 2f, 0f), new Vector3(-2f, -1f, 0f) };
                }
            }
        }
        else if (currentRotation == "L")
        {
            if (desiredRotation == "L")
            {
                //L->2
                Debug.Log("L->2");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, -1f, 0f), new Vector3(0f, 2f, 0f), new Vector3(-1f, +2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(-2f, 0f, 0f), new Vector3(+1f, +2f, 0f), new Vector3(-2f, -1f, 0f) };
                }

            }
            else if (desiredRotation == "R")
            {
                //L->0
                Debug.Log("L->0");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(-2f, 0f, 0f), new Vector3(+1f, -2f, 0f), new Vector3(-2f, +1f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-2f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(-2f, +1f, 0f), new Vector3(+1f, -2f, 0f) };
                }

            }
        }
        else if (currentRotation == "R")
        {
            if (desiredRotation == "L")
            {
                //R->0
                Debug.Log("R->0");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(+1f, -1f, 0f), new Vector3(0f, +2f, 0f), new Vector3(+1f, +2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+2f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(+2f, +1f, 0f), new Vector3(-1f, -2f, 0f) };
                }

            }
            else if (desiredRotation == "R")
            {
                //R->2
                Debug.Log("R->2");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(+1f, -1f, 0f), new Vector3(0f, +2f, 0f), new Vector3(+1f, +2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(+2f, 0f, 0f), new Vector3(-1f, +2f, 0f), new Vector3(+2f, -1f, 0f) };
                }

            }
        }
        else if (currentRotation == "2")
        {
            if (desiredRotation == "L")
            {
                //2->L
                Debug.Log("2->L");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(+1f, 1f, 0f), new Vector3(0f, -2f, 0f), new Vector3(+1f, -2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(+2f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(+2f, +1f, 0f), new Vector3(-1f, -1f, 0f) };
                }

            }
            else if (desiredRotation == "R")
            {
                //2->R
                Debug.Log("2->R");
                if (!(gameObject.name[0] == 'I'))
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(-1f, +1f, 0f), new Vector3(0f, -2f, 0f), new Vector3(-1f, -2f, 0f) };
                }
                else if (gameObject.name[0] == 'I')
                {
                    return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(-2f, 0f, 0f), new Vector3(+1f, 0f, 0f), new Vector3(-2f, +1f, 0f), new Vector3(+1f, -1f, 0f) };
                }

            }
        }
        Debug.Log("returned on last return call.");
        return new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) };
    }

    string getCurrentRotation()
    {
        if (transform.rotation.z == 90 || transform.rotation.z == -270)
        {
            return "L";
        }
        else if (transform.rotation.z == 0)
        {
            return "0";
        }
        else if (transform.rotation.z == -90 || transform.rotation.z == 270)
        {
            return "R";
        }
        else if (transform.rotation.z == 180 || transform.rotation.z == -180)
        {
            return "2";
        }
        return null;
    }



}
*/