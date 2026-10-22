using System.Collections.Generic;
using Godot;
using LanguageExt;
using static LanguageExt.Prelude;

[Tool]
[GlobalClass]
public partial class AttackComponent : Node
{
    [Export]
    public float DamagePerSecond = 10;

    [Export]
    Timer _attackCooldownTimer
    {
        get;
        set
        {
            field = value;
            UpdateConfigurationWarnings();
        }
    }

    public Option<Timer> GetAttackCooldownTimer() => Optional(_attackCooldownTimer);

    bool _attackCooldownReached = true;

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];

        if (_attackCooldownTimer == null)
        {
            warnings.Add("This node has no timer. AttackOnCooldown will not be called.");
        }

        return [.. warnings];
    }

    public override void _Ready()
    {
        base._Ready();
        GetAttackCooldownTimer()
            .IfSome((timer) => timer.Timeout += () => _attackCooldownReached = true);
    }

    /// <summary>
    /// Call an attack with respect to a cool down timer. To add arguments you should use bind.
    /// </summary>
    /// <param name="attackMethod">Callable with no arguments</param>
    public void AttackOnCooldown(Callable attackMethod)
    {
        if (_attackCooldownReached)
        {
            _attackCooldownReached = false;
            attackMethod.Call();
            GetAttackCooldownTimer().IfSome((timer) => timer.Start());
        }
    }
}
