namespace IronRabbit.Syntax
{
    internal class ConstantToken : Token
    {
        private decimal _value;

        public ConstantToken(decimal value)
            : base(TokenKind.Constant)
        {
            _value = value;
        }

        public override string Text => _value.ToString();
        public override decimal Value => _value;
    }
}
