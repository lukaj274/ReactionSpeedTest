using Godot;

namespace ReactionSpeedTest.Scripts.Main_Menu;

public partial class ChangeScene : Button
{
	[Export] public PackedScene SceneToLoad;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnButtonPressed()
	{
		GetTree().ChangeSceneToPacked(SceneToLoad);
	}
}