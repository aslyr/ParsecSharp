using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ParsecSharp.Internal
{
    public sealed class Buffer<TToken> : IReadOnlyList<TToken>
    {
        private readonly TToken[] _buffer;

        private readonly Lazy<Buffer<TToken>> _next;

        public TToken this[int index] => this._buffer[index];

        public int Count => this._buffer.Length;

        public Buffer<TToken> Next => this._next.Value;

        public Buffer(TToken[] buffer, Func<Buffer<TToken>> next)
        {
            this._buffer = buffer;
            this._next = new Lazy<Buffer<TToken>>(next, false);
        }

        public IEnumerator<TToken> GetEnumerator()
            => this._buffer.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this._buffer.GetEnumerator();
    }
}
