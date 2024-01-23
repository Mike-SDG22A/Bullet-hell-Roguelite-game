using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

public partial class Gun : Weapon
{
    [Export] PackedScene bullet { get; set; }
	[Export] bool isEnemyGun = false;
	List<Enemy> enemies = new List<Enemy>();	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		damage = baseDmg;
		attackSpeed = baseAttackSpeed;
		cd = FIRE_RATE / attackSpeed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isEnemyGun)
		{

			foreach (Node obj in GetTree().CurrentScene.GetChildren()) 
			{
				if ( obj is Enemy en)
				{
					if (!enemies.Contains(en))
					{
						enemies.Add(en);
					}

				}
			
			}

			Enemy clossestEnemy = null;

			for (int i = 0; i < enemies.Count; i++)
			{

				if (IsInstanceValid(enemies[i])) { }
				else	{ enemies.RemoveAt(i); }
			}


			if (enemies.Count > 0)
			{
				clossestEnemy = enemies.Aggregate((x, y) => GlobalPosition.DistanceTo(x.GlobalPosition) < GlobalPosition.DistanceTo(y.GlobalPosition) ? x : y);

			}

			if (IsInstanceValid(clossestEnemy)) { }
			else { return; }




			LookAt(clossestEnemy.Position);
		}
		else
		{
			
        }


		cd -= (float)delta;

		if (cd < 0 ) 
		{
			Attack();
			cd = FIRE_RATE / attackSpeed;
		}
	}

    public override void Attack()
    {
		RigidBody3D obj = bullet.Instantiate() as RigidBody3D;

		if (obj is Bullet bul)
		{
			bul.dmg = damage;
		}

		if (!isEnemyGun)
		{
			CharacterBody3D parent = GetParent() as CharacterBody3D;

			obj.Position = parent.Position + Position;

		}
		else obj.Position = GlobalPosition;

		obj.Rotation = new Vector3(obj.Rotation.X, GlobalRotation.Y, obj.Rotation.Z);

		obj.Visible = true;
        GetTree().CurrentScene.AddChild(obj);
    }
}
