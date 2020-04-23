using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaTimerLibrary
{
    private GameObject gameController = GameObject.FindGameObjectWithTag( "GameController" );
    
    public void Begin( float seconds, Closure function ) {
        if( seconds < 0 ) { return; }
        gameController.GetComponent<LuaScriptLoader>().StartCoroutine( StartEnumerator( seconds, function ) ); // to-do: cache this!
    }

    [MoonSharpHidden] private Dictionary<string, int> repeatingTimers = new Dictionary<string, int>();
    public void Repeating( string identifier, float seconds, int repetitions, Closure function ) {
        if( repetitions < 0 || seconds < 0 ) { return; }
        if( repetitions == 0 ) { repetitions = -1; }
        repeatingTimers[identifier] = repetitions;
        gameController.GetComponent<LuaScriptLoader>().StartCoroutine( StartRepeatingEnumerator( identifier, seconds, function ) ); // to-do: cache this!
    }

    public void Remove( string identifier ) {
        if( repeatingTimers.ContainsKey( identifier ) ) {
            repeatingTimers.Remove( identifier );
        }
    }

    [MoonSharpHidden]
    private IEnumerator StartEnumerator( float seconds, Closure function ) {
        yield return new WaitForSeconds( seconds );
        try {
            Script luaScript = function.OwnerScript;
            luaScript.Call( function );
        } catch( InterpreterException ex ) {
            ConsoleController.ThrowExceptionToConsole( ex );
        }
    }

    [MoonSharpHidden]
    private IEnumerator StartRepeatingEnumerator( string identifier, float seconds, Closure function ) {
        while( repeatingTimers.ContainsKey( identifier ) && ( repeatingTimers[identifier] == -1 || repeatingTimers[identifier] > 0 ) ) {
            if( repeatingTimers[identifier] != -1 ) {
                repeatingTimers[identifier] = repeatingTimers[identifier] - 1;
                if( repeatingTimers[identifier] <= 0 ) { repeatingTimers.Remove( identifier ); }
            }
            yield return new WaitForSeconds( seconds );
            try {
                Script luaScript = function.OwnerScript;
                luaScript.Call( function );
            } catch( InterpreterException ex ) {
                ConsoleController.ThrowExceptionToConsole( ex );
            }
        }
    }
}
