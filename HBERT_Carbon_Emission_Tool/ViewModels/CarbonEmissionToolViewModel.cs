using System;
using System.Collections.Generic;
using CarbonEmissionTool.Models;
using CarbonEmissionTool.Services;
using CarbonEmissionTool.Settings;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarbonEmissionTool.ViewModels
{
    public class CarbonEmissionToolViewModel : DataErrorNotifier, IProjectDetails, INotifyPropertyChanged
    {
        private string _address;
        private string _name;
        private string _revision;
        private double _floorArea = 1.0;
        private RibaWorkstage _ribaWorkstage = RibaWorkstage.One;
        private ProjectType _projectType;

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
                _name = value;

                var propertyName = nameof(Name);
                ValidateInput(propertyName);


                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
            }
        }

        public string Revision
        {
            get => _revision;
            set
            {
                _revision = value;

                var propertyName = nameof(Revision);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
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

        public ProjectType ProjectType 
        { 
            get => _projectType;
            set
            {
                _projectType = value;

                OnPropertyChanged(nameof(ProjectType));
            }
        }

        public double FloorArea
        {
            get => _floorArea;
            set
            {
                _floorArea = value;

                var propertyName = nameof(FloorArea);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
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
        /// Constructs a new <see cref="CarbonEmissionToolViewModel"/>.
        /// </summary>
        public CarbonEmissionToolViewModel()
        {
            var document = ApplicationServices.Document;

            if (UserInputMonitor.HasInputCapture(document))
            {
                var inputCapture = UserInputMonitor.GetCapture(document);

                this.Name = inputCapture.Name;

                this.Revision = inputCapture.Revision;

                this.Address = inputCapture.Address;

                this.FloorArea = (int)inputCapture.FloorArea;

                this.BuildElements = inputCapture.BuildElements;

                this.Sectors = inputCapture.Sectors;

                this.RibaWorkstage = inputCapture.RibaWorkstage;

                this.ProjectType = inputCapture.ProjectType;
            }
            else
            {
                var projectInfo = document.ProjectInformation;

                this.Name = projectInfo.Name;

                this.Address = projectInfo.Address.Replace(Environment.NewLine, " ");

                this.BuildElements = new CheckBoxItemCollection(ApplicationSettings.BuildingElementNames);

                this.Sectors = new CheckBoxItemCollection(ApplicationSettings.SectorNames);
            }

            this.RibaWorkstages = Enum.GetValues(typeof(RibaWorkstage)).OfType<RibaWorkstage>().ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Data Validation implementation
        public override void ValidateInput(string propertyName)
        {
            var error = string.Empty;

            switch (propertyName)
            {
                case nameof(Name):
                    error = NameBaseValidation.Validate(this.Name);
                    break;

                case nameof(Revision):
                    error = NameBaseValidation.Validate(this.Revision);
                    break;

                case nameof(FloorArea):
                    if (this.FloorArea <= 0)
                        error = "Area must be greater than 0";
                    break;
            }

            AddError(propertyName, error);
        }
        #endregion
    }
}
