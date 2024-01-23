using Godot;
using System;
using System.Diagnostics;

public partial class Enemy : CharacterBody3D
{
	protected CharacterBody3D player;
    [Export] protected int giveExp = 10;
    [Export] public float hp = 100;
	[Export] protected float speed = 10;
	[Export] protected float timer;
	[Export] protected float maxTimer = 1;
	[Export] protected int dmg = 1;
    [Export] public float fallAccel { get; set; } = 75;
    protected Vector3 vel = Vector3.Forward;

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

        if (hp <= 0) 
        {
            GetTree().CurrentScene.GetNode<PlayerScript>("Player").AddExp(giveExp);
            QueueFree(); 
        }
    }

    public override void _PhysicsProcess(double delta)
	{
        Movement(delta);

        MoveAndSlide();

        object obj = Collision();

        timer -= 1 * (float)delta;
        if (obj != null)
        {
            if (obj is PlayerScript player && timer <= 0)
            {
                player.hp -= dmg;
                timer = maxTimer;
            }
        }
    }

    public virtual void Movement(double delta)
    {
        if (player != null)
        {
            if (!IsOnFloor())
            {
                vel.Y -= fallAccel * (float)delta;
            }
            else vel.Y = 0;

            Velocity = vel.Rotated(Vector3.Up, Rotation.Y) * speed;
        }
    }

    protected object Collision()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            return collision.GetCollider();
        }
        return null;
    }

}
