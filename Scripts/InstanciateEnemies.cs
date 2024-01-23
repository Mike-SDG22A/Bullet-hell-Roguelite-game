using Godot;
using System;

public partial class InstanciateEnemies : Node
{
    [Export] PackedScene[] obj;
    [Export] StaticBody3D body;
    [Export] float maxTimer = 3;
    [Export] public bool waveEnded = false;
    [Export] RichTextLabel waveTimerUI;
    const float BASE_WAVE_TIMER = 10;
    [Export] int wave = 1;
    float timer;
    float maxWaveTimer = BASE_WAVE_TIMER;
    float waveTimer = BASE_WAVE_TIMER;

    public override void _Ready()
    {
        timer = maxTimer;
    }

    public override void _Process(double delta)
    {
        if (!waveEnded) 
        {
            timer -= (float)delta;
            waveTimer -= (float)delta;
        }

        waveTimerUI.Text = $"[center]{Mathf.Round(waveTimer)}[/center]";


        if (waveTimer < 0 && !waveEnded)
        {
            waveEnded = true;
            maxTimer *= 0.90f;
            maxWaveTimer =  Mathf.Round(maxWaveTimer * 1.15f);
            maxWaveTimer = Mathf.Clamp(maxWaveTimer, BASE_WAVE_TIMER, BASE_WAVE_TIMER * 4);
            waveTimer = maxWaveTimer;
            wave++;
        }

        if (timer < 0 && !waveEnded)
        {
            SpawnEnemy();
            timer = maxTimer;
        }

        if (waveEnded)
        {
            foreach (Node obj in GetTree().CurrentScene.GetChildren())
            {
                if (obj is Enemy en)
                {
                    obj.QueueFree();
                }
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
}