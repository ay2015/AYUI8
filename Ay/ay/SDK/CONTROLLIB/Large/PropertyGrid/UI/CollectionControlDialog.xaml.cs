using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Xceed.Wpf.Toolkit
{
    /// <summary>
    /// CollectionControlDialog.xaml 的交互逻辑
    /// </summary>
    public partial class CollectionControlDialog : CollectionControlDialogBase
    {
        public CollectionControlDialog()
        {
            InitializeComponent();
        }
        private IList originalData = new List<object>();

        /// <summary>Identifies the ItemsSource dependency property.</summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CollectionControlDialog), new UIPropertyMetadata(null));

        /// <summary>Identifies the ItemsSourceType dependency property.</summary>
        public static readonly DependencyProperty ItemsSourceTypeProperty = DependencyProperty.Register("ItemsSourceType", typeof(Type), typeof(CollectionControlDialog), new UIPropertyMetadata(null));

        /// <summary>Identifies the NewItemTypes dependency property.</summary>
        public static readonly DependencyProperty NewItemTypesProperty = DependencyProperty.Register("NewItemTypes", typeof(IList), typeof(CollectionControlDialog), new UIPropertyMetadata(null));

        /// <summary>Identifies the IsReadOnly dependency property.</summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CollectionControlDialog), new UIPropertyMetadata(false));

        /// <summary>Identifies the EditorDefinitions dependency property.</summary>
        public static readonly DependencyProperty EditorDefinitionsProperty = DependencyProperty.Register("EditorDefinitions", typeof(EditorDefinitionCollection), typeof(CollectionControlDialog), new UIPropertyMetadata(null));

  
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>Gets or sets the type of ItemsSource.</summary>
        public Type ItemsSourceType
        {
            get
            {
                return (Type)GetValue(ItemsSourceTypeProperty);
            }
            set
            {
                SetValue(ItemsSourceTypeProperty, value);
            }
        }

        /// <summary>Gets or sets a list of custom item types that appear in the Add ComboBox.</summary>
        public IList<Type> NewItemTypes
        {
            get
            {
                return (IList<Type>)GetValue(NewItemTypesProperty);
            }
            set
            {
                SetValue(NewItemTypesProperty, value);
            }
        }

        /// <summary>Gets or sets whether the CollectionControlDialog is read-only.</summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary>Gets or sets the custom editors for the PropertyGrid located in the CollectionControl.</summary>
        public EditorDefinitionCollection EditorDefinitions
        {
            get
            {
                return (EditorDefinitionCollection)GetValue(EditorDefinitionsProperty);
            }
            set
            {
                SetValue(EditorDefinitionsProperty, value);
            }
        }


        public CollectionControlDialog(Type itemsourceType)
        : this()
        {
            ItemsSourceType = itemsourceType;
        }

        public CollectionControlDialog(Type itemsourceType, IList<Type> newItemTypes)
            : this(itemsourceType)
        {
            NewItemTypes = newItemTypes;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (ItemsSource != null)
            {
                foreach (object item in ItemsSource)
                {
                    originalData.Add(Clone(item));
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsSource is IDictionary && !AreDictionaryKeysValid())
            {
                MessageBox.Show("All dictionary items should have distinct non-null Key values.", "Warning");
            }
            else
            {
                base.DialogResult = _collectionControl.PersistChanges();
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _collectionControl.PersistChanges(originalData);
            base.DialogResult = false;
            Close();
        }

        private object Clone(object source)
        {
            if (source == null)
            {
                return null;
            }
            object obj = null;
            Type type = source.GetType();
            if (source is Array)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, source);
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    obj = (Array)binaryFormatter.Deserialize(memoryStream);
                }
            }
            else if (ItemsSource is IDictionary && type.IsGenericType && typeof(KeyValuePair<,>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                obj = GenerateEditableKeyValuePair(source);
            }
            else
            {
                try
                {
                    obj = FormatterServices.GetUninitializedObject(type);
                }
                catch (Exception)
                {
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    constructor.Invoke(obj, null);
                }
                else
                {
                    obj = source;
                }
            }
            if (obj != null)
            {
                PropertyInfo[] properties = type.GetProperties();
                PropertyInfo[] array = properties;
                foreach (PropertyInfo propertyInfo in array)
                {
                    try
                    {
                        ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
                        object[] array2 = (indexParameters.GetLength(0) == 0) ? null : new object[1]
                        {
                            indexParameters.GetLength(0) - 1
                        };
                        object value = propertyInfo.GetValue(source, array2);
                        if (propertyInfo.CanWrite)
                        {
                            if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(Transform) && !propertyInfo.PropertyType.Equals(typeof(string)))
                            {
                                if (propertyInfo.PropertyType.IsGenericType)
                                {
                                    Type type2 = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
                                    if (type2 != null && !type2.IsPrimitive && !type2.Equals(typeof(string)) && !type2.IsEnum)
                                    {
                                        object value2 = Clone(value);
                                        propertyInfo.SetValue(obj, value2, null);
                                    }
                                    else
                                    {
                                        propertyInfo.SetValue(obj, value, null);
                                    }
                                }
                                else
                                {
                                    object obj2 = Clone(value);
                                    if (obj2 != null)
                                    {
                                        if (array2 != null)
                                        {
                                            obj.GetType().GetMethod("Add").Invoke(obj, new object[1]
                                            {
                                                obj2
                                            });
                                        }
                                        else
                                        {
                                            propertyInfo.SetValue(obj, obj2, null);
                                        }
                                    }
                                }
                            }
                            else if (array2 != null)
                            {
                                obj.GetType().GetMethod("Add").Invoke(obj, new object[1]
                                {
                                    value
                                });
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, value, null);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return obj;
        }

        private object GenerateEditableKeyValuePair(object source)
        {
            Type type = source.GetType();
            if (type.GetGenericArguments() == null || type.GetGenericArguments().GetLength(0) != 2)
            {
                return null;
            }
            PropertyInfo property = type.GetProperty("Key");
            PropertyInfo property2 = type.GetProperty("Value");
            if (property != null && property2 != null)
            {
                return ListUtilities.CreateEditableKeyValuePair(property.GetValue(source, null), type.GetGenericArguments()[0], property2.GetValue(source, null), type.GetGenericArguments()[1]);
            }
            return null;
        }

        private bool AreDictionaryKeysValid()
        {
            IEnumerable<object> source = _collectionControl.Items.Select(delegate (object x)
            {
                PropertyInfo property = x.GetType().GetProperty("Key");
                if (property != null)
                {
                    return property.GetValue(x, null);
                }
                return null;
            });
            if (source.Distinct().Count() == _collectionControl.Items.Count)
            {
                return source.All((object x) => x != null);
            }
            return false;
        }
    }
}
