using System;
using Godot;
using LanguageExt;
using static LanguageExt.Prelude;

public partial class OnScreenScore : Control
{
    [Export]
    Label _scoreLabel = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameManagerSingleton
            .GetGameManagerSingleton()
            .IfSome((gameManager) =>
            {
                gameManager.GetScoreComponent()
                    .IfSome((scoreComponent) =>
                    {
                        scoreComponent.ScoreChanged += OnScoreChanged;
                        OnScoreChanged(scoreComponent, scoreComponent.GetScore());
                    });
            });

    }

    private void OnScoreChanged(ScoreComponent scoreComponent, float previousScore)
    {
        Option<Label> optScoreLabel = Optional(_scoreLabel);
        GD.Print(scoreComponent.GetScore());

        optScoreLabel.IfSome(
            (scoreLabel) => scoreLabel.Text = $"Score: {Math.Round(scoreComponent.GetScore())}"
        );
    }
}
