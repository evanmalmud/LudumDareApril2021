using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterBoolStat : SerializedUnityObject {

    //On a tie we aim for default value
    [SerializeField]
    public bool defaultValue { get; set; }
    public bool currentWithoutModvalue { get; set; }
    public bool finalValue { get; set; }

    private Dictionary<string, CharacterBoolModifier> modifiers = new Dictionary<string, CharacterBoolModifier>();

    public CharacterBoolStat(bool value)
    {
        defaultValue = value;
        currentWithoutModvalue = value;
    }

    public void modValue(bool value) {
        currentWithoutModvalue = value;
    }

    public void addModifier(CharacterBoolModifier mod)
    {
        if (!modifiers.ContainsKey(mod.modName)) {
            modifiers.Add(mod.modName, mod);
            calcFinalValue();
        }
    }

    public void removeModifier(CharacterBoolModifier mod)
    {
        modifiers.Remove(mod.modName);
        calcFinalValue();
    }

    public void removeByName(string name)
    {
        modifiers.Remove(name);
        calcFinalValue();
    }

    public void calcFinalValue()
    {
        int falseCount = 0;
        int trueCount = 0;
        foreach (KeyValuePair<string, CharacterBoolModifier> mod in modifiers) {
            switch (mod.Value.type) {
                case (CharacterBoolModifier.BoolModifierType.Bool):
                case (CharacterBoolModifier.BoolModifierType.TimedBool):
                    if(mod.Value.value) {
                        trueCount++;
                    } else {
                        falseCount++;
                    }
                    break;
                default:
                    break;
            }
        }
        if (trueCount > falseCount) {
            finalValue = true;
        } else if (falseCount > trueCount) {
            finalValue = false;
        } else {
            if(defaultValue) {
                finalValue = true;
            } else {
                finalValue = false;
            }
        }

    }
}
