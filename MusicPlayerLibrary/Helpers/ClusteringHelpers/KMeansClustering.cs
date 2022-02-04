using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicPlayerLibrary.Helpers.ClusteringHelpers
{
    public static class KMeansClustering
    {
        public static IEnumerable<Vector3> ClusterData(IEnumerable<Vector3> baseVectors, IEnumerable<Vector3> vectors)
        {
            ConcurrentBag<Cluster> clusters = new ConcurrentBag<Cluster>(baseVectors.Select(V => new Cluster(V)));
            bool repeat = false;
            do
            {
                repeat = false;
                Parallel.ForEach(vectors, (V) => { AddToNearestCluster(clusters, V); });
                Parallel.ForEach(clusters, (C) => { if (C.UpdateCentroid()) repeat = true; });
            } while (repeat);
            return clusters.OrderByDescending(C => C.Count).Where(C => C.Count > 0).Select(C => C.Centroid);
        }

        private static void AddToNearestCluster(IEnumerable<Cluster> baseClusters, Vector3 vector)
        {
            float minDistance = float.MaxValue;
            float distance;
            Cluster closestCluster = null;
            foreach (Cluster cluster in baseClusters)
            {
                distance = cluster.DistanceFromCentroidSqrt(vector);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCluster = cluster;
                    if (distance == 0) break;
                }
            }
            closestCluster?.AddVector(vector);
        }
    }
}