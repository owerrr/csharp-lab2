using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PolylineLib
{
    public readonly struct P
    {
        public readonly int X;
        public readonly int Y;

        public P(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";

        public static bool operator ==(P left, P right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(P left, P right) => !(left == right);

        public override bool Equals(object? obj) =>
            obj is P p && this == p;

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    public sealed class Polyline : IEquatable<Polyline>, IEnumerable<P>
    {
        private readonly P[] points;

        public Polyline() : this(new P(0, 0), new P(1, 1))
        {
        }

        public Polyline(params P[] points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length < 2)
                throw new ArgumentException("Polyline must have at least two points", nameof(points));

            for (int i = 0; i < points.Length - 2; i++)
            {
                if (AreCollinear(points[i], points[i + 1], points[i + 2]))
                {
                    throw new ArgumentException(
                        $"Points at indices {i}, {i + 1}, {i + 2} are collinear, which is not allowed.");
                }
            }

            this.points = (P[])points.Clone();
        }

        private static bool AreCollinear(P a, P b, P c)
        {
            int cross = (b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X);
            return cross == 0;
        }

        public int Count => points.Length;

        public double Length
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    sum += Distance(points[i], points[i + 1]);
                }
                return sum;
            }
        }

        private static double Distance(P a, P b)
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public IReadOnlyList<P> Points => Array.AsReadOnly(points);

        public override string ToString()
        {
            return string.Join("--", points.Select(p => p.ToString()));
        }

        public bool Equals(Polyline? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (points.Length != other.points.Length)
                return false;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != other.points[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object? obj) =>
            obj is Polyline other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var p in points)
                {
                    hash = hash * 31 + p.GetHashCode();
                }
                return hash;
            }
        }

        public static bool operator ==(Polyline? left, Polyline? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Polyline? left, Polyline? right) => !(left == right);

        public static Polyline operator +(Polyline first, Polyline second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));


            if (first.points[^1] != second.points[0])
                throw new ArgumentException("The end point of the first polyline must be the start point of the second polyline.");

            P[] newPoints = new P[first.points.Length + second.points.Length - 1];
            Array.Copy(first.points, newPoints, first.points.Length);
            Array.Copy(second.points, 1, newPoints, first.points.Length, second.points.Length - 1);

            return new Polyline(newPoints);
        }

        public P this[int index]
        {
            get
            {
                if (index < 0 || index >= points.Length)
                    throw new IndexOutOfRangeException();
                return points[index];
            }
        }

        public static explicit operator P[](Polyline polyline)
        {
            if (polyline == null) throw new ArgumentNullException(nameof(polyline));
            return (P[])polyline.points.Clone();
        }

        public IEnumerator<P> GetEnumerator()
        {
            foreach (var p in points)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Polyline Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            var parts = s.Split(new[] { "--" }, StringSplitOptions.None);
            if (parts.Length < 2)
                throw new FormatException("Polyline must contain at least two points.");

            var parsedPoints = new List<P>(parts.Length);
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (!trimmed.StartsWith("(") || !trimmed.EndsWith(")"))
                    throw new FormatException($"Point '{part}' is not in the format (x,y).");

                string inner = trimmed[1..^1];
                var coords = inner.Split(',');
                if (coords.Length != 2)
                    throw new FormatException($"Point '{part}' does not contain exactly two coordinates.");

                if (!int.TryParse(coords[0], out int x))
                    throw new FormatException($"X coordinate '{coords[0]}' is not a valid integer.");

                if (!int.TryParse(coords[1], out int y))
                    throw new FormatException($"Y coordinate '{coords[1]}' is not a valid integer.");

                parsedPoints.Add(new P(x, y));
            }

            return new Polyline(parsedPoints.ToArray());
        }
    }


}
