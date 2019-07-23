using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.contentcore
{
    public class ChangePropertyAction : TargetedTriggerAction<object>
    {
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(ChangePropertyAction), null);

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ChangePropertyAction), null);

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(ChangePropertyAction), null);

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(bool), typeof(ChangePropertyAction), null);

        public string PropertyName
        {
            get
            {
                return (string)GetValue(PropertyNameProperty);
            }
            set
            {
                SetValue(PropertyNameProperty, value);
            }
        }

        public object Value
        {
            get
            {
                return GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public Duration Duration
        {
            get
            {
                return (Duration)GetValue(DurationProperty);
            }
            set
            {
                SetValue(DurationProperty, value);
            }
        }

        public bool Increment
        {
            get
            {
                return (bool)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject != null && !string.IsNullOrEmpty(PropertyName) && base.Target != null)
            {
                Type type = base.Target.GetType();
                PropertyInfo property = type.GetProperty(PropertyName);
                ValidateProperty(property);
                object obj = Value;
                TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(property.PropertyType);
                Exception ex = null;
                try
                {
                    if (Value != null)
                    {
                        if (typeConverter != null && typeConverter.CanConvertFrom(Value.GetType()))
                        {
                            obj = typeConverter.ConvertFrom(Value);
                        }
                        else
                        {
                            typeConverter = TypeConverterHelper.GetTypeConverter(Value.GetType());
                            if (typeConverter != null && typeConverter.CanConvertTo(property.PropertyType))
                            {
                                obj = typeConverter.ConvertTo(Value, property.PropertyType);
                            }
                        }
                    }
                    if (Duration.HasTimeSpan)
                    {
                        ValidateAnimationPossible(type);
                        object currentPropertyValue = GetCurrentPropertyValue(base.Target, property);
                        AnimatePropertyChange(property, currentPropertyValue, obj);
                    }
                    else
                    {
                        if (Increment)
                        {
                            obj = IncrementCurrentValue(property);
                        }
                        property.SetValue(base.Target, obj, new object[0]);
                    }
                }
                catch (FormatException ex2)
                {
                    ex = ex2;
                }
                catch (ArgumentException ex3)
                {
                    ex = ex3;
                }
                catch (MethodAccessException ex4)
                {
                    ex = ex4;
                }
                if (ex != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionCannotSetValueExceptionMessage", new object[3]
                    {
                        (Value != null) ? Value.GetType().Name : "null",
                        PropertyName,
                        property.PropertyType.Name
                    }), ex);
                }
            }
        }

        private void AnimatePropertyChange(PropertyInfo propertyInfo, object fromValue, object newValue)
        {
            Storyboard storyboard = new Storyboard();
            Timeline timeline = typeof(double).IsAssignableFrom(propertyInfo.PropertyType) ? CreateDoubleAnimation((double)fromValue, (double)newValue) : (typeof(Color).IsAssignableFrom(propertyInfo.PropertyType) ? CreateColorAnimation((Color)fromValue, (Color)newValue) : ((!typeof(Point).IsAssignableFrom(propertyInfo.PropertyType)) ? CreateKeyFrameAnimation(fromValue, newValue) : CreatePointAnimation((Point)fromValue, (Point)newValue)));
            timeline.Duration = Duration;
            storyboard.Children.Add(timeline);
            if (base.TargetObject == null && base.TargetName != null && base.Target is Freezable)
            {
                Storyboard.SetTargetName(storyboard, base.TargetName);
            }
            else
            {
                Storyboard.SetTarget(storyboard, (DependencyObject)base.Target);
            }
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(propertyInfo.Name));
            storyboard.Completed += delegate
            {
                propertyInfo.SetValue(base.Target, newValue, new object[0]);
            };
            storyboard.FillBehavior = FillBehavior.Stop;
            FrameworkElement frameworkElement = base.AssociatedObject as FrameworkElement;
            if (frameworkElement != null)
            {
                storyboard.Begin(frameworkElement);
            }
            else
            {
                storyboard.Begin();
            }
        }

        private static object GetCurrentPropertyValue(object target, PropertyInfo propertyInfo)
        {
            FrameworkElement frameworkElement = target as FrameworkElement;
            target.GetType();
            object obj = propertyInfo.GetValue(target, null);
            if (frameworkElement != null && (propertyInfo.Name == "Width" || propertyInfo.Name == "Height") && double.IsNaN((double)obj))
            {
                obj = ((!(propertyInfo.Name == "Width")) ? ((object)frameworkElement.ActualHeight) : ((object)frameworkElement.ActualWidth));
            }
            return obj;
        }

        private void ValidateAnimationPossible(Type targetType)
        {
            if (Increment)
            {
                throw new InvalidOperationException("ChangePropertyActionCannotIncrementAnimatedPropertyChangeExceptionMessage");
            }
            if (!typeof(DependencyObject).IsAssignableFrom(targetType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionCannotAnimateTargetTypeExceptionMessage", new object[1]
                {
                    targetType.Name
                }));
            }
        }

        private Timeline CreateKeyFrameAnimation(object newValue, object fromValue)
        {
            ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
            DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
            discreteObjectKeyFrame.KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0L));
            discreteObjectKeyFrame.Value = fromValue;
            DiscreteObjectKeyFrame keyFrame = discreteObjectKeyFrame;
            DiscreteObjectKeyFrame discreteObjectKeyFrame2 = new DiscreteObjectKeyFrame();
            discreteObjectKeyFrame2.KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan);
            discreteObjectKeyFrame2.Value = newValue;
            DiscreteObjectKeyFrame keyFrame2 = discreteObjectKeyFrame2;
            objectAnimationUsingKeyFrames.KeyFrames.Add(keyFrame);
            objectAnimationUsingKeyFrames.KeyFrames.Add(keyFrame2);
            return objectAnimationUsingKeyFrames;
        }

        private Timeline CreatePointAnimation(Point fromValue, Point newValue)
        {
            PointAnimation pointAnimation = new PointAnimation();
            pointAnimation.From = fromValue;
            pointAnimation.To = newValue;
            return pointAnimation;
        }

        private Timeline CreateColorAnimation(Color fromValue, Color newValue)
        {
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = fromValue;
            colorAnimation.To = newValue;
            return colorAnimation;
        }

        private Timeline CreateDoubleAnimation(double fromValue, double newValue)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = fromValue;
            doubleAnimation.To = newValue;
            return doubleAnimation;
        }

        private void ValidateProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionCannotFindPropertyNameExceptionMessage", new object[2]
                {
                    PropertyName,
                    base.Target.GetType().Name
                }));
            }
            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionPropertyIsReadOnlyExceptionMessage", new object[2]
                {
                    PropertyName,
                    base.Target.GetType().Name
                }));
            }
        }

        private object IncrementCurrentValue(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionCannotIncrementWriteOnlyPropertyExceptionMessage", new object[1]
                {
                    propertyInfo.Name
                }));
            }
            object value = propertyInfo.GetValue(base.Target, null);
            object obj = value;
            Type propertyType = propertyInfo.PropertyType;
            TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(propertyInfo.PropertyType);
            object obj2 = Value;
            if (obj2 == null || value == null)
            {
                return obj2;
            }
            if (typeConverter.CanConvertFrom(obj2.GetType()))
            {
                obj2 = TypeConverterHelper.DoConversionFrom(typeConverter, obj2);
            }
            if (typeof(double).IsAssignableFrom(propertyType))
            {
                return (double)value + (double)obj2;
            }
            if (typeof(int).IsAssignableFrom(propertyType))
            {
                return (int)value + (int)obj2;
            }
            if (typeof(float).IsAssignableFrom(propertyType))
            {
                return (float)value + (float)obj2;
            }
            if (typeof(string).IsAssignableFrom(propertyType))
            {
                return (string)value + (string)obj2;
            }
            return TryAddition(value, obj2);
        }

        private static object TryAddition(object currentValue, object value)
        {
         
            Type type = value.GetType();
            Type type2 = currentValue.GetType();
            MethodInfo methodInfo = null;
            object obj2 = value;
            MethodInfo[] methods = type2.GetMethods();
            foreach (MethodInfo methodInfo2 in methods)
            {
                if (string.Compare(methodInfo2.Name, "op_Addition", StringComparison.Ordinal) == 0)
                {
                    ParameterInfo[] parameters = methodInfo2.GetParameters();
                    Type parameterType = parameters[1].ParameterType;
                    if (parameters[0].ParameterType.IsAssignableFrom(type2))
                    {
                        if (!parameterType.IsAssignableFrom(type))
                        {
                            TypeConverter typeConverter = TypeConverterHelper.GetTypeConverter(parameterType);
                            if (!typeConverter.CanConvertFrom(type))
                            {
                                continue;
                            }
                            obj2 = TypeConverterHelper.DoConversionFrom(typeConverter, value);
                        }
                        if (methodInfo != null)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "ChangePropertyActionAmbiguousAdditionOperationExceptionMessage", new object[1]
                            {
                                type2.Name
                            }));
                        }
                        methodInfo = methodInfo2;
                    }
                }
            }
            if (methodInfo != null)
            {
                return methodInfo.Invoke(null, new object[2]
                {
                    currentValue,
                    obj2
                });
            }
            return value;
        }
    }
}
