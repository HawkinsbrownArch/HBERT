using System.Collections;
using System.Collections.Generic;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// A collection of <see cref="CheckBoxItem"/>'s which are binded to the view with
    /// check box controls.
    /// </summary>
    public class CheckBoxItemCollection : IEnumerable<CheckBoxItem>
    {
        private List<CheckBoxItem> ItemList { get; }

        /// <summary>
        /// Constructs a new <see cref="CheckBoxItemCollection"/> from the <paramref name="itemNames"/>.
        /// </summary>
        public CheckBoxItemCollection(List<string> itemNames)
        {
            this.ItemList = new List<CheckBoxItem>();

            foreach (var name in itemNames)
            {
                var selectedItem = new CheckBoxItem(name);

                this.ItemList.Add(selectedItem);
            }
        }

        public IEnumerator<CheckBoxItem> GetEnumerator()
        {
            foreach (var item in ItemList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
