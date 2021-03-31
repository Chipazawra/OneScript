﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Globalization;
using OneScript.Core;
using ScriptEngine.Machine.Values;
using ScriptEngine.Types;

namespace ScriptEngine.Machine
{
    public static class ValueFactory
    {
        public static IValue Create()
        {
            return UndefinedValue.Instance;
        }

        public static IValue Create(string value)
        {
            return StringValue.Create(value);
        }

        public static IValue Create(bool value)
        {
            return value ? BooleanValue.True : BooleanValue.False;
        }

        public static IValue Create(decimal value)
        {
            return NumberValue.Create(value);
        }

        public static IValue Create(int value)
        {
            return NumberValue.Create(value);
        }

        public static IValue Create(DateTime value)
        {
            return new DateValue(value);
        }

        public static IValue CreateInvalidValueMarker()
        {
            return InvalidValue.Instance;
        }

        public static IValue CreateNullValue()
        {
            return NullValue.Instance;
        }

        public static IValue Create(IRuntimeContextInstance instance)
        {
            return (IValue)instance;
        }

        public static IValue Parse(string presentation, DataType type)
        {
            IValue result;
            switch (type)
            {
                case DataType.Boolean:

                    if (String.Compare(presentation, "истина", StringComparison.OrdinalIgnoreCase) == 0 
                        || String.Compare(presentation, "true", StringComparison.OrdinalIgnoreCase) == 0 
                        || String.Compare(presentation, "да", StringComparison.OrdinalIgnoreCase) == 0)
                        result = ValueFactory.Create(true);
                    else if (String.Compare(presentation, "ложь", StringComparison.OrdinalIgnoreCase) == 0 
                             || String.Compare(presentation, "false", StringComparison.OrdinalIgnoreCase) == 0
                             || String.Compare(presentation, "нет", StringComparison.OrdinalIgnoreCase) == 0)
                        result = ValueFactory.Create(false);
                    else
                        throw RuntimeException.ConvertToBooleanException();

                    break;
                case DataType.Date:
                    string format;
                    if (presentation.Length == 14)
                        format = "yyyyMMddHHmmss";
                    else if (presentation.Length == 8)
                        format = "yyyyMMdd";
                    else if (presentation.Length == 12)
                        format = "yyyyMMddHHmm";
                    else
                        throw RuntimeException.ConvertToDateException();

                    if (presentation == "00000000"
                     || presentation == "000000000000"
                     || presentation == "00000000000000")
                    {
                        result = ValueFactory.Create(new DateTime());
                    }
                    else
                        try
                        {
                            result = ValueFactory.Create(DateTime.ParseExact(presentation, format, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        catch (FormatException)
                        {
                            throw RuntimeException.ConvertToDateException();
                        }

                    break;
                case DataType.Number:
                    var numInfo = NumberFormatInfo.InvariantInfo;
                    var numStyle = NumberStyles.AllowDecimalPoint
                                |NumberStyles.AllowLeadingSign
                                |NumberStyles.AllowLeadingWhite
                                |NumberStyles.AllowTrailingWhite;

                    try
                    {
                        result = ValueFactory.Create(Decimal.Parse(presentation, numStyle, numInfo));
                    }
                    catch (FormatException)
                    {
                        throw RuntimeException.ConvertToNumberException();
                    }
                    break;
                case DataType.String:
                    result = ValueFactory.Create(presentation);
                    break;
                case DataType.Undefined:
                    result = ValueFactory.Create();
                    break;
                case DataType.GenericValue:
                    if (string.Compare(presentation, "null", StringComparison.OrdinalIgnoreCase) == 0)
                        result = ValueFactory.CreateNullValue();
                    else
                        throw new NotSupportedException("constant type is not supported");

                    break;
                default:
                    throw new NotSupportedException("constant type is not supported");
            }

            return result;
        }

        private class InvalidValue : IValue
        {
            private static IValue _instance = new InvalidValue();

            internal static IValue Instance => _instance;

            #region IValue Members

            public DataType DataType => DataType.NotAValidValue;

            public TypeDescriptor SystemType => throw new NotImplementedException();

            public decimal AsNumber()
            {
                throw new NotImplementedException();
            }

            public DateTime AsDate()
            {
                throw new NotImplementedException();
            }

            public bool AsBoolean()
            {
                throw new NotImplementedException();
            }

            public string AsString()
            {
                throw new NotImplementedException();
            }

            public IContext AsContext()
            {
                throw new NotImplementedException();
            }

            public IValue GetRawValue()
            {
                return this;
            }

            #endregion

            #region IComparable<IValue> Members

            public int CompareTo(IValue other)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IEquatable<IValue> Members

            public bool Equals(IValue other)
            {
                return ReferenceEquals(other, this);
            }

            #endregion
        }


        public static IValue Add(IValue op1, IValue op2)
        {
            var type1 = op1.SystemType;

            if (type1 == BasicTypes.String)
            {
                return Create(op1.AsString() + op2.AsString());
            }

            if (type1 == BasicTypes.Date && op2.SystemType == BasicTypes.Number)
            {
                var date = op1.AsDate();
                return Create(date.AddSeconds((double)op2.AsNumber()));
            }

            // все к числовому типу.
            return Create(op1.AsNumber() + op2.AsNumber());
        }

        public static IValue Sub(IValue op1, IValue op2)
        {
            if (op1.SystemType == BasicTypes.Number)
            {
                return Create(op1.AsNumber() - op2.AsNumber());
            }
            if (op1.SystemType == BasicTypes.Date && op2.SystemType == BasicTypes.Number)
            {
                var date = op1.AsDate();
                var result = date.AddSeconds(-(double)op2.AsNumber());
                return Create(result);
            }
            if (op1.SystemType == BasicTypes.Date && op2.SystemType == BasicTypes.Date)
            {
                var span = op1.AsDate() - op2.AsDate();
                return Create((decimal)span.TotalSeconds);
            }

            // все к числовому типу.
            return Create(op1.AsNumber() - op2.AsNumber());
        }

        public static IValue Mul(IValue op1, IValue op2)
        {
            return Create(op1.AsNumber() * op2.AsNumber());
        }

        public static IValue Div(IValue op1, IValue op2)
        {
            if (op2.AsNumber() == 0)
            {
                throw RuntimeException.DivideByZero();
            }
            return Create(op1.AsNumber() / op2.AsNumber());
        }

        public static IValue Mod(IValue op1, IValue op2)
        {
            if (op2.AsNumber() == 0)
            {
                throw RuntimeException.DivideByZero();
            }
            return Create(op1.AsNumber() % op2.AsNumber());
        }

        public static IValue Neg(IValue op1)
        {
            return Create(op1.AsNumber() * -1);
        }
    }
}
