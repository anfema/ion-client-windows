using Anfema.Amp.Parsing;
using System;
using System.Text.RegularExpressions;

namespace Anfema.Amp.DataModel
{
    public class KeyValueItem
    {
        private string _name;
        private ValueType _type;
        private Object _value;

        public KeyValueItem(KeyValuePairRaw kvp)
        {
            _type = getValueType(kvp.value);
            _name = kvp.name;
            _value = kvp.value;
        }
        
        public string name
        {
            get { return _name; }
        }

        public ValueType type
        {
            get { return _type; }
        }
        
        public Object value
        {
            get { return _value; }
        }
        
        // Trys to detect the value type of the given string. This is not the ideal, but there is actually no other way to get this question solved
        private ValueType getValueType(string property)
        {
            // Try to read false or true to detect bool types. This can cause a issue, when the value matches exact this string but not encoding a bool type
            if (property.ToLower().Equals("false") || property.ToLower().Equals("true"))
            {
                return ValueType.BOOL;
            }

            // Regex to detect if the whole string consits only of number relevant characters
            if (Regex.IsMatch(property, @"^\d+$"))
            {
                return ValueType.NUMBER;
            }

            // If none of the other cases matches then it must be a string value
            return ValueType.STRING;
        }
    }
}