using System;
using System.Collections.Generic;
using System.Xml;

using MonoDroid.Utils;

namespace MonoDroid.Generation {

	public class StringSymbol : ISymbol {

		public string DefaultValue {
			get { return "IntPtr.Zero"; }
		}

		public string FullName {
			get { return "string"; }
		}

		public bool IsGeneric {
			get { return false; }
		}

		public string JavaName {
			get { return "java.lang.String"; }
		}

		public string JniName {
			get { return "Ljava/lang/String;"; }
		}

		public string NativeType {
			get { return "IntPtr"; }
		}

		public bool IsEnum {
			get { return false; }
		}

		public bool IsArray {
			get { return false; }
		}

		public string ElementType {
			get { return null; }
		}

		public string ReturnCast => string.Empty;

		public string GetObjectHandleProperty (string variable)
		{
			return $"((global::Java.Lang.Object) {variable}).Handle";
		}

		public string GetGenericType (Dictionary<string, string> mappings)
		{
			return null;
		}

		public string FromNative (CodeGenerationOptions opt, string var_name, bool owned)
		{
			return String.Format ("JniEnvironment.Strings.ToString (ref {0}, {1})", var_name, owned ? "JniObjectReferenceOptions.CopyAndDispose" : "JniObjectReferenceOptions.None");
		}

		public string ToNative (CodeGenerationOptions opt, string var_name, Dictionary<string, string> mappings = null)
		{
			return String.Format ("JniEnvironment.Strings.NewString ({0}).Handle", var_name);
		}

		public bool Validate (CodeGenerationOptions opt, GenericParameterDefinitionList type_params, CodeGeneratorContext context)
		{
			return true;
		}

		public string Call (CodeGenerationOptions opt, string var_name)
		{
			return opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name));
		}

		public string[] PostCallback (CodeGenerationOptions opt, string var_name)
		{
			return new string [0];
		}

		public string[] PostCall (CodeGenerationOptions opt, string var_name)
		{
			return new string[]{
				string.Format ("JniObjectReference.Dispose (ref {0}, JniObjectReferenceOptions.CopyAndDispose);", opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name))),
			};
		}

		public string[] PreCallback (CodeGenerationOptions opt, string var_name, bool owned)
		{
			return new string[] { String.Format ("var {0} = JniEnvironment.Strings.ToString ({1});", opt.GetSafeIdentifier (var_name), opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name))) };
		}

		public string[] PreCall (CodeGenerationOptions opt, string var_name)
		{
			return new string[] { String.Format ("JniObjectReference {0} = JniEnvironment.Strings.NewString ({1});", opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)), opt.GetSafeIdentifier (var_name)) };
		}

		public bool NeedsPrep { get { return true; } }
	}
}

