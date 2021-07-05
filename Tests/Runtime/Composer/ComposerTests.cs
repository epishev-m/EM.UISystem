using System;
using System.Collections.Generic;
using EM.UI;
using NUnit.Framework;

internal sealed class ComposerTests
{
	[Test]
	public void Composer_Constructor_Exception1()
	{
		// Arrange
		var modesController = new ModesController();
		var actual = false;

		// Act
		try
		{
			var unused = new Composer(null, modesController);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Composer_Constructor_Exception2()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var actual = false;

		// Act
		try
		{
			var unused = new Composer(panels, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Composer_Constructor_InParallel()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var modesController = new ModesController();

		// Act
		var composer = new Composer(panels, modesController);
		var actual = composer.InParallel();

		// Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void Composer_Constructor_InSequence()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var modesController = new ModesController();

		// Act
		var composer = new Composer(panels, modesController);
		var actual = composer.InSequence();

		// Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void Composer_Constructor_GetPanel()
	{
		// Arrange
		const string name = "test";
		var expected = new PanelTest(name);
		var panels = new List<IPanel>
		{
			expected
		};
		var modesController = new ModesController();

		// Act
		var composer = new Composer(panels, modesController);
		var actual = composer.GetPanel(name);

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

		public bool IsOpened => false;

		public bool IsInteractable
		{
			get;
			set;
		}

		public PanelTest(string name)
		{
			Name = name;
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
	}

	#endregion
}