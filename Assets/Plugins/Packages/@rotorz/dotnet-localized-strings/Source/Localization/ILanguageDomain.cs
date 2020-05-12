// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// The interface of a language domain providing language selection and access to
    /// the strings localized for the currently active culture.
    /// </summary>
    public interface ILanguageDomain : ILocalizedStrings
    {
        /// <summary>
        /// Occurs when the <see cref="ILanguageDomain"/> is loaded.
        /// </summary>
        /// <seealso cref="Load"/>
        event EventHandler Loaded;


        /// <summary>
        /// Loads language domain for the specified culture.
        /// </summary>
        /// <param name="culture">Culture to load.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="culture"/> is <c>null</c>.
        /// </exception>
        void Load(CultureInfo culture);
    }
}
