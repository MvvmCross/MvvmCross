namespace Phone7.Fx.IO.Compression
{
    internal class Match
    {
        internal MatchState State { get; set; }

        internal int Position { get; set; }

        internal int Length { get; set; }

        internal byte Symbol { get; set; }
    }
}