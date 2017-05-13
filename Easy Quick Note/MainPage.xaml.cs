using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Easy_Quick_Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        bool darkTheme = false;

        public MainPage()
        {
            this.InitializeComponent();
            darkTheme = false;
        }

        private async void saveButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Note", new List<string>() { ".eqn" });
            savePicker.SuggestedFileName = "Note";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, textBox.Text);
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        private async void loadButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".eqn");
            openPicker.FileTypeFilter.Add(".txt");
            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            if (file!=null)
            {
                Stream stream = await file.OpenStreamForReadAsync();
                StreamReader streamreader = new StreamReader(stream);
                textBox.Text = streamreader.ReadToEnd();
            }
        }


        private async void closeButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            ElementTheme requestedTheme;
            if (darkTheme == true)
            {
                requestedTheme = ElementTheme.Dark;
            }
            else
            {
                requestedTheme = ElementTheme.Light;
            }

            //Application.Current.Exit();
            var btn = sender as Button;
            var dialog = new ContentDialog()
            {
                Title = "Are you sure?",
                RequestedTheme = requestedTheme,
                //FullSizeDesired = true,
                MaxWidth = this.ActualWidth // Required for Mobile!
            };

            // Setup Content
            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = "Are you sure you want to close without saving? Note: Pressing cancel when saving WILL close EQN.",
                TextWrapping = TextWrapping.Wrap,
            });

            var cb = new CheckBox
            {
                Content = "Agree"
            };

            var save = new CheckBox
            {
                Content = "Save before closing"
            };

            cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Path = new PropertyPath("IsPrimaryButtonEnabled"),
                Mode = BindingMode.TwoWay,
            });

            save.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Mode = BindingMode.TwoWay,
            });

            panel.Children.Add(cb);
            panel.Children.Add(save);
            dialog.Content = panel;

            // Add Buttons
            dialog.PrimaryButtonText = "OK";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.PrimaryButtonClick += async delegate
            {
                btn.Content = "Result: OK";
                if (save.IsChecked == true)
                {
                    var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                    savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                    savePicker.FileTypeChoices.Add("Note", new List<string>() { ".eqn" });
                    savePicker.SuggestedFileName = "Note";
                    Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                    if (file != null)
                    {
                        Windows.Storage.CachedFileManager.DeferUpdates(file);
                        await Windows.Storage.FileIO.WriteTextAsync(file, textBox.Text);
                        Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                        
                        Application.Current.Exit();
                    }
                    Application.Current.Exit();
                }
                Application.Current.Exit();
            };

            dialog.SecondaryButtonText = "Cancel";
            dialog.SecondaryButtonClick += delegate
            {
                btn.Content = "Result: Cancel";
            };

            // Show Dialog
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {
                btn.Content = "Result: NONE";
            }
        }

        private async void settingsButton_ClickAsync(object sender, RoutedEventArgs e)
        {

            ElementTheme requestedTheme;
            if (darkTheme == true)
            {
                requestedTheme = ElementTheme.Dark;
            } else
            {
                requestedTheme = ElementTheme.Light;
            }

            //Application.Current.Exit();
            var btn = sender as Button;
            var dialog = new ContentDialog()
            {
                Title = "Change settings",
                RequestedTheme = requestedTheme,
                //FullSizeDesired = true,
                MaxWidth = this.ActualWidth // Required for Mobile!
            };

            // Setup Content
            var panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = "Change the settings of Easy Quick Note!",
                TextWrapping = TextWrapping.Wrap,
            });

            var cb = new CheckBox
            {
                Content = "Dark theme (Only applies to dialouges)"
            };

            cb.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = dialog,
                Mode = BindingMode.TwoWay,
            });

            panel.Children.Add(cb);
            //panel.Children.Add(save);
            dialog.Content = panel;

            // Add Buttons
            dialog.PrimaryButtonText = "Save Settings";
            dialog.PrimaryButtonClick += delegate
            {
                btn.Content = "Result: OK";
                if (cb.IsChecked == true)
                {
                    darkTheme = true;
                }

                if (cb.IsChecked == false)
                {
                    darkTheme = false;
                }
            };

            dialog.SecondaryButtonText = "Cancel";
            dialog.SecondaryButtonClick += delegate
            {
                btn.Content = "Result: Cancel";
            };

            // Show Dialog
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.None)
            {
                btn.Content = "Result: NONE";
            }
        }
    }
}
