### **2. STRUCTURA APLICAȚIEI**

Aplicația a fost realizată în Xamarin Forms pentru **Android** și **UWP** , iar ca și mediu de stocare persistentă sa folosit **SQLite**. Xamarin Forms cuprinde **C#** pentru partea de backend și **XAML** pentru partea de front end și definire a aspectului vizual.

Structural, este formată din 3 pagini principale **History** , **Favorite** și **Contacts** care sunt și taburile din tabview. Pe langa paginile principale avem și paginile secundare: **AddContact** , **DialPage** , **DisContact** , **SearchPage** și **SettingsPage**.

1. **AddContact** - In aceasta pagina se poate adăuga un contact nou.

Un contact conțină: nume, prenume, nr de telefon (cu verificare dacă nr este de România și este valid), email(cu verificare de validitate), toggle(on - add to favorite contacts , off - not to favorite contacts), o imagine de profil aleasă din galerie.

![](RackMultipart20210613-4-19wjtlx_html_b72253250ba66276.png) ![](RackMultipart20210613-4-19wjtlx_html_539c4629d192e780.png)

1. **DisContact** - Este o pagina în care se afișează contactul alături de următoarele posibilități: sunare, trimitere mail sau ștergere contact.

Daca apare o steluta reprezinta faptul ca acel contact este la favorite.

![](RackMultipart20210613-4-19wjtlx_html_3b5fcfcf4b573694.png) ![](RackMultipart20210613-4-19wjtlx_html_c3b6b15fc1b469e4.png)

1. **DialPage** - Pagina prezinta o tastatura care permite introducerea unui nr de telefon, iar în partea de sus se face o selecție pe numerele de telefon care încep cu nr scris până în momentul de fata. Daca se selecteaza un nr din lista se completeaza automat label-ul si se poate apela numarul.

![](RackMultipart20210613-4-19wjtlx_html_20f25b311109ce88.png) ![](RackMultipart20210613-4-19wjtlx_html_58f78d8bc40efbef.png)

1. **SearchPage** - Se poate face search prin contacte dupa Nume, in bara de search apar nr de contacte. Se poate selecta contactul din lista care apare

![](RackMultipart20210613-4-19wjtlx_html_1c5133b9d987601d.png) ![](RackMultipart20210613-4-19wjtlx_html_6c35959888649830.png)

1. **SettingsPage** - nu am implementat nimic în settings interesant, sunt niște elemente hardcodate
2. **History** - aici apare istoria apelurilor și a emailurilor trimise sub forma &quot;Call - nume&quot; sau &quot;Email - nume &quot; sau &quot;Call - nr de telefon&quot; pentru nr de telefon care nu se afla în agenda alături de data și ora . În partea de sus apare numărul de apeluri și mailuri din pagina. Pe fiecare pagina principala avem un buton pentru deschidere a **DialPage.**

![](RackMultipart20210613-4-19wjtlx_html_d2fb9fb907796130.png)

1. **Favorite** - Pagina prezinta intr-un layout cu 2 coloane si n lini contactele favorite , culoarea de background este random si se modifica la updatarea UI-ului.

![](RackMultipart20210613-4-19wjtlx_html_60cfd6516e075aa9.png) ![](RackMultipart20210613-4-19wjtlx_html_f72e8d5647c5a84d.png)

1. **Contacts** - Prezinta o lista cu contactele din telefon nume si nr de telefon

![](RackMultipart20210613-4-19wjtlx_html_5d467e4b8f55b883.png)

Structuri de date

Ca și structuri de date am folosit următoarele modele pentru Baza de date:

Pentru un contact:

namespaceAgendaTelefonica.Models

{

publicclassContact

{

[PrimaryKey, AutoIncrement]

publicint id { set; get; }

[MaxLength(250), Unique]

publicstring firstName { get; set; }

[MaxLength(250), Unique]

publicstring secondName { get; set; }

[MaxLength(250), Unique]

publicstring phoneNumber { get; set; }

[MaxLength(250), Unique]

publicstring email { get; set; }

publicbool favorite { get; set; }

publicbyte[] profilPicture { get; set; }

public SQLiteConnection getConnection()

{

returnnew SQLiteConnection(App.DataBaseLocation);

}

}

}

History Element:

namespaceAgendaTelefonica.Models

{

publicclassHistoryElem

{

[PrimaryKey, AutoIncrement]

publicint id { set; get; }

publicint id_Contact { set; get; }

public DateTime date { set; get; }

publicstring phoneNumber { set; get; }

publicbool IsEmail { set; get; }

public SQLiteConnection getConnection()

{

returnnew SQLiteConnection(App.DataBaseLocation);

}

}

}

### **3. Detalii tehnice de implementare**

Limbaje folosite :

- C# - pentru partea de backend
- XAML - pentru parte de interfața grafica
- SQL - pentru interogarea bazei de date.

