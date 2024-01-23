using Godot;
using System;

public partial class Bullet : RigidBody3D
{
    [Export] float speed = 5;
    [Export] public float dmg = 1;
    [Export] bool playerBullet;
    [Export] float timeToLive = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        ContactMonitor = true;
        MaxContactsReported = 1;

	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        SetAxisVelocity(Vector3.Forward.Rotated(Vector3.Up.Normalized(), Rotation.Y) * speed);

        timeToLive -= (float)delta;

        if (timeToLive <= 0) 
        {
            QueueFree();
        }

        object obj = Collision();
        if (obj != null)
        {
            if (obj is PlayerScript player && playerBullet)
            {
                player.hp -= dmg;
                QueueFree();
            }
            else if (obj is Enemy en)
            {
                en.hp -= dmg;
                QueueFree();
            }
            else
            {
                QueueFree();
            }
        }
    }


    object Collision()
    {
        for (int i = 0; i < GetCollidingBodies().Count; i++)
        {
            var collision = GetCollidingBodies()[i];
            return collision;
        }
        return null;
    }
}
