using System.Collections;
using System.Collections.Generic;

namespace CarbonEmissionTool.Models
{
    /// <summary>
    /// Stores a collection of <see cref="IHeading"/> objects when a new chart is generated that is used for the placement
    /// of Revit Text Notes. 
    /// </summary>
    public class HeadingCollection : IEnumerable<IHeading>
    {
        /// <summary>
        /// The origin ponits of the text labels.
        /// </summary>
        private List<IHeading> Annotations { get; }

        /// <summary>
        /// Constructs a new HeadingCollection with optional inputs to apply to any <see cref="IHeading"/>
        /// added to the collection. 
        /// </summary>
        public HeadingCollection()
        {
            this.Annotations = new List<IHeading>();
        }

        /// <summary>
        /// Adds a <see cref="IHeading"/> object to the collection.
        /// </summary>
        public void Add(IHeading heading)
        {
            Annotations.Add(heading);
        }

        public IEnumerator<IHeading> GetEnumerator()
        {
            foreach (var annotation in Annotations)
            {
                yield return annotation;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
