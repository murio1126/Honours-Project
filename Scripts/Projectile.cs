using Godot;
using System;

public partial class Projectile : Area2D
{
    public Vector2 Direction;
    public float Speed = 400f;
    public float MaxDistance = 600f;
    public int Damage = 1;

    private Vector2 _startPosition;

    public override void _Ready()
    {
        _startPosition = GlobalPosition;
        BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        // Move projectile
        GlobalPosition += Direction * Speed * (float)delta;

        // Delete if too far
        if (GlobalPosition.DistanceTo(_startPosition) > MaxDistance)
            QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {

       // GD.Print("entered");
       // if (body.IsInGroup("player"))
         //   return;

        if (body.HasMethod("TakeDamage"))
            body.Call("TakeDamage", Damage);

        QueueFree(); // always delete on collision
    }
}
