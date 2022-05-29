using System;
using EM.UI;
using NUnit.Framework;

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
			var unused = new ViewInfo(null, Modes.None);
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
		var expected = new PanelTest();

		// Act
		var panelInfo = new ViewInfo(expected, Modes.None);
		var actual = panelInfo.Panel;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ViewInfo_Mode()
	{
		// Arrange
		const Modes expected = Modes.None;
		var panel = new PanelTest();

		// Act
		var panelInfo = new ViewInfo(panel, expected);
		var actual = panelInfo.Mode;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#region Nested

	private sealed class PanelTest :
		IPanel
	{
		public bool IsOpened
		{
			get;
		}

		public bool IsInteractable
		{
			get;
			set;
		}

		public void Open(Action onPanelOpened)
		{
			throw new NotImplementedException();
		}

		public void Close(Action onPanelClosed)
		{
			throw new NotImplementedException();
		}

		public PanelTest()
		{
			IsOpened = false;
		}
	}

	#endregion
}
