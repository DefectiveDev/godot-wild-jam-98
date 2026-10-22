using Godot;
using Godot.Collections;
using LanguageExt;
using static LanguageExt.Prelude;

// https://github.com/DefectiveDev/godot-wild-jam-98/issues/5
// Create game manager to adjust settings and access player globally

public partial class GameManagerSingleton : Node
{
    private static Option<GameManagerSingleton> s_gameManagerSingleton = new();

    public static Option<GameManagerSingleton> GetGameManagerSingleton()
    {
        if (s_gameManagerSingleton.IsNone)
        {
            Option<GameManagerSingleton> gameManager = new();

            Option<SceneTree> tree = (SceneTree)Engine.GetMainLoop();

            tree.IfSome(
                (tree) =>
                {
                    var node = GD.Load<PackedScene>(
                            "res://assets/scenes/game_manager/game_manager.tscn"
                        )
                        .Instantiate<GameManagerSingleton>();
                    Callable.From(() => tree.CurrentScene.AddChild(node)).CallDeferred();
                    gameManager = node;
                }
            );
            s_gameManagerSingleton = gameManager;

        }

        return s_gameManagerSingleton;
    }

    public GameManagerSingleton()
    {
        if (s_gameManagerSingleton.IsNone)
        {
            s_gameManagerSingleton = Optional(this);
        }
        s_gameManagerSingleton.IfSome(
            (someGameManager) =>
            {
                if (someGameManager != this)
                {
                    QueueFree();
                }
            }
        );
    }

    [Export]
    private ScoreComponent _scoreComponent = null;

    public Option<ScoreComponent> GetScoreComponent() => Optional(_scoreComponent);

    [Export]
    Player _player;
    public Option<Player> GetPlayer()
    {
        Option<Player> player = Optional(_player);

        if (player.IsNone)
        {
            var tree = Engine.GetMainLoop() as SceneTree;
            _player = tree.GetFirstNodeInGroup("player") as Player;
            player = Optional(_player);
        }
        return player;
    }

    [Export]
    Array<ColorAndShapeResource> _colorAndShapeResources = [];

    public Array<ColorAndShapeResource> GetColorAndShapeResources() => _colorAndShapeResources;
}
