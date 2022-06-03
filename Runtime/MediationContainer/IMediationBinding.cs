namespace EM.UI
{

public interface IMediationBinding
{
	void To<T>()
		where T : IMediator;
}

}