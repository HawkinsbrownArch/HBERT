using CarbonEmissionTool.Model.Items;
using System.Collections;
using System.Collections.Generic;

namespace CarbonEmissionTool.Model.Collections
{
    /// <summary>
    /// A collection of <see cref="SelectedItem"/>'s which are binded to the view with
    /// check box controls.
    /// </summary>
    public class SelectedItemCollection : IEnumerable<SelectedItem>
    {
        private List<SelectedItem> ItemList { get; }

        /// <summary>
        /// Constructs a new <see cref="SelectedItemCollection"/> from the <paramref name="itemNames"/>.
        /// </summary>
        public SelectedItemCollection(List<string> itemNames)
        {
            this.ItemList = new List<SelectedItem>();

            foreach (var name in itemNames)
            {
                var selectedItem = new SelectedItem(name);

                this.ItemList.Add(selectedItem);
            }
        }

        public IEnumerator<SelectedItem> GetEnumerator()
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
