using System;
using System.Collections.Generic;
using System.Xml;

using MonoDroid.Utils;

namespace MonoDroid.Generation {

	public class ArraySymbol : ISymbol {

		static ISymbol byte_sym = new SimpleSymbol ("0", "byte", "byte", "B");

		ISymbol sym;
		bool is_params;

		public ArraySymbol (ISymbol sym)
		{
			if (sym.FullName == "sbyte")
				this.sym = byte_sym;
			else
				this.sym = sym;
		}

		public string DefaultValue {
			get { return "IntPtr.Zero"; }
		}

		public string ElementType {
			get {
				return sym.FullName;
			}
		}

		public string FullName {
			get { return (is_params ? "params " : String.Empty) + ElementType + "[]"; }
		}

		public bool IsGeneric {
			get { return !string.IsNullOrEmpty (sym.GetGenericType (null)); }
		}

		public bool IsParams {
			get { return is_params; }
			set { is_params = value; }
		}

		public string JavaName {
			get { return is_params ? sym.JavaName + "..." : sym.JavaName + "[]"; }
		}

		public string JniName {
			get { return "[" + sym.JniName; }
		}

		public string NativeType {
			get { return "IntPtr"; }
		}

		public bool IsEnum {
			get { return false; }
		}

		public bool IsArray {
			get { return true; }
		}

		public string ReturnCast => string.Empty;

		public string GetObjectHandleProperty (string variable)
		{
			return sym.GetObjectHandleProperty (variable);
		}

		public string GetGenericType (Dictionary<string, string> mappings)
		{
			return null;
		}

		public string FromNative (CodeGenerationOptions opt, string var_name, bool owned)
		{
			return String.Format ("({0}[]{4}) global::Java.Util.InteroperableArrays.GetArray<{3}> (ref {1}, {2})", opt.GetOutputName (ElementType), var_name, owned ? "JniObjectReferenceOptions.CopyAndDispose" : "JniObjectReferenceOptions.None", opt.GetOutputName (sym.FullName), opt.NullableOperator);
		}

		public string ToNative (CodeGenerationOptions opt, string var_name, Dictionary<string, string> mappings = null)
		{
			return String.Format ("global::Java.Util.InteroperableArrays.NewArray ({0}).PeerReference.NewLocalRef().Handle", var_name);
		}

		public bool Validate (CodeGenerationOptions opt, GenericParameterDefinitionList type_params, CodeGeneratorContext context)
		{
			return sym.Validate (opt, type_params, context);
		}

		public string Call (CodeGenerationOptions opt, string var_name)
		{
			return opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name));
		}

		public string[] PostCallback (CodeGenerationOptions opt, string var_name)
		{
			string[] result = new string [2];
			result [0] = String.Format ("if ({0} != null)", opt.GetSafeIdentifier (var_name));
			result [1] = String.Format ("\tglobal::Java.Util.InteroperableArrays.ArrayCopy({0}, {1});", opt.GetSafeIdentifier (var_name), opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)));
			return result;
		}

		public string[] PostCall (CodeGenerationOptions opt, string var_name)
		{
			string native_name = opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name));
			string[] result = new string [2];
			result [0] = String.Format ("if ({0} != null)", opt.GetSafeIdentifier (var_name));
			result [1] = String.Format ("\t{0}.CopyTo({1}, 0);", native_name, opt.GetSafeIdentifier (var_name));
			return result;
		}

		public string[] PreCallback (CodeGenerationOptions opt, string var_name, bool owned)
		{
			return new string[] { String.Format ("var {1} = ({0}[]{4}) global::Java.Util.InteroperableArrays.GetArray<{3}> ({2}, JniHandleOwnership.DoNotTransfer);", opt.GetOutputName (ElementType), opt.GetSafeIdentifier (var_name), opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)), opt.GetOutputName (sym.FullName), opt.NullableOperator) };
		}

		public string[] PreCall (CodeGenerationOptions opt, string var_name)
		{
			return new string[] { String.Format ("var {0} = global::Java.Util.InteroperableArrays.NewArray ({1});", opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)), opt.GetSafeIdentifier (var_name)) };
		}

		public bool NeedsPrep { get { return true; } }
	}
}

