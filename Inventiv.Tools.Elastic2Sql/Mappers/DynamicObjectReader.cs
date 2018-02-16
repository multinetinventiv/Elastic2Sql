using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Inventiv.Tools.Elastic2Sql.Mappers
{
	public class DynamicObjectReader
	{
		public static object GetPropertyValue(object objectValue, string member, Type type)
		{
			if (objectValue == null) { throw new ArgumentNullException(nameof(objectValue)); }
			if (member == null) { throw new ArgumentNullException(nameof(member)); }

			var scope = objectValue.GetType();
			var provider = objectValue as IDynamicMetaObjectProvider;

			object value = null;

			if (provider != null)
			{
				value = GetPropertyValueByProvider(objectValue, member, provider, scope);
			}
			else
			{
				var propertyInfo = objectValue.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance);
				if (propertyInfo != null)
				{
					value = propertyInfo.GetValue(objectValue, null);
				}
			}

			try
			{
				return value == null ? null : Convert.ChangeType(value, type);
			}
			catch (Exception)
			{
				return value;
			}
		}

		private static object GetPropertyValueByProvider(object o, string member, IDynamicMetaObjectProvider provider,Type scope)
		{
			var param = Expression.Parameter(typeof(object));
			var mobj = provider.GetMetaObject(param);
			var binder =
				(GetMemberBinder) Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, member, scope,
					new[] {CSharpArgumentInfo.Create(0, null)});
			var ret = mobj.BindGetMember(binder);
			var final = Expression.Block(
				Expression.Label(CallSiteBinder.UpdateLabel),
				ret.Expression
			);
			var lambda = Expression.Lambda(final, param);
			var del = lambda.Compile();
			return del.DynamicInvoke(o);
		}
	}
}