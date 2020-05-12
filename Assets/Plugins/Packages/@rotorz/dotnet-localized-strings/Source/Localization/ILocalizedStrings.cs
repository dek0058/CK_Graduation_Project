// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Provides access to localized strings.
    /// </summary>
    public interface ILocalizedStrings
    {
        /// <summary>
        /// Gets the culture of the localization.
        /// </summary>
        CultureInfo Culture { get; }


        /// <summary>
        /// Gets localized message text.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string text = localizedStrings.Text("Hello, world!");
        /// ]]></code>
        /// </example>
        /// <param name="message">Non-translated message text in root language.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        string Text(string message);

        /// <summary>
        /// Gets localized message text with an explicit context to help disambiguate
        /// meaning for translators.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string actionText = localizedStrings.ParticularText("Action", "New");
        /// ]]></code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="message">Non-translated message text in root language.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        string ParticularText(string context, string message);

        /// <summary>
        /// Gets localized message text taking plural form into consideration using the
        /// given numeric value.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// int appleCount = 3;
        /// string text = localizedStrings.PluralText(
        ///     "You ate an apple.", "You ate {0} apples.", appleCount
        /// );
        /// ]]></code>
        /// </example>
        /// <param name="singularMessage">Non-translated message text in root language
        /// for singular form of <paramref name="value"/>.</param>
        /// <param name="pluralMessage">Non-translated message text in root language
        /// for plural form of <paramref name="value"/>.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        string PluralText(string singularMessage, string pluralMessage, int value);

        /// <summary>
        /// Gets localized message text taking plural form into consideration using the
        /// given numeric value with an explicit context to help disambiguate meaning for
        /// translators.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// int appleCount = 3;
        /// string text = localizedStrings.ParticularPluralText(
        ///     "Fruit", "You ate an apple.", "You ate {0} apples.", appleCount
        /// );
        /// ]]></code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="singularMessage">Non-translated message text in root language
        /// for singular form of <paramref name="value"/>.</param>
        /// <param name="pluralMessage">Non-translated message text in root language
        /// for plural form of <paramref name="value"/>.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        string ParticularPluralText(string context, string singularMessage, string pluralMessage, int value);


        /// <summary>
        /// Gets localized proper name. See the gettext manual, section Names.
        /// </summary>
        /// <remarks>
        /// <para>This method automatically annotates translated names with the original
        /// non-translated name when the translation doesn't already include the original
        /// non-translated name in parenthesis; for instance, "TRANSLATION (ORIGINAL)".</para>
        /// </remarks>
        /// <param name="name">Non-translated proper name.</param>
        /// <returns>
        /// The localized proper name or non-translated proper name if not localized.
        /// </returns>
        string ProperName(string name);


        /// <summary>
        /// Formats action message text with ellipsis hint to highlight that the action
        /// will present the user with a different window.
        /// </summary>
        /// <remarks>
        /// <para>This method does not attempt to localize <paramref name="actionMessage"/>;
        /// use the other text methods of this interface to perform localization where
        /// required in addition to using this method.</para>
        /// </remarks>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string actionText = localizedStrings.ParticularText("Action", "New");
        /// string actionTextWithEllipsis = localizedStrings.OpensWindow(actionText);
        /// ]]></code>
        /// </example>
        /// <param name="actionMessage">Action message text.</param>
        /// <returns>
        /// The formatted action message text.
        /// </returns>
        string OpensWindow(string actionMessage);
    }
}
