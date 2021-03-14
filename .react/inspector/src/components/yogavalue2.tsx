import { ReactUnity as RU, UnityEngine as UE } from '@reactunity/renderer/editor';
import clsx from 'clsx';
import { useRef } from 'react';
import { convertLengthToYoga, convertYogaToLength } from '../helpers';
import type { StylePropComponentProps } from '../props';

const YogaValue2 = ReactUnity.Types.YogaValue2;

export function YogaValue2Field({ className, label, onChange, value }: StylePropComponentProps<RU.Types.YogaValue2>) {
  const x = convertYogaToLength(value.X);
  const y = convertYogaToLength(value.Y);

  const xRef = useRef<RU.Editor.Components.BaseFieldComponent<any, UE.UIElements.StyleLength>>();
  const yRef = useRef<RU.Editor.Components.BaseFieldComponent<any, UE.UIElements.StyleLength>>();

  const changed = () => {
    const xVal = xRef.current.Element.value;
    const yVal = yRef.current.Element.value;
    const val = new YogaValue2(convertLengthToYoga(xVal), convertLengthToYoga(yVal));
    onChange?.({ newValue: val });
  };

  return <view className={clsx(className, 'unity-composite-field', 'unity-base-field')}>
    <text className={clsx('unity-base-field__label', 'unity-composite-field__label')}>{label}</text>

    <view className={clsx('unity-base-field__input', 'unity-composite-field__input')}>
      <length label="X" value={x} onChange={changed} ref={xRef}
        className={clsx('react-unity_field_no-grow', 'react-unity__field__inline', 'unity-composite-field__field', 'unity-composite-field__field--first')} />
      <length label="Y" value={y} onChange={changed} ref={yRef}
        className={clsx('react-unity_field_no-grow', 'react-unity__field__inline', 'unity-composite-field__field')} />
      <view className="unity-composite-field__field-spacer"></view>
    </view>
  </view>;
}
