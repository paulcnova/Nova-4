
namespace Nova.Boot;

using Nova.UI;

using Godot;

/// <summary>A node used for startup to load resources in on boot.</summary>
public partial class BootLoader : Node
{
	#region Properties
	
	/// <summary>The loading screen being used</summary>
	private LoadingScreen loadingScreen;
	
	/// <summary>Gets the starting scene to load in after the boot load has finished.</summary>
	[Export] public PackedScene StartScene { get; private set; }
	
	/// <summary>Gets the default loading screen used when loading.</summary>
	[Export] public PackedScene DefaultLoadScreen { get; private set; }
	
	/// <summary>Gets the node that will contain the loading screen and current scene.</summary>
	[Export] public Node SceneContainer { get; private set; }
	
	#endregion // Properties
	
	#region Godot Methods
	
	/// <inheritdoc/>
	public override void _Ready()
	{
		LoadingScreen loading = this.DefaultLoadScreen.Instantiate<LoadingScreen>();
		
		this.loadingScreen = loading;
		this.SceneContainer.AddChild(loading);
		
		DRML.ContentLoaded += this.OnContentLoaded;
		DRML.LoadingCompleted += this.OnLoadingCompleted;
		DRML.LoadAllContent();
	}
	
	#endregion // Godot Methods
	
	#region Public Methods
	
	/// <summary>Loads the scene from the given resource path.</summary>
	/// <param name="path">The path to the scene in the resources.</param>
	/// <remarks>Unloads the current scene.</remarks>
	public void LoadScene(string path) => this.LoadScene(ResourceLoader.Load<PackedScene>(path));
	
	/// <summary>Loads the scene from the given packed scene.</summary>
	/// <param name="scene">The scene to instantiate and load.</param>
	/// <remarks>Unloads the current scene.</remarks>
	public void LoadScene(PackedScene scene)
	{
		Node node = scene.Instantiate<Node>();
		
		this.SceneContainer.QueueFreeChildren();
		this.SceneContainer.AddChild(node);
	}
	
	#endregion // Public Methods
	
	#region Private Methods
	
	/// <summary>Called when content is being loaded.</summary>
	/// <param name="resource">The resource being loaded in.</param>
	/// <param name="current">The current count of resources being loaded in.</param>
	/// <param name="max">The maximum count of resources being loaded in.</param>
	private void OnContentLoaded(DisplayableResource resource, int current, int max)
	{
		if(this.loadingScreen == null) { return; }
		
		this.loadingScreen.UpdateLoadingBar(resource, current, max);
	}
	
	/// <summary>Called when the content has done being loaded.</summary>
	private void OnLoadingCompleted()
	{
		if(this.loadingScreen == null) { return; }
		
		this.loadingScreen.LoadingIsCompleted(Callable.From(this.ChangeSceneToStart));
	}
	
	/// <summary>Changes the scene to the <see cref="StartScene"/>.</summary>
	private void ChangeSceneToStart()
	{
		this.SceneContainer.RemoveChild(this.loadingScreen);
		this.loadingScreen = null;
		
		Node start = this.StartScene.Instantiate<Node>();
		
		this.SceneContainer.AddChild(start);
	}
	
	#endregion // Private Methods
}
