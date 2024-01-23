using Godot;
using System;

public partial class RandomBuffGenerator : Control
{
	[Export] public Buff[] buff = new Buff[3];

	public override void _Ready()
	{
		Visible = false;
		RandomizeBuffs();
    }

	public void RandomizeBuffs()
	{
		for (int i = 0; i < buff.Length && i < GetChildCount(); i++)
		{
			ChangeBuff(out buff[i]);

			if (buff[i].stat == null)
			{
				RandomizeBuffs();
			}

			var obj = GetChild(i);

			if (obj is BuffObj buffObj)
			{
				buffObj.buff = new Buff(buff[i]);

				string text = buff[i].stat.ToString() + ": +" + buff[i].amount.ToString();

				if (buff[i].amount < 0.6f) 
				{
					text = buff[i].stat.ToString() + ": +" + Mathf.Round(buff[i].amount * 100) + "%";
                }

				buffObj.text.Text = text;
			}
		}
	}

	void ChangeBuff(out Buff buff)
	{
		buff = new Buff();

        var random = new RandomNumberGenerator();
        random.Randomize();
        int type = random.RandiRange(0, 5);
		float val;
		float used;

        switch (type)
		{
			case 0:
				buff.stat = "Health";
				buff.amount = random.RandiRange(5, 25);
				break;
			case 1:
				buff.stat = "MovementSpeed";
				val = random.RandiRange(101, 110);
				used = val / 100;
				used--;
                buff.amount =  used;
				break;
			case 2:
				buff.stat = "AttackSpeed";
				val = random.RandiRange(110, 130);
                used = val / 100;
				used--;
                buff.amount =used;
				break;
			case 3:
				buff.stat = "Attack";
				val = random.RandiRange(110, 130);
                used = val / 100;
				used--;
                buff.amount = used;
                break;
        }
    }
}
