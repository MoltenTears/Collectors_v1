using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetting : ScriptableObject
{
    public enum Setting
    {
        NONE,
        EASY,
        MEDIUM,
        HARD
    }

    public DifficultySetting.Setting difficultySetting = Setting.NONE;
}
