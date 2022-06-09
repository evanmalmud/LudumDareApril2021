using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesScript : SerializedMonoBehaviour
{

    public Dictionary<UpgradeScriptableObject.BackgroundType, GameObject> backgrounds;

    [ValueDropdown("backgrounds")]
    [OnValueChanged("updateBackground")]
    public KeyValuePair<UpgradeScriptableObject.BackgroundType, GameObject> background;


    public Dictionary<UpgradeScriptableObject.SymbolType, GameObject> symbols;

    [ValueDropdown("symbols")]
    [OnValueChanged("updateSymbols")]
    public KeyValuePair<UpgradeScriptableObject.SymbolType, GameObject> symbol;

    public Dictionary<UpgradeScriptableObject.ArrowType, GameObject> arrows;

    [ValueDropdown("arrows")]
    [OnValueChanged("updateArrows")]
    public KeyValuePair<UpgradeScriptableObject.ArrowType, GameObject> arrow;

    public UpgradeScriptableObject scriptObject;

    private void OnEnable()
    {
        if(scriptObject != null)
        {
            if (backgrounds.ContainsKey(scriptObject.backgroundType))
            {
                background = new KeyValuePair<UpgradeScriptableObject.BackgroundType, GameObject>(scriptObject.backgroundType, backgrounds[scriptObject.backgroundType]);
            }
            if (symbols.ContainsKey(scriptObject.symbolType))
            {
                symbol = new KeyValuePair<UpgradeScriptableObject.SymbolType, GameObject>(scriptObject.symbolType, symbols[scriptObject.symbolType]);
            }
            if (arrows.ContainsKey(scriptObject.arrowType))
            {
                arrow = new KeyValuePair<UpgradeScriptableObject.ArrowType, GameObject>(scriptObject.arrowType, arrows[scriptObject.arrowType]);
            }
        }
        updateGameObjects();
    }

    public void updateGameObjects()
    {
        updateBackground();
        updateSymbols();
        updateArrows();
    }

    private void updateBackground()
    {
        foreach (KeyValuePair<UpgradeScriptableObject.BackgroundType, GameObject> go in backgrounds)
        {
            if (go.Value != null)
            {
                go.Value.SetActive(false);
            }
        }
        if(background.Value != null)
        {
            background.Value.SetActive(true);
        }
    }

    private void updateSymbols()
    {
        foreach (KeyValuePair<UpgradeScriptableObject.SymbolType, GameObject> go in symbols)
        {
            if (go.Value != null)
            {
                go.Value.SetActive(false);
            }
        }
        if (symbol.Value != null)
        {
            symbol.Value.SetActive(true);
        }
    }

    private void updateArrows()
    {
        foreach (KeyValuePair<UpgradeScriptableObject.ArrowType, GameObject> go in arrows)
        {
            if (go.Value != null)
            {
                go.Value.SetActive(false);
            }
        }
        if (arrow.Value != null)
        {
            arrow.Value.SetActive(true);
        }
    }




}
