using System;
using System.Windows.Input;
using CarbonEmissionTool.Models;

namespace CarbonEmissionTool.ViewModels
{
    /// <summary>
    /// The <see cref="ICommand"/> to run <see cref="CarbonEmissionToolMain"/> mainline function.
    /// </summary>
    public class RunHbertCommand : ICommand
    {
        private IPublishDetails _publishDetails;

        /// <summary>
        /// Constructs a new <see cref="RunHbertCommand"/>.
        /// </summary>
        public RunHbertCommand(IPublishDetails publishDetails)
        {
            _publishDetails = publishDetails;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            var projectDetails = (IProjectDetails) parameter;

            CarbonEmissionToolMain.ComputeEmbodiedCarbon(projectDetails, _publishDetails);
        }
    }
}
