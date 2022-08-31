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
			modesController.Remove(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#region Nested

	private sealed class PanelViewTest : PanelView
	{
	}

	#endregion
}
