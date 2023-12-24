using Godot;

//parameters/Idle/blend_position


namespace TestingGodot.Characters;

public partial class PlayerCat : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 100;

	public AnimationTree AnimationTree { get; set; }
	public AnimationNodeStateMachinePlayback StateMachine { get; set; }

	[Export]
	public Vector2 StartingDirection { get; set; } = new Vector2(0, 1);

	public Vector2 ScreenSize;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		AnimationTree = GetNode<AnimationTree>("AnimationTree");
		StateMachine = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
		UpdateAnimationParameters(StartingDirection);
	}

	public override void _Process(double delta)
	{
		var inputDirection = new Vector2
		(
			Input.GetActionStrength("right") - Input.GetActionStrength("left"),
			Input.GetActionStrength("down") - Input.GetActionStrength("up")
		);

		UpdateAnimationParameters(inputDirection);

		Velocity = inputDirection * Speed;

		GetNode<Sprite2D>("Cat");
		MoveAndSlide();
		PickNewState();
	}

	public void UpdateAnimationParameters(Vector2 moveInput)
	{
		if(moveInput != Vector2.Zero)
		{
			AnimationTree.Set("parameters/Idle/blend_position", moveInput);
			AnimationTree.Set("parameters/Walk/blend_position", moveInput);
		}
	} 

	public void PickNewState()
	{
		if(Velocity != Vector2.Zero)
		{
			StateMachine.Travel("Walk");
		}
		else
		{
			StateMachine.Travel("Idle");
		}
	}
}
