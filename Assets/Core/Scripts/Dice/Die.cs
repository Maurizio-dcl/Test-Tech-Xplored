namespace TestTechXplored.Core.Scripts.Dice
{
    public readonly struct Die
    {
        public static readonly Die D2 = new(2);
        public static readonly Die D3 = new(3);
        public static readonly Die D4 = new(4);
        public static readonly Die D6 = new(6);
        public static readonly Die D8 = new(8);
        public static readonly Die D10 = new(10);
        public static readonly Die D12 = new(12);
        public static readonly Die D20 = new(20);

        public int Faces { get; }

        private Die(int faces)
        {
            Faces = faces;
        }
    }
}