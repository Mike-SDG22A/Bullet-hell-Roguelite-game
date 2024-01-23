using Godot;
using System;

public partial class RangedEnemy : Enemy
{
    [Export] float minDistance = 4;

	public override void _Ready()
	{
        player = GetTree().CurrentScene.GetNode<CharacterBody3D>("Player");
    }

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

    public override void Movement(double delta)
    {

        if (player != null && Position.DistanceTo(player.Position) > minDistance)
        {
            vel = Vector3.Forward;
        } else if (player != null && Position.DistanceTo(player.Position) < minDistance - 0.5f)
        {
            vel = -Vector3.Forward;
        }
        else if (player != null)
        {
            vel = Vector3.Zero;
        }

        if (!IsOnFloor())
        {
            vel.Y -= fallAccel * (float)delta;
        }
        else vel.Y = 0;

        Velocity = vel.Rotated(Vector3.Up, Rotation.Y) * speed;
    }
}