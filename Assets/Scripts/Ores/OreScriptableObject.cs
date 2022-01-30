using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ore", menuName = "ScriptableObjects/OreScriptableObject", order = 1)]
public class OreScriptableObject : ScriptableObject {

    public GameState.DepthType depthType;

    public OreType oreType;

    public int value;

    public List<Sprite> spriteOptions;

    public enum OreType {
        COMMON,
        RARE,
        LEGENDARY
    };

}