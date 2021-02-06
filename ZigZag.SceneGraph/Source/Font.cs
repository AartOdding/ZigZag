using System.Collections.Generic;


namespace ZigZag.SceneGraph
{
    readonly struct Font
    {
        public Font(int fontID, Dictionary<char, Glyph> glyphs)
        {
            FontID = fontID;
            m_glyphs = new Dictionary<char, Glyph>(glyphs); // Makes deep copy.
        }

        public readonly int FontID
        {
            get;
        }

        public readonly IReadOnlyDictionary<char, Glyph> Glyphs
        {
            get
            {
                return m_glyphs;
            }
        }

        private readonly Dictionary<char, Glyph> m_glyphs;
    }
}
