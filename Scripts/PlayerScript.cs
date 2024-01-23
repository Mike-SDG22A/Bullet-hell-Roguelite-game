using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerScript : CharacterBody3D
{
    const float BASE_HP = 100;
    float maxHP = BASE_HP;
    [Export] public float hp { get; set; } = BASE_HP;

    const float BASE_SPEED = 14;
    [Export] public float speed { get; set; } = BASE_SPEED;

    [Export] public float fallAccel { get; set; } = 75;
    [Export] private int exp { get; set; } = 0;
    [Export] private int expNeeded { get; set; } = 100;
    [Export] public float level { get; set; } = 1;
    [Export] public int money { get; set; } = 0;

    [Export] InstanciateEnemies waveScript;

    public int amountOfLevelsGained;

    public List<Weapon> weapons = new List<Weapon> ();
    
    public List<Buff> buffs = new List<Buff>();

    [Export] public RandomBuffGenerator generator;

    private Vector3 _targetVelocity = Vector3.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach(var obj in GetChildren())
        {
            if (obj is Weapon weapon)
            {
                if (!weapons.Contains(weapon))
                {
                    weapons.Add(weapon);
                }
            }
        }

    }

    public override void _Process(double delta)
    {
        if (exp >= expNeeded)
        {
            level++;
            exp = 0;
            hp = maxHP;
            expNeeded = (int)(expNeeded * 1.5f);
            amountOfLevelsGained++;
        }


        if (amountOfLevelsGained > 0 && waveScript.waveEnded)
        {
            generator.Visible = true;
        }
        else if (amountOfLevelsGained <= 0 && waveScript.waveEnded) 
        {
            generator.Visible = false;
            waveScript.waveEnded = false;
        }

        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/Hp").Text = "Health: " + hp + " / " + maxHP ;
        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/HBoxContainer/HBoxContainer/Level").Text = "Level: " + level;
        GetTree().CurrentScene.GetNode<Label>("GUI/VBoxContainer/HBoxContainer/HBoxContainer2/VBoxContainer/Exp").Text = "Exp: " + exp + "/" + expNeeded;


        GetBuffs();
        if (speed > BASE_SPEED * 2)
        {
            speed = BASE_SPEED * 2;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (hp <= 0) { GD.Print("You are dead"); }

        Movement(delta);
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    void Movement(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        }

        _targetVelocity.X = direction.X * speed;
        _targetVelocity.Z = direction.Z * speed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= fallAccel * (float)delta;
        }


        Velocity = _targetVelocity.Rotated(Vector3.Up.Normalized(), Rotation.Y);
        MoveAndSlide();
    }

    object Collision()
    {
        MoveAndSlide();
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            return collision.GetCollider();
        }
        return null;
    }

    public void AddExp(int exp)
    {
        this.exp += exp;
    }

    public void GetBuffs()
    {
        float attack = 0;
        float health = 0;
        float movementSpeed = 0;
        float attackSpeed = 0;

        foreach(Buff buff in buffs)
        {
            switch (buff.stat) 
            {
                case "Attack":
                    attack += buff.amount;
                    break;
                case "Health":
                    health += buff.amount;
                    break;
                case "MovementSpeed":
                    movementSpeed += buff.amount;
                    break;
                case "AttackSpeed":
                    attackSpeed += buff.amount;
                    break;
            }
        }

        maxHP = BASE_HP + health;
        speed = BASE_SPEED * (movementSpeed + 1);

        foreach (Weapon weapon in weapons) 
        {
            weapon.IncreaseDmg(attack + 1);
            weapon.IncreaseAttackSpeed(attackSpeed + 1);
        }
    }
}
