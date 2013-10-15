namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class Apple
    {
        public int ID { get; set; }
        public int Size { get; set; }
        public SurfaceFinish Skin { get; set; }
        public Color Color { get; set; }
        public int Hardness { get; set; }

        public override string ToString()
        {
            return string.Format( "大小: {0}, 果皮: {1}, 色泽: {2}, 硬度: {3}, 编号({4})", Size, Skin, Color, Hardness,ID);
        }
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