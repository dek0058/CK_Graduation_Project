// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Localization.Internal.NGettext;
using System.Globalization;

namespace Rotorz.Games.Localization.Internal
{
    /// <exclude/>
    internal sealed class NGettextLocalizedStrings : ILocalizedStrings
    {
        private readonly Catalog catalog;
        private readonly CultureInfo culture;
        private readonly bool isRightToLeft;


        public NGettextLocalizedStrings(Catalog catalog)
        {
            this.catalog = catalog;
            this.culture = catalog.CultureInfo;
            this.isRightToLeft = this.culture.TextInfo.IsRightToLeft;
        }


        public Catalog Catalog {
            get { return this.catalog; }
        }

        public CultureInfo Culture {
            get { return this.culture; }
        }


        public string Text(string message)
        {
            return this.catalog.GetString(message);
        }

        public string ParticularText(string context, string message)
        {
            return this.catalog.GetParticularString(context, message);
        }

        public string PluralText(string singularMessage, string pluralMessage, int value)
        {
            return this.catalog.GetPluralString(singularMessage, pluralMessage, value);
        }

        public string ParticularPluralText(string context, string singularMessage, string pluralMessage, int value)
        {
            return this.catalog.GetParticularPluralString(context, singularMessage, pluralMessage, value);
        }


        public string ProperName(string name)
        {
            string translatedName = this.catalog.GetParticularString("Proper Name", name);
            return ProperNameUtility.FormatProperName(name, translatedName);
        }


        public string OpensWindow(string actionMessage)
        {
            return this.isRightToLeft
                ? "..." + actionMessage
                : actionMessage + "...";
        }
    }
}
