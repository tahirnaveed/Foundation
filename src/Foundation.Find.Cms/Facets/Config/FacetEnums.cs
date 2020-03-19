namespace Foundation.Find.Cms.Facets.Config
{
#pragma warning disable CA1008 // Enums should have zero value
    public enum FacetDisplayMode
#pragma warning restore CA1008 // Enums should have zero value
    {
        [EnumSelectionDescription(Text = "Checkbox", Value = "Checkbox")]
        Checkbox = 1,
        [EnumSelectionDescription(Text = "Button", Value = "Button")]
        Button,
        [EnumSelectionDescription(Text = "Color Swatch", Value = "ColorSwatch")]
        ColorSwatch,
        [EnumSelectionDescription(Text = "Size Swatch", Value = "SizeSwatch")]
        SizeSwatch,
        [EnumSelectionDescription(Text = "Numeric Range", Value = "Range")]
        Range,
        [EnumSelectionDescription(Text = "Rating", Value = "Rating")]
        Rating,
        [EnumSelectionDescription(Text = "Slider", Value = "Slider")]
        Slider,
        [EnumSelectionDescription(Text = "Price Range", Value = "PriceRange")]
        PriceRange,
    }

#pragma warning disable CA1008 // Enums should have zero value
    public enum FacetContentFieldName
#pragma warning restore CA1008 // Enums should have zero value
    {
        [EnumSelectionDescription(Text = "Type of Content", Value = "PageTypes")]
        ContentType = 1,
        [EnumSelectionDescription(Text = "Category", Value = "ContentCategory")]
        Categories,
        [EnumSelectionDescription(Text = "Interests", Value = "TagList")]
        Interests,
        [EnumSelectionDescription(Text = "Article Type", Value = "ArticleType")]
        ArticleType,
    }

#pragma warning disable CA1008 // Enums should have zero value
    public enum FacetFieldType
#pragma warning restore CA1008 // Enums should have zero value
    {
        [EnumSelectionDescription(Text = "String", Value = "String")]
        String = 1,
        [EnumSelectionDescription(Text = "List of String", Value = "ListOfString")]
        ListOfString,
        [EnumSelectionDescription(Text = "Integer", Value = "Integer")]
        Integer,
        [EnumSelectionDescription(Text = "2 Decimal Places", Value = "Double")]
        Double,
        [EnumSelectionDescription(Text = "Boolean", Value = "Boolean")]
        Boolean,
        [EnumSelectionDescription(Text = "Enhanced Boolean", Value = "NullableBoolean")]
        NullableBoolean
    }

#pragma warning disable CA1008 // Enums should have zero value
    public enum FacetDisplayDirection
#pragma warning restore CA1008 // Enums should have zero value
    {
        [EnumSelectionDescription(Text = "Vertical", Value = "Vertical")]
        Vertical = 1,
        [EnumSelectionDescription(Text = "Horizontal", Value = "Horizontal")]
        Horizontal
    }
}
