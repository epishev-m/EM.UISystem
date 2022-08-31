using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using EM.UI;
using NUnit.Framework;

public sealed class MediatorTests
{
	[Test]
	public void Mediator_Initialize_Exception()
	{
		// Arrange
		var actual = false;
		var mediator = new TestMediator();

		// Act
		try
		{
			mediator.Initialize(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Mediator_Initialize_Exception2()
	{
		// Arrange
		var actual = false;
		var mediator = new TestMediator();
		var view = new TestViewOther();

		// Act
		try
		{
			mediator.Initialize(view);
		}
		catch (ArgumentException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Mediator_DoubleInitialize_Initialize_Exception()
	{
		// Arrange
		var actual = false;
		var mediator = new TestMediator();
		var view = new TestView();
		mediator.Initialize(view);

		// Act
		try
		{
			mediator.Initialize(view);
		}
		catch (InvalidOperationException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void Mediator_Initialize()
	{
		// Arrange
		var mediator = new TestMediator();
		var expected = new TestView();

		// Act
		mediator.Initialize(expected);
		var actual = mediator.GetView();


		// Assert
		Assert.AreEqual(expected, actual);
	}
	
	

	[Test]
	public void Mediator_Release()
	{
		// Arrange
		var mediator = new TestMediator();
		var expected = new TestView();

		// Act
		mediator.Initialize(expected);
		mediator.Release();
		var actual = mediator.GetView();

		// Assert
		Assert.Null(actual);
	}

	#region Nested

	private sealed class TestMediator : Mediator<TestView>
	{
		#region Mediator

		public override void Enable()
		{
			throw new NotImplementedException();
		}

		public override void Disable()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Mediator

		public IView GetView()
		{
			return View;
		}

		#endregion
	}

	[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
	private sealed class TestView : IView
	{
		public bool IsEnabled
		{
			get;
		}

		public IEnumerable<IView> Children
		{
			get;
		}

		public void AddView(IView view)
		{
			throw new NotImplementedException();
		}

		public void RemoveView(IView view)
		{
			throw new NotImplementedException();
		}
	}

	[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
	private sealed class TestViewOther : IView
	{
		public bool IsEnabled
		{
			get;
		}

		public IEnumerable<IView> Children
		{
			get;
		}

		public void AddView(IView view)
		{
			throw new NotImplementedException();
		}

		public void RemoveView(IView view)
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}