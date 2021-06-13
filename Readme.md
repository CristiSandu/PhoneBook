# PhoneBook

### **2. STRUCTURA APLICAȚIEI**

Aplicația a fost realizată în Xamarin Forms pentru **Android** și **UWP** , iar ca și mediu de stocare persistentă sa folosit **SQLite**. Xamarin Forms cuprinde **C#** pentru partea de backend și **XAML** pentru partea de front end și definire a aspectului vizual.

Structural, este formată din 3 pagini principale **History** , **Favorite** și **Contacts** care sunt și taburile din tabview. Pe langa paginile principale avem și paginile secundare: **AddContact** , **DialPage** , **DisContact** , **SearchPage** și **SettingsPage**.

1. **AddContact** - In aceasta pagina se poate adăuga un contact nou.

Un contact conțină: nume, prenume, nr de telefon (cu verificare dacă nr este de România și este valid), email(cu verificare de validitate), toggle(on - add to favorite contacts , off - not to favorite contacts), o imagine de profil aleasă din galerie.

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine1.jpg)

2. **DisContact** - Este o pagina în care se afișează contactul alături de următoarele posibilități: sunare, trimitere mail sau ștergere contact.  
   Daca apare o steluta reprezinta faptul ca acel contact este la favorite.

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine2.jpg)

3. **DialPage** - Pagina prezinta o tastatura care permite introducerea unui nr de telefon, iar în partea de sus se face o selecție pe numerele de telefon care încep cu nr scris până în momentul de fata. Daca se selecteaza un nr din lista se completeaza automat label-ul si se poate apela numarul.

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine3.jpg)

4. **SearchPage** - Se poate face search prin contacte dupa Nume, in bara de search apar nr de contacte. Se poate selecta contactul din lista care apare

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine4.jpg)

5. **SettingsPage** - nu am implementat nimic în settings interesant, sunt niște elemente hardcodate

6. **History** - aici apare istoria apelurilor și a emailurilor trimise sub forma &quot;Call - nume&quot; sau &quot;Email - nume &quot; sau &quot;Call - nr de telefon&quot; pentru nr de telefon care nu se afla în agenda alături de data și ora . În partea de sus apare numărul de apeluri și mailuri din pagina. Pe fiecare pagina principala avem un buton pentru deschidere a **DialPage.**

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine5.jpg)

7. **Favorite** - Pagina prezinta intr-un layout cu 2 coloane si n lini contactele favorite , culoarea de background este random si se modifica la updatarea UI-ului.

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine6.jpg)

8. **Contacts** - Prezinta o lista cu contactele din telefon nume si nr de telefon

![](https://github.com/CristiSandu/PhoneBook/Images/Imagine7.jpg)

Modele Baza de date

Pentru un contact:

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

History Element:

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

### **3. Detalii tehnice de implementare**

Limbaje folosite :

- C# - pentru partea de backend
- XAML - pentru parte de interfața grafica
- SQL - pentru interogarea bazei de date.

C# - este un limbaj orientat obiect dezvoltat de Microsoft

XAML - este un limbaj declarativ dezvoltat tot de Microsoft inițial pentru WPF(Windows Presentation Foundation) mai târziu utilizat și la Xamarin Forms.
![](https://github.com/CristiSandu/PhoneBook/Images/Imagine8.jpg)

Baza de date are următoarea formă:
![](https://github.com/CristiSandu/PhoneBook/Images/Imagine9.jpg)

Poza de profil o salvez ca un șir de biți în baza de date.

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

In principiu fiecare pagina prezinta un UI propriu definit pentru fiecare functionalitate în parte.

La liste am folosit un CollectionView peste tot in code unde am de afisat sub forma de lista în care am definit cum sa arate datele pe care le afisez:

Exemplu pagina de Favorite:

```xml

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
```

Butoanele din bara de sus le-am adaugat identic în fiecare pagina principala:

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
```

Conectiunea cu baza de date am facut-o cu ajutorul NuGet-ului SQLite care are metodele de conectare pentru toate platformele. După instalarea NuGet-ului am modelat cate un tip de date pentru fiecare tabel și în pagina de add am inserat datele în baza de date iar în paginile de Contacte, Favorite, History, Search am interogat baza de date și am afisat rezultatul.

Style-urile fiecărui tip de controler (culoare font text, background) le-am definit în App.xaml ca un dicționar astfel avandu-le intr-un singur loc.

```xml

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

### **4. Resurse hardware și software necesare**

- Un telefon cu Android 8.0+
- Un PC cu Windows 10, XBOX sau orice dispozitiv care poate instala aplicații UWP - (Universal Windows Platform)
