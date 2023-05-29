namespace EM.UI
{

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using BindingKey = System.ValueTuple<System.Type, System.Type>;

public sealed class ScreenSystem : Binder,
	IScreenSystem
{
	private readonly IViewModelFactory _viewModelFactory;

	private readonly Dictionary<BindingKey, View> _keysViewMap = new();

	private readonly Stack<ValueTuple<object, object>> _screensStack = new();

	private readonly IUiRoot _uiRoot;

	private object _currentScreen;

	private object _currentPopup;

	private object _currentTooltip;

	#region IScreenSystem

	public IScreenSystemLifeTimeBinding Bind(object key)
	{
		return base.Bind(key) as IScreenSystemLifeTimeBinding;
	}

	public void Unbind(LifeTime lifeTime)
	{
		Unbind(binding =>
		{
			var diBinding = (ScreenSystemBinding) binding;
			var result = diBinding.LifeTime == lifeTime;

			return result;
		});
	}

	public async UniTask OpenAsync(object key,
		CancellationToken ct)
	{
		await OpenAsync(key, null, ct);
	}

	public UniTask OpenAsync(object key,
		object data,
		CancellationToken ct)
	{
		if (GetBinding(key) is not ScreenSystemBinding binding)
		{
			return UniTask.CompletedTask;
		}

		return binding.Type switch
		{
			UiTypes.None => UniTask.CompletedTask,
			UiTypes.Screen => OpenScreenAsync(binding, data, ct),
			UiTypes.Popup => OpenPopupAsync(binding, data, ct),
			UiTypes.Tooltip => OpenTooltipAsync(binding, data, ct),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	public async UniTask BackAsync(CancellationToken ct)
	{
		if (_currentTooltip != null)
		{
			Enable(_currentPopup);

			if (_currentPopup == null)
			{
				Enable(_currentScreen);
			}

			await CloseTooltipAsync(ct);
			return;
		}

		if (_currentPopup != null)
		{
			Enable(_currentScreen);
			await ClosePopupAsync(ct);
			return;
		}

		if (_currentScreen != null)
		{
			await CloseScreenAsync(ct);
			await OpenLastScreen(ct);
		}
	}

	#endregion

	#region Binder

	protected override IBinding GetRawBinding(object key,
		object name)
	{
		return new ScreenSystemBinding(key, name, BindingResolver);
	}

	#endregion

	#region ScreenSystem

	public ScreenSystem(IUiRoot uiRoot,
		IViewModelFactory viewModelFactory)
	{
		_uiRoot = uiRoot;
		_viewModelFactory = viewModelFactory;
	}

	private async UniTask OpenScreenAsync(ScreenSystemBinding binding,
		object data,
		CancellationToken ct)
	{
		if (_currentScreen == binding.Key)
		{
			return;
		}

		if (_currentScreen != null)
		{
			_screensStack.Push((_currentScreen, data));
			await CloseScreenAsync(ct);
		}

		await OpenAsync(binding, data, ct);
		_currentScreen = binding.Key;
	}

	private async UniTask OpenPopupAsync(ScreenSystemBinding binding,
		object data,
		CancellationToken ct)
	{
		if (_currentPopup == binding.Key)
		{
			return;
		}

		Disable(_currentScreen);
		await ClosePopupAsync(ct);
		await OpenAsync(binding, data, ct);
		_currentPopup = binding.Key;
	}

	private async UniTask OpenTooltipAsync(ScreenSystemBinding binding,
		object data,
		CancellationToken ct)
	{
		if (_currentTooltip == binding.Key)
		{
			return;
		}

		Disable(_currentPopup);
		Disable(_currentScreen);
		await CloseTooltipAsync(ct);
		await OpenAsync(binding, data, ct);
		_currentTooltip = binding.Key;
	}

	private async UniTask OpenLastScreen(CancellationToken ct)
	{
		if (_screensStack.TryPop(out var info))
		{
			if (GetBinding(info.Item1) is not ScreenSystemBinding binding)
			{
				return;
			}

			await OpenScreenAsync(binding, info.Item2, ct);
			_currentScreen = info.Item1;
		}
	}

	private async UniTask CloseTooltipAsync(CancellationToken ct)
	{
		await CloseAsync(_currentTooltip, ct);
		_currentTooltip = null;
	}

	private async UniTask ClosePopupAsync(CancellationToken ct)
	{
		await UniTask.WhenAll(CloseTooltipAsync(ct),
			CloseAsync(_currentPopup, ct));
		_currentPopup = null;
	}

	private async UniTask CloseScreenAsync(CancellationToken ct)
	{
		await UniTask.WhenAll(ClosePopupAsync(ct),
			CloseAsync(_currentScreen, ct));
		_currentScreen = null;
	}

	private async UniTask CloseAsync(object key,
		CancellationToken ct)
	{
		if (key == null)
		{
			return;
		}

		if (GetBinding(key) is ScreenSystemBinding currentBinding)
		{
			var closeList = currentBinding.Values
				.Select(obj => (BindingKey) obj);

			await CloseViewsAsync(closeList, ct);
		}
	}

	private async UniTask OpenAsync(ScreenSystemBinding binding,
		object data,
		CancellationToken ct)
	{
		var views = new List<View>();
		var openList = binding.Values.Select(obj => (BindingKey) obj);

		foreach (var key in openList)
		{
			var view = await _uiRoot.GetPanelViewAsync(key.Item1, ct);
			var viewModel = _viewModelFactory.Get(key.Item2);
			viewModel.SetData(data);
			view.SetViewModel(viewModel);
			views.Add(view);
			_keysViewMap.Add(key, view);
		}

		var tasks = views.Select(view => view.OpenAsync(ct));
		await UniTask.WhenAll(tasks);
	}

	private async UniTask CloseViewsAsync(IEnumerable<BindingKey> closeList,
		CancellationToken ct)
	{
		var views = new List<View>();

		foreach (var key in closeList)
		{
			if (_keysViewMap.Remove(key, out var view))
			{
				views.Add(view);
			}
		}

		var tasks = views.Select(view => view.CloseAsync(ct));
		await UniTask.WhenAll(tasks);
	}

	private void Disable(object key)
	{
		SetInteractableByKey(key, false);
	}

	private void Enable(object key)
	{
		SetInteractableByKey(key, true);
	}

	private void SetInteractableByKey(object key, bool value)
	{
		if (key == null)
		{
			return;
		}

		if (GetBinding(key) is not ScreenSystemBinding currentBinding)
		{
			return;
		}

		var closeList = currentBinding.Values
			.Select(obj => (BindingKey) obj);

		foreach (var bindingKey in closeList)
		{
			if (_keysViewMap.TryGetValue(bindingKey, out var view))
			{
				view.IsInteractable = value;
			}
		}
	}

	#endregion
}

}