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
		public UniTask<Result<GameObject>> InstantiateAsync(string key,
			CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public UniTask<Result<GameObject>> InstantiateAsync(string key,
			Transform parent,
			CancellationToken ct)
		{
			throw new NotImplementedException();
		}

		public UniTask<Result<T>> InstantiateAsync<T>(string key,
			CancellationToken ct)
			where T : Component
		{
			var gameObject = new GameObject();
			var component = (Component) gameObject.transform;
			var result = new SuccessResult<T>((T) component);

			return new UniTask<Result<T>>(result);
		}

		public UniTask<Result<T>> InstantiateAsync<T>(string key,
			Transform parent,
			CancellationToken ct)
			where T : Component
		{
			throw new NotImplementedException();
		}

		public Result<T> LoadAsset<T>(string path)
			where T : UnityEngine.Object
		{
			throw new NotImplementedException();
		}

		public Result ReleaseInstance(GameObject gameObject)
		{
			throw new NotImplementedException();
		}

		public void ReleaseAsset<T>(T asset)
			where T : UnityEngine.Object
		{
			throw new NotImplementedException();
		}
	}

	private sealed class TestPanelView : UIView
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