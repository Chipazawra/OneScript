﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/

using OneScript.Core;
using ScriptEngine.Types;

namespace ScriptEngine.Machine.Values
{
    public class TypeTypeValue : GenericValue
    {
        private readonly TypeDescriptor _type;

        public TypeTypeValue(TypeDescriptor type)
        {
            _type = type;
        }

        public override TypeDescriptor SystemType => BasicTypes.Type;

        public override string AsString()
        {
            return _type.ToString();
        }

        public override bool Equals(IValue other)
        {
            if(other?.SystemType == SystemType)
            {
                var otherVal = other.GetRawValue() as TypeTypeValue;
                return otherVal._type == this._type;
            }

            return false;
        }

		public override bool Equals(object obj)
		{
			if (obj is TypeTypeValue value)
				return Equals(value);
			return false;
		}

		public override int GetHashCode()
		{
			return _type.GetHashCode();
		}

        public TypeDescriptor TypeValue => _type;
    }
}
