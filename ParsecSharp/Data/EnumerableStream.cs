using System;
using System.Collections.Generic;
using System.Linq;
using ParsecSharp.Internal;

namespace ParsecSharp
{
    public sealed class EnumerableStream<TToken> : IParsecStateStream<TToken>
    {
        private const int MaxBufferSize = 1024;

        private readonly IDisposable resource;

        private readonly Buffer<TToken> _buffer;

        private readonly LinearPosition _position;

        private int Index => this._position.Column % MaxBufferSize;

        public TToken Current => this._buffer[this.Index];

        public bool HasValue => this.Index < this._buffer.Count;

        public IPosition Position => this._position;

        public IParsecStateStream<TToken> Next => new EnumerableStream<TToken>(this.resource, (this.Index == MaxBufferSize - 1) ? this._buffer.Next : this._buffer, this._position.Next());

        public EnumerableStream(IEnumerable<TToken> source) : this(source.GetEnumerator())
        { }

        public EnumerableStream(IEnumerator<TToken> enumerator) : this(enumerator, GenerateBuffer(enumerator), LinearPosition.Initial)
        { }

        private EnumerableStream(IDisposable resource, Buffer<TToken> buffer, LinearPosition position)
        {
            this.resource = resource;
            this._buffer = buffer;
            this._position = position;
        }

        private static Buffer<TToken> GenerateBuffer(IEnumerator<TToken> enumerator)
        {
            try
            {
                var buffer = Enumerable.Repeat(enumerator, MaxBufferSize)
                    .TakeWhile(enumerator => enumerator.MoveNext())
                    .Select(enumerator => enumerator.Current)
                    .ToArray();
                return new Buffer<TToken>(buffer, () => GenerateBuffer(enumerator));
            }
            catch
            {
                enumerator.Dispose();
                throw;
            }
        }

        public void Dispose()
            => this.resource.Dispose();

        public bool Equals(IParsecState<TToken> other)
            => other is EnumerableStream<TToken> state && this._buffer == state._buffer && this._position == state._position;

        public sealed override bool Equals(object obj)
            => obj is EnumerableStream<TToken> state && this._buffer == state._buffer && this._position == state._position;

        public sealed override int GetHashCode()
            => this._buffer.GetHashCode() ^ this._position.GetHashCode();

        public sealed override string ToString()
            => (this.HasValue)
                ? this.Current?.ToString() ?? string.Empty
                : "<EndOfStream>";
    }
}
