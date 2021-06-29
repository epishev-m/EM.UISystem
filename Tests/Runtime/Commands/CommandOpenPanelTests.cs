using System;
using EM.UI;
using NUnit.Framework;

internal sealed class CommandOpenPanelTests
{
	[Test]
	public void CommandOpenPanelNone_Constructor_Exception1()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesControllerTest();

		// Act
		try
		{
			var unused = new CommandOpenPanelNone(null, modesController);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandOpenPanelNone_Constructor_Exception2()
	{
		// Arrange
		var actual = false;
		var panel = new PanelTest();

		// Act
		try
		{
			var unused = new CommandOpenPanelNone(panel, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandOpenPanelModal_Constructor_Exception1()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesControllerTest();

		// Act
		try
		{
			var unused = new CommandOpenPanelModal(null, modesController);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void CommandOpenPanelModal_Constructor_Exception2()
	{
		// Arrange
		var actual = false;
		var panel = new PanelTest();

		// Act
		try
		{
			var unused = new CommandOpenPanelModal(panel, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
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

	private sealed class ModesControllerTest :
		IModesController
	{
		public bool TryGetPanelInfo(
			IPanel panel,
			out PanelInfo panelInfo)
		{
			throw new NotImplementedException();
		}

		public void PrepareAdd(
			Modes mode,
			Action onCompleted)
		{
			throw new NotImplementedException();
		}

		public void PrepareRemove(
			Modes mode,
			Action onCompleted)
		{
			throw new NotImplementedException();
		}

		public void Add(
			IPanel panel,
			Modes mode)
		{
			throw new NotImplementedException();
		}

		public void Remove(
			IPanel panel,
			Modes mode)
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}
