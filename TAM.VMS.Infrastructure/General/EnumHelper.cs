﻿using System.ComponentModel;
using System.Reflection;

namespace TAM.VMS.Infrastructure.General
{
	public static class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            if (en == null)
                return string.Empty;

            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }


        /// <summary>
        /// Filter the enum based on an attribute
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> FilterEnumWithAttributeOf<TEnum, TAttribute>()
            where TEnum : struct
            where TAttribute : class
        {
            foreach (var field in
                typeof(TEnum).GetFields(BindingFlags.GetField |
                                         BindingFlags.Public |
                                         BindingFlags.Static))
            {
                if (field.GetCustomAttributes(typeof(TAttribute), false).Length > 0)
                    yield return (TEnum)field.GetValue(null);
            }
        }

        public static object GetDescription(object retiredToDonation)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }
}
