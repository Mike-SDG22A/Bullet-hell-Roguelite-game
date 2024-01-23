using Godot;
using System;

public partial class Buff : Node
{
    [Export] public string stat { get; set; }
    [Export] public float amount { get; set;}

    public Buff() { }
    public Buff(Buff buff)
    {
        stat = buff.stat;
        amount = buff.amount;
    }
}
