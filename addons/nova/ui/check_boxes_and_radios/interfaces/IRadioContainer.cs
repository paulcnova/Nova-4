
namespace Nova.UI;

using Godot;

/// <summary>An interface for a radio container in general.</summary>
public interface IRadioContainer
{
	#region Properties
	
	/// <summary>Gets and sets if the radio container should select the first slot by default.</summary>
	bool DefaultSelectFirstSlot { get; set; }
	
	/// <summary>Gets the selected radio button within the container.</summary>
	Button Selected { get; }
	
	#endregion // Properties
	
	#region Public Methods
	
	/// <summary>Forces selecting the first slot.</summary>
	void ForceSelectFirstSlot();
	
	/// <summary>Sets the selected button to the given button.</summary>
	/// <param name="button">The radio button to set selected.</param>
	void SetSelected(Button button);
	
	/// <summary>Selects the radio button by searching for the text of the button.</summary>
	/// <param name="text">The text to search with.</param>
	void SelectUsingText(string text);
	
	#endregion // Public Methods
}
