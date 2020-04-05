﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using MoonSharp.Interpreter;

public class IncludeLuaFiles : MonoBehaviour
{
    private static string[] unwriteableGlobals = new string[] {
        "PushGlobal",
        "GetGlobal"
    };

    private static Dictionary<string, DynValue> luaGlobals = new Dictionary<string, DynValue>();

    private static DynValue PushGlobal( string identifier, DynValue value ) {
        if( unwriteableGlobals.Contains( identifier ) ) { return DynValue.NewNil(); }
        luaGlobals[identifier] = value;
        return value;
    }

    private static DynValue GetGlobal( string identifier ) {
        return luaGlobals[identifier];
    }

    private void AssignLuaGlobals( Script luaScript ) {
        luaScript.Globals["PushGlobal"] = (Func<string, DynValue, DynValue>)PushGlobal;
        luaScript.Globals["GetGlobal"] = (Func<string, DynValue>)GetGlobal;
        foreach( KeyValuePair<string, DynValue> globalString in luaGlobals ) {
            luaScript.Globals[globalString.Key] = globalString.Value;
        }
    }

    void Start()
    {
        string modulePath = Path.Combine( Application.streamingAssetsPath, "Modules" );
        string[] allModules = Directory.GetFiles( modulePath, "*.*", SearchOption.AllDirectories );
        foreach ( var file in allModules ){
            if( file.Substring( Mathf.Max( 0, file.Length - 4 ) ) == ".lua" ) {
                Script luaScript = new Script();
                AssignLuaGlobals( luaScript );
                DynValue luaOutput = luaScript.DoString( File.ReadAllText( file ) );
                Debug.Log( luaOutput );
            }
        }
    }
}