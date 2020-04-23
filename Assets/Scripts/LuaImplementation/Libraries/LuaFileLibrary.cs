using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
class LuaFileLibrary
{
    [MoonSharpHidden]
    private bool ValidPath( string path ) {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        if( path.Length <= dataPath.Length || path.Substring( 0, dataPath.Length ) != dataPath || path == dataPath ) {
            ConsoleController.ThrowWarning(
                "File library path called out of data folder, cancelling."
            );
            return false;
        }
        return true;
    }

    [MoonSharpHidden]
    private void EnsureDataFolderExists() {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        if( !Directory.Exists( dataPath ) ) {
            Directory.CreateDirectory( dataPath );
        }
    }

    public DynValue Exists( string path ) {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );

        bool fileExists = false;
        if( !ValidPath( path ) ) { return DynValue.NewBoolean( false ); }
        if( Directory.Exists( path ) ) { fileExists = true; }
        if( File.Exists( path ) ) { fileExists = true; }
        return DynValue.NewBoolean( fileExists );
    }

    public void CreateDirectory( string path ) {
        EnsureDataFolderExists();
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );
        if( !ValidPath( path ) ) { return; }
        if( !Directory.Exists( path ) ) {
            Directory.CreateDirectory( path );
        }
    }

    public void Write( string path, string contents ) {
        EnsureDataFolderExists();
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );
        if( !ValidPath( path ) ) { return; }
        if( Directory.Exists( Path.GetDirectoryName( path ) ) ) {
            StreamWriter fileWriter = new StreamWriter( path );
            fileWriter.Write( contents );
            fileWriter.Close();
        }
    }

    public DynValue Read( string path ) {
        string dataPath = Path.Combine( Application.streamingAssetsPath, "data" );
        path = Path.Combine( dataPath, path );
        if( !ValidPath( path ) ) { return DynValue.NewNil(); }
        if( File.Exists( path ) ) {
            StreamReader fileReader = new StreamReader( path );
            string fileContents = fileReader.ReadToEnd();
            fileReader.Close();
            return DynValue.NewString( fileContents );
        }
        return DynValue.NewNil();
    }
}