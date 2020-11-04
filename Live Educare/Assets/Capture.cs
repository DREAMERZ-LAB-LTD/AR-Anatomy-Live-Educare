using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Capture : MonoBehaviour
{
	public int counter;
    private void Start()
    {
		DontDestroyOnLoad(this.gameObject);
		this.transform.name = "Image Capture";
    }

    void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			StartCoroutine(Captutre());
		}
	}

	//Capturee Screem Pixels as Texture2D
	public IEnumerator Captutre()
	{
		yield return new WaitForEndOfFrame();

		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();


		yield return new WaitForEndOfFrame();
		byte[] bytes = texture.EncodeToPNG();


		string filename = "Photo"+ counter + ".png";
		var path = Application.dataPath + "/" + filename;
		File.WriteAllBytes(path, bytes);
		counter++;
		Debug.Log("Path : " + path);

	}

}
