using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    public enum EnemyState
    {
        Idle,
        Detect,
        Chase,
        Attack,
        CheckTarget
    }

    [Export] public float MoveSpeed = 90f;
    [Export] public float DetectionRange = 200f;
    [Export] public float AttackCooldown = 1f;
    [Export] public float Damage = 10f;

    private EnemyState _state = EnemyState.Idle;
    [Export] Node2D player;
    private float _lastAttackTime = 0f;

    public override void _Ready()
    {
        
    }

     public override void _PhysicsProcess(double delta)
    {
        switch (_state)
        {
            case EnemyState.Idle: HandleIdle(); break;
            case EnemyState.Detect: HandleDetect(); break;
            case EnemyState.Chase: HandleChase(delta); break;
            case EnemyState.Attack: HandleAttack(); break;
            case EnemyState.CheckTarget: HandleCheck(); break;
        }
    }

    private void HandleIdle()
    {
        if (IsPlayerInRange())
        {
            _state = EnemyState.Detect;
        }
    }

    private void HandleDetect()
    {
        _state = EnemyState.Chase;
    }

    private void HandleChase(double delta)
    {
        if (!IsInstanceValid(player))
            return;

        Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
        Velocity = dir * MoveSpeed;
        MoveAndSlide();

        if (IsPlayerInAttackRange())
        {
            _state = EnemyState.Attack;
        }
    }

    private void HandleAttack()
    {
        float now = (float)Time.GetTicksMsec() / 1000f;

        if (now - _lastAttackTime >= AttackCooldown)
        {
            player.Call("TakeDamage", Damage);
            _lastAttackTime = now;
        }

        _state = EnemyState.CheckTarget;
    }

    private void HandleCheck()
    {
        if (!IsPlayerInAttackRange())
            _state = EnemyState.Chase;
    }

    private bool IsPlayerInRange()
    {
        return GlobalPosition.DistanceTo(player.GlobalPosition) <= DetectionRange;
    }

    private bool IsPlayerInAttackRange()
    {
        return GlobalPosition.DistanceTo(player.GlobalPosition) <= 22f;
    }
}
