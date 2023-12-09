using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoCatalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // инициализируем список кузовов
        Bodies bodies = new Bodies();
        // инициализируем список производителей
        Manufactures manufactures = new Manufactures();
        // инициализируем каталог
        Catalog catalog = new Catalog();

        public MainWindow()
        {
            InitializeComponent();
            
            // задаем ресурс каталога для отображения в ListBox
            catalogList.ItemsSource = catalog.GetCars();

            // добавляем всех производителей в Combobox
            updateManufactures();

            // добавляем все кузова в Combobox
            updateBodies();

        }

        // метод очищения полей в окне
        private void clearChildrenBoxes(StackPanel panel) 
        { 
            foreach(var child in panel.Children) 
            {
                if (child is TextBox) ((TextBox)child).Clear();
                if (child is ComboBox) ((ComboBox)child).SelectedValue = "Не выбрано";
            }
        }


                                                                                                    /*  ОКНО ДОБАВЛЕНИЯ КУЗОВОВ   */




        // метод обновления списка кузовов и ComboBox
        private void updateBodies()
        {
            bodiesComboBox.Items.Clear();
            // конфиг кнопки внутри ComboBox
            Button x = new Button();
            x.Content = "Добавить";
            x.Click += addInBodies;

            bodiesComboBox.Items.Add(x);
            foreach (Body body in bodies.GetBodies()) bodiesComboBox.Items.Add(body.Name.ToString() + ", количество дверей: " + body.CountOfDoors.ToString());
        }


        // кнопка открытия окна добавления кузовов
        private void addInBodies(object sender, RoutedEventArgs e)
        {
            clearChildrenBoxes(bodiesAddingPanel);
            // закрываем одно окно и открываем другое
            catalogAddingPanel.Visibility = Visibility.Hidden;
            bodiesAddingPanel.Visibility = Visibility.Visible;
        }

        // кнопка подтверждения добавления кузова в список кузовов
        private void addButtonBody_Click(object sender, RoutedEventArgs e)
        {
            // создаем экземпляр нового кузова и закрываем окно
            Body body = new Body(name: nameBodyTextBox.Text, countOfDoors: int.Parse(countOfDoorsBodyTextBox.Text));
            bodies.AddBody(body);

            updateBodies();

            // закрываем окно
            cancelButtonBody_Click(sender, e);
            // показываем окно добавления в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;

            // в качестве выбранного элемента задаем последний
            bodiesComboBox.SelectedIndex = bodies.GetBodies().Count;
        }

        // закрытие окна добавления кузова
        private void cancelButtonBody_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления кузова
            bodiesAddingPanel.Visibility = Visibility.Hidden;
            // открываем окно добавления автомобиля в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;
        }






                                                                                                    /*  ОКНО ДОБАВЛЕНИЯ ПРОИЗВОДИТЕЛЕЙ   */


        // метод обновления списка производителей
        private void updateManufactures()
        {
            manufacturesComboBox.Items.Clear();
            // конфиг кнопки внутри ComboBox
            Button x = new Button();
            x.Content = "Добавить";
            x.Click += addInManufacturers;

            manufacturesComboBox.Items.Add(x);
            foreach (Manufacturer manufacturer in manufactures.GetManufacturers()) manufacturesComboBox.Items.Add(manufacturer.Name);
        }

        // кнопка открытия окна добавления производителя в список
        private void addInManufacturers(object sender, RoutedEventArgs e)
        {
            clearChildrenBoxes(manufacturesAddingPanel);
            // закрываем одно окно и открываем другое
            catalogAddingPanel.Visibility = Visibility.Hidden;
            manufacturesAddingPanel.Visibility = Visibility.Visible;
        }

        // закрытие окна добавления автомобиля в каталог
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            catalogAddingPanel.Visibility = Visibility.Hidden;
        }

        // закрытие окна добавления производителя
        private void cancelButtonManufacture_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления производителя
            manufacturesAddingPanel.Visibility = Visibility.Hidden;
            // открываем окно добавления автомобиля в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;
        }

        // кнопка подтверждения добавления производителя в список производителей
        private void addButtonManufacture_Click(object sender, RoutedEventArgs e)
        {
            // создаем экземпляр нового производителя и закрываем окно
            Manufacturer manufacturer = new Manufacturer(name: nameManufactureTextBox.Text, yearOfFoundation: int.Parse(yearOfFoundationTextBox.Text), country: countryTextBox.Text);
            manufactures.AddManufacture(manufacturer);

            updateManufactures();

            // закрываем окно
            cancelButtonManufacture_Click(sender, e);
            // показываем окно добавления в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;

            // в качестве выбранного элемента задаем последний
            manufacturesComboBox.SelectedIndex = manufactures.GetManufacturers().Count;
        }





                                                                                                        /* ОКНО ДОБАВЛЕНИЯ В КАТАЛОГ */





        // окно добавления автомобиля в каталог
        private void addInCatalog(object sender, RoutedEventArgs e)
        {
            // очищаем форму
            clearChildrenBoxes(catalogAddingPanel);
            // открываем окно
            catalogAddingPanel.Visibility = Visibility.Visible;
        }

        // кнопка подтверждения добавления автомобиля в список
        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }


       
    }
}
