using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ay.contentcore
{
    public class RemoveElementAction : TargetedTriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject != null && base.Target != null)
            {
                DependencyObject parent = base.Target.Parent;
                Panel panel = parent as Panel;
                if (panel != null)
                {
                    panel.Children.Remove(base.Target);
                }
                else
                {
                    ContentControl contentControl = parent as ContentControl;
                    if (contentControl != null)
                    {
                        if (contentControl.Content == base.Target)
                        {
                            contentControl.Content = null;
                        }
                    }
                    else
                    {
                        ItemsControl itemsControl = parent as ItemsControl;
                        if (itemsControl != null)
                        {
                            itemsControl.Items.Remove(base.Target);
                        }
                        else
                        {
                            Page page = parent as Page;
                            if (page != null)
                            {
                                if (page.Content == base.Target)
                                {
                                    page.Content = null;
                                }
                            }
                            else
                            {
                                Decorator decorator = parent as Decorator;
                                if (decorator != null)
                                {
                                    if (decorator.Child == base.Target)
                                    {
                                        decorator.Child = null;
                                    }
                                }
                                else if (parent != null)
                                {
                                    throw new InvalidOperationException("UnsupportedRemoveTargetExceptionMessage");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
