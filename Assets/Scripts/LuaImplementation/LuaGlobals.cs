using System.Collections.Generic;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaGlobals
{
    [MoonSharpHidden]
    private static Dictionary<string, DynValue> luaGlobals = new Dictionary<string, DynValue>();

    [MoonSharpHidden]
    private static void EnsureDefaultGlobals( Script luaScript ) {
        luaScript.Globals["File"] = new LuaFileLibrary();
        luaScript.Globals["Timer"] = new LuaTimerLibrary();
        luaScript.Globals["Globals"] = new LuaGlobals();
    }
    
    [MoonSharpHidden]
    public static void AssignScriptGlobals( Script luaScript ) {
	    UserData.RegisterAssembly();
        EnsureDefaultGlobals( luaScript );
    }

    public static void Push( string identifier, DynValue value ) {
        luaGlobals[identifier] = value;
    }

    public static DynValue Get( string identifier ) {
        return luaGlobals[identifier];
    }
    
    public static DynValue CallFunc( string identifier, params object[] args ) {
        return luaGlobals[identifier].Function.Call( args );
    }
}
