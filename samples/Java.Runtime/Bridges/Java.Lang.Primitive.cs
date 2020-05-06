using System;

namespace Java.Lang
{
    partial class Boolean : IConvertible
    {
        TypeCode IConvertible.GetTypeCode() => TypeCode.Boolean;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Value);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(Value);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private bool Value => BooleanValue();

        public static implicit operator Boolean(bool value) => new Boolean(value);

        public static explicit operator bool(Boolean value) => value.BooleanValue();
    }

    partial class Character : IConvertible
    {
        TypeCode IConvertible.GetTypeCode() => TypeCode.Char;
        bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Value);
        byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(Value);
        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Value);
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Value);
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value);
        double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Value);
        short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(Value);
        int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(Value);
        long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(Value);
        sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(Value);
        float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Value);
        string IConvertible.ToString(IFormatProvider provider) => Convert.ToString(Value);
        ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value);
        uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value);
        ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value);
        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private char Value => CharValue();

        public static implicit operator Character(char value) => new Character(value);

        public static explicit operator char(Character value) => value.Value;
    }

    partial class Number : IConvertible
    {
        public virtual TypeCode GetTypeCode() => throw new NotImplementedException();

        bool IConvertible.ToBoolean(IFormatProvider provider) => throw new InvalidCastException();
        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();
        public virtual object ToType(Type conversionType, IFormatProvider provider) => throw new InvalidCastException();

        char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(ShortValue());
        decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(DoubleValue());
        double IConvertible.ToDouble(IFormatProvider provider) => DoubleValue();
        sbyte IConvertible.ToSByte(IFormatProvider provider) => ByteValue();
        byte IConvertible.ToByte(IFormatProvider provider) => (byte)ByteValue();
        short IConvertible.ToInt16(IFormatProvider provider) => ShortValue();
        ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort)ShortValue();
        int IConvertible.ToInt32(IFormatProvider provider) => IntValue();
        uint IConvertible.ToUInt32(IFormatProvider provider) => (uint)IntValue();
        long IConvertible.ToInt64(IFormatProvider provider) => LongValue();
        ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)LongValue();
        float IConvertible.ToSingle(IFormatProvider provider) => FloatValue();
        string IConvertible.ToString(IFormatProvider provider) => ToString();
    }

    partial class Long : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Int64;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private long Value => LongValue();

        public static implicit operator Long(long value) => new Long(value);

        public static explicit operator long(Long value) => value.Value;

        public static implicit operator Long(ulong value) => new Long((long)value);

        public static explicit operator ulong(Long value) => (ulong)value.Value;
    }

    partial class Integer : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Int32;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private int Value => IntValue();

        public static implicit operator Integer(int value) => new Integer(value);

        public static explicit operator int(Integer value) => value.Value;

        public static implicit operator Integer(ulong value) => new Integer((int)value);

        public static explicit operator uint(Integer value) => (uint)value.Value;
    }
    
    partial class Short : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Int16;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private short Value => ShortValue();

        public static implicit operator Short(short value) => new Short(value);

        public static explicit operator short(Short value) => value.Value;

        public static implicit operator Short(ushort value) => new Short((short)value);

        public static explicit operator ushort(Short value) => (ushort)value.Value;
    }

    partial class Byte : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Int16;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private sbyte Value => ByteValue();

        public static implicit operator Byte(sbyte value) => new Byte(value);

        public static explicit operator sbyte(Byte value) => value.Value;

        public static implicit operator Byte(byte value) => new Byte((sbyte)value);

        public static explicit operator byte(Byte value) => (byte)value.Value;
    }

    partial class Double : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Double;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private double Value => DoubleValue();

        public static implicit operator Double(double value) => new Double(value);

        public static explicit operator double(Double value) => value.Value;
    }

    partial class Float : IConvertible
    {
        public override TypeCode GetTypeCode() => TypeCode.Single;
        public override object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Value, conversionType, provider);

        private float Value => FloatValue();

        public static implicit operator Float(float value) => new Float(value);

        public static explicit operator float(Float value) => value.Value;
    }
}
