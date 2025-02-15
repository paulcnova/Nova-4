
namespace Nova.UI;

using Godot;

using Nova;

/// <summary>A node that moves UI Controls that are direct children onto the UI Manager.</summary>
[GlobalClass] public partial class UIControlLoader : Node
{
	#region Godot Methods
	
	/// <inheritdoc/>
	public override void _EnterTree()
	{
		if(UIManagerNode.Instance == null)
		{
			GDX.PrintWarning("UI Manager is not instantiated! Could not load UI Controls.");
			return;
		}
		
		foreach(Node child in this.GetChildren())
		{
			if(child is Page)
			{
				this.InsertPage(child as Page);
			}
			if(child is Widget)
			{
				this.InsertWidget(child as Widget);
			}
		}
		this.QueueFree();
	}
	
	#endregion // Godot Methods
	
	#region Private Methods
	
	/// <summary>Inserts a page into the UI Manager.</summary>
	/// <param name="page">The page to move.</param>
	private void InsertPage(Page page)
	{
		if(UIManager.ContainsPage(page.GetType()))
		{
			page.QueueFree();
			return;
		}
		page.GetParent().RemoveChild(page);
		UIManager.AddPage(page);
	}
	
	/// <summary>Inserts a widget into the UI Manager.</summary>
	/// <param name="widget">The widget to move.</param>
	private void InsertWidget(Widget widget)
	{
		if(UIManager.ContainsWidget(widget.GetType()))
		{
			widget.QueueFree();
			return;
		}
		widget.GetParent().RemoveChild(widget);
		UIManager.AddWidget(widget);
	}
	
	#endregion // Private Methods
}
