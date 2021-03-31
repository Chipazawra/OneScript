/*----------------------------------------------------------
This Source Code Form is subject to the terms of the
Mozilla Public License, v.2.0. If a copy of the MPL
was not distributed with this file, You can obtain one
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using OneScript.Core;
using OneScript.Language.SyntaxAnalysis.AstNodes;
using ScriptEngine.Compiler;
using ScriptEngine.Machine;
using ScriptEngine.Types;

namespace OneScript.StandardLibrary.Native
{
    public class BinaryOperationCompiler
    {
        private ExpressionType _opCode;

        public Expression Compile(BinaryOperationNode node, Expression left, Expression right)
        {
            _opCode = ExpressionHelpers.TokenToOperationCode(node.Operation);
            
            if (IsIValue(left.Type))
            {
                return CompileDynamicOperation(left, right);
            }
            else
            {
                return CompileStaticOperation(left, right);
            }
        }

        private Expression CompileStaticOperation(Expression left, Expression right)
        {
            if (IsNumeric(left.Type))
            {
                return MakeNumericOperation(left, right);
            }
            
            if (left.Type == typeof(DateTime))
            {
                return DateOperation(left, right);
            }
            
            if (left.Type == typeof(string))
            {
                if (_opCode != ExpressionType.Add)
                {
                    throw new CompilerException($"Operator {_opCode} is not defined for strings");
                }
                
                return StringAddition(left, right);
            }
            
            if (left.Type == typeof(bool))
            {
                return MakeLogicalOperation(left, right);
            }
            
            throw new CompilerException($"Operation {_opCode} is not defined for {left.Type} and {right.Type}");
        }

        private Expression MakeNumericOperation(Expression left, Expression right)
        {
            if (IsNumeric(right.Type))
            {
                return Expression.MakeBinary(_opCode, left, right);
            }
            if(IsIValue(right.Type))
            {
                return Expression.MakeBinary(_opCode, left, ExpressionHelpers.ToNumber(right));
            }

            throw new CompilerException($"Operation {_opCode} is not defined for Number and {right.Type}");
        }

        private Expression DateOperation(Expression left, Expression right)
        {
            if (IsNumeric(right.Type))
            {
                return DateOffsetOperation(left, right);
            }

            if (_opCode == ExpressionType.Subtract && right.Type == typeof(DateTime))
            {
                return DateDiffExpression(left, right);
            }
            else if (IsIValue(right.Type))
            {
                var propType = Expression.Property(right, nameof(IValue.SystemType));
                var isDate = Expression.Equal(propType, Expression.Constant(BasicTypes.Date));
                
                if(_opCode == ExpressionType.Subtract)
                    return Expression.Condition(isDate, DateDiffExpression(left, ExpressionHelpers.ToDate(right)),
                        DateOffsetOperation(left, ExpressionHelpers.ToNumber(right)));
                else
                    return DateOffsetOperation(left, ExpressionHelpers.ToNumber(right));

            }
            
            throw new CompilerException($"Operation {_opCode} is not defined for dates");
        }
        
        private Expression DateDiffExpression(Expression left, Expression right)
        {
            var spanExpr = Expression.Subtract(left, ExpressionHelpers.ToDate(right));
            var totalSeconds = Expression.Property(spanExpr, nameof(TimeSpan.TotalSeconds));
            var decimalSeconds = Expression.Convert(totalSeconds, typeof(decimal));

            return decimalSeconds;
        }
        
        private Expression DateOffsetOperation(Expression left, Expression right)
        {
            var adder = typeof(DateTime).GetMethod(nameof(DateTime.AddSeconds));
            Debug.Assert(adder != null);
            
            var toDouble = Expression.Convert(right, typeof(double));
            Expression arg;
            if (_opCode == ExpressionType.Add)
                arg = toDouble;
            else if (_opCode == ExpressionType.Subtract)
                arg = Expression.Negate(toDouble);
            else
            {
                throw new CompilerException($"Operation {_opCode} is not defined for dates");
            }

            return Expression.Call(left, adder, arg);
        }

        private Expression MakeLogicalOperation(Expression left, Expression right)
        {
            if (IsIValue(right.Type))
            {
                return Expression.MakeBinary(_opCode, left, ExpressionHelpers.ToBoolean(right));
            }
            else
            {
                return Expression.MakeBinary(_opCode, left, Expression.Convert(right, typeof(bool)));
            }
        }
        
        private Expression StringAddition(Expression left, Expression right)
        {
            if (IsIValue(right.Type))
            {
                return Expression.Add(left, ExpressionHelpers.ToString(right));
            }
            else
            {
                return Expression.Add(left, right);
            }
        }
        
        private Expression CompileDynamicOperation(Expression left, Expression right)
        {
            switch (_opCode)
            {
                case ExpressionType.Add:
                    return MakeDynamicAddition(left, right);
                case ExpressionType.Subtract:
                    return MakeDynamicSubtraction(left, right);
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    return MakeNumericOperation(
                        ExpressionHelpers.ToNumber(left),
                        ExpressionHelpers.ToNumber(right));
                default:
                    throw new CompilerException($"Operation {_opCode} is not defined for IValues");
            }
        }

        private Expression MakeDynamicSubtraction(Expression left, Expression right)
        {
            var typeOfLeft = ExpressionHelpers.GetIValueSystemType(left);
            var throwException = CreateConversionException(typeOfLeft);

            return Expression.IfThenElse(Expression.Equal(typeOfLeft, Expression.Constant(BasicTypes.Number)), 
                MakeNumericOperation(ExpressionHelpers.ToNumber(left), right), 
                Expression.IfThenElse(Expression.Equal(typeOfLeft, Expression.Constant(BasicTypes.Date)), 
                    DateOperation(ExpressionHelpers.ToDate(left), right),
                    throwException));
        }

        private UnaryExpression CreateConversionException(Expression typeOfLeft)
        {
            var exceptionConstructor = typeof(CompilerException).GetConstructor(new[] {typeof(string)});
            Debug.Assert(exceptionConstructor != null);

            var message = Expression.Add(
                Expression.Constant($"Operation {_opCode} is not defined for type "),
                typeOfLeft);

            var createException = Expression.New(exceptionConstructor, message);
            var throwException = Expression.Throw(createException);
            return throwException;
        }

        private Expression MakeDynamicAddition(Expression left, Expression right)
        {
            var typeOfLeft = ExpressionHelpers.GetIValueClrType(left);
            var exception = CreateConversionException(typeOfLeft);

            return Expression.IfThenElse(Expression.Equal(typeOfLeft, Expression.Constant(BasicTypes.String)),
                StringAddition(ExpressionHelpers.ToString(left), right),
                Expression.IfThenElse(Expression.Equal(typeOfLeft, Expression.Constant(BasicTypes.Date)),
                    DateOperation(left, right),
                    Expression.IfThenElse(Expression.Equal(typeOfLeft, Expression.Constant(BasicTypes.Number)),
                        MakeNumericOperation(ExpressionHelpers.ToNumber(left), right),
                        exception)));
        }

        private static bool IsNumeric(Type type) => type.IsNumeric();
        
        private static bool IsIValue(Type type) => type.IsIValue();
    }
}