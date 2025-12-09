using Godot;
using System;


[GlobalClass]
public partial class PlayerData : Resource
{
    [Export] public float moveSpeed { get; set; } = 180f;
    [Export] public float acceleration { get; set; } = 12f;
    [Export] public float deceleration { get; set; } = 18f;
}
