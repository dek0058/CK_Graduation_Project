// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// A repository of <see cref="ILocalizedStrings"/> that are localized for different
    /// cultures using the "*.mo" gettext format.
    /// </summary>
    /// <example>
    /// <para>When multiple paths are specified, language files are loaded and flattened
    /// such that strings from latter paths replace those from former ones in the
    /// resulting flattened <see cref="ILocalizedStrings"/> instance.</para>
    /// <code language="csharp"><![CDATA[
    /// var paths = new string[] {
    ///     primaryLanguagesPath,
    ///     overrideLanguagesPath,
    /// };
    ///
    /// var loader = new LocalizedStringsLoaderMo();
    /// var repository = new LocalizedStringsPathsRepository(paths, ".mo", loader);
    /// ]]></code>
    /// <para>Strings localized for the desired culture can then be accessed like so:</para>
    /// <code language="csharp"><![CDATA[
    /// // The returned `ILocalizedStrings` object should be cached and reused.
    /// var localizedStrings = repository.GetLocalizedStrings(culture);
    ///
    /// // An example of writing localized text to the console.
    /// Console.WriteLine(localizedStrings.Text("Hello, world!"));
    /// ]]></code>
    /// </example>
    public sealed class LocalizedStringsPathsRepository : ILocalizedStringsRepository
    {
        private readonly string[] paths;
        private readonly string fileExtension;
        private readonly Regex cultureFileNameRegex;
        private readonly ILocalizedStringsLoader loader;


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStringsPathsRepository"/> class.
        /// </summary>
        /// <param name="paths">An array of paths where localization files can be found.
        /// Latter paths have precedence over former paths.</param>
        /// <param name="fileExtension">File extension; for instance, ".mo".</param>
        /// <param name="loader">Object for loading localized strings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="paths"/> or <paramref name="loader"/> are <c>null</c>.
        /// </exception>
        public LocalizedStringsPathsRepository(string[] paths, string fileExtension, ILocalizedStringsLoader loader)
        {
            ExceptionUtility.CheckArgumentNotNull(paths, "paths");
            ExceptionUtility.CheckArgumentNotNull(fileExtension, "fileExtension");
            ExceptionUtility.CheckArgumentNotNull(loader, "loader");

            string filePattern = string.Format(
                @"^(?<Locale>[a-z]+([\-\\_][a-z]+)*){0}$",
                Regex.Escape(fileExtension)
            );

            this.paths = paths.ToArray();
            this.fileExtension = fileExtension;
            this.cultureFileNameRegex = new Regex(filePattern, RegexOptions.IgnoreCase);
            this.loader = loader;
        }


        /// <inheritdoc/>
        public CultureInfo[] DiscoverAvailableLocalizations()
        {
            var cultures = new List<CultureInfo>();
            foreach (var locale in this.DiscoverAvailableLocales()) {
                try {
                    cultures.Add(CultureInfo.CreateSpecificCulture(locale));
                }
                catch (Exception) {
                }
            }

            return cultures.ToArray();
        }

        private IEnumerable<string> DiscoverAvailableLocales()
        {
            var locales = new HashSet<string>();
            foreach (string path in this.paths) {
                locales.UnionWith(this.DiscoverAvailableLocalesAtPath(path));
            }
            return locales;
        }

        private IEnumerable<string> DiscoverAvailableLocalesAtPath(string path)
        {
            if (!Directory.Exists(path)) {
                return Enumerable.Empty<string>();
            }

            return
                from match in
                    from filePath in Directory.GetFiles(path)
                    select this.cultureFileNameRegex.Match(Path.GetFileName(filePath))
                where match.Success
                select match.Groups["Locale"].Value.Replace('_', '-');
        }


        /// <inheritdoc/>
        public ILocalizedStrings GetLocalizedStrings(CultureInfo culture)
        {
            var catalogFileStreams = this.GetCatalogFileStreams(culture);
            try {
                return this.loader.Load(catalogFileStreams, culture);
            }
            finally {
                foreach (var stream in catalogFileStreams) {
                    stream.Dispose();
                }
            }
        }

        private IEnumerable<Stream> GetCatalogFileStreams(CultureInfo culture)
        {
            return this.paths
                .Select(path => ResolveCatalogFilePath(path, this.fileExtension, culture))
                .Select(catalogFilePath => OpenCatalogFileStream(catalogFilePath))
                .Where(stream => stream != null);
        }


        private static string ResolveCatalogFilePath(string path, string fileExtension, CultureInfo culture)
        {
            string localeName = culture.Name.Replace('-', '_');
            string catalogFileName = localeName + fileExtension;

            return Path.Combine(path, catalogFileName);
        }

        private static Stream OpenCatalogFileStream(string catalogFilePath)
        {
            try {
                return File.OpenRead(catalogFilePath);
            }
            catch (DirectoryNotFoundException) {
                return null;
            }
            catch (FileNotFoundException) {
                return null;
            }
        }
    }
}
