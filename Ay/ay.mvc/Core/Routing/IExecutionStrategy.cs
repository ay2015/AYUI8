using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Ay.MvcFramework.AyMarkupExtension;

namespace Ay.MvcFramework
{
    /// <summary>
    /// Defines the interface for a strategy of execution for the CommandBehaviorBinding
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Gets or sets the Behavior that we execute this strategy
        /// </summary>
        CommandBehaviorBinding Route { get; set; }
     
        /// <summary>
        /// Executes according to the strategy type
        /// </summary>
        /// <param name="parameter">The parameter to be used in the execution</param>
        void Execute(object parameter);
    }
    

}
