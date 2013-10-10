namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class Apple
    {
        public int Size { get; set; }
        public SurfaceFinish Skin { get; set; }
        public decimal Weight { get; set; }
        public Color Color { get; set; }

        public override string ToString()
        {
            return string.Format("Size: {0}, Skin: {1}, Weight: {2}, Color: {3}, Hardness: {4}", Size, Skin, Weight, Color, Hardness);
        }

        public int Hardness { get; set; }
    }

    public enum Color
    {
        Bright,
        Middle,
        Dark
    }
    public enum SurfaceFinish
    {
        Smooth,
        Middle,
        Rough
    }

}