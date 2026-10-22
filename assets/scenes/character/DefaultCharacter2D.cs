using Godot;
using Godot.Collections;
using LanguageExt;
using static LanguageExt.Prelude;

public abstract partial class DefaultCharacter2D : CharacterBody2D
{
    [Export]
    HealthComponent _healthComponent;
    public Option<HealthComponent> HealthComponent
    {
        get => Optional(_healthComponent);
        set
        {
            value.IfSome((v) => _healthComponent = v);
            if (value.IsNone)
            {
                _healthComponent = null;
            }
        }
    }

    [Export]
    AttackComponent _attackComponent;
    public Option<AttackComponent> AttackComponent
    {
        get => Optional(_attackComponent);
        set
        {
            value.IfSome((v) => _attackComponent = v);
            if (value.IsNone)
            {
                _attackComponent = null;
            }
        }
    }

    [Export]
    MovementComponent2D _movementComponent2D;
    public Option<MovementComponent2D> MovementComponent2D
    {
        get => Optional(_movementComponent2D);
        set
        {
            value.IfSome((v) => _movementComponent2D = v);
            if (value.IsNone)
            {
                _movementComponent2D = null;
            }
        }
    }

    [Export]
    ProgressBar2DComponent _progressBar2DComponent;
    protected Option<ProgressBar2DComponent> p_progressBar2DComponent
    {
        get => Optional(_progressBar2DComponent);
        set
        {
            value.IfSome((v) => _progressBar2DComponent = v);
            if (value.IsNone)
            {
                _progressBar2DComponent = null;
            }
        }
    }

    [Export]
    public Array<Node> NodesToQueueFreeOnDeath = [];

    protected int p_currentColorIndex = 0;

    protected abstract void SetColorIndex(int colorIndex);

    public abstract int GetColorIndex();

    public override void _Ready()
    {
        HealthComponent.IfSome(
            (healthComponent) => healthComponent.HealthChanged += UpdateHealthBar
        );

        base._Ready();
    }

    void UpdateHealthBar(HealthComponent healthComponent)
    {
        p_progressBar2DComponent.IfSome(
            (progressBar) =>
                progressBar.Update(healthComponent.GetHealth(), 0, healthComponent.MaxHealth)
        );
    }
}
