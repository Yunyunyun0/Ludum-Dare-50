using UnityEngine;
using System.Collections;

public class DatabaseLoader : MonoBehaviour {

	public static DatabaseLoader cardDB;

	public TextAsset database;

	public CardContainer cc;

	void Awake()
	{
		if (cardDB == null)
			cardDB = this;
		else if (cardDB != this)
			Destroy (gameObject);
	}

	void Start () 
	{
		DontDestroyOnLoad (this);

		cc = CardContainer.Load (database);
	}
}