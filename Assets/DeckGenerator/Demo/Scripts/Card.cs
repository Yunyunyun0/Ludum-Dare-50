using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Card
{
	[XmlAttribute("ID")]
	public int id;

	[XmlElement("Name")]
	public string name;

	[XmlElement("Desc")]
	public string description;

	[XmlElement("Type")]
	public string type;

	[XmlElement("Rarity")]
	public string rarity;

	[XmlElement("Rating")] //Used for AI Deck Generation
	public int rating;
}