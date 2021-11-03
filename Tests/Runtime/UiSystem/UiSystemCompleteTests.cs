using System;
using System.Collections.Generic;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;

internal sealed class ComposerCompleteTests
{
	[Test]
	public void UiSystemComplete_Constructor_Exception1()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Complete(null, modesController, uiSystem);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemComplete_Constructor_Exception2()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var root = new CommandSequence();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Complete(root, null, uiSystem);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemComplete_Constructor_Exception3()
	{
		// Arrange
		var root = new CommandSequence();
		var modesController = new ModesController();
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Complete(root, modesController, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemComplete_GetCommand()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var expected = new CommandSequence();

		// Act
		var complete = new UiSystem.Complete(expected, modesController, uiSystem);
		var actual = complete.GetCommand();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void UiSystemComplete_InSequence()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();

		// Act
		var complete = new UiSystem.Complete(root, modesController, uiSystem);
		var actual = complete.InSequence();

		// Assert
		Assert.NotNull(actual);
	}

	[Test]
	public void UiSystemComplete_InParallel()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();

		// Act
		var complete = new UiSystem.Complete(root, modesController, uiSystem);
		var actual = complete.InParallel();

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

	private sealed class PanelsCreatorTest :
		IPanelsCreator
	{
		public IEnumerable<IPanel> CreatePanels()
		{
			var panels = new List<IPanel>
			{
				new PanelTest()
			};

			return panels;
		}
	}

	#endregion
}
