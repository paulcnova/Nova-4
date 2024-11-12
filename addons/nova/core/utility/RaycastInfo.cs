
namespace Nova;

using Godot;
using Godot.Collections;

/// <summary>A class that gives you information on a raycast.</summary>
public sealed class RaycastInfo
{
	#region Properties
	
	/// <summary>The dictionary that returns when casting a ray.</summary>
	private Dictionary dictionary;
	
	/// <summary>Gets if the raycast hit something.</summary>
	public bool IsHit => this.dictionary.ContainsKey("position");
	
	/// <summary>Gets the position of the where the raycast hit.</summary>
	public Vector3 Position => this.dictionary.TryGetValue("position", out Variant position) ? position.AsVector3() : Vector3.Zero;
	
	/// <summary>Gets the normal of the surface the raycast hit.</summary>
	public Vector3 Normal => this.dictionary.TryGetValue("normal", out Variant normal) ? normal.AsVector3() : Vector3.Zero;
	
	/// <summary>Gets the face index of the surface the raycast hit.</summary>
	public int FaceIndex => this.dictionary.TryGetValue("face_index", out Variant faceIndex) ? faceIndex.AsInt32() : -1;
	
	/// <summary>Gets the collider ID of the object the raycast hit.</summary>
	public long ColliderID => this.dictionary.TryGetValue("collider_id", out Variant colliderID) ? colliderID.AsInt64() : -1;
	
	/// <summary>Gets the collider the raycast hit.</summary>
	public Variant Collider => this.dictionary.TryGetValue("collider", out Variant collider) ? collider : default;
	
	/// <summary>
	public int Shape => this.dictionary.TryGetValue("shape", out Variant shape) ? shape.AsInt32() : -1;
	public Rid RID => this.dictionary.TryGetValue("rid", out Variant rid) ? rid.AsRid() : default;
	public Dictionary Dictionary => this.dictionary;
	
	public RaycastInfo(Dictionary dictionary)
	{
		this.dictionary = dictionary;
	}
	
	#endregion // Properties
	
	#region Public Methods
	
	public static implicit operator RaycastInfo(Dictionary dictionary) => new RaycastInfo(dictionary);
	
	#endregion // Public Methods
}
