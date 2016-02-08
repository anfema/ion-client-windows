using Anfema.Amp.Parsing;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace Anfema.Amp.DataModel
{
    public class KeyValueItem
    {
        public string name { get; set; }
        public ValueType type { get; set; }
        public Object value { get; set; }


        public KeyValueItem(KeyValuePairRaw kvp)
        {
            type = getValueType(kvp.value);
            name = kvp.name;
            value = kvp.value;
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