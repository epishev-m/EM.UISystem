using System;
using System.Collections.Generic;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;

internal sealed class ComposerBatchTests
{
	[Test]
	public void UiSystemBatch_Constructor_Exception1()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Batch(null, modesController, uiSystem);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemBatch_Constructor_Exception2()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Batch(root, null, uiSystem);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemBatch_Constructor_Exception3()
	{
		// Arrange
		var modesController = new ModesController();
		var root = new CommandSequence();
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem.Batch(root, modesController, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemBatch_Open()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var root = new CommandSequence();
		var uiSystem = new UiSystem(panelsCreator, modesController);

		// Act
		var expected = new UiSystem.Batch(root, modesController, uiSystem);
		var actual = expected.Open("test");

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void UiSystemBatch_Close()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var root = new CommandSequence();
		var uiSystem = new UiSystem(panelsCreator, modesController);

		// Act
		var expected = new UiSystem.Batch(root, modesController, uiSystem);
		var actual = expected.Close("test");

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void UiSystemBatch_GetCommand()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var expected = new CommandSequence();
		var uiSystem = new UiSystem(panelsCreator, modesController);

		// Act
		var composerBatch = new UiSystem.Batch(expected, modesController, uiSystem);
		var actual = composerBatch.GetCommand();

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void UiSystemBatch_OnComplete_Exception()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();
		var actual = false;

		// Act
		var batch = new UiSystem.Batch(root, modesController, uiSystem);

		try
		{
			var unused = batch.OnComplete(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystemBatch_OnComplete()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();

		// Act
		var batch = new UiSystem.Batch(root, modesController, uiSystem);
		var composerComplete = batch.OnComplete(() => { });

		// Assert
		Assert.IsNotNull(composerComplete);
	}

	[Test]
	public void UiSystemBatch_InSequence()
	{
		// Arrange
		var panelsCreator = new PanelsCreatorTest();
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var root = new CommandSequence();

		// Act
		var batch = new UiSystem.Batch(root, modesController, uiSystem);
		var actual = batch.InSequence();

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
