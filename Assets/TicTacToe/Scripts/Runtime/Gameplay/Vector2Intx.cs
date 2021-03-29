using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace TicTactoe.Gameplay
{
    /// <summary>
    /// Thread-safe alternative to Unity's Vector2Int
    /// </summary>
    [Serializable]
    public struct Vector2Intx : IEquatable<Vector2Intx>, IFormattable
    {
        public int x
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_X; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { m_X = value; }
        }


        public int y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return m_Y; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { m_Y = value; }
        }

        private int m_X;
        private int m_Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Intx(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        // Access the /x/ or /y/ component using [0] or [1] respectively.
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }
        }

        // Returns the length of this vector (RO).
        public float magnitude { get { return (float)System.Math.Sqrt(x * x + y * y); } }

        // Returns the squared length of this vector (RO).
        public int sqrMagnitude { get { return x * x + y * y; } }

        public int lenght { get { return x * y; } }

        // Returns the distance between /a/ and /b/.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2Intx a, Vector2Intx b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;

            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
        }

        // Returns a vector that is made from the smallest components of two vectors.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx Min(Vector2Intx lhs, Vector2Intx rhs) { return new Vector2Intx(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y)); }

        // Returns a vector that is made from the largest components of two vectors.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx Max(Vector2Intx lhs, Vector2Intx rhs) { return new Vector2Intx(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y)); }

        // Multiplies two vectors component-wise.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx Scale(Vector2Intx a, Vector2Intx b) { return new Vector2Intx(a.x * b.x, a.y * b.y); }

        // Multiplies every component of this vector by the same component of /scale/.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Vector2Intx scale) { x *= scale.x; y *= scale.y; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector2Intx min, Vector2Intx max)
        {
            x = Math.Max(min.x, x);
            x = Math.Min(max.x, x);
            y = Math.Max(min.y, y);
            y = Math.Min(max.y, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator -(Vector2Intx v)
        {
            return new Vector2Intx(-v.x, -v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator +(Vector2Intx a, Vector2Intx b)
        {
            return new Vector2Intx(a.x + b.x, a.y + b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator -(Vector2Intx a, Vector2Intx b)
        {
            return new Vector2Intx(a.x - b.x, a.y - b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator *(Vector2Intx a, Vector2Intx b)
        {
            return new Vector2Intx(a.x * b.x, a.y * b.y);
        }

        public static Vector2Intx operator *(int a, Vector2Intx b)
        {
            return new Vector2Intx(a * b.x, a * b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator *(Vector2Intx a, int b)
        {
            return new Vector2Intx(a.x * b, a.y * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Intx operator /(Vector2Intx a, int b)
        {
            return new Vector2Intx(a.x / b, a.y / b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2Intx lhs, Vector2Intx rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2Intx lhs, Vector2Intx rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Vector2Intx)) return false;

            return Equals((Vector2Intx)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2Intx other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        /// *listonly*
        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }

        public static Vector2Intx zero { get { return s_Zero; } }
        public static Vector2Intx one { get { return s_One; } }
        public static Vector2Intx up { get { return s_Up; } }
        public static Vector2Intx down { get { return s_Down; } }
        public static Vector2Intx left { get { return s_Left; } }
        public static Vector2Intx right { get { return s_Right; } }

        private static readonly Vector2Intx s_Zero = new Vector2Intx(0, 0);
        private static readonly Vector2Intx s_One = new Vector2Intx(1, 1);
        private static readonly Vector2Intx s_Up = new Vector2Intx(0, 1);
        private static readonly Vector2Intx s_Down = new Vector2Intx(0, -1);
        private static readonly Vector2Intx s_Left = new Vector2Intx(-1, 0);
        private static readonly Vector2Intx s_Right = new Vector2Intx(1, 0);

    }
}
