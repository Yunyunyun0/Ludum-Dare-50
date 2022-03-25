using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeckGen;

public class GeneratorControls : MonoBehaviour {

	public Button bGenerate;
	public Button bAutoFill;
	public InputField iMin;
	public InputField iMax;
	public InputField iLimit;
	public InputField iTargetRating;
	public Text deckRating;
	public Text deckSize;

	public Transform deckContainer;
	public GameObject cardPrefab;

	int min;
	int max;
	int limit;
	int target;

	int deckTotalSum;
	int cardCount;

	public List<Card> deck = new List<Card>();

	void Start()
	{
		bGenerate.onClick.AddListener (() => GatherInfo ());
		bAutoFill.onClick.AddListener (() => GenerateInfo ());
	}

	void GatherInfo()
	{
		min = int.Parse (iMin.text);
		max = int.Parse (iMax.text);
		if (iLimit.text != null)
			limit = int.Parse (iLimit.text);
		target = int.Parse (iTargetRating.text);

		CreateDeck ();
	}

	void GenerateInfo()
	{
		iMin.text = "10";
		iMax.text = "15";
		iLimit.text = "2";
		iTargetRating.text = "235";
	}

	void CreateDeck()
	{
		deck = new List<Card> ();
		deck = DeckGenerator.Generate (DatabaseLoader.cardDB.cc.cards, min, max, limit, target);
		DisplayDeck ();
	}

	void DisplayDeck()
	{
		ClearDeck ();

		foreach (Card card in deck) {
			GameObject displayCard = Instantiate (cardPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			displayCard.GetComponent<CardModeler> ().cardID = card.id;
			displayCard.transform.SetParent (deckContainer, false);
			deckTotalSum += card.rating;
			cardCount++;
		}

		deckRating.text = "Deck Rating: " + deckTotalSum;
		deckSize.text = "Card Count: " + cardCount;

	}

	void ClearDeck()
	{
		deckRating.text = "Deck Rating: ";
		deckSize.text = "Card Count: ";
		cardCount = 0;
		deckTotalSum = 0;
		
		foreach (Transform t in deckContainer) {
			Destroy (t.gameObject);
		}
	}
}
