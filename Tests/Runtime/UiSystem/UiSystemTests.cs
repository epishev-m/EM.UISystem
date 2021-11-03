using System;
using System.Collections;
using System.Collections.Generic;
using EM.UI;
using NUnit.Framework;

internal sealed class UiSystemTests
{
	[Test]
	public void UiSystem_Constructor_Exception1()
	{
		// Arrange
		var modesController = new ModesController();
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem(null, modesController);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystem_Constructor_Exception2()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var actual = false;

		// Act
		try
		{
			var unused = new UiSystem(panelsCreator, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystem_Constructor_InParallel()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var modesController = new ModesController();

		// Act
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = uiSystem.InParallel();

		// Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void USystem_Constructor_InSequence()
	{
		// Arrange
		var panels = new List<IPanel>
		{
			new PanelTest("test")
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var modesController = new ModesController();

		// Act
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = uiSystem.InSequence();

		// Assert
		Assert.IsNotNull(actual);
	}

	[Test]
	public void UiSystem_GetPanel_Exception()
	{
		// Arrange
		const string name = "test";
		var expected = new PanelTest(name);
		var panels = new List<IPanel>
		{
			expected
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = false;

		// Act
		try
		{
			var unused = uiSystem.GetPanel(name);
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystem_CreatePanels_Exception()
	{
		// Arrange
		const string name = "test";
		var panels = new List<IPanel>
		{
			new PanelTest(name)
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);
		var actual = false;

		// Act
		uiSystem.CreatePanels();

		try
		{
			uiSystem.CreatePanels();
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiSystem_CreatePanels_GetPanel()
	{
		// Arrange
		const string name = "test";
		var expected = new PanelTest(name);
		var panels = new List<IPanel>
		{
			expected
		};
		var panelsCreator = new PanelsCreatorTest(panels);
		var modesController = new ModesController();
		var uiSystem = new UiSystem(panelsCreator, modesController);

		// Act
		uiSystem.CreatePanels();
		var actual = uiSystem.GetPanel(name);

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

	private sealed class PanelsCreatorTest :
		IPanelsCreator
	{
		private readonly IEnumerable<IPanel> panels;

		public PanelsCreatorTest(IEnumerable<IPanel> panels)
		{
			this.panels = panels;
		}

		public IEnumerable<IPanel> CreatePanels()
		{
			return panels;
		}
	}

	#endregion
}