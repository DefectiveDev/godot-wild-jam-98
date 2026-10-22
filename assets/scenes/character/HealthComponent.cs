using System;
using Godot;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Export]
    public int MaxHealth = 100;

    int _currentHealth;

    bool _isDead = false;

    [Signal]
    public delegate void HealthChangedEventHandler(HealthComponent healthComponent);

    [Signal]
    public delegate void DiedEventHandler(HealthComponent healthComponent);

    public override void _Ready()
    {
        base._Ready();
        _currentHealth = MaxHealth;
    }

    public int SetHealth(int health)
    {
        if (_isDead)
        {
            return _currentHealth;
        }

        health = Math.Clamp(health, 0, MaxHealth);

        if (health != GetHealth())
        {
            EmitSignalHealthChanged(this);
        }

        if (GetHealth() <= 0)
        {
            _isDead = true;
            EmitSignalDied(this);
        }

        _currentHealth = health;

        return _currentHealth;
    }

    public int GetHealth() => _currentHealth;

    public bool GetIsDead() => _isDead;
}
