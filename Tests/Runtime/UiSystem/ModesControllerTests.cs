using System;
using EM.UI;
using NUnit.Framework;
using UnityEngine;

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

	[Test]
	public void ModalLogicController_Add_TryGetPanelView_True()
	{
		// Arrange
		var modesController = new ModalLogicController();
		var gameObject = new GameObject();
		var panelView = gameObject.AddComponent<PanelViewTest>();

		// Act
		modesController.Add(panelView, Modes.None);
		var actual = modesController.TryGetPanelView<PanelViewTest>(out var actualPanelView);

		// Assert
		Assert.IsTrue(actual);
		Assert.NotNull(actualPanelView);
	}

	[Test]
	public void ModalLogicController_AddModal_TryGetPanelView_True()
	{
		// Arrange
		var modesController = new ModalLogicController();
		var gameObject = new GameObject();
		var panelView = gameObject.AddComponent<PanelViewTest>();

		// Act
		modesController.Add(panelView, Modes.Modal);
		var actual = modesController.TryGetPanelView<PanelViewTest>(out var actualPanelView);

		// Assert
		Assert.IsTrue(actual);
		Assert.NotNull(actualPanelView);
	}

	[Test]
	public void ModalLogicController_Add_TryGetPanelView_False()
	{
		// Arrange
		var modesController = new ModalLogicController();

		// Act
		var actual = modesController.TryGetPanelView<PanelViewTest>(out var actualPanelView);

		// Assert
		Assert.IsFalse(actual);
		Assert.Null(actualPanelView);
	}

	[Test]
	public void ModalLogicController_Remove()
	{
		// Arrange
		var modesController = new ModalLogicController();
		var gameObject = new GameObject();
		var panelView = gameObject.AddComponent<PanelViewTest>();

		// Act
		modesController.Add(panelView, Modes.None);
		modesController.Remove(panelView);
		var actual = modesController.TryGetPanelView<PanelViewTest>(out var actualPanelView);

		// Assert
		Assert.IsFalse(actual);
		Assert.Null(actualPanelView);
	}

	[Test]
	public void ModalLogicController_RemoveModal()
	{
		// Arrange
		var modesController = new ModalLogicController();
		var gameObject = new GameObject();
		var panelView = gameObject.AddComponent<PanelViewTest>();

		// Act
		modesController.Add(panelView, Modes.Modal);
		modesController.Remove(panelView);
		var actual = modesController.TryGetPanelView<PanelViewTest>(out var actualPanelView);

		// Assert
		Assert.IsFalse(actual);
		Assert.Null(actualPanelView);
	}

	#region Nested

	private sealed class PanelViewTest : UIView
	{
		protected override void OnInitialize()
		{
			throw new NotImplementedException();
		}

		protected override void OnRelease()
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}
