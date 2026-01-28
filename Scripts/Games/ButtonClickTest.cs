using Godot;

namespace ReactionSpeedTest.Scripts.Games;

public partial class ButtonClickTest : Button
{
	[Export] public PackedScene SceneToLoad;
	[Export] public ColorRect ColorRect;
	[Export] public Label TimeLabel;
	[Export] public Timer Timer;

	private RandomNumberGenerator _randomGenerator = new RandomNumberGenerator();
	private bool _isPlaying;
	private State _state;
	private double _elapsedTime;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_state = State.Default;
		_elapsedTime = 0.0;
		
		Timer.Timeout += OnTimerTimeout;
		
		Timer.WaitTime = 1.0;
		Timer.OneShot = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (_state)
		{
			case State.Default:
			{
				ColorRect.Color = new Color(0, 0, 255);
				Text = "Start!";
				break;
			}
			case State.CountingDown:
			{
				ColorRect.Color = new Color(255, 0, 0);
				TimeLabel.Text = "WAIT";
				break;
			}
			case State.CountingUp:
			{
				ColorRect.Color = new Color(0, 255, 0);
				UpdateTimerDisplay();
				Text = "Stop!";
				break;
			}
		}
	}

	public async void WaitTimer(float seconds)
	{
		Hide();
		_isPlaying = true;
		_state = State.CountingDown;
		
		await ToSignal(GetTree().CreateTimer(seconds), Timer.SignalName.Timeout);
		Timer.Start();
		_isPlaying = false;
		_state = State.CountingUp;
		Show();
	}

	public void OnButtonPressed()
	{
		switch (_state)
		{
			case State.Default:
			{
				// Start the game
				StartGame();
				break;
			}
			case State.CountingDown: break;
			case State.CountingUp:
			{
				// Stop the game
				StopGame();
				break;
			}
		}
	}

	private void StartGame()
	{
		var r = _randomGenerator.RandfRange(0.5f, 2.5f);
		WaitTimer(r);
	}

	private void StopGame()
	{
		Timer.Stop();
		UpdateTimerDisplay();
		_state = State.Default;
	}

	private void OnTimerTimeout()
	{
		_elapsedTime += Timer.WaitTime;
		UpdateTimerDisplay();
	}

	private void UpdateTimerDisplay()
	{
		int minutes = (int)(_elapsedTime / 60);
		int seconds = (int)(_elapsedTime % 60);
		TimeLabel.Text = $"{minutes:D2}:{seconds:D2}";
	}
}

public enum State
{
	Default,
	CountingDown,
	CountingUp
}