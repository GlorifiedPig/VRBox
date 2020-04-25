using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaGlobals : MonoBehaviour
{
    private static Dictionary<string, DynValue> luaGlobals = new Dictionary<string, DynValue>();

    private static void EnsureDefaultGlobals( Script luaScript ) {
        luaScript.Globals["File"] = new LuaFileLibrary();
        luaScript.Globals["Timer"] = new LuaTimerLibrary();
    }
    
    public static void AssignScriptGlobals( Script luaScript ) {
	    UserData.RegisterAssembly();
        EnsureDefaultGlobals( luaScript );
    }
}
