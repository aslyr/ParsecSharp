using System;
using System.Collections.Generic;
using ParsecSharp.Internal;

namespace ParsecSharp
{
    public sealed class ArrayStream<TToken> : IParsecState<TToken, ArrayStream<TToken>>
    {
        private readonly IReadOnlyList<TToken> _source;

        private readonly LinearPosition _position;

        public TToken Current => this._source[this._position.Column];

        public bool HasValue => this._position.Column < this._source.Count;

        public IPosition Position => this._position;

        public IDisposable? InnerResource => default;

        public ArrayStream<TToken> Next => new ArrayStream<TToken>(this._source, this._position.Next());

        public ArrayStream(IReadOnlyList<TToken> source) : this(source, LinearPosition.Initial)
        { }

        private ArrayStream(IReadOnlyList<TToken> source, LinearPosition position)
        {
            this._source = source;
            this._position = position;
        }

        public IParsecState<TToken> GetState()
            => this;

        public void Dispose()
        { }

        public bool Equals(ArrayStream<TToken> other)
            => this._source == other._source && this._position == other._position;

        public sealed override bool Equals(object? obj)
            => obj is ArrayStream<TToken> state && this._source == state._source && this._position == state._position;

        public sealed override int GetHashCode()
            => this._source.GetHashCode() ^ this._position.GetHashCode();

        public sealed override string ToString()
            => (this.HasValue)
                ? this.Current?.ToString() ?? string.Empty
                : "<EndOfStream>";
    }
}
