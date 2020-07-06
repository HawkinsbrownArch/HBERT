using System;
using System.Windows.Input;
using CarbonEmissionTool.Models;

namespace CarbonEmissionTool.ViewModels
{
    /// <summary>
    /// Currently not used.
    /// </summary>
    public class ProjectTypeSelectionCommand : ICommand
    {
        private IProjectDetails _projectDetails;

        /// <summary>
        /// Constructs a new <see cref="ProjectTypeSelectionCommand"/>
        /// </summary>
        public ProjectTypeSelectionCommand(IProjectDetails projectDetails)
        {
            _projectDetails = projectDetails;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            var projectType = (ProjectType)Enum.Parse(typeof(ProjectType), parameter.ToString());

            _projectDetails.ProjectType = projectType;
        }
    }
}
