namespace EM.UI
{

using Foundation;

public interface IScreenSystemLifeTimeBinding
{
	LifeTime LifeTime
	{
		get;
	}

	IScreenSystemTypeBinding InGlobal();

	IScreenSystemTypeBinding InLocal();

	IScreenSystemTypeBinding SetLifeTime(LifeTime lifeTime);
}

}