C# - este un limbaj orientat obiect dezvoltat de Microsoft

XAML - este un limbaj declarativ dezvoltat tot de Microsoft inițial pentru WPF(Windows Presentation Foundation) mai târziu utilizat și la Xamarin Forms.

![](RackMultipart20210613-4-19wjtlx_html_9afb73d544448cb8.png)

Baza de date are următoarea formă:

![](RackMultipart20210613-4-19wjtlx_html_df4999a66cd7037c.png)

Poza de profil o salvez ca un șir de biți în baza de date.

privateasyncvoidAddPhoto_Clicked(object sender, EventArgs e)

{

try

{

var result_photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions

{

Title = &quot;Pick a photo!&quot;

});_// folosim media picker ca sa luam poza din memorie_

_// salvăm rezultatul ca și un stream de date_

var stream = await result_photo.OpenReadAsync();

_// convertim stream-ul în imagine și afisam_

contactPhoto.Source = ImageSource.FromStream(() =\&gt; stream);

_// convertim în șir de bytes[] apeland metodă de mai jos_

photoSave = GetImageBytes(stream);

}

catch (NullReferenceException ex)

{

Console.WriteLine($&quot;CapturePhotoAsync THREW: {ex.Message}&quot;);

}

}

privatebyte[] GetImageBytes(Stream stream)

{

byte[] ImageBytes;

using (var memoryStream = new System.IO.MemoryStream())

{

stream.CopyTo(memoryStream);

ImageBytes = memoryStream.ToArray();

}

return ImageBytes;

}

In principiu fiecare pagina prezinta un UI propriu definit pentru fiecare functionalitate în parte.

La liste am folosit un CollectionView peste tot in code unde am de afisat sub forma de lista în care am definit cum sa arate datele pe care le afisez:

Exemplu pagina de Favorite:

\&lt;CollectionView x:Name=&quot;favoriteListView&quot;

ItemsSource=&quot;{Binding Contact}&quot;

AbsoluteLayout.LayoutBounds = &quot;0.5,0&quot;

AbsoluteLayout.LayoutFlags = &quot;PositionProportional&quot;

SelectionChanged=&quot;favoriteListView_SelectionChanged&quot;

SelectionMode=&quot;Single&quot; \&gt;

\&lt;CollectionView.ItemsLayout\&gt;

\&lt;GridItemsLayout Orientation=&quot;Vertical&quot;

Span=&quot;2&quot;

HorizontalItemSpacing=&quot;5&quot;

VerticalItemSpacing=&quot;5&quot; /\&gt;

\&lt;/CollectionView.ItemsLayout\&gt;

\&lt;CollectionView.ItemTemplate\&gt;

\&lt;DataTemplate\&gt;

\&lt;Frame Style=&quot;{StaticResource FrameStyle}&quot;

CornerRadius =&quot;0&quot;

BackgroundColor=&quot;{Binding Path=firstName, Converter={StaticResource ColorConvertorFavorite}}&quot;\&gt;

\&lt;Grid Padding=&quot;5,5,5,5&quot; \&gt;

\&lt;Grid.RowDefinitions\&gt;

\&lt;RowDefinition Height=&quot;Auto&quot; /\&gt;

\&lt;RowDefinition Height=&quot;Auto&quot; /\&gt;

\&lt;RowDefinition Height=&quot;100&quot; /\&gt;

\&lt;/Grid.RowDefinitions\&gt;

\&lt;Grid.ColumnDefinitions\&gt;

\&lt;ColumnDefinition Width=&quot;Auto&quot; /\&gt;

\&lt;/Grid.ColumnDefinitions\&gt;

\&lt;Label Grid.Column=&quot;1&quot;

Text=&quot;{Binding firstName}&quot;

Style=&quot;{StaticResource LabelStyle}&quot; /\&gt;

\&lt;Label Grid.Row=&quot;1&quot;

Grid.Column=&quot;1&quot;

Style=&quot;{StaticResource LabelStyle}&quot;

Text=&quot;{Binding Path=secondName}&quot; /\&gt;

\&lt;/Grid\&gt;

\&lt;/Frame\&gt;

\&lt;/DataTemplate\&gt;

\&lt;/CollectionView.ItemTemplate\&gt;

\&lt;/CollectionView\&gt;

Butoanele din bara de sus le-am adaugat identic în fiecare pagina principala:

\&lt;ContentPage.ToolbarItems\&gt;

\&lt;ToolbarItem x:Name=&quot;searchBtn&quot;Text=&quot;Search&quot;

IconImageSource=&quot;search.png&quot;

Order=&quot;Primary&quot;

Priority=&quot;0&quot;

Clicked=&quot;searchBtn_Clicked&quot;/\&gt;

