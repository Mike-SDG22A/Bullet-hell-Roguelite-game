using Godot;
using System;
using System.Diagnostics;

public partial class Enemy : CharacterBody3D
{
	CharacterBody3D player;
	[Export] float speed = 10;
	[Export] float timer;
	[Export] float maxTimer = 1;
	[Export] int dmg = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        player = GetTree().CurrentScene.GetNode<CharacterBody3D>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null)
		{
			LookAt(new Vector3(player.Position.X, Position.Y, player.Position.Z), Vector3.Up);
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
            Velocity = Vector3.Forward.Rotated(Vector3.Up, Rotation.Y) * speed;
            MoveAndSlide();
        }

        object obj = Collision();

        timer -= 1 * (float)delta;
        if (obj != null)
        {
            if (obj.GetType().Name == "PlayerScript" && timer <= 0)
            {
                GetTree().CurrentScene.GetNode<PlayerScript>("Player").hp -= dmg;
                timer = maxTimer;
            }
        }
    }

    object Collision()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            return collision.GetCollider();
        }
        return null;
    }

}
