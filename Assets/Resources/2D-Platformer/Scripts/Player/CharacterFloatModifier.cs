using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterFloatModifier : SerializedUnityObject {
    public string modName { get; set; }
    public FloatModifierType type { get; set; }
    public float value { get; set; }
    public float duration { get; set; } = 0;
    public enum FloatModifierType {
        Flat,
        Percent,
        TimedFlat,
        TimedPercent,
    }

    public CharacterFloatModifier(string name, FloatModifierType type, float value, float duration = 0)
    {
        this.modName = name;
        this.type = type;
        this.value = value;
        this.duration = duration;
    }
}
