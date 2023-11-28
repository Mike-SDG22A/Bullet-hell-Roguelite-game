using Godot;
using System;

public partial class PlayerScript : CharacterBody3D
{
    [Export] public float speed { get; set; } = 14;
    [Export] public float fallAccel { get; set; } = 75;
    [Export] public int hp { get; set; } = 100;
    [Export] public float exp { get; set; } = 0;
    [Export] public float expNeeded { get; set; } = 100;
    [Export] public float level { get; set; } = 1;
    [Export] public int money { get; set; } = 0;


    private Vector3 _targetVelocity = Vector3.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (exp >= expNeeded)
        {
            level++;
            exp -= expNeeded;
            expNeeded *= 1.5f;
        }

        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/Hp").Text = "Health: " + hp;
        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/HBoxContainer/HBoxContainer/Level").Text = "Level: " + level;
        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/HBoxContainer/HBoxContainer2/VBoxContainer/Exp").Text = "Exp: " + exp + "/" + expNeeded;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (hp <= 0) { GD.Print("You are dead"); }

        Movement(delta);
    }



    void Movement(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        }

        _targetVelocity.X = direction.X * speed;
        _targetVelocity.Z = direction.Z * speed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= fallAccel * (float)delta;
        }


        Velocity = _targetVelocity.Rotated(Vector3.Up.Normalized(), Rotation.Y);
        MoveAndSlide();
    }

    object Collision()
    {
        MoveAndSlide();
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            return collision.GetCollider();
        }
        return null;
    }

    void AddExp(float exp)
    {
        this.exp += exp;
    }

}
