using Microsoft.Xaml.Behaviors;
using MqttToolsMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MqttToolsMVVM.Services
{
    public class ScrollToEndBehavior : Behavior<ItemsControl>
    {
        /// <summary>
        /// Обеспечивает передачу ссылки на конкретный ItemsControl для скроллинга на последний элемент.
        /// </summary>
        private ItemsControl _itemsControl;

        protected override void OnAttached()
        {
            base.OnAttached();

            var sourceCollection = this.AssociatedObject.ItemsSource as ObservableCollection<LogMessage>;
            if (sourceCollection == null) return;

            sourceCollection.CollectionChanged +=
                new NotifyCollectionChangedEventHandler(DataGridCollectionChanged);

            _itemsControl = this.AssociatedObject;
        }

        /// <summary>
        /// Обеспечивает скролл на последний добавленный элемент.
        /// </summary>
        private void DataGridCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_itemsControl != null)
            {
                var border = VisualTreeHelper.GetChild(_itemsControl, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }
        }
    }
}