using System;

namespace NetTopologySuite.Geometries.Implementation
{
    /// <summary>
    /// Creates CoordinateSequences represented as an array of Coordinates.
    /// </summary>
    [Serializable]
    public sealed class CoordinateArraySequenceFactory : CoordinateSequenceFactory
    {
        private CoordinateArraySequenceFactory() : base(Ordinates.XYZM) { }

        /// <summary>
        /// Returns the singleton instance of CoordinateArraySequenceFactory.
        /// </summary>
        public static CoordinateArraySequenceFactory Instance { get; } = new CoordinateArraySequenceFactory();

        /// <summary>
        ///  Returns a CoordinateArraySequence based on the given array (the array is not copied).
        /// </summary>
        /// <param name="coordinates">the coordinates, which may not be null nor contain null elements.</param>
        /// <returns></returns>
        public override CoordinateSequence Create(Coordinate[] coordinates)
        {
            return new CoordinateArraySequence(coordinates);
        }

        public override CoordinateSequence Create(CoordinateSequence coordSeq)
        {
            return new CoordinateArraySequence(coordSeq);
        }

        public override CoordinateSequence Create(int size, int dimension, int measures)
        {
            int spatial = dimension - measures;

            if (measures > 1)
            {
                measures = 1; // clip measures
            }

            if (spatial > 3)
            {
                spatial = 3; // clip spatial dimension
                // throw new ArgumentException("spatial dimension must be <= 3");
            }

            if (spatial < 2)
            {
                spatial = 2; // handle bogus dimension
            }

            return new CoordinateArraySequence(size, spatial + measures, measures);
        }
    }
}
