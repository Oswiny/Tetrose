using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject I;
    public GameObject J;
    public GameObject L;
    public GameObject O;
    public GameObject S;
    public GameObject T;
    public GameObject Z;

    public GameObject spawnedPiece;
    List<GameObject> bag;
    List<GameObject> currentlyShownPieces;

    void Start()
    {

        bag = createBag();
        currentlyShownPieces = showPieces();

        spawnedPiece = spawnPieceTop(bag[0]);



    }

    private void Update()
    {
        if (spawnedPiece.GetComponent<Movement>().isLocked == true)
        {
            bag.RemoveAt(0);
            currentlyShownPieces = showPieces();
            spawnedPiece = spawnPieceTop(bag[0]);
            canUseHold = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            printBag(bag);
        }

        hold();
    }

    GameObject currentlyHoldPiece;
    GameObject holdPiece;
    bool canUseHold = true;
    bool isHoldEmpty = true;
    GameObject shownCurrentlyHoldPiece;
    void hold()
    {
        if (Input.GetKeyDown(KeyCode.H) && canUseHold)
        {
            Destroy(spawnedPiece);

            if (isHoldEmpty)
            {
                currentlyHoldPiece = getOriginalPiece(spawnedPiece);

                bag.RemoveAt(0);
                spawnedPiece = spawnPieceTop(bag[0]);

                showPieces();
                isHoldEmpty = false;
            }
            else
            {
                //Debug.Log("else worked");

                holdPiece = getOriginalPiece(spawnedPiece);
                spawnedPiece = spawnPieceTop(currentlyHoldPiece);
                currentlyHoldPiece = getOriginalPiece(holdPiece);
            }


            spawnedPiece.GetComponent<Movement>().enabled = true;

            Destroy(shownCurrentlyHoldPiece);
            shownCurrentlyHoldPiece = Instantiate(currentlyHoldPiece, new Vector3(10f, 11.5f, 0f), currentlyHoldPiece.transform.rotation);


            canUseHold = false;
        }
    }

    public GameObject getOriginalPiece(GameObject piece)
    {
        if (piece.name[0] == 'I')
        {
            return I;
        }
        else if (piece.name[0] == 'J')
        {
            return J;
        }
        else if (piece.name[0] == 'L')
        {
            return L;
        }
        else if (piece.name[0] == 'O')
        {
            return O;
        }
        else if (piece.name[0] == 'S')
        {
            return S;
        }
        else if (piece.name[0] == 'T')
        {
            return T;
        }
        else if (piece.name[0] == 'Z')
        {
            return Z;
        }
        return null;
    }




    // THIS IS SO SHIT
    List<GameObject> createBag()
    {
        GameObject empty = new GameObject();
        List<GameObject> pieces = new List<GameObject>() { I, J, L, O, S, T, Z };
        int numPieces = pieces.Count;
        List<GameObject> bag = new List<GameObject>() { empty, empty, empty, empty, empty, empty };
        for (int i = 0; i < numPieces - 1; i++)
        {
            int rnd = Random.Range(0, numPieces - 1);
            while (true)
            {
                if (bag[i] == empty && !(bag.Contains(pieces[rnd])))
                {
                    bag[i] = pieces[rnd];
                    break;
                }
                else
                {
                    rnd = Random.Range(0, numPieces - 1);
                }
            }
        }
        return bag;
    }


    void printBag(List<GameObject> bag)
    {
        foreach (GameObject piece in bag)
        {
            Debug.Log(piece);
        }
    }

    GameObject spawnPieceTop(GameObject piece)
    {

        UnityEngine.Vector3 pos = new UnityEngine.Vector3(0, 11, 0);
        pos += getOffset(piece);
        GameObject instantiatedObject = Instantiate(piece, pos, piece.transform.rotation);

        instantiatedObject.GetComponent<Movement>().enabled = true;

        return instantiatedObject;
    }








    int numOfShownPieces;

    List<GameObject> instantiatedPieces = new List<GameObject>();
    List<GameObject> showPieces()
    {

        instantiatedPieces.ForEach(item => Destroy(item));
        instantiatedPieces = new List<GameObject>();

        if (bag.Count > 3)
        {
            for (int i = 1; i < 4; i++)
            {
                Vector3 pos = new Vector3(10, 8 - ((i) * 4), 0) + getOffset(bag[i]);
                GameObject shownPiece = Instantiate(bag[i], pos, bag[i].transform.rotation);
                instantiatedPieces.Add(shownPiece);
            }
        }
        else if (bag.Count <= 3)
        {
            addToBag();
            showPieces();
        }


        return instantiatedPieces;
    }

    UnityEngine.Vector3 getOffset(GameObject piece)
    {
        UnityEngine.Vector3 offset = UnityEngine.Vector3.zero;
        if (piece.name == "J" || piece.name == "L" || piece.name == "S" || piece.name == "Z" || piece.name == "T")
        {
            offset = new UnityEngine.Vector3(-0.5f, 0.5f, 0);
        }
        else if (piece.name == "O")
        {
            offset = new UnityEngine.Vector3(0f, 1f, 0f);
        }

        return offset;
    }


    List<GameObject> addToBag()
    {
        List<GameObject> newBag = createBag();
        printBag(newBag);
        foreach (GameObject newPiece in newBag)
        {
            bag.Add(newPiece);
        }
        return bag;
    }

}
