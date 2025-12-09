using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public PlayerData Data;

    private Vector2 _velocity;

    [Export] public float AttackCooldown = 0.25f;
    private float _attackTimer = 0f;

    [Export] public PackedScene ProjectileScene;


    public override void _Ready()
    {
        ProjectileScene = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");
    }

    public override void _Process(double delta)
    {
        _attackTimer -= (float)delta;

        bool wantsToShoot = Input.IsActionPressed("attack") || Input.IsActionJustPressed("ui_accept");
        // "attack" = mouse click, "ui_accept" = space

        if (wantsToShoot && _attackTimer <= 0)
        {
            Shoot();
            _attackTimer = AttackCooldown;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        // --- INPUT ---
        Vector2 input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
       Vector2 targetVel = input * Data.moveSpeed;

        // --- ACCEL/DECEL ---
        float ramp = input == Vector2.Zero ? Data.deceleration : Data.acceleration;

        _velocity = _velocity.Lerp(targetVel, ramp * dt);

        Velocity = _velocity;
        MoveAndSlide();
    }


    private void Shoot()
    {
        var projectile = ProjectileScene.Instantiate<Projectile>();

        Vector2 dir = (GetGlobalMousePosition() - GlobalPosition).Normalized();

        projectile.Direction = dir;
        projectile.GlobalPosition = GlobalPosition + dir * 20f; // spawn slightly ahead
        projectile.Rotation = dir.Angle();

        GetParent().AddChild(projectile);
    }

}
