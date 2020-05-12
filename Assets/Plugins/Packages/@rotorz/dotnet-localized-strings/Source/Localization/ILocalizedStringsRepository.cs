// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Represents a repository of <see cref="ILocalizedStrings"/> that have translations
    /// available for zero-or-more cultures.
    /// </summary>
    public interface ILocalizedStringsRepository
    {
        /// <summary>
        /// Discovers the cultures that translations are available in.
        /// </summary>
        /// <returns>
        /// An array of zero-or-more cultures.
        /// </returns>
        CultureInfo[] DiscoverAvailableLocalizations();

        /// <summary>
        /// Gets an object representing the localized strings of a specific culture.
        /// </summary>
        /// <remarks>
        /// <para>This method always returns a <see cref="ILocalizedStrings"/> instance
        /// even if no translations are available for the specified culture.</para>
        /// </remarks>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// The loaded <see cref="ILocalizedStrings"/> instance.
        /// </returns>
        ILocalizedStrings GetLocalizedStrings(CultureInfo culture);
    }
}
