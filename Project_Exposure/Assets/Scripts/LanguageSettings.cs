﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language : int
{
    NL = 0,
    EN = 1,
    DE = 2,
    RU = 3,
    BG = 4,
}

public static class LanguageSettings
{
    public static Language Language { get; set; }
}
