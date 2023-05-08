namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class UiRoot : IUiRoot
{
	private readonly Dictionary<Type, List<View>> _viewPanels = new();

	private readonly Dictionary<Type, LifeTime> _lifeTimeInfoList = new();

	private readonly IAssetsManager _assetsManager;

	private Transform _rootTransform;

	#region IUiRoot

	public void CreateRootTransform(string assetId)
	{
		Requires.ValidOperation(_rootTransform == null, this);

		var result = _assetsManager.Instantiate<Transform>(assetId);

		if (result.Failure)
		{
			return;
		}

		_rootTransform = result.Data;

		//The condition is necessary for the correct operation of unit tests
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(_rootTransform);
		}
	}

	public async UniTask LoadPanelViewAsync<TView>(CancellationToken ct)
		where TView : View
	{
		Requires.ValidOperation(_rootTransform != null, this);

		var type = typeof(TView);

		if (type.GetCustomAttribute(typeof(ViewAssetAttribute)) is ViewAssetAttribute attribute)
		{
			var result = await _assetsManager.InstantiateAsync<TView>(attribute.Id, _rootTransform, ct);

			if (result.Failure)
			{
				return;
			}

			var panelView = result.Data;
			PutObject<TView>(panelView, attribute.LifeTime);
		}
	}

	public void UnloadPanelView(LifeTime lifeTime)
	{
		var targetList = _lifeTimeInfoList.Where(pair => pair.Value == lifeTime).ToArray();

		foreach (var (key, _) in targetList)
		{
			if (!_viewPanels.Remove(key, out var panelsViewsList))
			{
				_lifeTimeInfoList.Remove(key);

				continue;
			}

			var openedPanelsViews = new List<View>();

			foreach (var panelView in panelsViewsList)
			{
				if (panelView == null)
				{
					continue;
				}

				if (panelView.IsOpened)
				{
					openedPanelsViews.Add(panelView);

					continue;
				}

				_assetsManager.ReleaseInstance(panelView.gameObject);
			}

			if (openedPanelsViews.Any())
			{
				_viewPanels.Add(key, openedPanelsViews);

				continue;
			}

			_lifeTimeInfoList.Remove(key);
		}
	}

	public async UniTask<TView> GetPanelViewAsync<TView>(CancellationToken ct)
		where TView : View
	{
		Requires.ValidOperation(_rootTransform != null, this);

		var panelView = GetObject<TView>();

		if (panelView != null)
		{
			return (TView) panelView;
		}

		await LoadPanelViewAsync<TView>(ct);
		panelView = GetObject<TView>();

		return (TView) panelView;
	}

	#endregion

	#region UiRoot

	public UiRoot(IAssetsManager assetsManager)
	{
		Requires.NotNullParam(assetsManager, nameof(assetsManager));

		_assetsManager = assetsManager;
	}

	private View GetObject<TView>()
		where TView : View
	{
		if (!_viewPanels.TryGetValue(typeof(TView), out var list))
		{
			return null;
		}

		var panelView = list.FirstOrDefault(panelView => !panelView.IsOpened);

		return panelView;
	}

	private void PutObject<TView>(View panelView,
		LifeTime lifeTime)
		where TView : View
	{
		var key = typeof(TView);

		if (!_viewPanels.TryGetValue(key, out var list))
		{
			list = new List<View>(4);
			_viewPanels.Add(key, list);
			_lifeTimeInfoList.Add(key, lifeTime);
		}

		list.Add(panelView);
	}

	#endregion
}

}