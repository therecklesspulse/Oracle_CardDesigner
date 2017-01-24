using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public static class ModifiersManager {

	public static void ApplyModifier(string className, string modName)
    {
        MethodInfo theMethod;
        //theMethod = typeof(ModifierMethods).GetMethod(modName);
        theMethod = Type.GetType(className).GetMethod(modName);
        theMethod.Invoke(new ModifierMethods(), null);
    }
}