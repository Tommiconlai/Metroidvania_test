namespace DoubleSide.Flip
{
    /// <summary>
    /// I due lati dello stesso volume fisico del livello.
    /// SideA = facciata pubblica, SideB = retro funzionale (vedi GDD §2).
    /// </summary>
    public enum FlipSide
    {
        SideA = 0,
        SideB = 1
    }

    public static class FlipSideExtensions
    {
        /// <summary>Ritorna il lato opposto.</summary>
        public static FlipSide Opposite(this FlipSide side)
            => side == FlipSide.SideA ? FlipSide.SideB : FlipSide.SideA;
    }
}
