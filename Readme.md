# PhoneBook

2. STRUCTURA APLICATIEI

Aplicatia a fost realizata in **Xamarin Forms** pentru **Andorid** si **UWP**, iar ca si mediu de stocare persistenta sa folosit **SQLite**. Xamarin Forms cuprinde **C#** pentru partea de backend si **XAML** pentru partea de frontend si definire a aspectului vizual.

Structural, este formata din 3 pagini principale **History**, **Favorite** si **Contacts** care sunt si taburile din tabview. Pe langa paginile principale avem si paginile secundare: **AddContact**,
**DialPage**, **DisContact** ,**SearchPage** si **SettingsPage**.

```csharp
namespace AgendaTelefonica.Models
{
   public class Contact
   {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }

        [MaxLength(250), Unique]
        public string firstName { get; set; }

        [MaxLength(250), Unique]
        public string secondName { get; set; }

        [MaxLength(250), Unique]
        public string phoneNumber { get; set; }

        [MaxLength(250), Unique]
        public string email { get; set; }

        public bool favorite { get; set; }

        public byte[] profilPicture { get; set; }


        public  SQLiteConnection getConnection()
        {
            return new SQLiteConnection(App.DataBaseLocation);
        }

    }
}
```

```csharp
namespace AgendaTelefonica.Models
{
   public class HistoryElem
    {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }

        public int id_Contact { set; get; }

        public DateTime date { set; get; }

        public string phoneNumber { set; get; }

        public bool IsEmail { set; get; }



        public SQLiteConnection getConnection()
        {
            return new SQLiteConnection(App.DataBaseLocation);
        }
    }
}

```

```csharp
private async void addPhoto_Clicked(object sender, EventArgs e)
{
    try
    {
        var result_photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
        {
            Title = "Pick a photo!"
        });// folosim media picker ca sa luam poza din memorie

        // salvam rezultatul ca si un stream de date
        var stream = await result_photo.OpenReadAsync();

        // convertim strem-ul in imagine si afisam
        contactPhoto.Source = ImageSource.FromStream(() => stream);

        // convertim in sir de bytes[] apeland metoda de mai jos
        photoSave = GetImageBytes(stream);
    }
    catch (NullReferenceException ex)
    {
        Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
    }
}

private byte[] GetImageBytes(Stream stream)
{
    byte[] ImageBytes;
    using (var memoryStream = new System.IO.MemoryStream())
    {
        stream.CopyTo(memoryStream);
        ImageBytes = memoryStream.ToArray();
    }
    return ImageBytes;
}
```

```xml
<ContentPage.ToolbarItems>
    <ToolbarItem x:Name="searchBtn" Text="Search"
                    IconImageSource="search.png"
                    Order="Primary"
                    Priority="0"
                    Clicked="searchBtn_Clicked"/>
    <ToolbarItem x:Name="addContBtn" Order="Primary"
                    IconImageSource="plus.png"
                    Text="Add"
                    Priority="1"
                    Clicked="addContBtn_Clicked" />
    <ToolbarItem x:Name ="settingsBtn" Order="Secondary"
                    Text="Settings"
                    Priority="2"
                    Clicked="settingsBtn_Clicked"/>
</ContentPage.ToolbarItems>


 <CollectionView x:Name="favoriteListView"
                        ItemsSource="{Binding Contact}"
                        AbsoluteLayout.LayoutBounds = "0.5,0"
                        AbsoluteLayout.LayoutFlags = "PositionProportional"
                        SelectionChanged="favoriteListView_SelectionChanged"
                        SelectionMode="Single" >
            <CollectionView.ItemsLayout>
                <GridItemsLayout Orientation="Vertical" Span="2" HorizontalItemSpacing="5" VerticalItemSpacing="5" />

            </CollectionView.ItemsLayout>
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <Frame  Style="{StaticResource FrameStyle}" CornerRadius ="0" BackgroundColor="{Binding Path=firstName, Converter={StaticResource ColorConvertorFavorite}}">

                        <Grid Padding="5,5,5,5"  >
                            <Grid.RowDefinitions>
                                <RowDefinition Height="Auto" />
                                <RowDefinition Height="Auto" />
                                <RowDefinition Height="100" />
                            </Grid.RowDefinitions>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="Auto" />
                            </Grid.ColumnDefinitions>
                            <Label Grid.Column="1"
                                   Text="{Binding firstName}"
                                   Style="{StaticResource LabelStyle}" />

                            <Label Grid.Row="1"
                                   Grid.Column="1"
                                   Style="{StaticResource LabelStyle}"
                                   Text="{Binding Path=secondName}" />
                        </Grid>
                    </Frame>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>


<Application.Resources>

    <Color x:Key="AppPrimaryColor">#007ac1</Color>
    <Color x:Key="AppBackgroundColor">#67daff</Color>
    <Color x:Key="PrimaryColor">#00a9f4</Color>
    
    <Color x:Key="CallColor">#01C853</Color>


    <Color x:Key="SecondaryColor">#ffc107</Color>
    <Color x:Key="SecondaryColor_L">#fff350</Color>
    <Color x:Key="SecondaryColor_B">#c79100</Color>

    <Color x:Key="TertiaryColor">Silver</Color>
    <Color x:Key="TextColor">Black</Color>
    <Color x:Key="PlaceHolderColor">Gray</Color>
    <Color x:Key="ButtonColor">#646364</Color>

    <Style x:Key="CallButtonStyle" TargetType="Button">
        <Setter Property="BackgroundColor" Value="{StaticResource CallColor}"/>
        <Setter Property="CornerRadius" Value="90"/>
        <Setter Property="ImageSource" Value="callphone.png"/>
    </Style>

    <Style x:Key="ButtonStyle" TargetType="Button">
        <Setter Property="BackgroundColor" Value="{StaticResource ButtonColor}"/>
        <Setter Property="TextColor" Value="{StaticResource SecondaryColor}" />
        <Setter Property="CornerRadius" Value="4"/>
    </Style>


    <Style x:Key="FrameStyle" TargetType="Frame">
        <Setter Property="BackgroundColor" Value="{StaticResource SecondaryColor}"/>
        <Setter Property="Padding" Value="10,10,10,10" />
        <Setter Property="CornerRadius" Value="10"/>
        <Setter Property="HasShadow" Value="True"/>
        <Setter Property="BorderColor" Value="{StaticResource AppPrimaryColor}"/>
    </Style>

    <Style x:Key="LabelStyle" TargetType="Label">
        <Setter Property="TextColor" Value="{StaticResource TextColor}"/>
        <Setter Property="FontAttributes" Value="Bold"/>
        <Setter Property="FontSize" Value="Large"/>
        <Setter Property="HorizontalOptions" Value="StartAndExpand"/>
        <Setter Property="VerticalOptions" Value="Center"/>
    </Style>

</Application.Resources>

```
