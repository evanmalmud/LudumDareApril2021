using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "ScriptableObjects/ArtifactScriptableObject", order = 1)]
public class ArtifactScriptableObject : ScriptableObject
{

    public GameState.DepthType depthType;

    public ArtifactType artifactType;

    public Sprite dirtySprite;
    public Sprite cleanSprite;

    public enum ArtifactType {
        COMMON,
        RARE,
        LEGENDARY,
        PERSONAL
    };

}
