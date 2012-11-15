using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.SceneInitialization.ShooterSceneInit
{
    /// <summary>
    /// Allow the user to sample points uniformly at random from a rectangular frustum.
    /// </summary>
    public class Frustum
    {
        private double screenDepth;
        private double deltaX, deltaY;
        private double maxDepth;

        public Frustum(double screenWidth, double screenHeight, double screenDepth, double maxDepth)
        {
            this.screenDepth = screenDepth;
            this.deltaX = screenWidth / screenDepth;
            this.deltaY = screenHeight / screenDepth;
            this.maxDepth = maxDepth;
        }

        /// <summary>
        /// samples a single point from the frustum's volume uniformly at random
        /// </summary>
        /// <param name="rand">
        /// The random objectused to sample the point.  Two equal random objects will always result in the same sampled point.
        /// </param>
        /// <returns>
        /// A point sampled uniformly at random from the frustum.
        /// Note that this point is in the frustum's local coordinate space, where:
        /// The apex of the frustum is at the origin,
        /// The frustum extends in the +Z direction,
        /// The x direction corresponds to the width of the frustum, and
        /// The y direction corresponds to the height of the frustum.
        /// </returns>
        public Vector3 samplePoint(Random rand)
        {
            //while no point has been found
            while (true)
            {
                //sample a point from the bounding box of the frustum
                Vector3 point = samplePointFromBoundingBox(rand);

                //compute the size of the bounding rectangle at the selected depth
                double maxX = deltaX * point.Z / 2.0;
                double maxY = deltaY * point.Z / 2.0;
                
                //if it is actually in the frustum
                if (point.X <= maxX
                    && point.X >= -maxX
                    && point.Y <= maxY
                    && point.Y >= -maxY)
                {
                    //return it
                    return point;
                }
            }
        }

        /// <summary>
        /// Selects a point uniformly at random from the frustum's minimal bounding box.
        /// </summary>
        /// <param name="rand">
        /// The random object used to sample the point.  Two equal random objects will always result in the same sampled point.
        /// </param>
        /// <returns></returns>
        private Vector3 samplePointFromBoundingBox(Random rand)
        {
            float x = (float)(rand.NextDouble() * deltaX * maxDepth - deltaX * maxDepth);
            float y = (float)(rand.NextDouble() * deltaY * maxDepth - deltaY * maxDepth);
            float z = (float)(rand.NextDouble() * (maxDepth - screenDepth) + screenDepth);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Transforms the given point in local Euclidean space to the weird local pseudo-spherical space
        /// </summary>
        /// <param name="euclidean"></param>
        /// <returns>
        /// The provided point in pseudo-spherical space.
        /// X: the proportion across the width of the frustum that the point is.  Range: 0 to 1
        /// Y: the proportion across the height of the frustum that the point is  Range: 0 to 1
        /// Z: the depth in the frustum that the point is at (unchanged).  Range: screenDepth to maxDepth
        /// </returns>
        public Vector3 TransformFromEuclideanSpaceToPseudoSphericalSpace(Vector3 euclidean)
        {
            //compute the size of the bounding rectangle at the selected depth
            double width = deltaX * euclidean.Z;
            double height = deltaY * euclidean.Z;

            double x = euclidean.X / width + 0.5;
            double y = euclidean.Y / height + 0.5;

            return new Vector3(2 * (float)x, 2 * (float)y, euclidean.Z);
        }
    }
}
