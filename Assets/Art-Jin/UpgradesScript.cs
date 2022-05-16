using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesScript : MonoBehaviour
{

    public List<GameObject> backgrounds;

    [ValueDropdown("backgrounds")]
    [OnValueChanged("updateBackground")]
    public GameObject background;


    public List<GameObject> symbols;

    [ValueDropdown("symbols")]
    [OnValueChanged("updateSymbols")]
    public GameObject symbol;


    public List<GameObject> arrows;

    [ValueDropdown("arrows")]
    [OnValueChanged("updateArrows")]
    public GameObject arrow;

    public void updateGameObjects()
    {
        updateBackground();
        updateSymbols();
        updateArrows();
    }

    private void updateBackground()
    {
        foreach(GameObject go in backgrounds)
        {
            go.SetActive(false);
        }
        if(background != null)
        {
            background.SetActive(true);
        }
    }

    private void updateSymbols()
    {
        foreach (GameObject go in symbols)
        {
            go.SetActive(false);
        }
        if (symbol != null)
        {
            symbol.SetActive(true);
        }
    }

    private void updateArrows()
    {
        foreach (GameObject go in arrows)
        {
            go.SetActive(false);
        }
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }




}
