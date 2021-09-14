using System.Collections.Generic;
using ReactUnity.Converters;

namespace ReactUnity.Styling.Shorthands
{
    public class AllShorthand : StyleShorthand
    {
        public override List<IStyleProperty> ModifiedProperties { get; } = CssProperties.AllProperties;

        public AllShorthand(string name) : base(name) { }

        public override List<IStyleProperty> Modify(IDictionary<IStyleProperty, object> collection, object value)
        {
            if (value == null) return null;

            var str = value.ToString();
            var splits = ParserHelpers.SplitWhitespace(str);

            if (splits.Count != 1) return null;

            var val = splits[0];
            var converted = AllConverters.GeneralConverter.Convert(val);

            if (converted is CssKeyword k)
            {
                foreach (var item in ModifiedProperties)
                {
                    collection[item] = k;
                }
                return ModifiedProperties;
            }
            return null;
        }
    }
}
