using Godot;
using System;

public partial class InstanciateEnemies : Node
{
	[Export] PackedScene[] obj;
    [Export] StaticBody3D body;
    [Export] float maxTimer = 3;
    float timer;
    float maxWaveTimer = 60;
    float waveTimer;

    public override void _Ready()
    {
        timer = maxTimer;
        waveTimer = maxWaveTimer;
    }

    public override void _Process(double delta)
    {
        timer -= (float)delta;

        if (waveTimer < 0)
        {
            maxTimer *= 0.90f;
            waveTimer = maxWaveTimer;
        }

        if (timer < 0)
        {
            SpawnEnemy();
            timer = maxTimer;
        }

    }

    void SpawnEnemy()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();
        int enemy = random.RandiRange(0, obj.Length - 1);
        CharacterBody3D o = obj[enemy].Instantiate() as CharacterBody3D;


        float x = body.Position.X + (body.Scale.X / 2);
        float z = body.Position.Z + (body.Scale.Z / 2);

        Vector3 pos = new Vector3(random.RandfRange(-x, x),
            2, 
            random.RandfRange(-z, z));
        
        o.Position = pos;


        GetTree().CurrentScene.AddChild(o);
    }
}
