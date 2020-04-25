using System;
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class LuaScriptLoader : MonoBehaviour
{
    public Script luaScript;
    private GameObject playerObject;
    private void Start() {
        playerObject = GameObject.FindGameObjectWithTag( "Player" );
        Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
        Script.DefaultOptions.DebugPrint = s => ConsoleController.PrintToConsole( s );
        luaScript = new Script();
        LuaGlobals.AssignScriptGlobals( luaScript );
        LoadLuaLibraries();
        LoadLuaModules();
    }

    private void LoadLuaLibraries() {
        string librariesPath = Path.Combine( Application.streamingAssetsPath, "libraries" );
        foreach( var folder in Directory.GetDirectories( librariesPath, "*.*", SearchOption.AllDirectories ) ) {
            string librariesFolder = Path.Combine( librariesPath, folder + "/lua" );
            string autorunFolder = Path.Combine( librariesFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                foreach ( var file in Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories ) ) {
                    if( Path.GetExtension( file ) == ".lua" ) {
                        try {
                            ((ScriptLoaderBase)luaScript.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths( librariesFolder + "/?;" + librariesFolder + "/?.lua" );
                            DynValue luaOutput = luaScript.DoFile( file );
                        } catch( InterpreterException ex ) {
                            ConsoleController.ThrowExceptionToConsole( ex );
                        }
                    }
                }
            }
        }
        luaScript.DoString( "Hook.Call( 'LibrariesLoaded' )" );
    }

    private void LoadLuaModules() {
        string modulesPath = Path.Combine( Application.streamingAssetsPath, "modules" );
        foreach( var folder in Directory.GetDirectories( modulesPath, "*.*", SearchOption.AllDirectories ) ) {
            string moduleFolder = Path.Combine( modulesPath, folder + "/lua" );
            string autorunFolder = Path.Combine( moduleFolder, "autorun" );
            if( Directory.Exists( autorunFolder ) ) {
                foreach ( var file in Directory.GetFiles( autorunFolder, "*.*", SearchOption.AllDirectories ) ) {
                    if( Path.GetExtension( file ) == ".lua" ) {
                        try {
                            ((ScriptLoaderBase)luaScript.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths( moduleFolder + "/?;" + moduleFolder + "/?.lua" );
                            DynValue luaOutput = luaScript.DoFile( file );
                        } catch( InterpreterException ex ) {
                            ConsoleController.ThrowExceptionToConsole( ex );
                        }
                    }
                }
            }
        }
        luaScript.DoString( "Hook.Call( 'ModulesLoaded' )" );
    }
}