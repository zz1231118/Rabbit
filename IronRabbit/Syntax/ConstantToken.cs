namespace IronRabbit.Syntax
{
    internal class ConstantToken : Token
    {
        private double _value;

        public ConstantToken(double value)
            : base(TokenKind.Constant)
        {
            _value = value;
        }

        public override string Text => _value.ToString();
        public override double Value => _value;
    }
}
