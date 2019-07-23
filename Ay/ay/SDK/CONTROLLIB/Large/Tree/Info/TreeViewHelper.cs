using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ay
{
    public static class TreeViewHelper
    {
        /// <summary>
        /// Expands all children of a TreeView
        /// </summary>
        /// <param name="treeView">The TreeView whose children will be expanded</param>
        public static void ExpandAll(this TreeView treeView)
        {
            ExpandSubContainers(treeView);
        }
        /// <summary>
        /// Expands all children of a TreeView or TreeViewItem
        /// </summary>
        /// <param name="parentContainer">The TreeView or TreeViewItem containing the children to expand</param>
        private static void ExpandSubContainers(ItemsControl parentContainer)
        {
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //expand the item
                    currentContainer.IsExpanded = true;
                    //if the item's children are not generated, they must be expanded
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            //once the children have been generated, expand those children's children then remove the event handler
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                ExpandSubContainers(currentContainer);
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the children have already been generated, so we can now expand those children
                    {
                        ExpandSubContainers(currentContainer);
                    }
                }
            }
        }
        /// <summary>
        /// Searches a TreeView for the provided object and selects it if found
        /// </summary>
        /// <param name="treeView">The TreeView containing the item</param>
        /// <param name="item">The item to search and select</param>
        public static void SelectItem(this TreeView treeView, object item)
        {
            ExpandAndSelectItem(treeView, item);
        }
        /// <summary>
        /// Finds the provided object in an ItemsControl's children and selects it
        /// </summary>
        /// <param name="parentContainer">The parent container whose children will be searched for the selected item</param>
        /// <param name="itemToSelect">The item to select</param>
        /// <returns>True if the item is found and selected, false otherwise</returns>
        private static bool ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            //check all items at the current level
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                //if the data item matches the item we want to select, set the corresponding
                //TreeViewItem IsSelected to true
                if (item == itemToSelect && currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();
                    //the item was found
                    return true;
                }
            }
            //if we get to this point, the selected item was not found at the current level, so we must check the children
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                //if children exist
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //keep track of if the TreeViewItem was expanded or not
                    bool wasExpanded = currentContainer.IsExpanded;
                    //expand the current TreeViewItem so we can check its child TreeViewItems
                    currentContainer.IsExpanded = true;
                    //if the TreeViewItem child containers have not been generated, we must listen to
                    //the StatusChanged event until they are
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                if (ExpandAndSelectItem(currentContainer, itemToSelect) == false)
                                {
                                    //The assumption is that code executing in this EventHandler is the result of the parent not
                                    //being expanded since the containers were not generated.
                                    //since the itemToSelect was not found in the children, collapse the parent since it was previously collapsed
                                    currentContainer.IsExpanded = false;
                                }
                                //remove the StatusChanged event handler since we just handled it (we only needed it once)
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the containers have been generated, so look for item to select in the children
                    {
                        if (ExpandAndSelectItem(currentContainer, itemToSelect) == false)
                        {
                            //restore the current TreeViewItem's expanded state
                            currentContainer.IsExpanded = wasExpanded;
                        }
                        else //otherwise the node was found and selected, so return true
                        {
                            return true;
                        }
                    }
                }
            }
            //no item was found
            return false;
        }

        /// <summary>
        /// Returns the TreeViewItem of a data bound object.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>The TreeViewItem of the data bound object or null.</returns>
        public static TreeViewItem GetItemFromObject(this TreeView treeView, object obj)
        {
            try
            {
                DependencyObject dObject = GetContainerFormObject(treeView, obj);
                TreeViewItem tvi = dObject as TreeViewItem;
                while (tvi == null)
                {
                    dObject = VisualTreeHelper.GetParent(dObject);
                    tvi = dObject as TreeViewItem;
                }
                return tvi;
            }
            catch { }
            return null;
        }

        private static DependencyObject GetContainerFormObject(ItemsControl item, object obj)
        {
            if (item == null)
                return null;

            DependencyObject dObject = null;
            dObject = item.ItemContainerGenerator.ContainerFromItem(obj);

            if (dObject != null)
                return dObject;

            var query = from childItem in item.Items.Cast<object>()
                        let childControl = item.ItemContainerGenerator.ContainerFromItem(childItem) as ItemsControl
                        select GetContainerFormObject(childControl, obj);

            return query.FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Selects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void SelectObject(this TreeView treeView, object obj)
        {
            treeView.SelectObject(obj, true);
        }

        /// <summary>
        /// Selects or deselects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="selected">select or deselect</param>
        public static void SelectObject(this TreeView treeView, object obj, bool selected)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsSelected = selected;
            }
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is selected.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is selected, and false if it is not selected or obj is not in the tree.</returns>
        public static bool IsObjectSelected(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsSelected;
            }
            return false;
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is focused.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is focused, and false if it is not focused or obj is not in the tree.</returns>
        public static bool IsObjectFocused(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsFocused;
            }
            return false;
        }

        /// <summary>
        /// Expands a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void ExpandObject(this TreeView treeView, object obj)
        {
            treeView.ExpandObject(obj, true);
        }

        /// <summary>
        /// Expands or collapses a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="expanded">expand or collapse</param>
        public static void ExpandObject(this TreeView treeView, object obj, bool expanded)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsExpanded = expanded;
                if (expanded)
                {
                    // update layout, so that following calls to f.e. SelectObject on child nodes will 
                    // find theire TreeViewNodes
                    treeView.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Returns if a douta bound object of a TreeView is expanded.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is expanded, and false if it is collapsed or obj is not in the tree.</returns>
        public static bool IsObjectExpanded(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsExpanded;
            }
            return false;
        }

        /// <summary>
        /// Retuns the parent TreeViewItem.
        /// </summary>
        /// <param name="item">TreeViewItem</param>
        /// <returns>Parent TreeViewItem</returns>
        public static TreeViewItem GetParentItem(this TreeViewItem item)
        {
            var dObject = VisualTreeHelper.GetParent(item);
            TreeViewItem tvi = dObject as TreeViewItem;
            while (tvi == null)
            {
                dObject = VisualTreeHelper.GetParent(dObject);
                tvi = dObject as TreeViewItem;
            }
            return tvi;
        }

        /// <summary>
        /// ay 2015-05-23 15:34:08 增加
        /// </summary>
        /// <param name="container"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }
                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }
                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate. Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.
                container.ApplyTemplate();
                ItemsPresenter itemsPresenter =
                (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = WpfTreeHelper.FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();
                        itemsPresenter = WpfTreeHelper.FindVisualChild<ItemsPresenter>(container);
                    }
                }
                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;
                MyVirtualizingStackPanel virtualizingPanel =
                itemsHostPanel as MyVirtualizingStackPanel;
                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    if (virtualizingPanel != null)
                    {
                        // Bring the item into view so
                        // that the container will be generated.
                        virtualizingPanel.BringIntoView(i);
                        subContainer =
                        (TreeViewItem)container.ItemContainerGenerator.
                        ContainerFromIndex(i);
                    }
                    else
                    {
                        subContainer = (TreeViewItem)container.ItemContainerGenerator.
                        ContainerFromIndex(i);
                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        subContainer.BringIntoView();
                    }
                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// ay 2015-05-23 15:34:08 增加,不需要查找子容器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TreeViewItem GetTreeViewItem2(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }
                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }
                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate. Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.
                container.ApplyTemplate();
                ItemsPresenter itemsPresenter =
                (ItemsPresenter)container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = WpfTreeHelper.FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();
                        itemsPresenter = WpfTreeHelper.FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);
                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;
                MyVirtualizingStackPanel virtualizingPanel =
                itemsHostPanel as MyVirtualizingStackPanel;
                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                    if (subContainer.DataContext == item)
                    {
                        subContainer.IsExpanded = false;
                        return subContainer as TreeViewItem;
                    }
                }
            }
            return null;
        }



        public class MyVirtualizingStackPanel : VirtualizingStackPanel
        {
            /// <summary>
            /// Publically expose BringIndexIntoView.
            /// </summary>
            public void BringIntoView(int index)
            {
                this.BringIndexIntoView(index);
            }
        }

        //        <TreeView VirtualizingStackPanel.IsVirtualizing="True">
        //<!--Use the custom class MyVirtualizingStackPanel
        //as the ItemsPanel for the TreeView and
        //TreeViewItem object.-->
        //<TreeView.ItemsPanel>
        //<ItemsPanelTemplate>
        //<src:MyVirtualizingStackPanel/>
        //</ItemsPanelTemplate>
        //</TreeView.ItemsPanel>
        //<TreeView.ItemContainerStyle>
        //<Style TargetType="TreeViewItem">
        //<Setter Property="ItemsPanel">
        //<Setter.Value>
        //<ItemsPanelTemplate>
        //<src:MyVirtualizingStackPanel/>
        //</ItemsPanelTemplate>
        //</Setter.Value>
        //</Setter>
        //</Style>
        //</TreeView.ItemContainerStyle>
        //</TreeView>



    }
}