namespace ParsecSharp.Internal
{
    public sealed class EmptyPosition<TToken> : IPosition<TToken, EmptyPosition<TToken>>
    {
        public static EmptyPosition<TToken> Initial { get; } = new EmptyPosition<TToken>();

        public int Line => 0;

        public int Column => -1;

        private EmptyPosition()
        { }

        public EmptyPosition<TToken> Next(TToken token)
            => this;

        public int CompareTo(IPosition other)
            => (this.Equals(other)) ? 0 : -1;

        public bool Equals(IPosition other)
            => ReferenceEquals(this, other);

        public sealed override bool Equals(object? obj)
            => ReferenceEquals(this, obj);

        public sealed override int GetHashCode()
            => 0;

        public sealed override string ToString()
            => "Position: none";
    }
}
