using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ItemCollection")]
public class CardContainer{


	[XmlArray("Cards")]
	[XmlArrayItem("Card")]
	public List<Card> cards = new List<Card>();

	public static CardContainer Load(TextAsset cardDatabase)
	{
		TextAsset _xml = cardDatabase;

		XmlSerializer serializer = new XmlSerializer (typeof(CardContainer));

		StringReader reader = new StringReader (_xml.text);

		CardContainer cards = serializer.Deserialize (reader) as CardContainer;

		reader.Close ();

		return cards;
	}
}
