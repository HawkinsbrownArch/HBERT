using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Handler for user input data errors. Implement in view models requiring the <see cref="INotifyDataErrorInfo"/>.
    /// </summary>
    public abstract class DataErrorNotifier : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public virtual IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        public bool HasErrors => _errorsByPropertyName.Any();
        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string error)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);

                OnErrorsChanged(propertyName);
            }

            _errorsByPropertyName[propertyName] = new List<string> { error };

            OnErrorsChanged(propertyName);
        }

        public abstract void ValidateInput(string propertyName);
    }
}
