using System;
using Godot;
using Godot.Collections;
using LanguageExt;
using static LanguageExt.Prelude;

public partial class Player : DefaultCharacter2D
{
    [Export]
    AudioStreamPlayer2D _hurtAudioStreamer;

    [Export]
    AudioStreamPlayer2D _deathAudioStreamer;

    [Export]
    PackedScene _onDeathScene;

    [Export]
    InputComponent _inputComponent;

    [Export]
    Sprite2D _sprite2DToChangeColor;

    [Export]
    RadialMenu _radialMenu;

    [Export]
    AnimatedSprite2D _animatedSprite2D;

    [Export]
    FlashBeamShapeCast2D _flashBeamShapeCast2D;

    Array<ColorAndShapeResource> _colorAndShapeResources = [];

    Player() => AddToGroup("player");

    public override void _Ready()
    {
        base._Ready();

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

        Optional(_radialMenu)
            .IfSome((radialMenu) =>
                radialMenu.SelectedColorIndex += SetColorIndex
            );

        Optional(_sprite2DToChangeColor)
            .IfSome((sprite2DToChangeColor) =>
                sprite2DToChangeColor.SelfModulate = _colorAndShapeResources[p_currentColorIndex].Color
            );

        Optional(_flashBeamShapeCast2D)
            .IfSome((flashBeamShapeCast2D) =>
                flashBeamShapeCast2D.CollisionHappened += OnOverlappingBodiesProcess
            );
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var isDead = false;

        HealthComponent.IfSome((healthComponent) =>
        {
            isDead = healthComponent.GetIsDead();
        });

        if (isDead)
        {
            return;
        }

        var moveDirection = Vector2.Zero;

        Optional(_inputComponent).IfSome((inputComponent) =>
        {
            inputComponent.Update();
            moveDirection = inputComponent.GetMoveDirection();
        });

        MovementComponent2D.IfSome((movementComponent) =>
        {
            movementComponent.Update(delta, this, moveDirection);
        });
    }

    private void OnHealthChanged(HealthComponent healthComponent)
    {
        if (healthComponent.GetIsDead())
        {
            return;
        }

        Optional(_hurtAudioStreamer).IfSome((hurtAudioStreamer) =>
            hurtAudioStreamer.Play());
    }

    private void OnDeath(HealthComponent _)
    {
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

        Optional(_deathAudioStreamer).IfSome(
            (deathAudioStreamer) => deathAudioStreamer.Play());

        Optional(_animatedSprite2D).IfSome((animatedSprite2D) =>
        {
            animatedSprite2D.AnimationFinished += () =>
            {
                var instantiateDeath = Callable.From(() =>
                {
                    GetTree().CurrentScene.AddChild(_onDeathScene.Instantiate());
                });

                instantiateDeath.CallDeferred();
            };
        });

        Optional(_animatedSprite2D).IfSome((animatedSprite2D) => animatedSprite2D.Play("death"));
    }

    public override int GetColorIndex() => p_currentColorIndex;

    protected override void SetColorIndex(int colorIndex)
    {
        colorIndex = Math.Clamp(colorIndex, 0, _colorAndShapeResources.Length() - 1);
        p_currentColorIndex = colorIndex;

        _sprite2DToChangeColor.SelfModulate = _colorAndShapeResources[p_currentColorIndex].Color;
    }

    static void Attack(Array<Node2D> overlappingBodies, AttackComponent attackComponent, int colorIndex)
    {
        foreach (var body in overlappingBodies)
        {
            Option<Enemy> enemy = new();

            if (body is Enemy enemyBody)
            {
                enemy = Optional(enemyBody);
            }
            else if (body.Owner is Enemy ownerEnemyBody)
            {
                enemy = Optional(ownerEnemyBody);
            }


            enemy.IfSome((enemy) =>
            {
                if (colorIndex == enemy.GetColorIndex())
                {
                    enemy.HealthComponent.IfSome((healthComponent) =>
                    {
                        attackComponent.GetAttackCooldownTimer().IfSome((attackCooldownTimer) =>
                        {

                            var dps = (int)Math.Ceiling((double)(attackComponent.DamagePerSecond * attackCooldownTimer.WaitTime));

                            healthComponent.SetHealth
                            (
                                healthComponent.GetHealth() - dps
                            );
                        });

                    });
                }
            });
        }
    }

    private void OnOverlappingBodiesProcess(Array<Node2D> overlappingBodies)
    {
        AttackComponent.IfSome((attackComponent) =>
        {
            void attack() => Attack(overlappingBodies, attackComponent, p_currentColorIndex);
            attackComponent.AttackOnCooldown(Callable.From(attack));
        });
    }


}
