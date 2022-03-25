using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace DeckGen 
{
	public static class DeckGenerator  
	{
		/// <summary>
		/// Generates a rating based deck that can be used for AI in card games. Returns a List<Card>.
		/// </summary>
		/// <param name="cardDatabase">Your game's card database in the List format.</param>
		/// <param name="deckSizeMin">The minimum amount of cards that can be in a playable deck.</param>
		/// <param name="deckSizeMax">The maximum amount of cards that can be in a playable deck.</param>
		/// </param name="dupeLimit"> The max amount of each unique card a deck can have. If 0 then no limit will be applied.</param>
		/// <param name="desiredRating">What is the designered rating of the deck? This will found by adding all the ratings of the cards in the deck.</param>
		public static List<Card> Generate (List<Card> cardDatabase, int deckSizeMin, int deckSizeMax, int dupeLimit,  int desiredRating) { 

			return Generate (cardDatabase, deckSizeMin, deckSizeMax, dupeLimit, desiredRating, 512);
		}

		/// <summary>
		/// Generates a rating based deck that can be used for AI in card games. Returns a List<Card>.
		/// </summary>
		/// <param name="cardDatabase">Your game's card database in the List format.</param>
		/// <param name="deckSizeMin">The minimum amount of cards that can be in a playable deck.</param>
		/// <param name="deckSizeMax">The maximum amount of cards that can be in a playable deck.</param>
		/// </param name="dupeLimit"> The max amount of each unique card a deck can have. If 0 then no limit will be applied.</param>
		/// <param name="desiredRating">What is the designered rating of the deck? This will found by adding all the ratings of the cards in the deck.</param>
		/// <param name="iterationLimitor">Max amount of times you want the generate to iterate. The higher the max the more accurate the generation, but the more processing it takes. Default is 512.</param>
		public static List<Card> Generate(List<Card> cardDatabase, int deckSizeMin, int deckSizeMax, int dupeLimit, int desiredRating, int iterationLimitor)
		{
			int maxIterations = iterationLimitor;
			int deckSize = Random.Range(deckSizeMin, deckSizeMax);
			List<Card> deck = new List<Card>();

			for (int i = 0; i < deckSize; i++) {
				deck.Add (cardDatabase[Random.Range (1, cardDatabase.Count)]);
			}

			int sum = GetRatingSum (deck);
			while (sum != desiredRating && maxIterations > 0) {

				int difference = desiredRating - sum;

				int index = Random.Range (0, deck.Count);
				Card toReplace = deck[index];

				int newRating = toReplace.rating + difference;

				int newIndex;
				Card replacement = GetClosestRating (cardDatabase, deck, dupeLimit, newRating, out newIndex);

				if(replacement != null)
					deck[index] = replacement;



				sum = GetRatingSum (deck);
				maxIterations--;
			}

			return deck;
		}

		static Card GetClosestRating (IEnumerable<Card> cardList, IEnumerable<Card> currentDeck, int cardLimit, int rating, out int closestIndex) {
			int closestValue = int.MaxValue;
			closestIndex = 0;
			Card closestCard = null;

			int index = 0;

			List<Card> newCardList = new List<Card> ();
			if (cardLimit != 0)
				newCardList = DupeFree (cardList, currentDeck, cardLimit);
			else {
				foreach (Card card in cardList)
					newCardList.Add (card);
			}

			foreach (Card card in newCardList) {
				int distance = Mathf.Abs (rating - card.rating); 

				if (distance < closestValue) {
					closestValue = distance;
					closestIndex = index;
					closestCard = card;
				}
				index++;
			}

			if (closestCard.id == 0)
				closestCard = newCardList [Random.Range (1, newCardList.Count)];

			return closestCard;
		}

		static int GetRatingSum (IEnumerable<Card> cardList) {
			int result = 0;
			foreach (Card card in cardList) {
				result += card.rating;
			}
			return result;
		}

		static List<Card> DupeFree(IEnumerable<Card> cardList, IEnumerable<Card> currentDeck, int cardLimit)
		{
			int limit = cardLimit - 1;

			List<Card> newCardList = new List<Card>();

			foreach (Card card in cardList) {
				newCardList.Add (card);
			}

			List<Card> deckCopy = new List<Card> ();

			foreach (Card card in currentDeck) {
				deckCopy.Add (card);
			}

			List<Card> duplicates = deckCopy.GroupBy (x => x).Where (g => g.Count () > limit).Select (y => y.Key).ToList ();

			foreach (Card card in newCardList.ToList()) {
				if(duplicates.Exists(x => x.id == card.id)){
						newCardList.Remove (card);
				}
			}

			return newCardList;
		}

		private static System.Random rng = new System.Random ();

		/// <summary>
		/// Shuffle a List using the Fisher-Yates shuffle algrithom. This function uses an static instanced System.Random instead of UnityEngine.Random.
		/// </summary>
		public static void Shuffle<T> (this IList<T> list)
		{
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = rng.Next (n + 1);
				T value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}
	}
}