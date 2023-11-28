using Godot;
using System;
using System.Diagnostics;

public partial class Movement : Node
{
	RigidBody3D rb;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rb = GetNode<RigidBody3D>("RigidBody3D");
		GD.Print(rb);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (rb != null)
		{
			Vector3 force = new Vector3(1, 0, 0);
			float multi = 0.0f;
			if (Input.IsKeyPressed(Key.Left)) { multi = 1.0f; }
			else if (Input.IsKeyPressed(Key.Right)) {  multi = -1.0f; }

			rb.ApplyForce(force * multi);
		}
	}
}
