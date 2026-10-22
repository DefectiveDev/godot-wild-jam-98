using Godot;

[GlobalClass]
public partial class ScoreComponent : Node
{
    [Export]
    float _defaultScore = 0;
    float _currentScore;

    [Signal]
    public delegate void ScoreChangedEventHandler(
        ScoreComponent scoreComponent,
        float previousScore
    );

    public ScoreComponent()
    {
        _currentScore = _defaultScore;
    }

    public float GetScore() => _currentScore;

    public float SetScore(float setScoreTo)
    {
        float previousScore = GetScore();

        _currentScore = setScoreTo;

        EmitSignalScoreChanged(this, previousScore);

        return _currentScore;
    }
}
