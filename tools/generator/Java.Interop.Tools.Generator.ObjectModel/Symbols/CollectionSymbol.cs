using System;
using System.Collections.Generic;


namespace MonoDroid.Generation {

	public class CollectionSymbol : ISymbol, IRequireGenericMarshal {
		
		string java_name;
		string managed_name;
		string marshaler;
		GenericParameterList parms;

		public CollectionSymbol (string java_name, string managed_name, string marshaler, string type_params)
		{
			this.java_name = java_name;
			this.managed_name = managed_name;
			this.marshaler = marshaler;
			if (!String.IsNullOrEmpty (type_params))
				parms = new GenericParameterList (type_params);
		}
		
		public string DefaultValue {
			get { return "IntPtr.Zero"; }
		}

		public string FullName {
			get { return parms == null || !parms.IsConcrete ? "Java.Interop." + managed_name : "Java.Interop." + managed_name + parms; }
		}

		public string JavaName {
			get { return java_name; }
		}

		public string JniName {
			get { return "L" + java_name.Replace (".", "/") + ";"; }
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

		public bool MayHaveManagedGenericArguments {
			get { return true; }
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

		public string FromNative (CodeGenerationOptions opt, string varname, bool owned)
		{
			return String.Format ("global::Java.Util.InteroperableArrays.ToLocal{0} (ref {1}, {2})",
					GetManagedTypeName (opt),
					opt.GetSafeIdentifier (varname),
					owned ? "JniObjectReferenceOptions.CopyAndDispose" : "JniObjectReferenceOptions.None");
		}

		string GetManagedTypeName (CodeGenerationOptions opt)
		{
			return opt.GetOutputName (marshaler +
					(parms != null && parms.IsConcrete
						 ? parms.ToString ()
						 : string.Empty));
		}

		public string ToNative (CodeGenerationOptions opt, string varname, Dictionary<string, string> mappings = null)
		{
			return string.Format ("({1}).PeerReference.NewLocalRef().Handle",
					GetManagedTypeName (opt),
					opt.GetSafeIdentifier (varname));
		}

		public bool Validate (CodeGenerationOptions opt, GenericParameterDefinitionList type_params, CodeGeneratorContext context)
		{
			return parms == null || parms.Validate (opt, type_params, context);
		}

		public string[] PreCallback (CodeGenerationOptions opt, string var_name, bool owned)
		{
			return new string[]{
				string.Format ("var {0} = global::Java.Util.InteroperableArrays.ToLocal{1} ({2}, {3});",
						opt.GetSafeIdentifier (var_name),
						GetManagedTypeName (opt),
						opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)),
						owned ? "JniHandleOwnership.TransferLocalRef" : "JniHandleOwnership.DoNotTransfer"),
			};
		}

		public string[] PostCallback (CodeGenerationOptions opt, string var_name)
		{
			return new string[]{
			};
		}

		public string[] PreCall (CodeGenerationOptions opt, string var_name)
		{
			return new string[] {
				string.Format ("var {0} = {2};",
						opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name)),
						GetManagedTypeName (opt),
						opt.GetSafeIdentifier (var_name)),
			};
		}

		public string Call (CodeGenerationOptions opt, string var_name)
		{
			return opt.GetSafeIdentifier (TypeNameUtilities.GetNativeName (var_name));
		}

		public string[] PostCall (CodeGenerationOptions opt, string var_name)
		{
			return new string[]{};
		}

		public bool NeedsPrep { get { return true; } }

		#region IRequireGenericMarshal implementation
		public string GetGenericJavaObjectTypeOverride ()
		{
			return TypeNameUtilities.GetGenericJavaObjectTypeOverride (managed_name, parms != null ? parms.ToString () : null);
		}
		public string ToInteroperableJavaObject (string var_name)
		{
			return TypeNameUtilities.GetNativeName (var_name);
		}
		#endregion
	}
}

