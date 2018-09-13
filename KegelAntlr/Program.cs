using System;
using System.Globalization;
using System.IO;
using Antlr4.Runtime;

namespace KegelAntlr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = Parser("PI - TaccaTuccaLand $ 1* 3 -87");

            var context = parser.calculation();

            var visitor = new Visitor();
            var result = visitor.VisitCalculation(context);

            Console.WriteLine(result);
        }

        private static KegelnParser Parser(string input)
        {
            var stringReader = new StringReader(input);

            using (stringReader)
            {
                var stream = new AntlrInputStream(stringReader);
                var lexer = new KegelnLexer(stream);

                var tokenStream = new CommonTokenStream(lexer);

                return new KegelnParser(tokenStream);
            }
        }
    }

    internal class Visitor : KegelnBaseVisitor<double>
    {
        public override double VisitCalculation(KegelnParser.CalculationContext context)
        {
            return VisitExpression(context.expression());
        }

        public override double VisitExpression(KegelnParser.ExpressionContext context)
        {
            var value = VisitMultiplyingExpression(context.multiplyingExpression(0));
            var ops = context.add();
            for (var i = 0; i < ops.Length; i++)
            {
                var op = ops[i];
                var second = VisitMultiplyingExpression(context.multiplyingExpression(i + 1));

                if (op.PLUS() != null)
                {
                    value += second;
                }

                if (op.MINUS() != null)
                {
                    value -= second;
                }
            }

            return value;
        }

        public override double VisitMultiplyingExpression(KegelnParser.MultiplyingExpressionContext context)
        {
            var value = VisitAtom(context.atom(0));
            var ops = context.mult();
            for (var i = 0; i < ops.Length; i++)
            {
                var op = ops[i];
                var second = VisitAtom(context.atom(i + 1));

                if (op.TIMES() != null)
                {
                    value *= second;
                }

                if (op.DIV() != null)
                {
                    value /= second;
                }
            }

            return value;
        }

        public override double VisitAtom(KegelnParser.AtomContext context)
        {
            if (context.Number() != null)
            {
                return double.Parse(context.Number().GetText(), CultureInfo.InvariantCulture);
            }

            if (context.taccTuccaLand() != null)
            {
                return VisitTaccTuccaLand(context.taccTuccaLand());
            }

            return Math.PI;
        }

        public override double VisitTaccTuccaLand(KegelnParser.TaccTuccaLandContext context)
        {
            return VisitExpression(context.expression()) * 2 + 4;
        }
    }
}