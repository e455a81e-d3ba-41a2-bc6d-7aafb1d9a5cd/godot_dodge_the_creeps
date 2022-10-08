using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene MobScene;
    public int Score;
    private HUD _hud;
    private AudioStreamPlayer _music;
    private AudioStreamPlayer _deathSound;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
        _hud = GetNode<HUD>("HUD");
        _music = GetNode<AudioStreamPlayer>("Music");
        _deathSound = GetNode<AudioStreamPlayer>("DeathSound");

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void GameOver()
    {
        _deathSound.Play();
        _music.Stop();
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        _hud.ShowGameOver();
    }

    public void NewGame()
    {
        _music.Play();
        GetTree().CallGroup("mobs", "queue_free");
        Score = 0;
        var startPosition = GetNode<Position2D>("StartPosition");
        GetNode("Player").Call("start", startPosition.Position.x, startPosition.Position.y);

        GetNode<Timer>("StartTimer").Start();
        _hud.UpdateScore(Score);
        _hud.ShowMessage("Get Ready!");
    }

    public void OnScoreTimerTimeout()
    {
        Score++;
        _hud.UpdateScore(Score);
    }

    public void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    public void OnMobTimerTimeout()
    {
        var mob = (Mob)MobScene.Instance();
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = GD.Randi();

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mob.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        // Choose the velocity.
        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
}
