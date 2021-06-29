using System;
using EM.UI;
using NUnit.Framework;

internal sealed class PanelInfoTests
{
	[Test]
	public void PanelInfo_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new PanelInfo(null, Modes.None);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void PanelInfo_Panel()
	{
		// Arrange
		var expected = new PanelTest();

		// Act
		var panelInfo = new PanelInfo(expected, Modes.None);
		var actual = panelInfo.Panel;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void PanelInfo_Mode()
	{
		// Arrange
		const Modes expected = Modes.None;
		var panel = new PanelTest();

		// Act
		var panelInfo = new PanelInfo(panel, expected);
		var actual = panelInfo.Mode;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#region Nested

	private sealed class PanelTest :
		IPanel
	{
		public string Name
		{
			get;
		}

		public bool IsOpened
		{
			get;
		}

		public bool IsInteractable
		{
			get;
			set;
		}

		public void Open(
			Action onPanelOpened)
		{
			throw new NotImplementedException();
		}

		public void Close(
			Action onPanelClosed)
		{
			throw new NotImplementedException();
		}

		public PanelTest()
		{
			Name = "test";
			IsOpened = false;
		}
	}

	#endregion
}
