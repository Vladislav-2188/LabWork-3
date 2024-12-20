using System.Diagnostics;

namespace CoordinateSystemsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nCoordinate Systems Laboratory Work");
                Console.WriteLine("1. Coordinate System Transformations (2D and 3D)");
                Console.WriteLine("2. Distance Calculations in Different Coordinate Systems");
                Console.WriteLine("3. Performance Benchmarking of Coordinate Systems");
                Console.WriteLine("4. Exit");
                Console.Write("\nSelect an option (1-4): ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            RunCoordinateTransformations();
                            break;
                        case 2:
                            RunDistanceCalculations();
                            break;
                        case 3:
                            RunPerformanceBenchmark();
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        // Helper functions
        static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        // Coordinate transformation functions
        static (double x, double y) PolarToCartesian(double radius, double angle)
        {
            double x = radius * Math.Cos(DegreesToRadians(angle));
            double y = radius * Math.Sin(DegreesToRadians(angle));
            return (x, y);
        }

        static (double radius, double angle) CartesianToPolar(double x, double y)
        {
            double radius = Math.Sqrt(x * x + y * y);
            double angle = RadiansToDegrees(Math.Atan2(y, x));
            if (angle < 0) angle += 360;
            return (radius, angle);
        }

        static (double x, double y, double z) SphericalToCartesian(double radius, double azimuth, double zenith)
        {
            double x = radius * Math.Sin(DegreesToRadians(zenith)) * Math.Cos(DegreesToRadians(azimuth));
            double y = radius * Math.Sin(DegreesToRadians(zenith)) * Math.Sin(DegreesToRadians(azimuth));
            double z = radius * Math.Cos(DegreesToRadians(zenith));
            return (x, y, z);
        }

        static (double radius, double azimuth, double zenith) CartesianToSpherical(double x, double y, double z)
        {
            double radius = Math.Sqrt(x * x + y * y + z * z);
            double azimuth = RadiansToDegrees(Math.Atan2(y, x));
            double zenith = RadiansToDegrees(Math.Acos(z / radius));

            if (azimuth < 0) azimuth += 360;
            return (radius, azimuth, zenith);
        }

        // Distance calculation functions
        static double DistanceCartesian2D(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        static double DistanceCartesian3D(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
        }

        static double DistancePolar(double r1, double theta1, double r2, double theta2)
        {
            return Math.Sqrt(Math.Pow(r1, 2) + Math.Pow(r2, 2) -
                   2 * r1 * r2 * Math.Cos(DegreesToRadians(theta2 - theta1)));
        }

        static double DistanceSphericalVolume(double r1, double theta1, double phi1, double r2, double theta2, double phi2)
        {
            double theta1Rad = DegreesToRadians(theta1);
            double theta2Rad = DegreesToRadians(theta2);
            double phi1Rad = DegreesToRadians(phi1);
            double phi2Rad = DegreesToRadians(phi2);

            return Math.Sqrt(
                Math.Pow(r1, 2) + Math.Pow(r2, 2) -
                2 * r1 * r2 * (
                    Math.Sin(theta1Rad) * Math.Sin(theta2Rad) * Math.Cos(phi1Rad - phi2Rad) +
                    Math.Cos(theta1Rad) * Math.Cos(theta2Rad)
                )
            );
        }

        static double DistanceSphericalGreatCircle(double r1, double theta1, double phi1, double r2, double theta2, double phi2)
        {
            double theta1Rad = DegreesToRadians(theta1);
            double theta2Rad = DegreesToRadians(theta2);
            double phi1Rad = DegreesToRadians(phi1);
            double phi2Rad = DegreesToRadians(phi2);

            // Using the average radius for the great circle calculation
            double avgRadius = (r1 + r2) / 2;

            return avgRadius * Math.Acos(
                Math.Sin(phi1Rad) * Math.Sin(phi2Rad) +
                Math.Cos(phi1Rad) * Math.Cos(phi2Rad) * Math.Cos(theta1Rad - theta2Rad)
            );
        }

        // Random point generation
        static List<(double radius, double azimuth, double zenith)> GeneratePoints(int count)
        {
            var points = new List<(double radius, double azimuth, double zenith)>();
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                points.Add((
                    radius: rand.NextDouble() * 10,
                    azimuth: rand.NextDouble() * 360,
                    zenith: rand.NextDouble() * 180
                ));
            }

            return points;
        }

        // Main functionality methods
        static void RunCoordinateTransformations()
        {
            Console.WriteLine("\nCoordinate System Transformations");
            Console.WriteLine("1. 2D Space (Polar-Cartesian)");
            Console.WriteLine("2. 3D Space (Spherical-Cartesian)");
            Console.Write("Select space dimension (1-2): ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.Write("\nEnter number of points to transform: ");
                if (!int.TryParse(Console.ReadLine(), out int count) || count < 1)
                {
                    Console.WriteLine("Invalid input. Using default value of 5.");
                    count = 5;
                }

                var points = GeneratePoints(count);

                if (choice == 1)
                {
                    Console.WriteLine("\n2D Coordinate Transformations:");
                    foreach (var point in points)
                    {
                        Console.WriteLine("\nOriginal Polar Coordinates:");
                        Console.WriteLine($"r = {point.radius:F2}, theta = {point.azimuth:F2}°");

                        var (x, y) = PolarToCartesian(point.radius, point.azimuth);
                        Console.WriteLine("Transformed to Cartesian Coordinates:");
                        Console.WriteLine($"x = {x:F2}, y = {y:F2}");

                        var (r2, theta2) = CartesianToPolar(x, y);
                        Console.WriteLine("Back to Polar Coordinates:");
                        Console.WriteLine($"r = {r2:F2}, theta = {theta2:F2}°");
                    }
                }
                else if (choice == 2)
                {
                    Console.WriteLine("\n3D Coordinate Transformations:");
                    foreach (var point in points)
                    {
                        Console.WriteLine("\nOriginal Spherical Coordinates:");
                        Console.WriteLine($"r = {point.radius:F2}, theta = {point.azimuth:F2}°, phi = {point.zenith:F2}°");

                        var (x, y, z) = SphericalToCartesian(point.radius, point.azimuth, point.zenith);
                        Console.WriteLine("Transformed to Cartesian Coordinates:");
                        Console.WriteLine($"x = {x:F2}, y = {y:F2}, z = {z:F2}");

                        var (r2, theta2, phi2) = CartesianToSpherical(x, y, z);
                        Console.WriteLine("Back to Spherical Coordinates:");
                        Console.WriteLine($"r = {r2:F2}, theta = {theta2:F2}°, phi = {phi2:F2}°");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid dimension choice.");
                }
            }
        }

        static void RunDistanceCalculations()
        {
            Console.WriteLine("\nDistance Calculations in Different Coordinate Systems");
            Console.Write("Enter number of point pairs to analyze: ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count < 1)
            {
                Console.WriteLine("Invalid input. Using default value of 3.");
                count = 3;
            }

            var points = GeneratePoints(count * 2);

            for (int i = 0; i < points.Count; i += 2)
            {
                var point1 = points[i];
                var point2 = points[i + 1];

                Console.WriteLine($"\nPoint Pair {i / 2 + 1}:");
                Console.WriteLine($"Point 1: (r={point1.radius:F2}, theta={point1.azimuth:F2}°, phi={point1.zenith:F2}°)");
                Console.WriteLine($"Point 2: (r={point2.radius:F2}, theta={point2.azimuth:F2}°, phi={point2.zenith:F2}°)");

                // 2D calculations
                var (x1, y1) = PolarToCartesian(point1.radius, point1.azimuth);
                var (x2, y2) = PolarToCartesian(point2.radius, point2.azimuth);
                Console.WriteLine("\nDistance Calculations:");
                Console.WriteLine($"2D Cartesian distance: {DistanceCartesian2D(x1, y1, x2, y2):F2}");
                Console.WriteLine($"Polar distance: {DistancePolar(point1.radius, point1.azimuth, point2.radius, point2.azimuth):F2}");

                // 3D calculations
                var (x1_3d, y1_3d, z1) = SphericalToCartesian(point1.radius, point1.azimuth, point1.zenith);
                var (x2_3d, y2_3d, z2) = SphericalToCartesian(point2.radius, point2.azimuth, point2.zenith);
                Console.WriteLine($"3D Cartesian distance: {DistanceCartesian3D(x1_3d, y1_3d, z1, x2_3d, y2_3d, z2):F2}");
                Console.WriteLine($"Spherical volume distance: {DistanceSphericalVolume(point1.radius, point1.azimuth, point1.zenith, point2.radius, point2.azimuth, point2.zenith):F2}");
                Console.WriteLine($"Great circle distance: {DistanceSphericalGreatCircle(point1.radius, point1.azimuth, point1.zenith, point2.radius, point2.azimuth, point2.zenith):F2}");
            }
        }

        static void RunPerformanceBenchmark()
        {
            Console.WriteLine("\n=== Performance Benchmarking of Coordinate Systems ===");
            Console.Write("Enter sample size for benchmarking: ");

            if (!int.TryParse(Console.ReadLine(), out int count) || count < 1)
            {
                Console.WriteLine("Invalid input. Using default value of 10000.");
                count = 10000;
            }

            var points = GeneratePoints(count);
            var stopwatch = new Stopwatch();

            // Define all 5 measurements
            var measurements = new List<(string name, Action action)>
    {
        ("1. 2D Distance in Cartesian Coordinates", () =>
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                var (x1, y1) = PolarToCartesian(points[i].radius, points[i].azimuth);
                var (x2, y2) = PolarToCartesian(points[i + 1].radius, points[i + 1].azimuth);
                DistanceCartesian2D(x1, y1, x2, y2);
            }
        }),

        ("2. Distance in Polar Coordinates", () =>
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                DistancePolar(
                    points[i].radius, points[i].azimuth,
                    points[i + 1].radius, points[i + 1].azimuth
                );
            }
        }),

        ("3. 3D Distance in Cartesian Coordinates", () =>
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                var (x1, y1, z1) = SphericalToCartesian(points[i].radius, points[i].azimuth, points[i].zenith);
                var (x2, y2, z2) = SphericalToCartesian(points[i + 1].radius, points[i + 1].azimuth, points[i + 1].zenith);
                DistanceCartesian3D(x1, y1, z1, x2, y2, z2);
            }
        }),

        ("4. Spherical Volume Distance", () =>
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                DistanceSphericalVolume(
                    points[i].radius, points[i].azimuth, points[i].zenith,
                    points[i + 1].radius, points[i + 1].azimuth, points[i + 1].zenith
                );
            }
        }),

        ("5. Great Circle Distance", () =>
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                DistanceSphericalGreatCircle(
                    points[i].radius, points[i].azimuth, points[i].zenith,
                    points[i + 1].radius, points[i + 1].azimuth, points[i + 1].zenith
                );
            }
        })
    };

            Console.WriteLine($"\nRunning benchmark with {count} points...\n");

            var results = new Dictionary<string, long>();

            // Run measurements
            foreach (var (name, action) in measurements)
            {
                stopwatch.Restart();
                action();
                stopwatch.Stop();
                results[name] = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"{name}: {stopwatch.ElapsedMilliseconds} ms");
            }

            // Find fastest and slowest methods
            var fastestMethod = results.MinBy(r => r.Value);
            var slowestMethod = results.MaxBy(r => r.Value);

            Console.WriteLine("\nPerformance Summary:");
            Console.WriteLine($"Fastest method: {fastestMethod.Key} ({fastestMethod.Value} ms)");
            Console.WriteLine($"Slowest method: {slowestMethod.Key} ({slowestMethod.Value} ms)");
            Console.WriteLine($"Time difference: {slowestMethod.Value - fastestMethod.Value} ms");
        }
    }
}
