
namespace Nova.UI;

using Godot;

/// <summary>A flow container that acts like a radio container.</summary>
[GlobalClass] public partial class RadioFlowContainer : FlowContainer, IRadioContainer
{
	#region Properties
	
	/// <inheritdoc/>
	[Export] public bool DefaultSelectFirstSlot { get; set; } = true;
	
	/// <inheritdoc/>
	public Button Selected { get; private set; }
	
	/// <summary>Gets if a child is selected.</summary>
	public bool IsChildSelected
	{
		get
		{
			foreach(Button button in this.GetChildren<Button>())
			{
				if(button.ButtonPressed) { return true;}
			}
			
			return false;
		}
	}
	
	/// <summary>An event for when a button gets selected.</summary>
	/// <param name="selected">The button that gets selected.</param>
	[Signal] public delegate void OnSelectedEventHandler(Button selected);
	
	#endregion // Properties
	
	#region Godot Methods
	
	/// <inheritdoc/>
	public override void _Ready()
	{
		foreach(Node child in this.GetChildren())
		{
			this.OnChildEnteredTree(child);
		}
		this.ChildEnteredTree += this.OnChildEnteredTree;
		this.ChildExitingTree += this.OnChildExitingTree;
		
		if(this.DefaultSelectFirstSlot && !this.IsChildSelected)
		{
			if(this.GetChildCount() > 0)
			{
				Node node = this.GetChild(0);
				
				if(node is Button button)
				{
					button.EmitSignal(Button.SignalName.Pressed);
				}
			}
		}
	}
	
	#endregion // Godot Methods
	
	#region Public Methods
	
	/// <inheritdoc/>
	public void ForceSelectFirstSlot()
	{
		if(this.DefaultSelectFirstSlot && this.GetChildCount() >= 1 && !this.IsChildSelected)
		{
			Node child = this.GetChild(0);
			
			if(child is Button button)
			{
				this.SetSelected(button);
			}
		}
	}
	
	/// <inheritdoc/>
	public void SetSelected(Button button)
	{
		this.Selected = button;
	}
	
	/// <inheritdoc/>
	public void SelectUsingText(string text) => this.SelectUsingText(text, true);
	
	/// <summary>Selects the radio button by searching from the text of the button.</summary>
	/// <param name="text">The text to search with.</param>
	/// <param name="emit">Set to true to emit signals.</param>
	public void SelectUsingText(string text, bool emit)
	{
		foreach(Node child in this.GetChildren())
		{
			if(child is Button button)
			{
				if(button.Text == text)
				{
					if(emit)
					{
						button.EmitSignal(Button.SignalName.Pressed);
					}
					else
					{
						button.ButtonPressed = true;
						this.OnSelect(button);
					}
					break;
				}
			}
		}
	}
	
	#endregion // Public Methods
	
	#region Private Methods
	
	private void OnChildEnteredTree(Node child)
	{
		foreach(StringName group in this.GetGroups())
		{
			child.AddToGroup(group);
		}
		if(child is Button button)
		{
			if(this.GetChildCount() == 1 && this.DefaultSelectFirstSlot)
			{
				this.SetSelected(button);
			}
			button.Pressed += () => this.OnSelect(button);
		}
	}
	
	private void OnChildExitingTree(Node child)
	{
		if(this.Selected == child)
		{
			this.Selected = null;
		}
	}
	
	/// <summary>Called when a button gets selected.</summary>
	/// <param name="button">The button that got selected.</param>
	protected virtual void OnSelect(Button button)
	{
		foreach(Node child in this.GetChildren())
		{
			if(child is Button btn)
			{
				btn.ButtonPressed = false;
				if(btn.HasMethod("UpdateOnUnselected"))
				{
					btn.Call("UpdateOnUnselected");
				}
			}
		}
		if(button.HasMethod("UpdateOnSelected"))
		{
			button.Call("UpdateOnSelected");
		}
		button.ButtonPressed = true;
		this.SetSelected(button);
		this.EmitSignal(SignalName.OnSelected, button);
	}
	
	#endregion // Private Methods
}
