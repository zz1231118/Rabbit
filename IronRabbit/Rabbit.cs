using IronRabbit.Compiler;
using IronRabbit.Expressions;
using IronRabbit.Builtin;

namespace IronRabbit
{
    public class Rabbit
    {
        private static readonly Dictionary<string, SystemLambdaExpression> systemFunctions = new Dictionary<string, SystemLambdaExpression>();

        static Rabbit()
        {
            Register<AbsLambdaExpression>();
            Register<AcosLambdaExpression>();
            Register<AsinLambdaExpression>();
            Register<AtanLambdaExpression>();
            Register<CeilingLambdaExpression>();
            Register<CoshLambdaExpression>();
            Register<CosLambdaExpression>();
            Register<ExpLambdaExpression>();
            Register<FloorLambdaExpression>();
            Register<LogLambdaExpression>();
            Register<MaxLambdaExpression>();
            Register<MinLambdaExpression>();
            Register<RoundLambdaExpression>();
            Register<SinhLambdaExpression>();
            Register<SinLambdaExpression>();
            Register<SqrtLambdaExpression>();
            Register<TanhLambdaExpression>();
            Register<TanLambdaExpression>();
        }

        internal static bool TryGetSystemLambda(string name, [MaybeNullWhen(false)] out SystemLambdaExpression lambda)
        {
            return systemFunctions.TryGetValue(name, out lambda);
        }

        internal static SystemLambdaExpression? GetSystemLambda(string name)
        {
            systemFunctions.TryGetValue(name, out var lambda);
            return lambda;
        }

        public static void Register<T>()
            where T : SystemLambdaExpression, new()
        {
            var lambda = new T();
            systemFunctions[lambda.Name] = lambda;
        }

        public static void Register(SystemLambdaExpression lambda)
        {
            if (lambda == null) throw new ArgumentNullException(nameof(lambda));

            systemFunctions[lambda.Name] = lambda;
        }

        public static LambdaExpression CompileFromSource(string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            LambdaExpression? lambda = null;
            var reader = SourceReader.From(source);
            var lexer = new Lexer(reader);
            var parser = new Parser(lexer);
            while (true)
            {
                var expression = parser.Parse();
                if (expression == null) break;
                else lambda = expression;
            }

            return lambda ?? throw new CompilerException(reader.Location, "The formula was not found");
        }

        public static LambdaExpression CompileFromFile(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            LambdaExpression? lambda = null;
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var output = new StreamReader(stream, System.Text.Encoding.UTF8);
            var reader = SourceReader.From(output);
            var lexer = new Lexer(reader);
            var parser = new Parser(lexer);
            while (true)
            {
                var expression = parser.Parse();
                if (expression == null) break;
                else lambda = expression;
            }

            return lambda ?? throw new CompilerException(reader.Location, "The formula was not found");
        }
    }
}
