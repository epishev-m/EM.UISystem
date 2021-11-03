using EM.UI;
using NUnit.Framework;
using System;
using UnityEngine;

internal sealed class PanelTests
{
	#region Constructor

	[Test]
	public void Panel_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new Panel(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Panel_Constructor_NotCanvasGroup_ExceptionCanvas()
	{
		// Arrange
		var actual = false;
		var gameObject = new GameObject();
		var canvas = gameObject.AddComponent<Canvas>();

		// Act
		try
		{
			var unused = new Panel(canvas);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Name

	[Test]
	public void Panel_Name()
	{
		// Arrange
		const string expected = "panel";
		var gameObject = new GameObject(expected);
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		var actual = panel.Name;

		// Assert
		Assert.AreEqual(expected, actual);
	}

	#endregion
	#region IsInteractable

	[Test]
	public void Panel_IsInteractable()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		var actual = panel.IsInteractable;

		// Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void Panel_Open_IsInteractable()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		var actual = panel.IsInteractable;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Panel_Open_Close_IsInteractable()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		panel.Close(null);
		var actual = panel.IsInteractable;

		// Assert
		Assert.IsFalse(actual);
	}

	#endregion
	#region IsOpened

	[Test]
	public void Panel_IsOpened()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		var actual = panel.IsOpened;

		// Assert
		Assert.IsFalse(actual);
	}

	[Test]
	public void Panel_Open_IsOpened()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		var actual = panel.IsOpened;

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Panel_Open_Close_IsOpened()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		panel.Close(null);
		var actual = panel.IsOpened;

		// Assert
		Assert.IsFalse(actual);
	}

	#endregion
	#region Open & Close

	[Test]
	public void Panel_Open()
	{
		// Arrange
		var actual = false;
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(() => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Panel_Close()
	{
		// Arrange
		var actual = false;
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(() => actual = true);

		// Assert
		Assert.IsTrue(actual);
	}

	#endregion
	#region Animation

	[Test]
	public void Panel_Open_Animation()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var animation = gameObject.AddComponent<Animation>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		var actual = animation.ShowValue;

		// Assert
		Assert.AreEqual(1, actual);
	}

	[Test]
	public void Panel_Open_Open_Animation()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var animation = gameObject.AddComponent<Animation>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		panel.Open(null);
		var actual = animation.ShowValue;

		// Assert
		Assert.AreEqual(1, actual);
	}

	[Test]
	public void Panel_Close_Animation()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var animation = gameObject.AddComponent<Animation>();
		var panel = new Panel(canvas);

		// Act
		panel.Close(null);
		var actual = animation.HideValue;

		// Assert
		Assert.AreEqual(0, actual);
	}

	[Test]
	public void Panel_Open_Close_Animation()
	{
		// Arrange
		var gameObject = new GameObject("panel");
		var canvas = gameObject.AddComponent<Canvas>();
		gameObject.AddComponent<CanvasGroup>();
		var animation = gameObject.AddComponent<Animation>();
		var panel = new Panel(canvas);

		// Act
		panel.Open(null);
		panel.Close(null);
		var actual = animation.HideValue;

		// Assert
		Assert.AreEqual(1, actual);
	}

	#endregion
	#region Nested

	private sealed class Animation :
		MonoBehaviour,
		IPanelAnimation
	{
		#region IPanelAnimation

		public void Hide(
			GameObject panel,
			Action onPanelHidden)
		{
			HideValue++;
			onPanelHidden?.Invoke();
		}

		public void Show(
			GameObject panel,
			Action onPanelShowed)
		{
			ShowValue++;
			onPanelShowed?.Invoke();
		}

		#endregion
		#region Animation

		public int HideValue
		{
			get;
			private set;
		}

		public int ShowValue
		{
			get;
			private set;
		}

		#endregion
	}

	#endregion
}