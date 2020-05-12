// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Localization.Internal.NGettext;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rotorz.Games.Localization.Internal
{
    /// <exclude/>
    internal static class NGettextUtility
    {
        public static Catalog CreateEmptyCatalog(CultureInfo culture)
        {
            return new Catalog(culture);
        }

        public static Catalog LoadCatalogFromStream(Stream stream)
        {
            return new Catalog(stream);
        }


        public static Catalog FlattenCatalogs(IEnumerable<Catalog> collection, CultureInfo culture)
        {
            var catalogs = collection.ToList();

            if (catalogs.Count == 0) {
                catalogs.Add(CreateEmptyCatalog(culture));
            }

            foreach (var catalog in catalogs.Skip(1)) {
                MergeSourceCatalogIntoTargetCatalog(catalog, catalogs.First());
            }

            return catalogs.First();
        }

        public static void MergeSourceCatalogIntoTargetCatalog(Catalog source, Catalog target)
        {
            var translations = target.Translations;
            foreach (var translation in source.Translations) {
                translations[translation.Key] = translation.Value.ToArray();
            }
        }
    }
}
