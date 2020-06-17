using System.Collections;
using CarbonEmissionTool.Model.Interfaces;
using System.Collections.Generic;

namespace CarbonEmissionTool.Model.Annotations
{
    /// <summary>
    /// Stores a collection of <see cref="IAnnotation"/> objects when a new chart is generated that is used for the placement
    /// of Revit Text Notes. 
    /// </summary>
    public class AnnotationCollection : IEnumerable<IAnnotation>
    {
        /// <summary>
        /// The origin ponits of the text labels.
        /// </summary>
        private List<IAnnotation> Annotations { get; }

        /// <summary>
        /// Constructs a new AnnotationCollection with optional inputs to apply to any <see cref="IAnnotation"/>
        /// added to the collection. 
        /// </summary>
        public AnnotationCollection()
        {
            this.Annotations = new List<IAnnotation>();
        }

        /// <summary>
        /// Adds a <see cref="IAnnotation"/> object to the collection.
        /// </summary>
        public void Add(IAnnotation annotation)
        {
            Annotations.Add(annotation);
        }

        public IEnumerator<IAnnotation> GetEnumerator()
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
