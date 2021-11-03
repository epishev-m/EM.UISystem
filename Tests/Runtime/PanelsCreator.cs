using System;
using System.Collections.Generic;
using EM.UI;
using NUnit.Framework;
using UnityEngine;

internal sealed class PanelsCreatorTests
{
	[Test]
	public void PanelsCreator_Constructor_Exception()
	{
		// Arrange
		var prefabs = new List<Canvas>();
		var actual = false;

		// Act
		try
		{
			var unused = new PanelsCreator(null, prefabs);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void PanelsCreator_Constructor_Exception1()
	{
		// Arrange
		var gameObject = new GameObject();
		var actual = false;

		// Act
		try
		{
			var unused = new PanelsCreator(gameObject.transform, null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void PanelsCreator_CreatePanels()
	{
		// Arrange
		var gameObject = new GameObject();
		var prefabs = new List<Canvas>();

		// Act
		var panelsCreator = new PanelsCreator(gameObject.transform, prefabs);
		var actual = panelsCreator.CreatePanels();

		// Assert
		Assert.IsNotNull(actual);
	}
}