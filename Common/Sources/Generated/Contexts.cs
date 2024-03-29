//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ContextsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts : Entitas.IContexts {

    public static Contexts sharedInstance {
        get {
            if (_sharedInstance == null) {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public CommandContext command { get; set; }
    public GameContext game { get; set; }
    public InputContext input { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { command, game, input }; } }

    public Contexts() {
        command = new CommandContext();
        game = new GameContext();
        input = new InputContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors) {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset() {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++) {
            contexts[i].Reset();
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EntityIndexGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

    public const string CommandOwner = "CommandOwner";
    public const string Id = "Id";
    public const string InputOwner = "InputOwner";
    public const string PlayerId = "PlayerId";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices() {
        command.AddEntityIndex(new Entitas.EntityIndex<CommandEntity, string>(
            CommandOwner,
            command.GetGroup(CommandMatcher.CommandOwner),
            (e, c) => ((CommandOwnerComponent)c).value));

        game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, int>(
            Id,
            game.GetGroup(GameMatcher.Id),
            (e, c) => ((IdComponent)c).value));

        input.AddEntityIndex(new Entitas.EntityIndex<InputEntity, string>(
            InputOwner,
            input.GetGroup(InputMatcher.InputOwner),
            (e, c) => ((InputOwnerComponent)c).value));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, string>(
            PlayerId,
            game.GetGroup(GameMatcher.PlayerId),
            (e, c) => ((PlayerIdComponent)c).value));
    }
}

public static class ContextsExtensions {

    public static System.Collections.Generic.HashSet<CommandEntity> GetEntitiesWithCommandOwner(this CommandContext context, string value) {
        return ((Entitas.EntityIndex<CommandEntity, string>)context.GetEntityIndex(Contexts.CommandOwner)).GetEntities(value);
    }

    public static GameEntity GetEntityWithId(this GameContext context, int value) {
        return ((Entitas.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Id)).GetEntity(value);
    }

    public static System.Collections.Generic.HashSet<InputEntity> GetEntitiesWithInputOwner(this InputContext context, string value) {
        return ((Entitas.EntityIndex<InputEntity, string>)context.GetEntityIndex(Contexts.InputOwner)).GetEntities(value);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithPlayerId(this GameContext context, string value) {
        return ((Entitas.EntityIndex<GameEntity, string>)context.GetEntityIndex(Contexts.PlayerId)).GetEntities(value);
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.VisualDebugging.CodeGeneration.Plugins.ContextObserverGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class Contexts {

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeContexObservers() {
        try {
            CreateContextObserver(command);
            CreateContextObserver(game);
            CreateContextObserver(input);
        } catch(System.Exception) {
        }
    }

    public void CreateContextObserver(Entitas.IContext context) {
        if (UnityEngine.Application.isPlaying) {
            var observer = new Entitas.VisualDebugging.Unity.ContextObserver(context);
            UnityEngine.Object.DontDestroyOnLoad(observer.gameObject);
        }
    }

#endif
}
