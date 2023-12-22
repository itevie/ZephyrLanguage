namespace ZephyrNew.Runtime.Values
{
    internal partial class Helpers
    {
        public static string VisualiseObject(Dictionary<string, RuntimeValue> values)
        {
            string result = "{";
            string indent = "  ";

            // Check if there is more than one value
            if (values.Count > 0)
            {
                result += "\n";
                indent = "  ";
            }

            // Loop through the properties
            int index = 0;
            foreach (KeyValuePair<string, RuntimeValue> kv in values)
            {
                result += indent + kv.Key;
                result += ": ";
                result += kv.Value.Visualise().Replace("\n", "\n" + indent);

                // Check to add ,
                if (values.Count - 1 != index)
                {
                    result += ",\n";
                }

                index++;
            }

            if (values.Count > 0) result += "\n";
            result += "}";

            return result;
        }
    }
}
