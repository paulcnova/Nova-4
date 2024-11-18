
namespace Nova.UI;

using Godot;

using System.Collections.Generic;

/// <summary>A horizontal box container that acts like a check box container.</summary>
[GlobalClass] public partial class CheckBoxHBoxContainer : HBoxContainer, ICheckBoxContainer
{
	#region Properties
	
	/// <inheritdoc/>
	[Export] public int MinSelections { get; set; } = 1;
	
	/// <inheritdoc/>
	[Export] public int MaxSelections { get; set; } = 1;
	
	/// <inheritdoc/>
	[Export] public int SelectionsCount { get; set; } = 0;
	
	/// <inheritdoc/>
	public bool IsAtMinimumSelections => this.SelectionsCount <= this.MinSelections;
	
	/// <inheritdoc/>
	public bool IsAtMaximumSelections => this.SelectionsCount >= this.MaxSelections;
	
	/// <inheritdoc cref="CheckBoxFlowContainer.OnSelectedEventHandler"/>
	[Signal] public delegate void OnSelectedEventHandler(Button[] buttons);
	
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
		this.CheckToDisableRest();
	}
	
	#endregion // Godot Methods
	
	#region Public Methods
	
	/// <inheritdoc/>
	public void CheckToDisableRest()
	{
		if(this.SelectionsCount >= this.MaxSelections)
		{
			this.DisableAllUnselected();
		}
	}
	
	/// <inheritdoc/>
	public void SelectUsingText(string text) => this.SelectUsingText(text, true);
	
	/// <inheritdoc cref="CheckBoxFlowContainer.SelectUsingText(string, bool)"/>
	public void SelectUsingText(string text, bool emit)
	{
		if(this.IsAtMaximumSelections) { return; }
		
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
	
	/// <inheritdoc/>
	public void SelectUsingText(string[] text) => this.SelectUsingText(text, true);
	
	/// <inheritdoc cref="CheckBoxFlowContainer.SelectUsingText(string[], bool)"/>
	public void SelectUsingText(string[] texts, bool emit)
	{
		foreach(string text in texts)
		{
			this.SelectUsingText(text, emit);
		}
	}
	
	/// <inheritdoc cref="CheckBoxFlowContainer.Emit"/>
	public void Emit()
	{
		List<Button> buttons = new List<Button>();
		
		foreach(Node child in this.GetChildren())
		{
			if(child is Button btn)
			{
				if(btn.ButtonPressed)
				{
					buttons.Add(btn);
				}
			}
		}
		
		this.EmitSignal(SignalName.OnSelected, buttons.ToArray());
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
			button.Pressed += () => this.OnSelect(button);
		}
	}
	
	/// <inheritdoc cref="CheckBoxFlowContainer.OnSelect(Button)"/>
	protected virtual void OnSelect(Button button)
	{
		// Ignore the button press since it shouldn't get lowered
		if(button.ButtonPressed == false && this.SelectionsCount <= this.MinSelections)
		{
			button.ButtonPressed = true;
			return;
		}
		if(button.ButtonPressed == false)
		{
			--this.SelectionsCount;
			this.EnableAll();
		}
		else
		{
			++this.SelectionsCount;
		}
		
		if(this.SelectionsCount >= this.MaxSelections)
		{
			this.DisableAllUnselected();
		}
		
		List<Button> buttons = new List<Button>();
		
		foreach(Node child in this.GetChildren())
		{
			if(child is Button btn)
			{
				if(btn.ButtonPressed)
				{
					buttons.Add(btn);
				}
			}
		}
		
		this.EmitSignal(SignalName.OnSelected, buttons.ToArray());
	}
	
	/// <inheritdoc cref="CheckBoxFlowContainer.EnableAll"/>
	protected void EnableAll()
	{
		foreach(Node child in this.GetChildren())
		{
			if(child is Button button)
			{
				button.Disabled = false;
			}
		}
	}
	
	/// <inheritdoc cref="CheckBoxFlowContainer.DisableAllUnselected"/>
	protected void DisableAllUnselected()
	{
		foreach(Node child in this.GetChildren())
		{
			if(child is Button button)
			{
				if(!button.ButtonPressed)
				{
					button.Disabled = true;
				}
			}
		}
	}
	
	#endregion // Private Methods
}
