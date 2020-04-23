
require( "types" )
require( "tableutil" )
require( "hooks" )

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

Hook.Attach( "TestHook", "UniqueHookID", function( bruh, bruh2 )
    print( bruh .. bruh2 )
    print( "UniqueHookID Called (TestHook)" )
end )

Hook.Attach( "TestHook", "UniqueHookID2", function()
    print( "UniqueHookID2 Called (TestHook)" )
end )

Hook.Attach( "TestHook", "UniqueHookID3", function()
    print( "UniqueHookID3 Called (TestHook)" )
end )

Timer.Begin( 5, function()
    Hook.Remove( "TestHook", "UniqueHookID3" )
    Hook.Call( "TestHook", "firstarg", "secondarg" )
end )