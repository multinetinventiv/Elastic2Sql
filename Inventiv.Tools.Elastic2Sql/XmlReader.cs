using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Xml.Linq;
using System.Collections;
using System.IO;

namespace Inventiv.Tools.Elastic2Sql
{
	public class XmlReader : DynamicObject, IEnumerable
	{
		private readonly List<XElement> elements;

		public XmlReader(string path)
		{
			var doc = XDocument.Load(path);
			elements = new List<XElement> { doc.Root };
		}

		protected XmlReader(XElement element)
		{
			elements = new List<XElement> { element };
		}

		protected XmlReader(IEnumerable<XElement> elements)
		{
			this.elements = new List<XElement>(elements);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = null;
			switch (binder.Name)
			{
				case "Value":
					result = elements[0].Value;
					break;
				case "Count":
					result = elements.Count;
					break;
				default:
					var attr = elements[0].Attribute(XName.Get(binder.Name));
					if (attr != null)
						result = attr;
					else
					{
						var items = elements.Descendants(XName.Get(binder.Name));
						if (items == null || !items.Any()) { return false; }

						result = new XmlReader(items);
					}
					break;
			}
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			var ndx = (int)indexes[0];
			result = new XmlReader(elements[ndx]);
			return true;
		}

		public IEnumerator GetEnumerator()
		{
			foreach (var element in elements)
			{
				yield return new XmlReader(element);
			}
		}
	}
}