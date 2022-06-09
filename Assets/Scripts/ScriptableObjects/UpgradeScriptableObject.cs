using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/UpgradeScriptableObject", order = 1)]
public class UpgradeScriptableObject : ScriptableObject
{

    public GameState.DepthType depthType;

    public BackgroundType backgroundType;

    public SymbolType symbolType;

    public ArrowType arrowType;

    public enum BackgroundType
    {
        BRONZE_HEX,
        SILVER_HEX,
        PURPLE_HEX,
        NONE,
        SILVER_SQUARE_WIRED,
        SILVER_SQUARE,
        SILVER_CIRCLE,
        SILVER_SQUARE_WIRED_NOBOX,
        SILVER_SQUARE_NOBOX,
        SILVER_CIRCLE_NOBOX_BLUE,
        SILVER_CIRCLE_NOBOX_LIGHTGRAY,
        SILVER_CIRCLE_NOBOX_DARKGRAY
    };

    public enum SymbolType
    {
        DRILL,
        JETPACK,
        O2,
        RADAR,
        SPEED,
        SHEILD
    };

    public enum ArrowType
    {
        RED_3,
        BLUE_3,
        GREEN_3,
        GOLD_3,
        GRAY_3,
        BLUE_2,
        RED_2,
        GREEN_2,
        GOLD_2,
        GRAY_2,
        BLUE_PLUS,
        RED_PLUS,
        BLUE_PLUS_SMALL
    };

}