using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using static ThemeApplier;

public class ThemeChanger : MonoBehaviour
{

    public TMP_Text themeName;
    public TMP_Text themeMaker;

    List<ThemeApplier.Theme> themeList = ThemeApplier.themeList;
    

    void Start()
    {
        
        Theme cromatica = new Theme("cromatica", "@jecoxart", hexToRgb("#007fff"), hexToRgb("#24256f"), hexToRgb("#ffb16c"), hexToRgb("#fede5b"), hexToRgb("#56b68b"), hexToRgb("#5f0e52"), hexToRgb("#fd1a43"), hexToRgb("#141218"), rgbToRgba(hexToRgb("#d9d9d9"), 0.09803921568f));
        themeList.Add(cromatica);

        Theme allWhite = new Theme("All White", " ", hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#fbf8fd"), hexToRgb("#141218"), rgbToRgba(hexToRgb("#d9d9d9"), 0.09803921568f));
        themeList.Add(allWhite);



        themeName.text = themeList[index].name;
        themeMaker.text = themeList[index].maker;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(index);
        }
        
    }

    int index = 0;
    public void onClickPrevTheme()
    {
        if (index - 1 < themeList.Count && index - 1 >= 0)
        {
            index--;
            themeName.text = themeList[index].name;
            themeMaker.text = themeList[index].maker;
            ThemeApplier.choosenTheme = index;
        }
        else if (index - 1 < 0)
        {
            index = themeList.Count - 1;
            themeName.text = themeList[index].name;
            themeMaker.text = themeList[index].maker;
            ThemeApplier.choosenTheme = index;
        }
    }

    public void onClickNextTheme()
    {
        if (index + 1 < themeList.Count)
        {
            index++;
            themeName.text = themeList[index].name;
            themeMaker.text = themeList[index].maker;
            ThemeApplier.choosenTheme = index;
        }
        else if (index + 1 >= themeList.Count)
        {
            index = 0;
            themeName.text = themeList[index].name;
            themeMaker.text = themeList[index].maker;
            ThemeApplier.choosenTheme = index;
        }
    }

    Color hexToRgb(string hex)
    {
        Color rgba;
        ColorUtility.TryParseHtmlString(hex, out rgba);
        return rgba;
    }

    Color rgbToRgba(Color rgb, float alpha)
    {
        return new Color(rgb.r, rgb.g, rgb.b, alpha);
        
    }


}
