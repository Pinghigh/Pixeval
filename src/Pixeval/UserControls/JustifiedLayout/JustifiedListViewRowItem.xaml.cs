using Microsoft.UI.Xaml;

namespace Pixeval.UserControls.JustifiedLayout;

public sealed partial class JustifiedListViewRowItem
{
    public JustifiedListViewRowItem(object item, DataTemplate template)
    {
        InitializeComponent();
        ContentTemplate = template;
        ItemContent = item;
    }

    public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
        nameof(ContentTemplate),
        typeof(DataTemplate),
        typeof(JustifiedListViewRowItem),
        PropertyMetadata.Create(DependencyProperty.UnsetValue, ContentTemplateChanged));

    public DataTemplate ContentTemplate
    {
        get => (DataTemplate) GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    private static void ContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is JustifiedListViewRowItem row && e.NewValue is DataTemplate template)
        {
            row.Presenter.ContentTemplate = template;
        }
    }

    public static readonly DependencyProperty ItemContentProperty = DependencyProperty.Register(
        nameof(ItemContent),
        typeof(object),
        typeof(JustifiedListViewRowItem),
        PropertyMetadata.Create(DependencyProperty.UnsetValue, ItemContentChanged));

    public object ItemContent
    {
        get => GetValue(ItemContentProperty);
        set => SetValue(ItemContentProperty, value);
    }

    private static void ItemContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is JustifiedListViewRowItem row && e.NewValue is { } obj)
        {
            row.Presenter.Content = obj;
        }
    }
}