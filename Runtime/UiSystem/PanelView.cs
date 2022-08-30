namespace EM.UI
{

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class PanelView : View
{
	[Header(nameof(PanelView))]

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	#region MonoBehaviour

	protected virtual void Awake()
	{
		if (_canvas == null)
		{
			_canvas = GetComponent<Canvas>();
		}

		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		_canvas.enabled = false;
		_canvasGroup.blocksRaycasts = false;
	}

	#endregion

	#region View

	public override bool IsEnabled => IsOpened;

	#endregion

	#region PanelView

	public bool IsOpened => _canvas.enabled;

	public bool IsInteractable
	{
		get => _canvasGroup.blocksRaycasts;
		set => _canvasGroup.blocksRaycasts = value;
	}

	public virtual UniTask OpenAsync(CancellationToken ct)
	{
		transform.SetAsLastSibling();

		if (_canvas.enabled)
		{
			return UniTask.CompletedTask;
		}

		_canvas.enabled = true;
		_canvasGroup.blocksRaycasts = true;

		return UniTask.CompletedTask;
	}

	public virtual UniTask CloseAsync(CancellationToken ct)
	{
		if (!_canvas.enabled)
		{
			return UniTask.CompletedTask;
		}

		_canvasGroup.blocksRaycasts = false;
		_canvas.enabled = false;

		return UniTask.CompletedTask;
	}

	#endregion
}

}