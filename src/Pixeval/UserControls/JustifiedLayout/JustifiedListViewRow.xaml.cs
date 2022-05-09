using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pixeval.Utilities;

namespace Pixeval.UserControls.JustifiedLayout;

public sealed partial class JustifiedListViewRow
{
    // throttle
    private bool _refreshing;

    public JustifiedListViewRow()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
        nameof(ContentTemplate),
        typeof(DataTemplate),
        typeof(JustifiedListViewRowItem),
        PropertyMetadata.Create(DependencyProperty.UnsetValue));

    public DataTemplate ContentTemplate
    {
        get => (DataTemplate) GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
        nameof(Spacing),
        typeof(double),
        typeof(JustifiedListViewRow),
        PropertyMetadata.Create(10));

    public double Spacing
    {
        get => (double) GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        nameof(Items),
        typeof(ICollection<JustifiedListViewRowItemContext>),
        typeof(JustifiedListViewRow),
        PropertyMetadata.Create(DependencyProperty.UnsetValue, ItemsChanged));

    public ICollection<JustifiedListViewRowItemContext> Items
    {
        get => (ICollection<JustifiedListViewRowItemContext>) GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is JustifiedListViewRow { RowItemsGrid: var container, _refreshing: false } row && e.NewValue is ICollection<JustifiedListViewRowItemContext> { Count: > 0 } collection)
        {
            row._refreshing = true;
            container.Children.Clear();
            container.ColumnDefinitions.Clear();

            container.Height = collection.First().LayoutHeight + row.Spacing;
            var columns = collection.Select(wrapper => new JustifiedListViewRowItem(wrapper, row.ContentTemplate)
            {
                VerticalAlignment = VerticalAlignment.Center
            });

            foreach (var (index, (column, (_, layoutWidth, _))) in columns.Zip(collection).Indexed())
            {
                container.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(index == 0 ? layoutWidth : layoutWidth + row.Spacing)
                });

                if (index != 0)
                {
                    column.HorizontalAlignment = HorizontalAlignment.Right;
                    column.VerticalAlignment = VerticalAlignment.Center;
                }

                Grid.SetColumn(column, index);
                container.Children.Add(column);
            }

            row._refreshing = false;
        }
    }
}