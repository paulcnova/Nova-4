
namespace Nova.UI;

/// <summary>An interface for check box containers in general.</summary>
public interface ICheckBoxContainer
{
	#region Properties
	
	/// <summary>Gets and sets the minimum selections required for the check box.</summary>
	int MinSelections { get; set; }
	
	/// <summary>Gets and sets the maximum selections required for the check box.</summary>
	int MaxSelections { get; set; }
	
	/// <summary>Gets and sets the amount of selections that can be made.</summary>
	int SelectionsCount { get; set; }
	
	/// <summary>Gets if the current selections meet the minimum amount of selections.</summary>
	bool IsAtMinimumSelections => this.SelectionsCount <= this.MinSelections;
	
	/// <summary>Gets if the current selections meet the maximum amount of selections.</summary>
	bool IsAtMaximumSelections => this.SelectionsCount >= this.MaxSelections;
	
	#endregion // Properties
	
	#region Methods
	
	/// <summary>Checks to see if the rest of the items are disables and disables them if they already aren't.</summary>
	void CheckToDisableRest();
	
	/// <summary>Selects a box using the given text.</summary>
	/// <param name="text">The text to match the box to select with.</param>
	void SelectUsingText(string text);
	
	/// <summary>Selects boxes using the given list of texts.</summary>
	/// <param name="texts">The list of texts to match the boxes to select.</param>
	void SelectUsingText(string[] texts);
	
	#endregion // Methods
}
