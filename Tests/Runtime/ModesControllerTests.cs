using System;
using EM.UI;
using NUnit.Framework;

internal sealed class ModalLogicControllerTests
{
	[Test]
	public void ModalLogicController_Add_Exception()
	{
		// Arrange
		var actual = false;
		var modesController = new ModalLogicController();

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
	public void ModalLogicController_Remove_Exception()
	{
		// Arrange
		var actual = false;
		var modesController = new ModalLogicController();

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
	public void ModalLogicController_PrepareAdd_Callback()
	{
		// Arrange
		var actual = false;
		var modesController = new ModalLogicController();

		// Act
		modesController.PrepareAdd(Modes.None, () => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModalLogicController_PrepareRemove_Callback()
	{
		// Arrange
		var actual = false;
		var modesController = new ModalLogicController();

		// Act
		modesController.PrepareRemove(Modes.None, () => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModalLogicController_TryGetViewInfo_Panel()
	{
		// Arrange
		var expected = new PanelTest();
		var modesController = new ModalLogicController();

		// Act
		modesController.Add(expected, Modes.None);
		modesController.TryGetViewInfo(expected, out var viewInfo);
		var actual = viewInfo.Panel;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ModalLogicController_TryGetViewInfo_Mode()
	{
		// Arrange
		const Modes expected = Modes.None;
		var panel = new PanelTest();
		var modesController = new ModalLogicController();

		// Act
		modesController.Add(panel, expected);
		modesController.TryGetViewInfo(panel, out var panelInfo);
		var actual = panelInfo.Mode;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void ModalLogicController_TryGetViewInfo_True()
	{
		// Arrange
		var view = new PanelTest();
		var modesController = new ModalLogicController();

		// Act
		modesController.Add(view, Modes.None);
		var actual = modesController.TryGetViewInfo(view, out _);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void ModalLogicController_TryGetViewInfo_False()
	{
		// Arrange
		var view = new PanelTest();
		var modesController = new ModalLogicController();

		// Act
		var actual = modesController.TryGetViewInfo(view, out _);

		// Assert
		Assert.IsFalse(actual);
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
