using System;
using EM.UI;
using NUnit.Framework;
using UnityEngine;

internal sealed class ViewInfoTests
{
	[Test]
	public void ViewInfo_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new PanelViewInfo(null, Modes.None);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ViewInfo_Panel()
	{
		// Arrange
		var gameObject = new GameObject(); 
		var expected = gameObject.AddComponent<PanelViewTest>();

		// Act
		var panelInfo = new PanelViewInfo(expected, Modes.None);
		var actual = panelInfo.PanelView;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ViewInfo_Mode()
	{
		// Arrange
		const Modes expected = Modes.None;
		var gameObject = new GameObject(); 
		var panelView = gameObject.AddComponent<PanelViewTest>();

		// Act
		var panelInfo = new PanelViewInfo(panelView, expected);
		var actual = panelInfo.Mode;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#region Nested

	private sealed class PanelViewTest : PanelView
	{
	}

	#endregion
}
