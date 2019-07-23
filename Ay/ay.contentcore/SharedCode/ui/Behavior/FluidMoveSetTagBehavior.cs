using System.Windows;

namespace ay.contentcore
{
	public sealed class FluidMoveSetTagBehavior : FluidMoveBehaviorBase
	{
		internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData)
		{
			TagData value;
			if (!FluidMoveBehaviorBase.TagDictionary.TryGetValue(tag, out value))
			{
				value = new TagData();
				FluidMoveBehaviorBase.TagDictionary.Add(tag, value);
			}
			value.ParentRect = newTagData.ParentRect;
			value.AppRect = newTagData.AppRect;
			value.Parent = newTagData.Parent;
			value.Child = newTagData.Child;
			value.Timestamp = newTagData.Timestamp;
		}
	}
}
