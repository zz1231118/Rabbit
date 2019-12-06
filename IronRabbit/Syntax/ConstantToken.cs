namespace IronRabbit.Syntax
{
    internal class ConstantToken : Token
    {
        private double value;

        public ConstantToken(double value)
            : base(TokenKind.Constant)
        {
            this.value = value;
        }

        public override string Text => value.ToString();
        public override double Value => value;
    }
}
