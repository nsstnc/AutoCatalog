using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.ComponentModel;
using Newtonsoft.Json;
using System.Collections;

namespace AutoCatalog
{

    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // флаг открытого каталога
        bool openedCatalog = false;

        
        // инициализируем список производителей
        Manufactures manufactures = new Manufactures();
        // инициализируем каталог
        Catalog catalog = new Catalog();

        // переменная для текущей подвески
        SuspensionAndBrakes currentSuspensionAndBrakes;
        
        // переменная для текущей комплектации
        Configuration currentConfiguration;
        
        // переменная для текущего двигателя
        Engine currentEngine;
        // переменная для текущей коробки передач
        Transmission currentTransmission;

        public MainWindow()
        {
            InitializeComponent();
            // считываем  json строку из файла и десериализируем ее в контейнер
            string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\catalog.json");
            catalog = JsonConvert.DeserializeObject<Catalog>(json);
            // то же самое для производителей
            json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\manufactures.json");
            manufactures = JsonConvert.DeserializeObject<Manufactures>(json);
           



            // задаем ресурсы каталога и производителей для отображения в ListBox
            catalogList.Items.Add(catalog.GetCars());
            manufactureList.Items.Add(manufactures.GetManufacturers());

            // добавляем всех производителей в Combobox
            updateManufactures();

            

            // обновляем каталог
            updateCatalog();
        }

        private void dropSelector()
        {
            catalogList.SelectedIndex = -1;
            manufactureList.SelectedIndex = -1;
        }

