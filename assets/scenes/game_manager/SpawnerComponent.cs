using System;
using Godot;
using LanguageExt;
using static LanguageExt.Prelude;

public partial class SpawnerComponent : Node
{
    [Export]
    Timer _spawnTimer = null;

    [Export]
    PackedScene _sceneToSpawn = null;

    [Export]
    float _offScreenPaddingPx = 64;

    Option<SceneTree> _tree = new();

    enum Side
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

    public override void _Ready()
    {
        _tree = Optional(Engine.GetMainLoop() as SceneTree);
        Optional(_spawnTimer).IfSome((spawnTimer) => spawnTimer.Timeout += OnSpawnTimerTimeOut);
    }

    private void OnSpawnTimerTimeOut()
    {
        Option<Node2D> node2D = new();

        Optional(_sceneToSpawn)
            .IfSome(
                (sceneToSpawn) =>
                {
                    node2D = Optional(sceneToSpawn.Instantiate<Node2D>());
                }
            );

        node2D.IfSome(
            (node2D) =>
            {
                var side = (Side)(GD.Randi() % Enum.GetNames<Side>().Length());

                Vector2 cameraCenterScreenPos = GetViewport()
                    .GetCamera2D()
                    .GetScreenCenterPosition();
                Vector2 viewportCenterPos = GetViewport().GetVisibleRect().GetCenter();

                float leftSideScreenPx = cameraCenterScreenPos.X - viewportCenterPos.X;
                float rightSideScreenPx = cameraCenterScreenPos.X + viewportCenterPos.X;
                float topSideScreenPx = cameraCenterScreenPos.Y - viewportCenterPos.Y;
                float bottomSideScreenPx = cameraCenterScreenPos.Y + viewportCenterPos.Y;

                switch (side)
                {
                    case Side.TOP:
                    case Side.BOTTOM:
                        int randomX = (int)GD.RandRange(rightSideScreenPx, leftSideScreenPx);
                        switch (side)
                        {
                            case Side.TOP:
                                node2D.Position = new Vector2(
                                    randomX,
                                    topSideScreenPx - _offScreenPaddingPx
                                );
                                break;
                            case Side.BOTTOM:
                                node2D.Position = new Vector2(
                                    randomX,
                                    bottomSideScreenPx + _offScreenPaddingPx
                                );
                                break;
                        }
                        break;

                    case Side.LEFT:
                    case Side.RIGHT:
                        int randomY = (int)GD.RandRange(topSideScreenPx, bottomSideScreenPx);
                        switch (side)
                        {
                            case Side.LEFT:
                                node2D.Position = new Vector2(
                                    leftSideScreenPx - _offScreenPaddingPx,
                                    randomY
                                );
                                break;
                            case Side.RIGHT:
                                node2D.Position = new Vector2(
                                    rightSideScreenPx + _offScreenPaddingPx,
                                    randomY
                                );
                                break;
                        }
                        break;
                }

                _tree.IfSome(
                    (tree) =>
                    {
                        Callable.From(() => tree.CurrentScene.AddChild(node2D)).CallDeferred();
                    }
                );
            }
        );
    }
}
