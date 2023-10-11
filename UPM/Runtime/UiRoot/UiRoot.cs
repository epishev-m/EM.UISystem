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

public sealed class UiRoot : IUiRoot
{
	private readonly Dictionary<Type, List<PanelView>> _viewPanels = new();

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
			UnityEngine.Object.DontDestroyOnLoad(_rootTransform);
		}
	}

	public async UniTask LoadPanelViewAsync(Type type,
		CancellationToken ct)
	{
		Requires.ValidOperation(_rootTransform != null, this);

		if (type.GetCustomAttribute(typeof(ViewAssetAttribute)) is ViewAssetAttribute attribute)
		{
			var result = await _assetsManager.InstantiateAsync<PanelView>(attribute.Id, _rootTransform, ct);

			if (result.Failure)
			{
				return;
			}

			var panelView = result.Data;
			PutObject(type, panelView, attribute.LifeTime);
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

			var openedPanelsViews = new List<PanelView>();

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

	public async UniTask<PanelView> GetPanelViewAsync(Type type,
		CancellationToken ct)
	{
		Requires.ValidOperation(_rootTransform != null, this);

		var panelView = GetObject(type);

		if (panelView != null)
		{
			return panelView;
		}

		await LoadPanelViewAsync(type, ct);
		panelView = GetObject(type);

		return panelView;
	}

	#endregion

	#region UiRoot

	public UiRoot(IAssetsManager assetsManager)
	{
		Requires.NotNullParam(assetsManager, nameof(assetsManager));

		_assetsManager = assetsManager;
	}

	private PanelView GetObject(Type type)
	{
		if (!_viewPanels.TryGetValue(type, out var list))
		{
			return null;
		}

		var panelView = list.FirstOrDefault(panelView => !panelView.IsOpened);

		return panelView;
	}

	private void PutObject(Type key,
		PanelView panelView,
		LifeTime lifeTime)
	{
		if (!_viewPanels.TryGetValue(key, out var list))
		{
			list = new List<PanelView>(4);
			_viewPanels.Add(key, list);
			_lifeTimeInfoList.Add(key, lifeTime);
		}

		list.Add(panelView);
	}

	#endregion
}

}