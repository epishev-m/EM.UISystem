using System;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;

public sealed class AssetAttributeTests
{
	[Test]
	public void AssetAttribute_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new AssetAttribute(string.Empty, LifeTime.Local);
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void AssetAttribute_Id()
	{
		// Arrange
		const string expected = "test";

		// Act
		var assetAttribute = new AssetAttribute(expected, LifeTime.Global);
		var actual = assetAttribute.Id;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void AssetAttribute_LifeTime()
	{
		// Arrange
		const LifeTime expected = LifeTime.Global;

		// Act
		var assetAttribute = new AssetAttribute("test", expected);
		var actual = assetAttribute.LifeTime;

		// Assert
		Assert.AreEqual(expected, actual);
	}
}