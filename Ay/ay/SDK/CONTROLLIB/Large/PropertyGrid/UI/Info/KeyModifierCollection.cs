using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Xceed.Wpf.Toolkit.Core.Input
{
	/// <summary>Represents a collection of key modifiers.</summary>
	[TypeConverter(typeof(KeyModifierCollectionConverter))]
	public class KeyModifierCollection : Collection<KeyModifier>
	{
		public bool AreActive
		{
			get
			{
				if (base.Count == 0)
				{
					return true;
				}
				if (Contains(KeyModifier.Blocked))
				{
					return false;
				}
				if (Contains(KeyModifier.Exact))
				{
					return IsExactMatch();
				}
				return MatchAny();
			}
		}

		private static bool IsKeyPressed(KeyModifier modifier, ICollection<Key> keys)
		{
			switch (modifier)
			{
			case KeyModifier.Alt:
				if (!keys.Contains(Key.LeftAlt))
				{
					return keys.Contains(Key.RightAlt);
				}
				return true;
			case KeyModifier.LeftAlt:
				return keys.Contains(Key.LeftAlt);
			case KeyModifier.RightAlt:
				return keys.Contains(Key.RightAlt);
			case KeyModifier.Ctrl:
				if (!keys.Contains(Key.LeftCtrl))
				{
					return keys.Contains(Key.RightCtrl);
				}
				return true;
			case KeyModifier.LeftCtrl:
				return keys.Contains(Key.LeftCtrl);
			case KeyModifier.RightCtrl:
				return keys.Contains(Key.RightCtrl);
			case KeyModifier.Shift:
				if (!keys.Contains(Key.LeftShift))
				{
					return keys.Contains(Key.RightShift);
				}
				return true;
			case KeyModifier.LeftShift:
				return keys.Contains(Key.LeftShift);
			case KeyModifier.RightShift:
				return keys.Contains(Key.RightShift);
			case KeyModifier.None:
				return true;
			default:
				throw new NotSupportedException("Unknown modifier");
			}
		}

		private static bool HasModifier(Key key, ICollection<KeyModifier> modifiers)
		{
			switch (key)
			{
			case Key.LeftAlt:
				if (!modifiers.Contains(KeyModifier.Alt))
				{
					return modifiers.Contains(KeyModifier.LeftAlt);
				}
				return true;
			case Key.RightAlt:
				if (!modifiers.Contains(KeyModifier.Alt))
				{
					return modifiers.Contains(KeyModifier.RightAlt);
				}
				return true;
			case Key.LeftCtrl:
				if (!modifiers.Contains(KeyModifier.Ctrl))
				{
					return modifiers.Contains(KeyModifier.LeftCtrl);
				}
				return true;
			case Key.RightCtrl:
				if (!modifiers.Contains(KeyModifier.Ctrl))
				{
					return modifiers.Contains(KeyModifier.RightCtrl);
				}
				return true;
			case Key.LeftShift:
				if (!modifiers.Contains(KeyModifier.Shift))
				{
					return modifiers.Contains(KeyModifier.LeftShift);
				}
				return true;
			case Key.RightShift:
				if (!modifiers.Contains(KeyModifier.Shift))
				{
					return modifiers.Contains(KeyModifier.RightShift);
				}
				return true;
			default:
				throw new NotSupportedException("Unknown key");
			}
		}

		private bool IsExactMatch()
		{
			HashSet<KeyModifier> keyModifiers = GetKeyModifiers();
			HashSet<Key> keysPressed = GetKeysPressed();
			if (Contains(KeyModifier.None))
			{
				if (keyModifiers.Count == 0)
				{
					return keysPressed.Count == 0;
				}
				return false;
			}
			foreach (KeyModifier item in keyModifiers)
			{
				if (!IsKeyPressed(item, keysPressed))
				{
					return false;
				}
			}
			foreach (Key item2 in keysPressed)
			{
				if (!HasModifier(item2, keyModifiers))
				{
					return false;
				}
			}
			return true;
		}

		private bool MatchAny()
		{
			if (Contains(KeyModifier.None))
			{
				return true;
			}
			HashSet<KeyModifier> keyModifiers = GetKeyModifiers();
			HashSet<Key> keysPressed = GetKeysPressed();
			foreach (KeyModifier item in keyModifiers)
			{
				if (IsKeyPressed(item, keysPressed))
				{
					return true;
				}
			}
			return false;
		}

		private HashSet<KeyModifier> GetKeyModifiers()
		{
			HashSet<KeyModifier> hashSet = new HashSet<KeyModifier>();
			using (IEnumerator<KeyModifier> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyModifier current = enumerator.Current;
					switch (current)
					{
					case KeyModifier.Ctrl:
					case KeyModifier.LeftCtrl:
					case KeyModifier.RightCtrl:
					case KeyModifier.Shift:
					case KeyModifier.LeftShift:
					case KeyModifier.RightShift:
					case KeyModifier.Alt:
					case KeyModifier.LeftAlt:
					case KeyModifier.RightAlt:
						if (!hashSet.Contains(current))
						{
							hashSet.Add(current);
						}
						break;
					}
				}
				return hashSet;
			}
		}

		private HashSet<Key> GetKeysPressed()
		{
			HashSet<Key> hashSet = new HashSet<Key>();
			if (Keyboard.IsKeyDown(Key.LeftAlt))
			{
				hashSet.Add(Key.LeftAlt);
			}
			if (Keyboard.IsKeyDown(Key.RightAlt))
			{
				hashSet.Add(Key.RightAlt);
			}
			if (Keyboard.IsKeyDown(Key.LeftCtrl))
			{
				hashSet.Add(Key.LeftCtrl);
			}
			if (Keyboard.IsKeyDown(Key.RightCtrl))
			{
				hashSet.Add(Key.RightCtrl);
			}
			if (Keyboard.IsKeyDown(Key.LeftShift))
			{
				hashSet.Add(Key.LeftShift);
			}
			if (Keyboard.IsKeyDown(Key.RightShift))
			{
				hashSet.Add(Key.RightShift);
			}
			return hashSet;
		}
	}
}
