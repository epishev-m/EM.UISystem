namespace EM.UI
{

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Foundation;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public static class MonoBehaviourExtensions
{
	public static void Subscribe(this Toggle toggle,
		UnityAction<bool> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(toggle, nameof(toggle));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		toggle.onValueChanged.AddListener(action);
		WaitUnsubscribeCancelAsync(toggle.onValueChanged, cts).Forget();
	}

	public static void Subscribe(this Button button,
		UnityAction action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(button, nameof(button));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		button.onClick.AddListener(action);
		WaitUnsubscribeCancelAsync(button.onClick, cts).Forget();
	}

	public static void Subscribe(this TMP_InputField inputField,
		UnityAction<string> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(inputField, nameof(inputField));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		inputField.onValueChanged.AddListener(action);
		WaitUnsubscribeCancelAsync(inputField.onValueChanged, cts).Forget();
	}

	public static void Subscribe(this Slider slider,
		UnityAction<float> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(slider, nameof(slider));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		slider.onValueChanged.AddListener(action);
		WaitUnsubscribeCancelAsync(slider.onValueChanged, cts).Forget();
	}

	public static void Subscribe<TValue>(this UnityEvent<TValue> unityEvent,
		UnityAction<TValue> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(unityEvent, nameof(unityEvent));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		unityEvent.AddListener(action);
		WaitUnsubscribeCancelAsync(unityEvent, cts).Forget();
	}

	public static void Subscribe<TValue>(this IRxProperty<TValue> property,
		Action<TValue> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(property, nameof(property));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		property.OnChanged += action;
		WaitUnsubscribeCancelAsync(property, action, cts).Forget();
	}

	public static void Subscribe<TValue>(this IAsyncRxProperty<TValue> property,
		Func<TValue, CancellationToken, UniTask> action,
		CancellationTokenSource cts)
	{
		Requires.NotNullParam(property, nameof(property));
		Requires.NotNullParam(action, nameof(action));
		Requires.NotNull(cts, nameof(cts));

		property.OnChanged += action;
		WaitUnsubscribeCancelAsync(property, action, cts).Forget();
	}

	private static async UniTask WaitUnsubscribeCancelAsync(UnityEventBase unityEvent,
		CancellationTokenSource cts)
	{
		await cts.Token.WaitUntilCanceled();
		unityEvent.RemoveAllListeners();
	}

	private static async UniTask WaitUnsubscribeCancelAsync<TValue>(IRxProperty<TValue> property,
		Action<TValue> action,
		CancellationTokenSource cts)
	{
		await cts.Token.WaitUntilCanceled();
		property.OnChanged -= action;
	}

	private static async UniTask WaitUnsubscribeCancelAsync<TValue>(IAsyncRxProperty<TValue> property,
		Func<TValue, CancellationToken, UniTask> action,
		CancellationTokenSource cts)
	{
		await cts.Token.WaitUntilCanceled();
		property.OnChanged -= action;
	}
}

}