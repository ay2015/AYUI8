using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Input;

namespace Ay.MvcFramework.AyMarkupExtension
{
    /// <summary>
    /// Defines the attached properties to create a MVC
    /// </summary>
    public class Mvc
    {
        #region Behavior

        /// <summary>
        /// Behavior Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(CommandBehaviorBinding), typeof(Mvc),
                new FrameworkPropertyMetadata((CommandBehaviorBinding)null));

        /// <summary>
        /// Gets the Behavior property. 
        /// </summary>
        private static CommandBehaviorBinding GetBehavior(DependencyObject d)
        {
            return (CommandBehaviorBinding)d.GetValue(BehaviorProperty);
        }

        /// <summary>
        /// Sets the Behavior property.  
        /// </summary>
        private static void SetBehavior(DependencyObject d, CommandBehaviorBinding value)
        {
            d.SetValue(BehaviorProperty, value);
        }

        #endregion

        //#region Command

        ///// <summary>
        ///// Command Attached Dependency Property
        ///// </summary>
        //public static readonly DependencyProperty CommandProperty =
        //    DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Mvc),
        //        new FrameworkPropertyMetadata((ICommand)null,
        //            new PropertyChangedCallback(OnCommandChanged)));

        ///// <summary>
        ///// Gets the Command property.  
        ///// </summary>
        //public static ICommand GetCommand(DependencyObject d)
        //{
        //    return (ICommand)d.GetValue(CommandProperty);
        //}

        ///// <summary>
        ///// Sets the Command property. 
        ///// </summary>
        //public static void SetCommand(DependencyObject d, ICommand value)
        //{
        //    d.SetValue(CommandProperty, value);
        //}

        ///// <summary>
        ///// Handles changes to the Command property.
        ///// </summary>
        //private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    CommandBehaviorBinding binding = FetchOrCreateBinding(d);
        //    binding.Command = (ICommand)e.NewValue;
        //}

        //#endregion



        #region IsSendEventArgs

        public static bool GetIsSendEventArgs(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSendEventArgsProperty);
        }

        public static void SetIsSendEventArgs(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSendEventArgsProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSendEventArgs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSendEventArgsProperty =
            DependencyProperty.RegisterAttached("IsSendEventArgs", typeof(bool), typeof(Mvc), new FrameworkPropertyMetadata(false,
                   new PropertyChangedCallback(OnIsSendEventArgsChanged)));


        private static void OnIsSendEventArgsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(d);
            binding.IsSendEventArgs = (bool)e.NewValue;
        }
        #endregion

        #region Action

        /// <summary>
        /// Action Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.RegisterAttached("Action", typeof(ActionResult), typeof(Mvc),
                new FrameworkPropertyMetadata((ActionResult)null,
                    new PropertyChangedCallback(OnActionChanged)));

        /// <summary>
        /// Gets the Action property.  
        /// </summary>
        public static ActionResult GetAction(DependencyObject d)
        {
            return (ActionResult)d.GetValue(ActionProperty);
        }

        /// <summary>
        /// Sets the Action property. 
        /// </summary>
        public static void SetAction(DependencyObject d, ActionResult value)
        {
            d.SetValue(ActionProperty, value);
        }

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Console.WriteLine("11111111111111");
            var _32 = d.GetBindingExpression(Mvc.ActionProperty);
            CommandBehaviorBinding binding = FetchOrCreateBinding(d);
            if (_32!=null)
            {
                var _bindname = _32.ParentBinding.Path.Path;
                binding.ActionName = _bindname.Substring(_bindname.LastIndexOf('.') + 1);
            }
            binding.Action = (ActionResult)e.NewValue;

            if (binding.Owner != null)
            {

                if (!binding.IsLockOwner)
                {
                    binding.AppFirstLoad();
                }
                binding.IsLockOwner = true;
            }
        }

        #endregion

        #region CommandParameter

        /// <summary>
        /// CommandParameter Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.RegisterAttached("Parameter", typeof(object), typeof(Mvc),
                new FrameworkPropertyMetadata((object)null,
                    new PropertyChangedCallback(OnParameterChanged)));

        /// <summary>
        /// Gets the CommandParameter property.  
        /// </summary>
        public static object GetParameter(DependencyObject d)
        {
            return (object)d.GetValue(ParameterProperty);
        }

        /// <summary>
        /// Sets the CommandParameter property. 
        /// </summary>
        public static void SetParameter(DependencyObject d, object value)
        {
            d.SetValue(ParameterProperty, value);
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(d);
            binding.CommandParameter = e.NewValue;
        }

        #endregion

        #region Event

        /// <summary>
        /// Event Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.RegisterAttached("Event", typeof(string), typeof(Mvc),
                new FrameworkPropertyMetadata((string)String.Empty,
                    new PropertyChangedCallback(OnEventChanged)));

        /// <summary>
        /// Gets the Event property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetEvent(DependencyObject d)
        {
            return (string)d.GetValue(EventProperty);
        }

        /// <summary>
        /// Sets the Event property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetEvent(DependencyObject d, string value)
        {
            d.SetValue(EventProperty, value);
        }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBehaviorBinding binding = FetchOrCreateBinding(d);
            //check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
            if (binding.Event != null && binding.Owner != null)
                binding.Dispose();
            //bind the new event to the command
            binding.BindEvent(d, e.NewValue.ToString());

       
        }

        #endregion

        #region Helpers
        //tries to get a CommandBehaviorBinding from the element. Creates a new instance if there is not one attached
        private static CommandBehaviorBinding FetchOrCreateBinding(DependencyObject d)
        {
            CommandBehaviorBinding binding = Mvc.GetBehavior(d);
            if (binding == null)
            {
                binding = new CommandBehaviorBinding();
                Mvc.SetBehavior(d, binding);
            }
            return binding;
        }
        #endregion

    }

}