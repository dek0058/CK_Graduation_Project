// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// A straightforward <see cref="ILanguageDomain"/> implementation which loads
    /// strings localized for a given culture using a <see cref="ILocalizedStringsRepository"/>
    /// instance that is given when constructing the <see cref="LanguageDomain"/>.
    /// </summary>
    public class LanguageDomain : ILanguageDomain
    {
        private readonly ILocalizedStringsRepository repository;

        private ILocalizedStrings strings;


        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageDomain"/> class.
        /// </summary>
        /// <param name="repository">Repository for accessing localized strings within
        /// the language domain.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="repository"/> is <c>null</c>.
        /// </exception>
        public LanguageDomain(ILocalizedStringsRepository repository)
        {
            ExceptionUtility.CheckArgumentNotNull(repository, "repository");

            this.repository = repository;
        }


        /// <inheritdoc/>
        public CultureInfo Culture { get; private set; }


        /// <inheritdoc/>
        public event EventHandler Loaded;


        /// <summary>
        /// Raises the <see cref="Loaded"/> event.
        /// </summary>
        protected virtual void OnLoaded()
        {
            var handler = this.Loaded;
            if (handler != null) {
                handler.Invoke(this, EventArgs.Empty);
            }
        }


        /// <inheritdoc/>
        public void Load(CultureInfo culture)
        {
            ExceptionUtility.CheckArgumentNotNull(culture, "culture");

            if (culture == this.Culture) {
                return;
            }

            this.strings = this.repository.GetLocalizedStrings(culture);
            this.Culture = culture;

            this.OnLoaded();
        }


        /// <inheritdoc/>
        public string Text(string message)
        {
            return this.strings.Text(message);
        }

        /// <inheritdoc/>
        public string ParticularText(string context, string message)
        {
            return this.strings.ParticularText(context, message);
        }

        /// <inheritdoc/>
        public string PluralText(string singularMessage, string pluralMessage, int value)
        {
            return this.strings.PluralText(singularMessage, pluralMessage, value);
        }

        /// <inheritdoc/>
        public string ParticularPluralText(string context, string singularMessage, string pluralMessage, int value)
        {
            return this.strings.ParticularPluralText(context, singularMessage, pluralMessage, value);
        }


        /// <inheritdoc/>
        public string ProperName(string name)
        {
            return this.strings.ProperName(name);
        }


        /// <inheritdoc/>
        public string OpensWindow(string actionMessage)
        {
            return this.strings.OpensWindow(actionMessage);
        }
    }
}
