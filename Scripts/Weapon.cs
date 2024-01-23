using Godot;
using System;

public partial class Weapon : Node3D
{
	[Export] protected float baseAttackSpeed;
	[Export] protected float baseDmg;
	
	[Export] protected float attackSpeed;
	[Export] protected float damage;
	[Export] protected float range;

	protected const int FIRE_RATE = 1;
	protected float cd;

	public override void _Ready()
	{
		damage = baseDmg;
		attackSpeed = baseAttackSpeed;
	}

	public virtual void Attack()
	{

	}

	public void IncreaseDmg(float buff)
	{
		damage = baseDmg * buff;
	}

	public void IncreaseAttackSpeed(float buff)
	{
		attackSpeed = baseAttackSpeed * buff;
	}
}
