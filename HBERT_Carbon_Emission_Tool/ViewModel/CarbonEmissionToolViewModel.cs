using CarbonEmissionTool.Annotations;
using CarbonEmissionTool.Model.Collections;
using CarbonEmissionTool.Model.Enums;
using CarbonEmissionTool.Model.Interfaces;
using CarbonEmissionTool.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using CarbonEmissionTool.Services.Caches;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.ViewModel
{
    class CarbonEmissionToolViewModel : IProjectDetails
    {
        private string _address;
        private string _name;
        private string _version;
        private string _ribaWorkstage;
        private bool _newBuild;
        private double _floorArea;
        private ProjectType _projectType;
        private FamilySymbol _titleBlock;
        private View3D _axoView;

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

                OnPropertyChanged(nameof(Name));
            }
        }

        public string Version
        {
            get => _version;
            set
            {
                _version = value;

                OnPropertyChanged(nameof(Version));
            }
        }

        public string RibaWorkstage
        {
            get => _ribaWorkstage;
            set
            {
                _ribaWorkstage = value;

                OnPropertyChanged(nameof(RibaWorkstage));
            }
        }

        public bool NewBuild
        {
            get => _newBuild;
            set
            {
                _newBuild = value;

                OnPropertyChanged(nameof(NewBuild));
            }
        }

        public double FloorArea
        {
            get => _floorArea;
            set
            {
                _floorArea = value;

                OnPropertyChanged(nameof(FloorArea));
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

        /// <summary>
        /// The title block used by HBERT for creating the sheet to present the embodied carbon result.
        /// </summary>
        public FamilySymbol TitleBlock
        {
            get => _titleBlock;
            set
            {
                _titleBlock = value;

                OnPropertyChanged(nameof(TitleBlock));
            }
        }

        /// <summary>
        /// The 3D view in Revit used by HBERT for processing and analysing the model.
        /// </summary>
        public View3D AxoView
        {
            get => _axoView;
            set
            {
                _axoView = value;

                OnPropertyChanged(nameof(AxoView));
            }
        }

        /// <summary>
        /// The building elements displayed as check boxes on the UI window.
        /// </summary>
        public SelectedItemCollection BuildElements { get; }

        /// <summary>
        /// The building sectors displayed as check boxes on the UI window.
        /// </summary>
        public SelectedItemCollection Sectors { get; }

        /// <summary>
        /// The <see cref="CarbonDataCache"/>.
        /// </summary>
        public CarbonDataCache CarbonDataCache { get; }

        /// <summary>
        /// The <see cref="ChartColorCache"/>.
        /// </summary>
        public ChartColorCache ChartColorCache { get; }

        /// <summary>
        /// The <see cref="FilledRegionCache"/>.
        /// </summary>
        public FilledRegionCache FilledRegionCache { get; }

        /// <summary>
        /// The <see cref="TextStyleCache"/>.
        /// </summary>
        public TextStyleCache TextStyleCache { get; }

        /// <summary>
        /// Constructs a new <see cref="CarbonEmissionToolViewModel"/>.
        /// </summary>
        public CarbonEmissionToolViewModel()
        {
            var projectInfo = ApplicationServices.Document.ProjectInformation;

            this.Name = projectInfo.Name;

            this.Address = projectInfo.Address;

            this.BuildElements = new SelectedItemCollection(ApplicationSettings.BuildingElementNames);

            this.Sectors = new SelectedItemCollection(ApplicationSettings.SectorNames);

            this.CarbonDataCache = new CarbonDataCache();

            this.ChartColorCache = new ChartColorCache();

            this.FilledRegionCache = new FilledRegionCache(this.ChartColorCache);

            this.TextStyleCache = new TextStyleCache();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