\&lt;ToolbarItem x:Name=&quot;addContBtn&quot;Order=&quot;Primary&quot;

IconImageSource=&quot;plus.png&quot;

Text=&quot;Add&quot;

Priority=&quot;1&quot;

Clicked=&quot;addContBtn_Clicked&quot; /\&gt;

\&lt;ToolbarItem x:Name =&quot;settingsBtn&quot;Order=&quot;Secondary&quot;

Text=&quot;Settings&quot;

Priority=&quot;2&quot;

Clicked=&quot;settingsBtn_Clicked&quot;/\&gt;

\&lt;/ContentPage.ToolbarItems\&gt;

Conectiunea cu baza de date am facut-o cu ajutorul NuGet-ului SQLite care are metodele de conectare pentru toate platformele. După instalarea NuGet-ului am modelat cate un tip de date pentru fiecare tabel și în pagina de add am inserat datele în baza de date iar în paginile de Contacte, Favorite, History, Search am interogat baza de date și am afisat rezultatul.

Style-urile fiecărui tip de controler (culoare font text, background) le-am definit în App.xaml ca un dicționar astfel avandu-le intr-un singur loc.

\&lt;Application.Resources\&gt;

\&lt;Color x:Key=&quot;AppPrimaryColor&quot;\&gt;#007ac1\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;AppBackgroundColor&quot;\&gt;#67daff\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;PrimaryColor&quot;\&gt;#00a9f4\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;CallColor&quot;\&gt;#01C853\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;SecondaryColor&quot;\&gt;#ffc107\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;SecondaryColor_L&quot;\&gt;#fff350\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;SecondaryColor_B&quot;\&gt;#c79100\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;TertiaryColor&quot;\&gt;Silver\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;TextColor&quot;\&gt;Black\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;PlaceHolderColor&quot;\&gt;Gray\&lt;/Color\&gt;

\&lt;Color x:Key=&quot;ButtonColor&quot;\&gt;#646364\&lt;/Color\&gt;

\&lt;Style x:Key=&quot;CallButtonStyle&quot;TargetType=&quot;Button&quot;\&gt;

\&lt;Setter Property=&quot;BackgroundColor&quot;Value=&quot;{StaticResource CallColor}&quot;/\&gt;

\&lt;Setter Property=&quot;CornerRadius&quot;Value=&quot;90&quot;/\&gt;

\&lt;Setter Property=&quot;ImageSource&quot;Value=&quot;callphone.png&quot;/\&gt;

\&lt;/Style\&gt;

\&lt;Style x:Key=&quot;ButtonStyle&quot;TargetType=&quot;Button&quot;\&gt;

\&lt;Setter Property=&quot;BackgroundColor&quot;Value=&quot;{StaticResource ButtonColor}&quot;/\&gt;

\&lt;Setter Property=&quot;TextColor&quot;Value=&quot;{StaticResource SecondaryColor}&quot; /\&gt;

\&lt;Setter Property=&quot;CornerRadius&quot;Value=&quot;4&quot;/\&gt;

\&lt;/Style\&gt;

\&lt;Style x:Key=&quot;FrameStyle&quot;TargetType=&quot;Frame&quot;\&gt;

\&lt;Setter Property=&quot;BackgroundColor&quot;Value=&quot;{StaticResource SecondaryColor}&quot;/\&gt;

\&lt;Setter Property=&quot;Padding&quot;Value=&quot;10,10,10,10&quot; /\&gt;

\&lt;Setter Property=&quot;CornerRadius&quot;Value=&quot;10&quot;/\&gt;

\&lt;Setter Property=&quot;HasShadow&quot;Value=&quot;True&quot;/\&gt;

\&lt;Setter Property=&quot;BorderColor&quot;Value=&quot;{StaticResource AppPrimaryColor}&quot;/\&gt;

\&lt;/Style\&gt;

\&lt;Style x:Key=&quot;LabelStyle&quot;TargetType=&quot;Label&quot;\&gt;

\&lt;Setter Property=&quot;TextColor&quot;Value=&quot;{StaticResource TextColor}&quot;/\&gt;

\&lt;Setter Property=&quot;FontAttributes&quot;Value=&quot;Bold&quot;/\&gt;

\&lt;Setter Property=&quot;FontSize&quot;Value=&quot;Large&quot;/\&gt;

\&lt;Setter Property=&quot;HorizontalOptions&quot;Value=&quot;StartAndExpand&quot;/\&gt;

\&lt;Setter Property=&quot;VerticalOptions&quot;Value=&quot;Center&quot;/\&gt;

\&lt;/Style\&gt;

\&lt;/Application.Resources\&gt;

### **4. Resurse hardware și software necesare**

- Un telefon cu Android 8.0+
- Un PC cu Windows 10, XBOX sau orice dispozitiv care poate instala aplicații UWP - (Universal Windows Platform)
