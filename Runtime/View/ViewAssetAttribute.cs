namespace EM.UI
{

using System;
using Foundation;

public class ViewAssetAttribute : Attribute
{
	public readonly string Id;

	public readonly LifeTime LifeTime;

	#region AssetAttribute

	public ViewAssetAttribute(string id,
		LifeTime lifeTime)
	{
		Requires.ValidArgument(!string.IsNullOrWhiteSpace(id), nameof(id));

		Id = id;
		LifeTime = lifeTime;
	}

	#endregion
}

}