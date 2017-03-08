using UnityEngine;
using System.Collections;

public class SpeechRecogTest : MonoBehaviour
{
	private string stringToEdit = "";

	private void OnGUI()
	{
		var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

		stringToEdit = jo.Call<string>("getStr");
		if (GUI.Button(new Rect(0, 200, 800, 100), stringToEdit))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				jo.Call("MyRecogStart");

			}
		}
	}
}