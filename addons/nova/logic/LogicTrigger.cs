
namespace Nova.Logic;

using Godot;

[GlobalClass] public abstract partial class LogicTrigger : Node
{
	#region Properties
	
	[Export] public bool AutoHook { get; set; } = true;
	[Export] public StringName HookName { get; set; }
	
	#endregion // Properties
	
	#region Godot Methods
	
	public override void _EnterTree()
	{
		Node parent = this.GetParent();
		
		if(parent == null)
		{
			this.QueueFree();
			return;
		}
		
		if(this.AutoHook)
		{
			this.AutoHookIn();
		}
		else if(!string.IsNullOrEmpty(this.HookName))
		{
			parent.Connect(this.HookName, Callable.From(this.TriggerLogic));
		}
	}
	
	public override void _ExitTree()
	{
		Node parent = this.GetParent();
		
		if(parent == null)
		{
			return;
		}
		
		if(this.AutoHook)
		{
			this.AutoHookOut();
		}
		else if(!string.IsNullOrEmpty(this.HookName))
		{
			parent.Disconnect(this.HookName, Callable.From(this.TriggerLogic));
		}
	}
	
	#endregion // Godot Methods
	
	#region Public Methods
	
	public abstract void TriggerLogic();
	
	#endregion // Public Methods
	
	#region Private Methods
	
	private void AutoHookIn()
	{
		Node parent = this.GetParent();
		
		if(parent is Button btn)
		{
			btn.Pressed += this.TriggerLogic;
		}
	}
	
	private void AutoHookOut()
	{
		Node parent = this.GetParent();
		
		if(parent is Button btn)
		{
			btn.Pressed -= this.TriggerLogic;
		}
	}
	
	#endregion // Private Methods
}
