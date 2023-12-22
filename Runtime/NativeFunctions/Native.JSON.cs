using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZephyrNew.Runtime.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZephyrNew.Runtime.NativeFunctions
{
    internal partial class Native
    {
        public static Package JsonPackage = new Package("JSON", new Values.ObjectValue(new
        {
            parse = new NativeFunction((options) =>
            {
                string contents = ((StringValue)options.Arguments[0]).Value;

                JObject? obj;
                
                try
                {
                    obj = JObject.Parse(contents);
                } catch (Exception e)
                {
                    throw new RuntimeException($"Internal JSON error: {e.Message}", options.Location);
                }

                if (obj == null)
                    throw new RuntimeException($"Failed to parse JSON string: deserialiser returned null", options.Location);

                return Helpers.JObjectToZephyrObj(obj);
            }, new Options("parse", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.String), "contents"),
            })),

            stringify = new NativeFunction((options) =>
            {
                ObjectValue contents = (ObjectValue)options.Arguments[0];

                return new StringValue(Helpers.StringifyZephyrObject(contents));
            }, new Options("parse", new List<Parameter>()
            {
                new Parameter(new VariableType(Values.ValueType.Object), "contents"),
            }))
        }));
    }
}
