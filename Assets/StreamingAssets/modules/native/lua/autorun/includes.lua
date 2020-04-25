
require( "types" )
require( "tableutil" )
require( "hooks" )

Globals.Push( "TestGlobal", function( bruh ) print(bruh) end )
Globals.Push( "TestTable", {"bruh"} )

-- below is a bunch of debugging stuff
table.Print({
    ["Bruh"] = {
        ["Boy"] = 5,
        ["Boy2"] = 4,
        ["Boy3"] = {
            "Bruh", "Bruh2"
        }
    },
    ["Bruv"] = 6
})

Hook.Attach( "TestHook", "UniqueHookID", function()
    print( "UniqueHookID Called (TestHook)" )
end )

Hook.Attach( "TestHook", "UniqueHookID2", function()
    print( "UniqueHookID2 Called (TestHook)" )
end )

Timer.Begin( 5, function()
    Hook.Remove( "TestHook", "UniqueHookID" )
    Hook.Call( "TestHook" )
end )