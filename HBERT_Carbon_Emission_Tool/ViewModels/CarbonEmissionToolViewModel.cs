using System;
using System.Collections.Generic;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CarbonEmissionTool.ViewModels
{
    public class CarbonEmissionToolViewModel : IProjectDetails, INotifyPropertyChanged
    {
        private string _address;
        private string _name;
        private string _revision;
        private double _floorArea;
        private RibaWorkstage _ribaWorkstage = RibaWorkstage.One;

        #region IProjectDetails implementation
        /// <summary>
        /// The address of the project. This is passed into the form when it is intialized to auto-populate the field.
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                _address = value;

                OnPropertyChanged(nameof(Address));
            }
        }

        /// <summary>
        /// The name of the project. This is passed into the form when it is intialized to auto-populate the field.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value == null ? "" : value;

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Revision
        {
            get => _revision;
            set
            {
                _revision = value;

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(Revision));
            }
        }

        public RibaWorkstage RibaWorkstage
        {
            get => _ribaWorkstage;
            set
            {
                _ribaWorkstage = value;

                OnPropertyChanged(nameof(RibaWorkstage));
            }
        }

        public ProjectType ProjectType { get; set; }

        public double FloorArea
        {
            get => _floorArea;
            set
            {
                _floorArea = value;

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(FloorArea));
            }
        }


        /// <summary>
        /// Returns true if the user has input a valid <see cref="Name"/>, <see cref="FloorArea"/>
        /// and <see cref="Revision"/> which enables the user to proceed to the next step of the
        /// main process.
        /// </summary>
        public bool CanPublish => !string.IsNullOrWhiteSpace(this.Name) & !string.IsNullOrWhiteSpace(this.Revision) &
                                  this.FloorArea > 0;

        /// <summary>
        /// A list of the <see cref="RibaWorkstage"/>'s.
        /// </summary>
        public List<RibaWorkstage> RibaWorkstages { get; }

        /// <summary>
        /// The building elements displayed as check boxes on the UI window.
        /// </summary>
        public CheckBoxItemCollection BuildElements { get; }

        /// <summary>
        /// The building sectors displayed as check boxes on the UI window.
        /// </summary>
        public CheckBoxItemCollection Sectors { get; }
        #endregion

        /// <summary>
        /// Command for binding to update the <see cref="ProjectType"/> when the user clicks the buttons.
        /// </summary>
        public ICommand UpdateProjectType { get; }

        /// <summary>
        /// The <see cref="IDataCapture"/> object.
        /// </summary>
        public IDataCapture DataCapture { get; }

        /// <summary>
        /// Constructs a new <see cref="CarbonEmissionToolViewModel"/>.
        /// </summary>
        public CarbonEmissionToolViewModel()
        {
            var projectInfo = ApplicationServices.Document.ProjectInformation;

            this.Name = projectInfo.Name;

            this.Address = projectInfo.Address.Replace(Environment.NewLine, " ");

            this.BuildElements = new CheckBoxItemCollection(ApplicationSettings.BuildingElementNames);

            this.Sectors = new CheckBoxItemCollection(ApplicationSettings.SectorNames);
            
            this.RibaWorkstages = Enum.GetValues(typeof(RibaWorkstage)).OfType<RibaWorkstage>().ToList();

            this.UpdateProjectType = new ProjectTypeSelectionCommand(this);

            this.DataCapture = new DataCapture();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
