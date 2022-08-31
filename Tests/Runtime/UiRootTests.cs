using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EM.Foundation;
using EM.UI;
using NUnit.Framework;
using UnityEngine;

public sealed class UiRootTests
{
	[Test]
	public void UiRoot_Constructor_Exception()
	{
		// Arrange
		var actual = false;

		// Act
		try
		{
			var unused = new UiRoot(null);
		}
		catch (ArgumentNullException)
		{
			actual = true;
		}

		// Assert
		Assert.IsTrue(actual);
	}

	[Test]
	public void UiRoot_LoadPanelViewAsync_GetPanelViewAsync()
	{
		// Arrange
		var assetsManager = new TestAssetsManager();
		var uiRoot = new UiRoot(assetsManager);
		var cancellationToken = new CancellationToken();

		// Act
		uiRoot.CreateRootTransform("test", cancellationToken);
		uiRoot.LoadPanelViewAsync<TestPanelView>(cancellationToken);
		var actual = uiRoot.GetPanelViewAsync<TestPanelView>(cancellationToken);

		// Assert
		Assert.NotNull(actual);
	}

	#region Nested

	private sealed class TestAssetsManager : IAssetsManager
	{
		public UniTask<GameObject> InstantiateAsync(string path,
			CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public UniTask<GameObject> InstantiateAsync(string path,
			Transform parent,
			CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public UniTask<T> InstantiateAsync<T>(string path,
			CancellationToken ct)
			where T : Component
		{
			var gameObject = new GameObject();
			var component = (Component) gameObject.transform;
			return new UniTask<T>((T)component);
		}

		public UniTask<T> InstantiateAsync<T>(string path,
			Transform parent,
			CancellationToken ct)
			where T : Component
		{
			throw new NotImplementedException();
		}

		public bool ReleaseInstance(GameObject gameObject)
		{
			throw new NotImplementedException();
		}
	}

	private sealed class TestPanelView : PanelView
	{
	}
	
	#endregion
}