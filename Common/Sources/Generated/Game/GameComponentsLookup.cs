//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class GameComponentsLookup {

    public const int Asset = 0;
    public const int AssetListener = 1;
    public const int Destroyed = 2;
    public const int DestroyedListener = 3;
    public const int Position = 4;

    public const int TotalComponents = 5;

    public static readonly string[] componentNames = {
        "Asset",
        "AssetListener",
        "Destroyed",
        "DestroyedListener",
        "Position"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(AssetComponent),
        typeof(AssetListenerComponent),
        typeof(DestroyedComponent),
        typeof(DestroyedListenerComponent),
        typeof(PositionComponent)
    };
}