using Godot;
using System;

public class HUD : CanvasLayer
{
    private Label _message;
    private Label _score;
    private Timer _timer;
    private Button _button;

    [Signal]
    public delegate void StartGame();

    public override void _Ready()
    {
        _message = GetNode<Label>("Message");
        _score = GetNode<Label>("ScoreLabel");
        _timer = GetNode<Timer>("MessageTimer");
        _button = GetNode<Button>("StartButton");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void ShowMessage(string text)
    {
        _message.Text = text;
        _message.Show();

        _timer.Start();
    }

    public async void ShowGameOver()
    {
        ShowMessage("Game Over");
        await ToSignal(_timer, "timeout");

        _message.Text = "Dodge the\n Creeps!";
        _message.Show();

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _button.Show();
    }

    public void UpdateScore(int score)
    {
        _score.Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        _button.Hide();
        EmitSignal("StartGame");
    }

    public void OnMessageTimerTimeout()
    {
        _message.Hide();
    }
}
