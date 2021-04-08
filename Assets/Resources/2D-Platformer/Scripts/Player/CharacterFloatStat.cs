using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterFloatStat : SerializedUnityObject {
    [SerializeField]
    public float defaultValue { get; set; }
    public float currentWithoutModvalue { get; set; }
    public float finalValue { get; set; }

    private Dictionary<string, CharacterFloatModifier> modifiers = new Dictionary<string, CharacterFloatModifier>();

    private Dictionary<string, float> timers = new Dictionary<string, float>();

    public CharacterFloatStat(float value)
    {
        defaultValue = value;
        currentWithoutModvalue = value;
        finalValue = value;
    }

    public void flatModifyCurrent(float value)
    {
        currentWithoutModvalue += value;
        calcFinalValue();
    }

    public void addModifier(CharacterFloatModifier mod)
    {
        if (!modifiers.ContainsKey(mod.modName)) {
            modifiers.Add(mod.modName, mod);
            calcFinalValue();
        }
        if(mod.duration > 0) {
            if (!timers.ContainsKey(mod.modName)) {
                timers.Add(mod.modName, mod.duration);
            }
        }
    }

    public void removeModifier(CharacterFloatModifier mod)
    {
        modifiers.Remove(mod.modName);
        timers.Remove(mod.modName);
        calcFinalValue();
    }

    public void removeByName(string name)
    {
        modifiers.Remove(name);
        timers.Remove(name);
        calcFinalValue();
    }

    public void calcFinalValue()
    {
        finalValue = currentWithoutModvalue;
        foreach (KeyValuePair<string, CharacterFloatModifier> mod in modifiers) {
            switch (mod.Value.type) {
                case (CharacterFloatModifier.FloatModifierType.Flat):
                    finalValue += mod.Value.value;
                    break;
                case (CharacterFloatModifier.FloatModifierType.Percent):
                    finalValue *= 1 + mod.Value.value/100;
                    break;
                default:
                    break;
            }
        }
    }

    public override string ToString()
    {
        return "defaultValue - " + defaultValue + " currentWithoutModvalue - " + 
            currentWithoutModvalue + " finalValue - " + finalValue + " modifiers - " + 
            modifiers + " timers - " + timers;
    }
}
