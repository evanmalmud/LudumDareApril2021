using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterBoolModifier : SerializedScriptableObject {

    public string modName { get; set; }
    public bool value { get; set; }
    public BoolModifierType type { get; set; }
    public float duration { get; set; } = 0;
    public enum BoolModifierType {
        Bool,
        TimedBool,
    }

    public CharacterBoolModifier(string name, BoolModifierType type, bool value, float duration = 0)
    {
        this.modName = name;
        this.type = type;
        this.value = value;
        this.duration = duration;
    }
}
