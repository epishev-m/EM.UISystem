using System;
using System.Collections.Generic;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;

internal sealed class ComposerSequenceTests
{
	[Test]
	public void ComposerSequence_Constructor_Exception1()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerSequence(null, modesController, composer);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerSequence_Constructor_Exception2()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerSequence(root, null, composer);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerSequence_Constructor_Exception3()
	{
		// Arrange
		var modesController = new ModesController();
		var root = new CommandSequence();
		var actual = false;

		// Act
		try
		{
			var unused = new ComposerSequence(root, modesController, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerSequence_Open()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var root = new CommandSequence();
		var composer = new Composer(panels, modesController);

		// Act
		var expected = new ComposerSequence(root, modesController, composer);
		var actual = expected.Open("test");

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ComposerSequence_Close()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var root = new CommandSequence();
		var composer = new Composer(panels, modesController);

		// Act
		var expected = new ComposerSequence(root, modesController, composer);
		var actual = expected.Close("test");

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ComposerSequence_GetCommand()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var expected = new CommandSequence();
		var composer = new Composer(panels, modesController);

		// Act
		var composerBatch = new ComposerSequence(expected, modesController, composer);
		var actual = composerBatch.GetCommand();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ComposerSequence_OnComplete_Exception()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();
		var actual = false;

		// Act
		var composerSequence = new ComposerSequence(root, modesController, composer);

		try
		{
			var unused = composerSequence.OnComplete(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ComposerSequence_OnComplete()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();

		// Act
		var composerSequence = new ComposerSequence(root, modesController, composer);
		var composerComplete = composerSequence.OnComplete(() => { });

		// Assert
		Assert.IsNotNull(composerComplete);
	}

	[Test]
	public void ComposerSequence_InParallel()
	{
		// Arrange
		var panels = new List<IPanel>()
		{
			new PanelTest()
		};
		var modesController = new ModesController();
		var composer = new Composer(panels, modesController);
		var root = new CommandSequence();

		// Act
		var composerSequence = new ComposerSequence(root, modesController, composer);
		var actual = composerSequence.InParallel();

		// Assert
		Assert.IsNotNull(actual);
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
