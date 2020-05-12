// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Localization.Internal;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// A loader that loads <see cref="ILocalizedStrings"/> from *.mo encoded streams.
    /// </summary>
    public sealed class LocalizedStringsLoaderMo : ILocalizedStringsLoader
    {
        /// <inheritdoc/>
        public ILocalizedStrings Load(IEnumerable<Stream> streams, CultureInfo culture)
        {
            var catalogs = from stream in streams
                           select NGettextUtility.LoadCatalogFromStream(stream);
            var catalog = NGettextUtility.FlattenCatalogs(catalogs, culture);

            return new NGettextLocalizedStrings(catalog);
        }
    }
}
