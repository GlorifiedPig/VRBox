using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaHookLibrary
{
    [MoonSharpHidden] private Dictionary<string, Dictionary<string, Closure>> cachedHooks = new Dictionary<string, Dictionary<string, Closure>>();

    public void Attach( string hookID, string uniqueID, Closure function ) {
        if( !cachedHooks.ContainsKey( hookID ) ) {
            cachedHooks[hookID] = new Dictionary<string, Closure>();
        }

        Dictionary<string, Closure> attachedDict = cachedHooks[hookID];
        attachedDict[uniqueID] = function;
        cachedHooks[hookID] = attachedDict;
    }

    public void Call( string hookID ) {
        if( cachedHooks.ContainsKey( hookID ) ) {
            foreach( KeyValuePair<string, Closure> cachedHook in cachedHooks[hookID] ) {
                Closure function = cachedHook.Value;
                try {
                    Script luaScript = function.OwnerScript;
                    luaScript.Call( function );
                } catch( InterpreterException ex ) {
                    ConsoleController.ThrowExceptionToConsole( ex );
                }
            }
        }
    }

    public void Remove( string hookID, string uniqueID ) {
        if( cachedHooks.ContainsKey( hookID ) ) {
            cachedHooks[hookID].Remove( uniqueID );
        }   
    }
}
