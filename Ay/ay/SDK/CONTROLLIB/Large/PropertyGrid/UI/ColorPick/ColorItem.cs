using System.Windows.Media;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a color in the ColorPicker.</summary>
	public class ColorItem
	{
		/// <summary>Gets or sets the color of the ColorItem.</summary>
		public Color? Color
		{
			get;
			set;
		}

		/// <summary>Gets or sets the name of the ColorItem.</summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the ColorItem class, specifying the Color and its name.</summary>
		/// <param name="color">A Color structure.</param>
		/// <param name="name">A string representing the name of the ColorItem.</param>
		public ColorItem(Color? color, string name)
		{
			Color = color;
			Name = name;
		}

		public override bool Equals(object obj)
		{
			ColorItem colorItem = obj as ColorItem;
			if (colorItem == null)
			{
				return false;
			}
			if (colorItem.Color.Equals(Color))
			{
				return colorItem.Name.Equals(Name);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Color.GetHashCode() ^ Name.GetHashCode();
		}
	}
}
