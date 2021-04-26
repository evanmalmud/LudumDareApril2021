using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueScriptableObject", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{
    [FMODUnity.EventRef]
    public string sfxName;

    public List<string> dialogueSentences;
    public List<float> sentenceWaitTimes;

    ShipMovement movementType;
    public enum ShipMovement {
        NONE,
        FLYRIGHT
    }
}
