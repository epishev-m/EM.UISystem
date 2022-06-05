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
		Id = id;
		LifeTime = lifeTime;
	}

	#endregion
}

}