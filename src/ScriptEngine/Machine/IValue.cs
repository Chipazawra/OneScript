﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Runtime.CompilerServices;
using ScriptEngine.Types;

namespace ScriptEngine.Machine
{
    static class ValueAdHocExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRuntimeContextInstance AsObject(this OneScript.Core.IValue value)
        {
            return (IRuntimeContextInstance) value.AsContext();
        }
    }
    
    // public interface IValue : IComparable<IValue>, IEquatable<IValue>
    // {
    //     DataType DataType { get; }
    //     TypeDescriptor SystemType { get; }
    //
    //     decimal AsNumber();
    //     DateTime AsDate();
    //     bool AsBoolean();
    //     string AsString();
    //     IRuntimeContextInstance AsObject();
    //     IValue GetRawValue();
    // }

}