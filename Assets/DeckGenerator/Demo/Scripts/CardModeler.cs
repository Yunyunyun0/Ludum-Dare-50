using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardModeler : MonoBehaviour {

	public int cardID = 0;

	public Text cardName;
	public Text cardType;
	public Text Description;
	public Text Rarity;
	public int cardRating;
	public string type;

	public Sprite[] cardBGs;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (PopulateCard ());
	}

	IEnumerator PopulateCard()
	{
		while (cardID == 0)
			yield return null;

		cardName.text = DatabaseLoader.cardDB.cc.cards [cardID].name;
		type = DatabaseLoader.cardDB.cc.cards [cardID].type;
		cardType.text = type;
		Description.text = DatabaseLoader.cardDB.cc.cards [cardID].description;
		Rarity.text = DatabaseLoader.cardDB.cc.cards [cardID].rarity;

		WhichBackground ();
	}

	void WhichBackground()
	{
		switch (type) {
		case "Weapon":
			GetComponent<Image>().overrideSprite = cardBGs [0];
			break;
		case "Consumable":
			GetComponent<Image>().overrideSprite = cardBGs [1];
			break;
		case "Armor":
			GetComponent<Image>().overrideSprite = cardBGs [2];
			break;
		case "Shield":
			GetComponent<Image>().overrideSprite = cardBGs [2];
			break;
		}
	}
}
