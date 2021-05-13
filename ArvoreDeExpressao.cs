using System;
using System.IO;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace DJOUZE
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            ParameterExpression result = Expression.Parameter(typeof(bool), "result");
            
            LabelTarget returnTarget = Expression.Label(typeof(bool));
            LabelExpression returnLabel = Expression.Label(returnTarget, Expression.Constant(true));
            GotoExpression returnExpression = Expression.Return(returnTarget,
             result, typeof(bool));

            BlockExpression block = Expression.Block(
                new[] { result },
                Expression.Assign(result, Expression.Constant(true)),
                Expression.IfThenElse(
                    Expression.Equal(Expression.Modulo(value, Expression.Constant(2)), Expression.Constant(0)),
                    Expression.Assign(result,Expression.Constant(true)),
                    Expression.Assign(result, Expression.Constant(false))
                ),
                returnExpression,returnLabel
            );
            int num = int.Parse(Console.ReadLine());
            bool isPar = Expression.Lambda<Func<int, bool>>(block, value).Compile()(num);
            Console.WriteLine(isPar);
            Console.ReadKey();
        }
    }
}
