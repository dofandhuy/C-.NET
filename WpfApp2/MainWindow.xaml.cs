using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            using (var context = new PePrn25fallB1Context())
            {
                // 1. Load DataGrid
                dgvDisplay.ItemsSource = context.Books.Include(b => b.Category).ToList();

                // 2. Load Categories cho cả 2 ComboBox
                var cats = context.Categories.ToList();
                cbAddCategory.ItemsSource = cats;
                cbAddCategory.DisplayMemberPath = "CategoryName";
                cbAddCategory.SelectedValuePath = "CategoryId";

                var filterCats = cats.Select(c => new { c.CategoryId, c.CategoryName }).ToList();
                filterCats.Insert(0, new { CategoryId = -1, CategoryName = "All" });
                cbFilterCategory.ItemsSource = filterCats;
                cbFilterCategory.DisplayMemberPath = "CategoryName";
                cbFilterCategory.SelectedValuePath = "CategoryId";
                cbFilterCategory.SelectedIndex = 0;

                // 3. Load Authors
                var authors = context.Authors.ToList();
                lbAuthors.ItemsSource = authors;

                var filterAuthors = authors.Select(a => new { a.AuthorId, a.AuthorName }).ToList();
                filterAuthors.Insert(0, new { AuthorId = -1, AuthorName = "All" });
                cbFilterAuthor.ItemsSource = filterAuthors;
                cbFilterAuthor.DisplayMemberPath = "AuthorName";
                cbFilterAuthor.SelectedValuePath = "AuthorId";
                cbFilterAuthor.SelectedIndex = 0;
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int catId = (int)cbFilterCategory.SelectedValue;
            int authId = (int)cbFilterAuthor.SelectedValue;

            using (var context = new PePrn25fallB1Context())
            {
                IQueryable<Book> query = context.Books.Include(b => b.Category).Include(b => b.BookAuthors);

                if (catId != -1)
                    query = query.Where(b => b.CategoryId == catId);

                if (authId != -1)
                    query = query.Where(b => b.BookAuthors.Any(ba => ba.AuthorId == authId));

                dgvDisplay.ItemsSource = query.ToList();
            }
        }
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new PePrn25fallB1Context())
                {
                    // 1. Tạo đối tượng Book
                    Book b = new Book()
                    {
                        Title = txtTitle.Text,
                        CategoryId = (int)cbAddCategory.SelectedValue,
                        PublishedYear = int.Parse(txtYear.Text),
                        Price = decimal.Parse(txtPrice.Text),
                        Quantity = int.Parse(txtQuantity.Text)
                    };

                    // 2. Lấy danh sách AuthorId được check
                    var selectedAuthors = new List<BookAuthor>();
                    foreach (var item in lbAuthors.Items)
                    {
                        var container = lbAuthors.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        var checkBox = FindVisualChild<CheckBox>(container);
                        if (checkBox != null && checkBox.IsChecked == true)
                        {
                            selectedAuthors.Add(new BookAuthor { AuthorId = (int)checkBox.Tag });
                        }
                    }

                    if (selectedAuthors.Count == 0)
                    {
                        MessageBox.Show("Please select at least 1 author!");
                        return;
                    }

                    b.BookAuthors = selectedAuthors;
                    context.Books.Add(b);
                    context.SaveChanges();

                    MessageBox.Show("Add successful!");
                    LoadData(); // Reload DataGrid
                    btnClear_Click(null, null); // Clear form
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // Hàm phụ để tìm CheckBox trong ListBox
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T) return (T)child;
                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }
    }
}