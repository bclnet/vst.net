using System.Runtime.CompilerServices;
using static Jacobi.Vst3.GeoConstants;

namespace Jacobi.Vst3
{
    public static class GeoConstants
    {
        public enum Direction
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest,
            NoDirection,  //same position or center point of a geometry

            NumberOfDirections,
        }

        public enum Orientation
        {
            Horizontal,
            Vertical,

            NumberOfOrientations
        }
    }

    partial class PlugExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Direction ToOpposite(this Direction dir) => dir switch
        {
            Direction.North => Direction.South,
            Direction.NorthEast => Direction.SouthWest,
            Direction.East => Direction.West,
            Direction.SouthEast => Direction.NorthWest,
            Direction.South => Direction.North,
            Direction.SouthWest => Direction.NorthEast,
            Direction.West => Direction.East,
            Direction.NorthWest => Direction.SouthEast,
            Direction.NoDirection => Direction.NoDirection,
            _ => Direction.NumberOfDirections
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Orientation ToOrientation(this Direction dir) => dir switch
        {
            Direction.North => Orientation.Vertical,
            Direction.East => Orientation.Horizontal,
            Direction.South => Orientation.Vertical,
            Direction.West => Orientation.Horizontal,
            _ => Orientation.NumberOfOrientations,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Orientation ToOrthogonalOrientation(this Orientation dir) => dir switch
        {
            Orientation.Vertical => Orientation.Horizontal,
            Orientation.Horizontal => Orientation.Vertical,
            _ => Orientation.NumberOfOrientations,
        };
    }
}
