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
	private readonly Dictionary<Type, List<UIView>> _viewPanels = new();

	private readonly Dictionary<Type, LifeTime> _lifeTimeInfoList = new();

	private readonly IAssetsManager _assetsManager;

	private Transform _rootTransform;

	#region IUiRoot

	public async UniTask CreateRootTransform(string assetId,
		CancellationToken ct)
	{
		Requires.ValidOperation(_rootTransform == null, this);

		var result = await _assetsManager.InstantiateAsync<Transform>(assetId, ct);

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
		where TView : UIView
	{
		Requires.ValidOperation(_rootTransform != null, this);

		var type = typeof(TView);

		if (type.GetCustomAttribute(typeof(AssetAttribute)) is AssetAttribute attribute)
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

			var openedPanelsViews = new List<UIView>();

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

	public async UniTask<UIView> GetPanelViewAsync<TView>(CancellationToken ct)
		where TView : UIView
	{
		Requires.ValidOperation(_rootTransform != null, this);

		var panelView = GetObject<TView>();

		if (panelView != null)
		{
			return panelView;
		}

		await LoadPanelViewAsync<TView>(ct);
		panelView = GetObject<TView>();

		return panelView;
	}

	#endregion

	#region UiRoot

	public UiRoot(IAssetsManager assetsManager)
	{
		Requires.NotNull(assetsManager, nameof(assetsManager));

		_assetsManager = assetsManager;
	}

	private UIView GetObject<TView>()
		where TView : UIView
	{
		if (!_viewPanels.TryGetValue(typeof(TView), out var list))
		{
			return null;
		}

		var panelView = list.FirstOrDefault(panelView => !panelView.IsOpened);

		return panelView;
	}

	private void PutObject<TView>(UIView panelView,
		LifeTime lifeTime)
		where TView : UIView
	{
		var key = typeof(TView);

		if (!_viewPanels.TryGetValue(key, out var list))
		{
			list = new List<UIView>(4);
			_viewPanels.Add(key, list);
			_lifeTimeInfoList.Add(key, lifeTime);
		}

		list.Add(panelView);
	}

	#endregion
}

}