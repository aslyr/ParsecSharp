namespace ParsecSharp.Internal
{
    public readonly struct TextPosition : IPosition
    {
        public static TextPosition Initial => new TextPosition(1, 1);

        public int Line { get; }

        public int Column { get; }

        public TextPosition(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }

        public TextPosition Next(char token)
            => (token == '\n')
                ? new TextPosition(this.Line + 1, 1)
                : new TextPosition(this.Line, this.Column + 1);

        public int CompareTo(IPosition other)
            => (this.Line != other.Line) ? this.Line.CompareTo(other.Line) : this.Column.CompareTo(other.Column);

        public bool Equals(IPosition other)
            => other is TextPosition position && this == position;

        public override bool Equals(object obj)
            => obj is TextPosition position && this == position;

        public override int GetHashCode()
            => this.Line >> 16 ^ this.Line << 16 ^ this.Column;

        public override string ToString()
            => $"Line: {this.Line.ToString()}, Column: {this.Column.ToString()}";

        public static bool operator ==(TextPosition left, TextPosition right)
            => left.Line == right.Line && left.Column == right.Column;

        public static bool operator !=(TextPosition left, TextPosition right)
            => !(left == right);
    }
}
