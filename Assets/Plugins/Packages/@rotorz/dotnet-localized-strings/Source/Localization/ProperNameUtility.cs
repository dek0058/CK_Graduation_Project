// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Utility functionality to assist with proper names.
    /// </summary>
    public static class ProperNameUtility
    {
        /// <summary>
        /// Automatically annotates translated version of a proper name with the original
        /// non-translated version when translation isn't already annotated with the
        /// original non-translated version in parenthesis.
        /// </summary>
        /// <example>
        /// <para>Given the following, the output would be "鲍步 (Bob)":</para>
        /// <code language="csharp"><![CDATA[
        /// Console.WriteLine(ProperNameUtility.FormatProperName("Bob", "鲍步"));
        /// ]]></code>
        /// <para>However, given the following, the translations would be left untouched
        /// since they are already annotated with the original non-translated proper name
        /// in parenthesis:</para>
        /// <code language="csharp"><![CDATA[
        /// Console.WriteLine(ProperNameUtility.FormatProperName("Bob", "鲍步 (Bob)"));
        /// // 鲍步 (Bob)
        ///
        /// Console.WriteLine(ProperNameUtility.FormatProperName("Bob", "鲍步(Bob)"));
        /// // 鲍步(Bob)
        /// ]]></code>
        /// </example>
        /// <param name="original"></param>
        /// <param name="translated"></param>
        /// <returns></returns>
        public static string FormatProperName(string original, string translated)
        {
            if (translated == original) {
                return original;
            }

            string annotation = string.Format("({0})", original);
            if (!translated.Contains(annotation)) {
                translated = string.Format("{0} ({1})", translated, original);
            }

            return translated;
        }
    }
}
