using Godot;
using System;

public partial class BuffObj : Node
{
	[Export] public int buffNum = 0;
	[Export] public Buff buff = new Buff();
	[Export] public TextEdit text;
	RandomBuffGenerator generator;
	PlayerScript player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		generator = GetParent() as RandomBuffGenerator;
        player = GetTree().CurrentScene.GetNode<CharacterBody3D>("Player") as PlayerScript;
    }


	public void AddBuff()
	{
        player.buffs.Add(new Buff(buff));
        generator.RandomizeBuffs();
		generator.Visible = false;
    }
}
