using Autodesk.Revit.DB;
using CarbonEmissionTool.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CarbonEmissionTool.ViewModels
{
    class PublishPageViewModel : IPublishDetails, INotifyPropertyChanged
    {
        private FamilySymbol _titleBlock;
        private View3D _axoView;
        private string _sheetName;
        private string _sheetNumber;
        private bool _canPublish;

        /// <summary>
        /// A list of all the 3D views in the active document.
        /// </summary>
        public List<View3D> ThreeDViews { get; }

        /// <summary>
        /// A list of all the title block <see cref="FamilySymbol"/>'s in the active document.
        /// </summary>
        public List<FamilySymbol> TitleBlocks { get; }

        /// <summary>
        /// The title block selected by the user for creating the sheet to present the embodied carbon result.
        /// </summary>
        public FamilySymbol TitleBlock
        {
            get => _titleBlock;
            set
            {
                _titleBlock = value;

                OnPropertyChanged(nameof(CanPublish));
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

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(AxoView));
            }
        }

        public ICommand PublishData { get; }

        /// <summary>
        /// The sheet name input by the user for placing the charts once HBERT is run.
        /// </summary>
        public string SheetName 
        { 
            get => _sheetName;
            set
            {
                _sheetName = value;

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(SheetName));
            }
        }

        /// <summary>
        /// The sheet number input by the user for placing the charts once HBERT is run.
        /// </summary>
        public string SheetNumber 
        { 
            get => _sheetNumber;
            set
            {
                _sheetNumber = value;

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(nameof(SheetNumber));
            }
        }

        /// <summary>
        /// The sheet number input by the user for placing the charts once HBERT is run.
        /// </summary>
        public bool CanPublish => !string.IsNullOrWhiteSpace(this.SheetNumber) &
                                  !string.IsNullOrWhiteSpace(this.SheetName) & this.AxoView != null &
                                  this.TitleBlock != null;

        /// <summary>
        /// Constructs a new <see cref="PublishPageViewModel"/>.
        /// </summary>
        public PublishPageViewModel()
        {
            this.ThreeDViews = RevitViewFilter.Get3DViews();

            this.TitleBlocks = TitleBlockFilter.GetAll();

            this.PublishData = new RunHbertCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
