using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using LanguageExt;
using static LanguageExt.Prelude;

// FIX: this was cause fps drops in editor
// [Tool]
public partial class Enemy : DefaultCharacter2D
{
    [Export]
    AudioStreamPlayer2D _audioStreamer
    {
        get;
        set
        {
            field = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    AnimatedSprite2D _sprite2DToChangeColor
    {
        get;
        set
        {
            field = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    VisibleOnScreenNotifier2D _visibleNotify
    {
        get;
        set
        {
            field = value;
            UpdateConfigurationWarnings();
        }
    }

    [Export]
    AnimationPlayer _animationPlayer
    {
        get;
        set
        {
            field = value;
            UpdateConfigurationWarnings();
        }
    }

    Array<ColorAndShapeResource> _colorAndShapeResources = [];

    Enemy()
    {
        MaxSlides = 1;
    }

    public override void _Ready()
    {

        GameManagerSingleton
            .GetGameManagerSingleton()
            .IfSome(
                (gameManager) => _colorAndShapeResources = gameManager.GetColorAndShapeResources()
            );

        HealthComponent.IfSome(
            (healthComponent) =>
            {
                healthComponent.Died += OnDeath;
                healthComponent.HealthChanged += OnHealthChanged;
            }
        );

        // FIX: make it so we can check this when spawning to get the initial speed.
        // If not on screen by the time player moves will this never run to increase speed?
        Optional(_visibleNotify)
            .IfSome(
                (visibleNotify) =>
                {
                    MovementComponent2D.IfSome(
                        (movementComponent2D) =>
                        {
                            visibleNotify.ScreenEntered += () =>
                                movementComponent2D.CurrentSpeed = movementComponent2D.MaxSpeed;
                            visibleNotify.ScreenExited += () =>
                                movementComponent2D.CurrentSpeed *= 3;
                        }
                    );
                }
            );

        // TODO: I believe this should be moved to use SetColorIndex()
        p_currentColorIndex = GD.RandRange(0, _colorAndShapeResources.Length() - 1);

        Optional(_sprite2DToChangeColor)
            .IfSome(
                (colorToChangeSprite2D) =>
                    colorToChangeSprite2D.SelfModulate = _colorAndShapeResources[
                        p_currentColorIndex
                    ].Color
            );

        base._Ready();
    }

    private void OnHealthChanged(HealthComponent healthComponent)
    {
        if (healthComponent.GetIsDead())
        {
            return;
        }

        Optional(_audioStreamer).IfSome((audioStreamer) => audioStreamer.Play());
    }

    private void OnDeath(HealthComponent _)
    {
        GameManagerSingleton
            .GetGameManagerSingleton()
            .IfSome(
                (gameManager) =>
                {
                    gameManager
                        .GetScoreComponent()
                        .IfSome(
                            (scoreComponent) =>
                            {
                                scoreComponent.SetScore(scoreComponent.GetScore() + 1);
                            }
                        );
                }
            );

        foreach (Node node in NodesToQueueFreeOnDeath)
        {
            Optional(node)
                .IfSome(
                    (node) =>
                    {
                        if (!node.IsQueuedForDeletion())
                        {
                            node.QueueFree();
                        }
                    }
                );
        }

        Optional(_animationPlayer)
            .IfSome(
                (animationPlayer) =>
                {
                    animationPlayer.AnimationFinished += (animationName) =>
                    {
                        if (animationName == "on_death")
                        {
                            QueueFree();
                        }
                    };

                    animationPlayer.Play("on_death");
                }
            );
    }

    public override int GetColorIndex() => p_currentColorIndex;

    protected override void SetColorIndex(int colorIndex)
    {
        colorIndex = Math.Clamp(colorIndex, 0, _colorAndShapeResources.Length() - 1);

        p_currentColorIndex = colorIndex;
        Optional(_sprite2DToChangeColor)
            .IfSome(
                (colorToChangeSprite2D) =>
                    colorToChangeSprite2D.SelfModulate = _colorAndShapeResources[
                        p_currentColorIndex
                    ].Color
            );
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var isDead = false;

        HealthComponent.IfSome((healthComponent) => isDead = healthComponent.GetIsDead());

        if (isDead)
        {
            return;
        }

        var moveDirection = Vector2.Zero;

        // TODO: get player position and set direction to that.
        GameManagerSingleton.GetGameManagerSingleton().IfSome((gameManager) =>
        {
            gameManager.GetPlayer().IfSome((player) => moveDirection = Position.DirectionTo(player.Position));
        });

        MovementComponent2D.IfSome(
            (movementComponent) =>
            {
                movementComponent.Update(delta, this, moveDirection);
            }
        );

        AttackComponent.IfSome(
            (attackComponent) =>
            {
                void attack() => Attack(this, attackComponent);

                attackComponent.AttackOnCooldown(Callable.From(attack));
            }
        );
    }

    static void Attack(CharacterBody2D characterBody2D, AttackComponent attackComponent)
    {
        Option<KinematicCollision2D> collision = Optional(characterBody2D.GetLastSlideCollision());

        collision.IfSome(
            (collision) =>
            {
                Node node = (Node)collision.GetCollider();
                var ownerNode = Optional(node.Owner as Player);

                ownerNode.IfSome((player) =>
                {
                    player.HealthComponent.IfSome((healthComponent) =>
                    {
                        attackComponent.GetAttackCooldownTimer().IfSome((attackCooldownTimer) =>
                        {
                            healthComponent.SetHealth
                            (
                                healthComponent.GetHealth() - (int)Math
                                    .Ceiling((double)(attackComponent.DamagePerSecond * attackCooldownTimer.WaitTime))
                            );
                        });
                    });
                });

            }
        );
    }

    // FIX: configure warings are not running despite null checks
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [.. base._GetConfigurationWarnings()];

        if (_audioStreamer == null)
        {
            warnings.Add("AudioStreamPlayer2D is missing audio will not play when hurt!");
        }

        if (_sprite2DToChangeColor == null)
        {
            warnings.Add(
                "Color to change sprite is missing color will not change with color index!"
            );
        }

        if (_visibleNotify == null)
        {
            warnings.Add(
                "Visible Notifier 2D is missing enemies will not accelerate when off screen!"
            );
        }

        if (_animationPlayer == null)
        {
            warnings.Add("Animation Player missing death animation will not play!");
        }

        return [.. warnings];
    }
}
