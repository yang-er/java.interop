using System;
using System.Runtime.InteropServices;

namespace Java.Interop {

	public sealed class JniStaticFieldInfo : JniFieldInfo
	{
		internal JniStaticFieldInfo (IntPtr fieldID)
			: base (fieldID)
		{
		}

		public JniObjectReference GetObjectValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticObjectField (@class, this);
		}

		public bool GetBooleanValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticBooleanField (@class, this);
		}

		public sbyte GetByteValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticByteField (@class, this);
		}

		public char GetCharacterValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticCharField (@class, this);
		}

		public short GetInt16Value (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticShortField (@class, this);
		}

		public int GetInt32Value (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticIntField (@class, this);
		}

		public long GetInt64Value (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticLongField (@class, this);
		}

		public float GetSingleValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticFloatField (@class, this);
		}

		public double GetDoubleValue (JniObjectReference @class)
		{
			return JniEnvironment.StaticFields.GetStaticDoubleField (@class, this);
		}

		public void SetValue (JniObjectReference @class, JniObjectReference value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, bool value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, sbyte value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, char value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, short value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, int value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, long value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, float value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}

		public void SetValue (JniObjectReference @class, double value)
		{
			JniEnvironment.StaticFields.SetStaticField (@class, this, value);
		}
	}
}
