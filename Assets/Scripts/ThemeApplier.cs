using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeApplier : MonoBehaviour
{
    // Start is called before the first frame update
    public static int choosenTheme;

    public Color colorOfAllText;

    public GameObject objectOfI;
    public GameObject objectOfJ;
    public GameObject objectOfL;
    public GameObject objectOfO;
    public GameObject objectOfS;
    public GameObject objectOfT;
    public GameObject objectOfZ;
    public GameObject objectOfPlayfield;
    public GameObject objectOfHoldArea;

    public class Theme
    {
        public string name;
        public string maker;
        public Color colorOfI;
        public Color colorOfJ;
        public Color colorOfL;
        public Color colorOfO;
        public Color colorOfS;
        public Color colorOfT;
        public Color colorOfZ;
        public Color colorOfPlayfield;
        public Color colorOfHoldArea;


        public Theme(string name, string maker, Color colorOfI, Color colorOfJ, Color colorOfL, Color colorOfO, Color colorOfS, Color colorOfT, Color colorOfZ, Color colorOfPlayfield, Color colorOfHoldArea)
        {
            this.name = name;
            this.maker = maker;
            this.colorOfI = colorOfI;
            this.colorOfJ = colorOfJ;
            this.colorOfL = colorOfL;
            this.colorOfO = colorOfO;
            this.colorOfS = colorOfS;
            this.colorOfT = colorOfT;
            this.colorOfZ = colorOfZ;
            this.colorOfPlayfield = colorOfPlayfield;
            this.colorOfHoldArea = colorOfHoldArea;
        }
    }

    public static List<Theme> themeList = new List<Theme>();

    void Start()
    {
        setTheme(themeList[choosenTheme]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    Color hexToRgba(string hex)
    {
        Color rgba;
        ColorUtility.TryParseHtmlString(hex, out rgba);
        return rgba;
    }

    void setColorOfPrefab(GameObject prefab, Color color)
    {
        foreach (Transform item in prefab.transform)
        {
            item.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    void setTheme(Theme theme)
    {

        setColorOfPrefab(objectOfI, theme.colorOfI);
        setColorOfPrefab(objectOfJ, theme.colorOfJ);
        setColorOfPrefab(objectOfL, theme.colorOfL);
        setColorOfPrefab(objectOfO, theme.colorOfO);
        setColorOfPrefab(objectOfS, theme.colorOfS);
        setColorOfPrefab(objectOfT, theme.colorOfT);
        setColorOfPrefab(objectOfZ, theme.colorOfZ);

        objectOfPlayfield.GetComponent<SpriteRenderer>().color = theme.colorOfPlayfield;
        objectOfHoldArea.GetComponent<SpriteRenderer>().color = theme.colorOfHoldArea;

    }
}
