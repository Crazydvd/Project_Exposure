using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageSettings 
{
    private static int language;

    public static int Language
    {
        get
        {
            return language;
        }
        set
        {
            language = value;
        }
    }

}
