using Autodesk.Revit.DB;
using CarbonEmissionTool.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CarbonEmissionTool.ViewModels
{
    class PublishPageViewModel : DataErrorNotifier, IPublishDetails, INotifyPropertyChanged
    {
        private FamilySymbol _titleBlock;
        private View3D _axoView;
        private string _sheetName;
        private string _sheetNumber;
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        /// <summary>
        /// A list of all the 3D views in the active document.
        /// </summary>
        public List<View3D> ThreeDViews { get; }

        /// <summary>
        /// A list of all the title block <see cref="Autodesk.Revit.DB.FamilySymbol"/>'s in the active document.
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

                var propertyName = nameof(TitleBlock);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
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

                var propertyName = nameof(AxoView);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
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

                var propertyName = nameof(SheetName);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
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

                var propertyName = nameof(SheetNumber);
                ValidateInput(propertyName);

                OnPropertyChanged(nameof(CanPublish));
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Returns true if all the inputs from the user are valid otherwise return false.
        /// </summary>
        public bool CanPublish => !string.IsNullOrWhiteSpace(this.SheetNumber) & NameUtils.ValidCharacters(this.SheetNumber) &
                                  !SheetUtils.Exists(this.SheetNumber) &
                                  !string.IsNullOrWhiteSpace(this.SheetName) & NameUtils.ValidCharacters(this.SheetName) &
                                  this.AxoView != null &
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

        #region Data Validation implementation
        public override void ValidateInput(string propertyName)
        {
            var error = string.Empty;
            
            switch (propertyName)
            {
                case nameof(SheetName):
                    error = SheetNameValidation.Validate(this.SheetName);
                    break;

                case nameof(SheetNumber):
                    error = SheetNumberValidation.Validate(this.SheetNumber);
                    break;

                case nameof(TitleBlock):
                    error = ComboBoxSelectedItemValidation.Validate(this.TitleBlock);
                    break;

                case nameof(AxoView):
                    error = ComboBoxSelectedItemValidation.Validate(this.AxoView);
                    break;
            }

            AddError(propertyName, error);
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
