using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //public static List<Vector3> pieceLocations = new List<Vector3>();
    public static List<GameObject> pieceObjects = new List<GameObject>();
    public static List<Vector3> borderLocations = new List<Vector3>();
    public bool isLocked;
    public float lockDelay;



    public ClearingAndPoints clearScript;
    public GhostPiece ghost;
    public Spawner spawner;
    public ModeSettings modeSettings;
    public Death death;



    float frameCount = 0;

    public bool isGravityEnabled;

    // Start is called before the first frame update
    void Start()
    {
        ghost = FindObjectOfType<GhostPiece>();

        spawner = FindObjectOfType<Spawner>();

        clearScript = FindObjectOfType<ClearingAndPoints>();

        modeSettings = FindObjectOfType<ModeSettings>();

        death = FindObjectOfType<Death>();

        isGravityEnabled = modeSettings.gravity;


        Application.targetFrameRate = 60;
        //pieceLocations.Add(new Vector3(3.5f, -3.5f, 0));
    }
    bool isAllowedToMove = true;
    // Update is called once per frame


    float timeA = 0f;
    float timeLimitA = 0.1f;

    float timeS = 0f;
    float timeLimitS = 0.1f;

    float timeD = 0f;
    float timeLimitD = 0.1f;
    void Update()
    {
        if (!isLocked && !EscapeMenu.isPaused)
        {

            if (Input.GetKey(KeyCode.D) && timeD >= timeLimitD)
            {
                timeD = 0;
                moveToIfAllowed(Vector3.right);
            }
            if (Input.GetKey(KeyCode.A) && timeA >= timeLimitA)
            {
                timeA = 0;
                moveToIfAllowed(Vector3.left);
            }
            if (Input.GetKey(KeyCode.S) && timeS >= timeLimitS)
            {
                timeS = 0;
                float prevYPos = transform.position.y;
                moveToIfAllowed(Vector3.down);
                float currYPos = transform.position.y;

                if (prevYPos != currYPos)
                {
                    clearScript.pointsUpdaterOnDrop(1, 1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotateToIfAllowed("L");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                rotateToIfAllowed("R");
            }


            if (isGravityEnabled)
            {
                int currentLevel = clearScript.currentLevel;
                //I dont really like how this works.
                float framesReqForFall = (float)Math.Max(60 / (Math.Pow((0.2 * currentLevel + 1), 2)), 1);



                if (frameCount > framesReqForFall)
                {
                    while (frameCount > framesReqForFall)
                    {
                        moveToIfAllowed(Vector3.down);
                        frameCount -= framesReqForFall;
                    }

                    frameCount = 0;
                }
                else
                {
                    frameCount++;
                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                int cellsFellFor = pushBottom();
                clearScript.pointsUpdaterOnDrop(2, cellsFellFor);
            }

            //ghostPiece.updateGhostPiece(gameObject);

            timeA += Time.deltaTime;
            timeS += Time.deltaTime;
            timeD += Time.deltaTime;

            ghost.updateGhostPiece(spawner.getOriginalPiece(gameObject), gameObject.transform);
            checkLock();

        }
        else if (isLocked)
        {
            if (gameObject.name == "DeleteBorderCube")
            {
                foreach (Transform cube in transform)
                {
                    borderLocations.Add(transform.position);
                    Destroy(gameObject);
                    GetComponent<Movement>().enabled = false;
                }
            }
            else
            {
                foreach (Transform cube in transform)
                {

                    cube.transform.position = new Vector3(roundOneAfterDot(cube.position.x), roundOneAfterDot(cube.position.y), roundOneAfterDot(cube.position.z));
                    pieceObjects.Add(cube.gameObject);
                    GetComponent<Movement>().enabled = false;
                }

                clearScript.Clearing();
                death.checkYLimit(gameObject);
            }
            

        }


    }

    void gravFall()
    {

    }


    float roundOneAfterDot(float number)
    {
        return (float)Math.Round((double)number, 1);
    }


    float timeWithoutMovingAndRotating = 0f;
    void checkLock()
    {
        List<Vector3> lowestCubes = findLowestCubes(getPosListOfCubes());

        bool isUnderFull = hasCubeUnder(lowestCubes);


        //checks for any movement key
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            timeWithoutMovingAndRotating = 0f;
        }
        else//if there isnt movement keeps timer going
        {
            timeWithoutMovingAndRotating += Time.deltaTime;
        }

        //if under of our piece is filled and we stayed without move inputs long enough it locks the piece in place
        if (isUnderFull && (timeWithoutMovingAndRotating >= lockDelay))
        {
            isLocked = true;
        }


    }

    List<Vector3> findLowestCubes(List<Vector3> cubes)
    {
        float currentMin = (cubes.Select(item => item.y)).Min();

        List<Vector3> lowestCubes = new List<Vector3>();

        foreach (Vector3 cube in cubes)
        {
            if (cube.y == currentMin)
            {
                lowestCubes.Add(cube);
            }
        }
        //out of list of cube positions finds lowest of them and returns them in a list
        return lowestCubes;
    }

    public List<Vector3> getPosListOfCubes()
    {
        List<Vector3> posOfCubes = new List<Vector3>();
        foreach (Transform cube in transform)
        {
            posOfCubes.Add(cube.position);
        }

        // returns a list of positions of child objects
        return posOfCubes;
    }

    bool hasCubeUnder(List<Vector3> cubes)
    {
        foreach (Vector3 cube in cubes)
        {
            for (int i = 0; i < pieceObjects.Count; i++)
            {
                if ((pieceObjects[i].transform.position == (cube - Vector3.up)) || ((-10.5f) >= (cube.y - 1f)) || isInContactWithOtherCubes())
                {
                    //returns true when piece either hits a cube under it or comes in contact with border
                    return true;
                }
            }

            if (pieceObjects.Count == 0 && ((-10.5f) >= (cube.y - 1f)))
            {
                return true;
            }
        }
        //returns false when it doesnt hit anything or didnt came to border
        return false;
    }


    bool isInContactWithOtherCubes()
    {
        List<Vector3> posList = getPosListOfCubes();

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


    //it should not work but it does ??????????????? switch to v2 based version if it doesnt start working properly
    int pushBottom()
    {
        List<Vector3> lowestCubes = findLowestCubes(getPosListOfCubes());
        bool isUnderFull = hasCubeUnder(lowestCubes);
        int cellsFellFor = 0;
        while (!isUnderFull)
        {
            transform.position = transform.position - Vector3.up;
            lowestCubes = findLowestCubes(getPosListOfCubes());
            isUnderFull = hasCubeUnder(lowestCubes);
            cellsFellFor++;
        }

        //locks the piece after completely pushing it
        isLocked = true;
        return cellsFellFor;
    }

    /*
    //push bottom v2
    int pushBottom()
    {
        int cellsFellFor = 0;
        while (!isInContactWithOtherCubes())
        {
            transform.position = transform.position - Vector3.up;
            cellsFellFor++;
        }

        isLocked = true;
        return cellsFellFor;
    }
    */











    public List<GameObject> getPieceObjects()
    {
        return pieceObjects;
    }

    void moveToIfAllowed(Vector3 move)
    {
        foreach (Transform cube in transform)
        {
            bool result = compareAnyElementExistsOfAinB(new List<Vector3>() { cube.position + move }, (pieceObjects.Select(item => item.transform.position).ToList()).Concat(borderLocations).ToList());
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



            bool result = compareAnyElementExistsOfAinB(rotatedAndKicked, (pieceObjects.Select(item => item.transform.position).ToList()).Concat(borderLocations).ToList());




            if (!result)
            {
                //Debug.Log(" INSIDE THE IF " + i);

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
