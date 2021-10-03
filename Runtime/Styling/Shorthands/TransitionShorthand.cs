using System.Collections.Generic;
using ReactUnity.Animations;
using ReactUnity.Converters;
using ReactUnity.Types;

namespace ReactUnity.Styling.Shorthands
{
    public class TransitionShorthand : StyleShorthand
    {
        public override List<IStyleProperty> ModifiedProperties { get; } = new List<IStyleProperty>
        {
            StyleProperties.transitionProperty,
            StyleProperties.transitionDuration,
            StyleProperties.transitionTimingFunction,
            StyleProperties.transitionDelay,
            StyleProperties.transitionPlayState,
        };

        public TransitionShorthand(string name) : base(name) { }

        public override List<IStyleProperty> Modify(IDictionary<IStyleProperty, object> collection, object value)
        {
            if (base.Modify(collection, value) != null) return ModifiedProperties;

            var commas = ParserHelpers.SplitComma(value?.ToString());
            var count = commas.Count;
            var names = new TransitionProperty[count];
            var states = new AnimationPlayState[count];
            var durations = new float[count];
            var easings = new TimingFunction[count];
            var delays = new float[count];

            for (int ci = 0; ci < count; ci++)
            {
                var comma = commas[ci];
                var splits = ParserHelpers.SplitWhitespace(comma);

                if (splits.Count == 0) return null;

                var durationSet = false;
                var delaySet = false;
                var playStateSet = false;
                var nameSet = false;
                var timingSet = false;

                for (int i = 0; i < splits.Count; i++)
                {
                    var split = splits[i];

                    var dur = AllConverters.DurationConverter.Convert(split);

                    if (dur is float f)
                    {
                        if (!durationSet)
                        {
                            durations[ci] = f;
                            durationSet = true;
                        }
                        else if (!delaySet)
                        {
                            delays[ci] = f;
                            delaySet = true;
                        }
                        else
                        {
                            return null;
                        }
                        continue;
                    }

                    var ps = !playStateSet ? AllConverters.Get<AnimationPlayState>().Convert(split) : null;

                    if (ps is AnimationPlayState psd)
                    {
                        states[ci] = psd;
                        playStateSet = true;
                        continue;
                    }

                    var tm = !timingSet ? AllConverters.TimingFunctionConverter.Convert(split) : null;

                    if (tm is TimingFunction tmf)
                    {
                        easings[ci] = tmf;
                        timingSet = true;
                        continue;
                    }


                    if (!nameSet)
                    {
                        var nm = AllConverters.TransitionPropertyConverter.Convert(split);

                        if (nm is TransitionProperty tp)
                        {
                            names[ci] = tp;
                            nameSet = true;
                        }
                        else return null;
                        continue;
                    }
                    else return null;
                }

                if (!nameSet) names[ci] = TransitionProperty.All;
            }

            collection[StyleProperties.transitionProperty] = new CssValueList<TransitionProperty>(names);
            collection[StyleProperties.transitionDuration] = new CssValueList<float>(durations);
            collection[StyleProperties.transitionTimingFunction] = new CssValueList<TimingFunction>(easings);
            collection[StyleProperties.transitionDelay] = new CssValueList<float>(delays);
            collection[StyleProperties.transitionPlayState] = new CssValueList<AnimationPlayState>(states);

            return ModifiedProperties;
        }
    }
}