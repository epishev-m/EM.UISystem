namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using IoC;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class UiRoot :
	IUiRoot
{
	private readonly Dictionary<Type, List<PanelView>> viewPanels = new();

	private readonly Dictionary<Type, LifeTime> lifeTimeInfoList = new();

	private readonly IAssetsManager assetsManager;

	private Transform rootTransform;

	#region IUiRoot

	public async UniTask CreateRootTransform(string assetId,
		CancellationToken ct)
	{
		Requires.ValidOperation(rootTransform == null, this, nameof(CreateRootTransform));

		rootTransform = await assetsManager.InstantiateAsync<Transform>(assetId, ct);
		Object.DontDestroyOnLoad(rootTransform);
	}

	public async UniTask LoadPanelViewAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(rootTransform != null, this, nameof(LoadPanelViewAsync));

		var type = typeof(T);

		if (type.GetCustomAttribute(typeof(AssetAttribute)) is AssetAttribute attribute)
		{
			var panelView = await assetsManager.InstantiateAsync<T>(attribute.Id, rootTransform, ct);
			PutObject<T>(panelView, attribute.LifeTime);
		}
	}

	public void UnloadPanelView(LifeTime lifeTime)
	{
		var targetList = lifeTimeInfoList.Where(pair => pair.Value == lifeTime).ToArray();

		foreach (var (key, _) in targetList)
		{
			if (!viewPanels.Remove(key, out var panelsViewsList))
			{
				lifeTimeInfoList.Remove(key);

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

				assetsManager.ReleaseInstance(panelView.gameObject);
			}

			if (openedPanelsViews.Any())
			{
				viewPanels.Add(key, openedPanelsViews);

				continue;
			}

			lifeTimeInfoList.Remove(key);
		}
	}

	public async UniTask<PanelView> GetPanelViewAsync<T>(CancellationToken ct)
		where T : PanelView
	{
		Requires.ValidOperation(rootTransform != null, this, nameof(GetPanelViewAsync));

		var panelView = GetObject<T>();

		if (panelView != null)
		{
			return panelView;
		}

		await LoadPanelViewAsync<T>(ct);
		panelView = GetObject<T>();

		return panelView;
	}

	#endregion

	#region UiRoot

	public UiRoot(IAssetsManager assetsManager)
	{
		this.assetsManager = assetsManager;
	}

	private PanelView GetObject<T>()
	{
		if (!viewPanels.TryGetValue(typeof(T), out var list))
		{
			return null;
		}

		var panelView = list.FirstOrDefault(panelView => !panelView.IsOpened);

		return panelView;
	}

	private void PutObject<T>(PanelView panelView,
		LifeTime lifeTime)
		where T : PanelView
	{
		var key = typeof(T);

		if (!viewPanels.TryGetValue(key, out var list))
		{
			list = new List<PanelView>(4);
			viewPanels.Add(key, list);
			lifeTimeInfoList.Add(key, lifeTime);
		}

		list.Add(panelView);
	}

	#endregion
}

}