        // обработчик нажатия по окну приложения, мимо кнопок
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dropSelector();
        }


        // функция для закрытия всех страниц
        private void hideAllPages()
        {
            manufactureList.Visibility = Visibility.Hidden;
            catalogList.Visibility = Visibility.Hidden;
            catalogAddingPanel.Visibility = Visibility.Hidden;
            manufacturesAddingPanel.Visibility = Visibility.Hidden;
            configurationCreatingPanel.Visibility = Visibility.Hidden;
            engineCreatingPanel.Visibility = Visibility.Hidden;
            transmissionCreatingPanel.Visibility = Visibility.Hidden;
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Hidden;
        }



        // метод создания краткой информации из свойств объекта
        private string createInfo(object obj)
        {
            string info = "";
            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                // если значение свойства строка или число, добавляем его в инфо
                if ((value is string) || (value is int) || (value is double)) info += value + ", ";
                // если значение свойства объект другого класса, то перебираем его свойства и добавляем в описание
                else
                {
                    info += "\n";
                    info += createInfo(value);
                }
            }
            return info;
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







                                                    /*  ОКНО ДОБАВЛЕНИЯ ПРОИЗВОДИТЕЛЕЙ   */



        // метод обновления списка производителей
        private void updateManufactures()
        {
            manufacturesComboBox.Items.Clear();
            manufactureList.Items.Clear();
            // конфиг кнопки внутри ComboBox
            Button x = new Button();
            x.Content = "Добавить";
            x.Click += addInManufacturers;

            manufacturesComboBox.Items.Add(x);
            foreach (Manufacturer manufacturer in manufactures.GetManufacturers())
            {
                manufacturesComboBox.Items.Add(manufacturer.Name);
                manufactureList.Items.Add(createInfo(manufacturer));
            }
        }

        // кнопка открытия окна добавления производителя в список
        private void addInManufacturers(object sender, RoutedEventArgs e)
        {
            clearChildrenBoxes(manufacturesAddingPanel);
            // закрываем одно окно и открываем другое
            hideAllPages();
            manufacturesAddingPanel.Visibility = Visibility.Visible;
        }

     

        // закрытие окна добавления производителя
        private void cancelButtonManufacture_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления производителя
            hideAllPages();
            // открываем необходимое окно
            if (openedCatalog) { catalogAddingPanel.Visibility = Visibility.Visible; }
            else manufactureList.Visibility = Visibility.Visible;
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
           

            // в качестве выбранного элемента задаем последний
            manufacturesComboBox.SelectedIndex = manufactures.GetManufacturers().Count;
        }





                                                                                                        /* ОКНО ДОБАВЛЕНИЯ В КАТАЛОГ */

        // метод обновления каталога
        private void updateCatalog() 
        { 
            catalogList.Items.Clear();
            foreach (Car car in catalog.GetCars()) catalogList.Items.Add(createInfo(car));
        }



        // окно добавления автомобиля в каталог
        private void addInCatalog(object sender, RoutedEventArgs e)
        {
            hideAllPages();
            // очищаем форму
            clearChildrenBoxes(catalogAddingPanel);
            // открываем окно
            catalogAddingPanel.Visibility = Visibility.Visible;
            openedCatalog = true;
        }

        // закрытие окна добавления автомобиля в каталог
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            hideAllPages();
            catalogList.Visibility= Visibility.Visible;
        }


        // кнопка подтверждения добавления автомобиля в список
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Car car = new Car(name: nameTextBox.Text, 
                generation: generationTextBox.Text, 
                manufacturer: manufactures.GetManufacturers()[manufacturesComboBox.SelectedIndex - 1],
                year: int.Parse(yearTextBox.Text),
                configuration: currentConfiguration,
                body: bodiesComboBox.Text,
                category: categoryTextBox.Text);

            catalog.AddCar(car);
            updateCatalog();
            hideAllPages();
            catalogList.Visibility = Visibility.Visible;

        }

       
                                                                                    /*  ОКНО СОЗДАНИЯ КОМПЛЕКТАЦИИ   */



        // окно создания комплектации
        private void createConfiguration(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления автомобиля в каталог
            hideAllPages();
            // открываем окно создания комплектации
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания комплектации
        private void cancelButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            catalogAddingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания комплектации
        private void createButtonConfig_Click(object sender, RoutedEventArgs e)
        {


            currentConfiguration = new Configuration(name: nameConfigTextBox.Text,
                engine: currentEngine,
                transmission: currentTransmission,
                suspensionAndBrakes: currentSuspensionAndBrakes,
                typeOfDrive: typeOfDriveConfigComboBox.Text,
                overClocking: decimal.Parse(overclockingConfigTextBox.Text),
                clearance: int.Parse(clearanceConfigTextBox.Text),
                curbWeight: int.Parse(curbWeightConfigTextBox.Text),
                fullWeight: int.Parse(fullWeightConfigTextBox.Text),
                fuelTankVolume: int.Parse(fuelTankVolumeConfigTextBox.Text),
                numberOfSeats: int.Parse(numberOfSeatsConfigTextBox.Text));

            string h = createInfo(currentConfiguration).Substring(0, 40) + "...";
            // если ни одна комплектация еще не была добавлена (длина комбобокса = 1, в нем только кнопка)
            if (configurationComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createConfig.Content = "Изменить";
                configurationComboBox.Items.Add(h);
            }
            else 
            {
                // иначе изменяем последний элемент комбобокса
                configurationComboBox.Items[configurationComboBox.Items.Count - 1] = h;
            };
            
            // в качестве выбранного элемента задаем последний
            configurationComboBox.SelectedIndex = configurationComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonConfig_Click(sender, e);
            
        }



                                                                                        /*  ОКНО СОЗДАНИЯ ДВИГАТЕЛЯ   */





        // окно создания двигателя
        private void createEngine(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно создания двигателя
            engineCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания двигателя
        private void cancelButtonEngine_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания двигателя
        private void createButtonEngine_Click(object sender, RoutedEventArgs e)
        {
            // задаем новый экземпляр текущего двигателя
            currentEngine = new Engine(name: nameEngineTextBox.Text.ToString(),
                typeOfEngine: typeEngineTextBox.Text.ToString(),
                cylinderArrangement: cylindersEngineTextBox.Text.ToString(),
                power: int.Parse(powerEngineTextBox.Text),
                volume: int.Parse(volumeEngineTextBox.Text),
                maxTorque: int.Parse(torqueEngineTextBox.Text),
                numberOfCylinders: int.Parse(numberOfCylindersEngineTextBox.Text),
                typeOfBoost: typeOfBoostEngineTextBox.Text.ToString(),
                fuelGrade: fuelGradeEngineTextBox.Text.ToString(),
                enginePowerSupplySystem: enginePowerSupplySystemEngineTextBox.Text.ToString());


            string h = createInfo(currentEngine).Substring(0, 40) + "...";

            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (engineComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createEngineButton.Content = "Изменить";
                engineComboBox.Items.Add(h);
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                engineComboBox.Items[engineComboBox.Items.Count - 1] = h;
            };

            // в качестве выбранного элемента задаем последний
            engineComboBox.SelectedIndex = engineComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonEngine_Click(sender, e);
        }




                                                                        /*  ОКНО СОЗДАНИЯ КОРОБКИ ПЕРЕДАЧ   */




        // окно создания коробки передач
        private void createTransmission(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации

            hideAllPages();
            // открываем окно создания трансмиссии
            transmissionCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания коробки передач
        private void cancelButtonTransmission_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания коробки передач
        private void createButtonTransmission_Click(object sender, RoutedEventArgs e)
        {
            // задаем новый экземпляр текущей коробки передач
            currentTransmission = new Transmission(name: nameTransmissionTextBox.Text,
                type: typeTransmissionTextBox.Text,
                numberOfGears: int.Parse(numberOfGearsTransmissionTextBox.Text));


            // если ни одна коробка еще не был добавлена (длина комбобокса = 1, в нем только кнопка)
            if (transmissionComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createTransmissionButton.Content = "Изменить";
                transmissionComboBox.Items.Add(createInfo(currentTransmission));
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                transmissionComboBox.Items[transmissionComboBox.Items.Count - 1] = createInfo(currentTransmission);
            };

            // в качестве выбранного элемента задаем последний
            transmissionComboBox.SelectedIndex = transmissionComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonTransmission_Click(sender, e);
        }


                                                                                    /*  ОКНО СОЗДАНИЯ ПОДВЕСКИ И ТОРМОЗОВ   */
        



        // окно создания подвески и тормозов
        private void createSuspensionAndBrakes(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно создания подвески и тормозов
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания подвески и тормозов
        private void cancelButtonSuspensionAndBrakes_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            hideAllPages();
            // открываем окно добавления автомобиля в каталог
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // подтверждение создания подвески и тормозов
        private void createButtonSuspensionAndBrakes_Click(object sender, RoutedEventArgs e)
        {
            // задаем новый экземпляр текущей коробки передач
            currentSuspensionAndBrakes = new SuspensionAndBrakes(typeOfFrontSuspension: typeOfFrontSuspensionComboBox.Text.ToString(),
                typeOfBackSuspension: typeOfBackSuspensionComboBox.Text.ToString(),
                backBrakes: backBrakesComboBox.Text.ToString(),
                frontBrakes: frontBrakesComboBox.Text.ToString());

            string h = createInfo(currentSuspensionAndBrakes).Substring(0, 40) + "...";
            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (suspensionAndBrakesComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс подвеску
                createSuspensionAndBrakesButton.Content = "Изменить";
                suspensionAndBrakesComboBox.Items.Add(h);
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                suspensionAndBrakesComboBox.Items[suspensionAndBrakesComboBox.Items.Count - 1] = h;
            };

            // в качестве выбранного элемента задаем последний
            suspensionAndBrakesComboBox.SelectedIndex = suspensionAndBrakesComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonSuspensionAndBrakes_Click(sender, e);
        }




                                                            /* ЗАКРЫТИЕ ОКНА */


        // действие при закрытии приложения
        private void Window_Close(object sender, CancelEventArgs e)
        {
            // очищаем файлы
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\catalog.json", string.Empty);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\manufactures.json", string.Empty);

           
            // используя поток записываем в json файл сериализованный объект контейнера
            using (StreamWriter w = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\catalog.json", false))
            {
                w.Write(JsonConvert.SerializeObject(catalog));
            }
            // то же самое для производителей
            using (StreamWriter w = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\manufactures.json", false))
            {
                w.Write(JsonConvert.SerializeObject(manufactures));
            }
            

        }



                                                             /* ОКНО ОТОБРАЖЕНИЯ КАТАЛОГА*/





        private void showCatalog(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (catalogList.IsVisible)
            { 
                hideAllPages();
            }
            else
            {
                hideAllPages();
                catalogList.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Visible;
                
            }
            
        }
        // удаление из каталога
        private void deleteFromCatalog()
        {
            // сохраняем индекс текущего выбранного элемента
            int selected = catalogList.SelectedIndex;

            // удаляем элемент
            catalog.RemoveCar(selected);
            // обновляем список
            updateCatalog();
            // задаем новый выбранный элемент предыдущему
            catalogList.SelectedIndex = selected - 1;
            catalogList.Focus();
        }

        // обработчик изменения выбранного элемента
        private void catalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catalogList.SelectedIndex == -1) DeleteButton.IsEnabled = false;
            else DeleteButton.IsEnabled = true;
        }
  

                                        /* ОКНО ОТОБРАЖЕНИЯ ПРОИЗВОДИТЕЛЕЙ */




        private void showManufactures(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (manufactureList.IsVisible)
            {
                hideAllPages();
            }
            else
            {
                hideAllPages();
                openedCatalog = false;
                manufactureList.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }
        // удаление из производителей
        private void deleteFromManufactures()
        {
            // сохраняем индекс текущего выбранного элемента
            int selected = manufactureList.SelectedIndex;

            // удаляем элемент
            manufactures.RemoveManufacture(selected);
            // обновляем список
            updateManufactures();
            // задаем новый выбранный элемент предыдущему
            manufactureList.SelectedIndex = selected - 1;
            manufactureList.Focus();

        }
        // обработчик изменения выбранного элемента
        private void manufacrures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (manufactureList.SelectedIndex == -1) DeleteButton.IsEnabled = false;
            else DeleteButton.IsEnabled = true;
        }

    
















        // кнопка удалить
        private void delete_click(object sender, RoutedEventArgs e)
        {
            if (manufactureList.IsVisible) deleteFromManufactures();
            else deleteFromCatalog();  
        }

        // кнопка добавить
        private void add_click(object sender, RoutedEventArgs e)
        {
            dropSelector();
            if (manufactureList.IsVisible) addInManufacturers(sender, e);
            else addInCatalog(sender, e);
        }

    }
}
