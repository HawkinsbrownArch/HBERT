using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarbonEmissionTool.Annotations;

namespace CarbonEmissionTool.Model.Items
{
    /// <summary>
    /// Represents the data backing a check box control in the UI window which can be selected by the user.
    /// </summary>
    public class SelectedItem : INotifyPropertyChanged
    {
        private bool _isSelected = false;

        /// <summary>
        /// The name of this <see cref="SelectedItem"/>. Also displayed in the checkbox label in the UI.
        /// </summary>
        public string Name { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        /// <summary>
        /// Constructs a new <see cref="SelectedItem"/>.
        /// </summary>
        public SelectedItem(string name)
        {
            this.Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
