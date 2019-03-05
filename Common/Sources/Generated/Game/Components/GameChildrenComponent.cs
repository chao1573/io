//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ChildrenComponent children { get { return (ChildrenComponent)GetComponent(GameComponentsLookup.Children); } }
    public bool hasChildren { get { return HasComponent(GameComponentsLookup.Children); } }

    public void AddChildren(System.Collections.Generic.List<int> newValue) {
        var index = GameComponentsLookup.Children;
        var component = CreateComponent<ChildrenComponent>(index);
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceChildren(System.Collections.Generic.List<int> newValue) {
        var index = GameComponentsLookup.Children;
        var component = CreateComponent<ChildrenComponent>(index);
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveChildren() {
        RemoveComponent(GameComponentsLookup.Children);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherChildren;

    public static Entitas.IMatcher<GameEntity> Children {
        get {
            if (_matcherChildren == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Children);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherChildren = matcher;
            }

            return _matcherChildren;
        }
    }
}