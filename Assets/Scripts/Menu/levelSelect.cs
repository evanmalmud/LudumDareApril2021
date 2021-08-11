using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
    public Material hoverMaterial;

    public Image asteroidOneSprite;
    public Image asteroidOneArrow;
    public TextMeshProUGUI asteroidOneText;

    public Image asteroidTwoSprite;
    public Image asteroidTwoArrow;
    public TextMeshProUGUI asteroidTwoText;

    public Image asteroidThreeSprite;
    public Image asteroidThreeArrow;
    public TextMeshProUGUI asteroidThreeText;


    public Color defaultColor;
    public Color highlightColor;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        asteroidOneSprite.material = null;
        asteroidOneArrow.material = null;
        asteroidTwoSprite.material = null;
        asteroidTwoArrow.material = null;
        asteroidThreeSprite.material = null;
        asteroidThreeArrow.material = null;
        asteroidOneText.color = defaultColor;
        asteroidTwoText.color = defaultColor;
        asteroidThreeText.color = defaultColor;
    }

    // Update is called once per frame
    public void setAstroidOneHover(bool enabled) {
        if(enabled) {
            asteroidOneSprite.material = hoverMaterial;
            asteroidOneArrow.material = hoverMaterial;
            asteroidOneText.color = highlightColor;
        } else {
            asteroidOneSprite.material = null;
            asteroidOneArrow.material = null;
            asteroidOneText.color = defaultColor;
        }
    }

    public void setAstroidTwoHover(bool enabled)
    {
        if (enabled) {
            asteroidTwoSprite.material = hoverMaterial;
            asteroidTwoArrow.material = hoverMaterial;
            asteroidTwoText.color = highlightColor;
        } else {
            asteroidTwoSprite.material = null;
            asteroidTwoArrow.material = null;
            asteroidTwoText.color = defaultColor;
        }
    }

    public void setAstroidThreeHover(bool enabled)
    {
        if (enabled) {
            asteroidThreeSprite.material = hoverMaterial;
            asteroidThreeArrow.material = hoverMaterial;
            asteroidThreeText.color = highlightColor;
        } else {
            asteroidThreeSprite.material = null;
            asteroidThreeArrow.material = null;
            asteroidThreeText.color = defaultColor;
        }
    }
}
