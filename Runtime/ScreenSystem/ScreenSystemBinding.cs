namespace EM.UI
{

using System;
using Foundation;

public sealed class ScreenSystemBinding : Binding,
	IScreenSystemLifeTimeBinding,
	IScreenSystemTypeBinding,
	IScreenSystemBinding
{
	#region IScreenSystemLifeTimeBinding

	public LifeTime LifeTime { get; private set; } = LifeTime.None;

	public IScreenSystemTypeBinding InGlobal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = LifeTime.Global;

		return this;
	}

	public IScreenSystemTypeBinding InLocal()
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = LifeTime.Local;

		return this;
	}

	public IScreenSystemTypeBinding SetLifeTime(LifeTime lifeTime)
	{
		Requires.ValidOperation(LifeTime == LifeTime.None, this);

		LifeTime = lifeTime;

		return this;
	}

	#endregion

	#region IScreenSystemTypeBinding

	public UiTypes Type { get; private set; } = UiTypes.None;

	public IScreenSystemBinding AsScreen()
	{
		Requires.ValidOperation(Type == UiTypes.None, this);

		Type = UiTypes.Screen;

		return this;
	}

	public IScreenSystemBinding AsPopup()
	{
		Requires.ValidOperation(Type == UiTypes.None, this);

		Type = UiTypes.Popup;

		return this;
	}

	public IScreenSystemBinding AsTooltip()
	{
		Requires.ValidOperation(Type == UiTypes.None, this);

		Type = UiTypes.Tooltip;

		return this;
	}

	public IScreenSystemBinding SetType(UiTypes type)
	{
		Requires.ValidOperation(Type == UiTypes.None, this);

		Type = type;

		return this;
	}

	#endregion

	#region IScreenSystemBinding

	public IScreenSystemBinding To<TView, TViewModel>()
		where TView : View
		where TViewModel : IViewModel
	{
		Requires.ValidOperation(LifeTime != LifeTime.None, this);

		var value = new ValueTuple<Type, Type>(typeof(TView), typeof(TViewModel));

		return base.To(value) as IScreenSystemBinding;
	}

	#endregion

	#region ScreenSystemBinding

	public ScreenSystemBinding(object key,
		object name,
		Resolver resolver) : base(key, name, resolver)
	{
	}

	#endregion
}

}