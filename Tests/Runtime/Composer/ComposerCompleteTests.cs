using System;
using System.Collections.Generic;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;

internal sealed class ComposerCompleteTests
{
	[Test]
	public void ComposerComplete_Constructor_Exception1()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerComplete(null, modesController, composer);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerComplete_Constructor_Exception2()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest()
		};
		var root = new CommandSequence();
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerComplete(root, null, composer);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerComplete_Constructor_Exception3()
	{
		// Arrange
		var root = new CommandSequence();
		var modesController = new ModesController();
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerComplete(root, modesController, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerComplete_GetCommand()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var expected = new CommandSequence();

		// Act
		var composerComplete = new ComposerComplete(expected, modesController, composer);
		var actual = composerComplete.GetCommand();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ComposerComplete_InSequence()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();

		// Act
		var composerComplete = new ComposerComplete(root, modesController, composer);
		var actual = composerComplete.InSequence();

		// Assert
		Assert.NotNull(actual);
	}

	[Test]
	public void ComposerComplete_InParallel()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();

		// Act
		var composerComplete = new ComposerComplete(root, modesController, composer);
		var actual = composerComplete.InParallel();

		// Assert
		Assert.NotNull(actual);
	}

	#region Nested

	private sealed class PanelTest :
		IPanel
	{
		public string Name => "test";

		public bool IsOpened => false;

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
	}

	#endregion
}
