using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ParsecSharp.Internal;

namespace ParsecSharp
{
    public static partial class Parser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> Any<TToken>()
            => Satisfy<TToken>(_ => true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> Token<TToken>(TToken token)
            => Satisfy<TToken>(x => EqualityComparer<TToken>.Default.Equals(x, token));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> Token<TToken>(TToken token, IEqualityComparer<TToken> comparer)
            => Satisfy<TToken>(x => comparer.Equals(x, token));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> Token<TToken>(TToken token, Func<TToken, TToken, bool> comparer)
            => Satisfy<TToken>(x => comparer(x, token));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, Unit> EndOfInput<TToken>()
            => Not(Any<TToken>()).WithMessage(fail => $"Expected <EndOfStream> but was '{fail.State.ToString()}'");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, Unit> Null<TToken>()
            => Pure<TToken, Unit>(Unit.Instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> OneOf<TToken>(IEnumerable<TToken> candidates)
            => Satisfy<TToken>(candidates.Contains);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> OneOf<TToken>(params TToken[] candidates)
            => OneOf(candidates.AsEnumerable());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> NoneOf<TToken>(IEnumerable<TToken> candidates)
            => Satisfy<TToken>(x => !candidates.Contains(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> NoneOf<TToken>(params TToken[] candidates)
            => NoneOf(candidates.AsEnumerable());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, IEnumerable<TToken>> Take<TToken>(int count)
            => Builder.Create<TToken, IEnumerable<TToken>>(state =>
                (state.AsEnumerable().Take(count).ToArray() is var result && result.Length == count)
                    ? Result.Success(result.AsEnumerable(), state.Advance(count))
                    : Result.Fail<TToken, IEnumerable<TToken>>($"At {nameof(Take)} -> An input does not have enough length", state));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, IEnumerable<TToken>> TakeWhile<TToken>(Func<TToken, bool> predicate)
            => Many(Satisfy(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, IEnumerable<TToken>> TakeWhile1<TToken>(Func<TToken, bool> predicate)
            => Many1(Satisfy(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, Unit> Skip<TToken>(int count)
            => Builder.Create<TToken, Unit>(state => (state.AsEnumerable().Take(count).Count() == count)
                ? Result.Success(Unit.Instance, state.Advance(count))
                : Result.Fail<TToken, Unit>($"At {nameof(Skip)} -> An input does not have enough length", state));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, Unit> SkipWhile<TToken>(Func<TToken, bool> predicate)
            => SkipMany(Satisfy(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, Unit> SkipWhile1<TToken>(Func<TToken, bool> predicate)
            => SkipMany1(Satisfy(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Parser<TToken, TToken> Satisfy<TToken>(Func<TToken, bool> predicate)
            => Builder.Create<TToken, TToken>(state => (state.HasValue && predicate(state.Current))
                ? Result.Success(state.Current, state.Next)
                : Result.Fail<TToken, TToken>(state));
    }
}
