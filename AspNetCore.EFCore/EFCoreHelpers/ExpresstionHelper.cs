using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AspNetCore.EFCore.EFCoreHelpers
{
    public class ExpressionHelper
    {
        /// <summary>
        /// Gets the model name from a lambda expression.
        /// </summary>
        /// 
        /// <returns>
        /// The model name.
        /// </returns>
        /// <param name="expression">The expression.</param>
        public static string GetExpressionText(LambdaExpression expression)
        {
            Stack<string> stack = new Stack<string>();
            Expression expression1 = expression.Body;
            while (expression1 != null)
            {
                if (expression1.NodeType == ExpressionType.Call)
                {
                    
                    MethodCallExpression methodCallExpression = (MethodCallExpression)expression1;
                    if (ExpressionHelper.IsSingleArgumentIndexer((Expression)methodCallExpression))
                    {
                        stack.Push(ExpressionHelper.GetIndexerInvocation(Enumerable.Single<Expression>((IEnumerable<Expression>)methodCallExpression.Arguments), Enumerable.ToArray<ParameterExpression>((IEnumerable<ParameterExpression>)expression.Parameters)));
                        expression1 = methodCallExpression.Object;
                    }
                    else
                        break;
                }
                else if (expression1.NodeType == ExpressionType.ArrayIndex)
                {
                    BinaryExpression binaryExpression = (BinaryExpression)expression1;
                    stack.Push(ExpressionHelper.GetIndexerInvocation(binaryExpression.Right, Enumerable.ToArray((IEnumerable<ParameterExpression>)expression.Parameters)));
                    expression1 = binaryExpression.Left;
                }
                else if (expression1.NodeType == ExpressionType.MemberAccess)
                {
                    MemberExpression memberExpression = (MemberExpression)expression1;
                    stack.Push("." + memberExpression.Member.Name);
                    expression1 = memberExpression.Expression;
                }
                else if (expression1.NodeType == ExpressionType.Parameter)
                {
                    stack.Push(string.Empty);
                    expression1 = (Expression)null;
                }
                else
                    break;
            }
            if (stack.Count > 0 && string.Equals(stack.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
                stack.Pop();
            if (stack.Count <= 0)
                return string.Empty;
            return Enumerable.Aggregate<string>((IEnumerable<string>)stack, (Func<string, string, string>)((left, right) => left + right)).TrimStart(new char[1]
            {
    '.'
            });
        }
    }
}
