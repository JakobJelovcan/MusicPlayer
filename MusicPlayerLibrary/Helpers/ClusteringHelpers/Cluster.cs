using ExtensionsLibrary.Extensions;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;

namespace MusicPlayerLibrary.Helpers.ClusteringHelpers
{
    public class Cluster
    {
        public Vector3 Centroid;
        private readonly ConcurrentStack<Vector3> Vectors;
        public int Count;

        public float DistanceFromCentroidSqrt(Vector3 vector3)
        {
            return Vector3.DistanceSquared(vector3, Centroid);
        }

        public bool UpdateCentroid()
        {
            if (!Vectors.Any()) return false;
            Vector3 oldCentroid = Centroid;
            Centroid = Vectors.Average();
            Count = Vectors.Count;
            Vectors.Clear();
            return DistanceFromCentroidSqrt(oldCentroid) > 0;
        }

        public void AddVector(Vector3 vector)
        {
            Vectors.Push(vector);
        }

        public Cluster(Vector3 vector)
        {
            Centroid = vector;
            Vectors = new ConcurrentStack<Vector3>();
            Count = 0;
        }
    }
}
