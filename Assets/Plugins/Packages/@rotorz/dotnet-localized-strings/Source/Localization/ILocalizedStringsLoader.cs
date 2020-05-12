// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// A loader that loads <see cref="ILocalizedStrings"/> from given data streams.
    /// </summary>
    public interface ILocalizedStringsLoader
    {
        /// <summary>
        /// Loads and flattens localized strings from the given collection of streams.
        /// </summary>
        /// <remarks>
        /// <para>This method always returns a <see cref="ILocalizedStrings"/> instance
        /// even if no streams are specified.</para>
        /// </remarks>
        /// <param name="streams">A collection of zero-or-more streams.</param>
        /// <param name="culture">Culture of the strings being loaded.</param>
        /// <returns>
        /// The loaded <see cref="ILocalizedStrings"/> instance.
        /// </returns>
        ILocalizedStrings Load(IEnumerable<Stream> streams, CultureInfo culture);
    }
}
