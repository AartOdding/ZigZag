using System;
using System.Collections.Generic;
using System.Collections.Immutable;


namespace ZigZag.SceneGraph
{
    static class TrigTable
    {
        public static ImmutableArray<float> Sin(int numSegments)
        {
            if (!m_sines.ContainsKey(numSegments))
            {
                var values = ImmutableArray.CreateBuilder<float>(numSegments);
                var deltaAngle = MathF.Tau / numSegments;

                for (int i = 0; i < numSegments; ++i)
                {
                    values.Add(MathF.Sin(i * deltaAngle));
                }

                m_sines.Add(numSegments, values.MoveToImmutable());
            }
            return m_sines[numSegments];
        }

        public static ImmutableArray<float> Cos(int numSegments)
        {
            if (!m_cosines.ContainsKey(numSegments))
            {
                var values = ImmutableArray.CreateBuilder<float>(numSegments);
                var deltaAngle = MathF.Tau / numSegments;

                for (int i = 0; i < numSegments; ++i)
                {
                    values.Add(MathF.Cos(i * deltaAngle));
                }

                m_cosines.Add(numSegments, values.MoveToImmutable());
            }
            return m_cosines[numSegments];
        }

        static readonly Dictionary<int, ImmutableArray<float>> m_sines = new Dictionary<int, ImmutableArray<float>>();
        static readonly Dictionary<int, ImmutableArray<float>> m_cosines = new Dictionary<int, ImmutableArray<float>>();
    }
}
