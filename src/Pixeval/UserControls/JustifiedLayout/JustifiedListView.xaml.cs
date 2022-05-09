using Microsoft.UI.Xaml;

namespace Pixeval.UserControls.JustifiedLayout;

public sealed partial class JustifiedListView
{
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        nameof(Items),
        typeof(object),
        typeof(JustifiedListView),
        PropertyMetadata.Create(DependencyProperty.UnsetValue, ItemsChanged));

    public object Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is JustifiedListView { RowsListView: var listView } && e.NewValue is { } items)
        {
            listView.ItemsSource = items;
        }
    }

    public JustifiedListView()
    {
        InitializeComponent();
    }
}