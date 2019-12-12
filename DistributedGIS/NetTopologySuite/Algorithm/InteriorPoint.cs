﻿using NetTopologySuite.Geometries;

namespace NetTopologySuite.Algorithm
{
    /// <summary>
    /// Computes an interior point of a <see cref="Geometry"/>.
    /// An interior point is guaranteed to lie in the interior of the Geometry,
    /// if it possible to calculate such a point exactly.
    /// Otherwise, the point may lie on the boundary of the geometry.
    /// <para>
    /// The interior point of an empty geometry is <code>POINT EMPTY</code>.
    /// </para>
    /// </summary>
    public static class InteriorPoint
    {
        /// <summary>
        /// Compute a location of an interior point in a <see cref="Geometry"/>.
        /// Handles all geometry types.
        /// </summary>
        /// <param name="geom">A geometry in which to find an interior point</param>
        /// <returns>the location of an interior point, or <c>null</c> if the input is empty
        /// </returns>
        public static Coordinate GetInteriorPoint(Geometry geom)
        {
            var factory = geom.Factory;

            if (geom.IsEmpty)
                return null;

            Coordinate interiorPt;
            switch (geom.Dimension)
            {
                case Dimension.Point:
                    interiorPt = InteriorPointPoint.GetInteriorPoint(geom);
                    break;

                case Dimension.Curve:
                    interiorPt = InteriorPointLine.GetInteriorPoint(geom);
                    break;

                default:
                    interiorPt = InteriorPointArea.GetInteriorPoint(geom);
                    break;
            }

            return interiorPt;
        }
    }
}
