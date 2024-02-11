#if !REACT_DISABLE_JINT && REACT_JINT_AVAILABLE
#define REACT_JINT
#endif

#if REACT_JINT
using System;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Runtime.Interop;
using ReactUnity.Helpers;

namespace ReactUnity.Scripting
{
    public class JintTypeConverter : DefaultTypeConverter
    {
        private ReactContext Context;

        public JintTypeConverter(ReactContext context, Engine engine) : base(engine)
        {
            Context = context;
        }


        public override bool TryConvert(object value, Type type, IFormatProvider formatProvider, out object converted)
        {
            if (type == typeof(Callback) || type == typeof(object))
            {
                if (value is Func<JsValue, JsValue[], JsValue> cb)
                {
                    converted = Callback.From(cb, Context);
                    return true;
                }
                if (value is Function oi)
                {
                    converted = Callback.From(oi, Context);
                    return true;
                }
            }

            return base.TryConvert(value, type, formatProvider, out converted);
        }

        public override object Convert(object value, Type type, IFormatProvider formatProvider)
        {
            if (type == typeof(object))
            {
                if (value is ObjectWrapper ow) return ow.Target;
                if (value is JsString js) return js.ToString();
            }

            return base.Convert(value, type, formatProvider);
        }
    }
}
#endif
