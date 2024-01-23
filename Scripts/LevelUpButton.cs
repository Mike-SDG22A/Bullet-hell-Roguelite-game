using Godot;
using System;

public partial class LevelUpButton : Button
{
	BuffObj buffObj;

	public override void _Ready()
	{
		buffObj = GetParent() as BuffObj;
	}

    public override void _Pressed()
    {
		buffObj.AddBuff();
    }
}
