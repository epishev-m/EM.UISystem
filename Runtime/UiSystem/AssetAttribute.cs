namespace EM.UI
{

using System;
using Foundation;

public class AssetAttribute : Attribute
{
	public readonly string Id;

	public readonly LifeTime LifeTime;

	#region AssetAttribute

	public AssetAttribute(string id,
		LifeTime lifeTime)
	{
		Requires.ValidArgument(!string.IsNullOrWhiteSpace(id), nameof(id));

		Id = id;
		LifeTime = lifeTime;
	}

	#endregion
}

}