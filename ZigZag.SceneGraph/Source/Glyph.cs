using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    readonly struct Glyph
    {
        public Glyph(char character, float advance, Rectangle planeBounds, Rectangle atlasBounds)
        {
            Character = character;
            Advance = advance;
            PlaneBounds = planeBounds;
            AtlasBounds = atlasBounds;
        }

        public readonly char Character
        {
            get;
        }

        public readonly float Advance
        {
            get;
        }

        public readonly Rectangle PlaneBounds
        {
            get;
        }

        public readonly Rectangle AtlasBounds
        {
            get;
        }
    }
}
