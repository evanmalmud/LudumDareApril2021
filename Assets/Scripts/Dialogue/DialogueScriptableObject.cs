using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueScriptableObject", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{

    public EventReference sfxName;
    //FMOD.Studio.EventInstance footstepLInstance;

    public List<string> dialogueSentences;
    public List<float> sentenceWaitTimes;

    ShipMovement movementType;
    public enum ShipMovement {
        NONE,
        FLYRIGHT
    }
}
