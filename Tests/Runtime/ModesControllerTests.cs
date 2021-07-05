using System;
using EM.UI;
using NUnit.Framework;

internal sealed class ModesControllerTests
{
	[Test]
	public void ModesController_Add_Exception()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesController();

		// Act
		try
		{
			modesController.Add(null, Modes.None);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModesController_Remove_Exception()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesController();

		// Act
		try
		{
			modesController.Remove(null, Modes.None);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModesController_PrepareAdd_Callback()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesController();

		// Act
		modesController.PrepareAdd(Modes.None, () => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModesController_PrepareRemove_Callback()
	{
		// Arrange
		var actual = false;
		var modesController = new ModesController();

		// Act
		modesController.PrepareRemove(Modes.None, () => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModesController_TryGetPanelInfo_Panel()
	{
		// Arrange
		var expected = new PanelTest();
		var modesController = new ModesController();

		// Act
		modesController.Add(expected, Modes.None);
		modesController.TryGetPanelInfo(expected, out var panelInfo);
		var actual = panelInfo.Panel;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ModesController_TryGetPanelInfo_Mode()
	{
		// Arrange
		const Modes expected = Modes.None;
		var panel = new PanelTest();
		var modesController = new ModesController();

		// Act
		modesController.Add(panel, expected);
		modesController.TryGetPanelInfo(panel, out var panelInfo);
		var actual = panelInfo.Mode;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ModesController_TryGetPanelInfo_True()
	{
		// Arrange
		var panel = new PanelTest();
		var modesController = new ModesController();

		// Act
		modesController.Add(panel, Modes.None);
		var actual = modesController.TryGetPanelInfo(panel, out _);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModesController_TryGetPanelInfo_False()
	{
		// Arrange
		var panel = new PanelTest();
		var modesController = new ModesController();

		// Act
		var actual = modesController.TryGetPanelInfo(panel, out _);

		// Assert
		Assert.IsFalse(actual);
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
