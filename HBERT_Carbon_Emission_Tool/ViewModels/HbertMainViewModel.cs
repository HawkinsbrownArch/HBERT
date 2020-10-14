using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarbonEmissionTool.Settings;

namespace CarbonEmissionTool.ViewModels
{
    /// <summary>
    /// The view model for the main application window which hosts the carbon emission tool pages
    /// which the user interacts with.
    /// </summary>
    class HbertMainViewModel : INotifyPropertyChanged
    {
        private bool _legalAcceptance = true;

        /// <summary>
        /// The legal statement shown to the user under the legal section.
        /// </summary>
        public string LegalStatement { get; }

        /// <summary>
        /// Returns true if the user accepts the legal terms or false if they reject the terms. 
        /// </summary>
        public bool LegalAcceptance
        {
            get => _legalAcceptance;
            set
            {
                _legalAcceptance = value;

                OnPropertyChanged(nameof(LegalAcceptance));
            }
        }

        /// <summary>
        /// Constructs a new <see cref="HbertMainViewModel"/>.
        /// </summary>
        public HbertMainViewModel()
        {
            this.LegalStatement = ApplicationSettings.LegalStatement;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
