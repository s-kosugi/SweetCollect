using TwitterKit.Unity;
using UnityEngine;

public class TwitterLogin : MonoBehaviour
{
	[SerializeField] private string _AccessToken;
	[SerializeField] private string _Secret;

	void Start()
	{
		Twitter.Init();
		this.TwitterAuth();
	}

	void Update() { }

	public void TwitterAuth()
	{
		Debug.Log("[Info] : start login");
		TwitterSession session = Twitter.Session;
		if (session == null)
		{
			Twitter.LogIn(LoginComplete, LoginFailure);
		}
		else
		{
			LoginComplete(session);
		}
	}

	public void LoginComplete(TwitterSession session)
	{
		Debug.Log("[Info] : Login success. " + session.authToken);
		_AccessToken = session.authToken.token;
		_Secret = session.authToken.secret;
	}

	public void LoginFailure(ApiError error)
	{
		Debug.Log("[Error ] : Login faild code =" + error.code + " msg =" + error.message);
	}
}
