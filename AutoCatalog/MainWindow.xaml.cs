using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // переменная для текущей подвески
        SuspensionAndBrakes currentSuspensionAndBrakes;
        // переменная для текущего производителя
        Manufacturer currentManufacturer;
        // переменная для текущей комплектации
        Configuration currentConfiguration;
        // переменная для текущего кузова
        Body currentBody;
        // переменная для текущего двигателя
        Engine currentEngine;
        // переменная для текущей коробки передач
        Transmission currentTransmission;

        public MainWindow()
        {
            InitializeComponent();
            
            // задаем ресурс каталога для отображения в ListBox
            catalogList.Items.Add(catalog.GetCars());

            // добавляем всех производителей в Combobox
            updateManufactures();

            // добавляем все кузова в Combobox
            updateBodies();

            // обновляем каталог
            updateCatalog();

        }


        // метод создания краткой информации из свойств объекта
        private string createInfo(object example)
        {
            string info = "";
            foreach (var property in example.GetType().GetProperties())
            {
                var value = property.GetValue(example);
                // если значение свойства строка или число, добавляем его в инфо
                if ((value is string) || (value is int)) info += value + ", ";
                // если значение свойства объект другого класса, то перебираем его свойства и добавляем в описание
                else info += string.Join(", ", value.GetType().GetProperties().Select(s => s.GetValue(value)));
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
            catalogList.Visibility = Visibility.Hidden;
            // очищаем форму
            clearChildrenBoxes(catalogAddingPanel);
            // открываем окно
            catalogAddingPanel.Visibility = Visibility.Visible;

        }

        // кнопка подтверждения добавления автомобиля в список
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Car car = new Car(name: nameTextBox.Text, 
                generation: int.Parse(generationTextBox.Text), 
                manufacturer: manufactures.GetManufacturers()[manufacturesComboBox.SelectedIndex - 1],
                year: int.Parse(yearTextBox.Text),
                configuration: currentConfiguration,
                body: bodies.GetBodies()[bodiesComboBox.SelectedIndex - 1],
                category: categoryTextBox.Text);

            catalog.AddCar(car);
            updateCatalog();
            catalogAddingPanel.Visibility = Visibility.Hidden;
            catalogList.Visibility = Visibility.Visible;

        }

        /*  ОКНО СОЗДАНИЯ КОМПЛЕКТАЦИИ   */



        // окно создания комплектации
        private void createConfiguration(object sender, RoutedEventArgs e)
        {
            // закрываем окно добавления автомобиля в каталог
            catalogAddingPanel.Visibility = Visibility.Hidden;
            // открываем окно создания комплектации
            configurationCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания комплектации
        private void cancelButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            configurationCreatingPanel.Visibility = Visibility.Hidden;
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
                overClocking: int.Parse(overclockingConfigTextBox.Text),
                clearance: int.Parse(clearanceConfigTextBox.Text),
                curbWeight: int.Parse(curbWeightConfigTextBox.Text),
                fullWeight: int.Parse(fullWeightConfigTextBox.Text),
                fuelTankVolume: int.Parse(fuelTankVolumeConfigTextBox.Text),
                numberOfSeats: int.Parse(numberOfSeatsConfigTextBox.Text));


            // если ни одна комплектация еще не была добавлена (длина комбобокса = 1, в нем только кнопка)
            if (configurationComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createConfig.Content = "Изменить";
                configurationComboBox.Items.Add(createInfo(currentConfiguration)); 
            }
            else 
            {
                // иначе изменяем последний элемент комбобокса
                configurationComboBox.Items[configurationComboBox.Items.Count - 1] = createInfo(currentConfiguration);
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
            configurationCreatingPanel.Visibility = Visibility.Hidden;
            // открываем окно создания двигателя
            engineCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания двигателя
        private void cancelButtonEngine_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            engineCreatingPanel.Visibility = Visibility.Hidden;
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

            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (engineComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс комплектацию
                createEngineButton.Content = "Изменить";
                engineComboBox.Items.Add(createInfo(currentEngine));
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                engineComboBox.Items[engineComboBox.Items.Count - 1] = createInfo(currentEngine);
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

            configurationCreatingPanel.Visibility = Visibility.Hidden;
            // открываем окно создания трансмиссии
            transmissionCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания коробки передач
        private void cancelButtonTransmission_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            transmissionCreatingPanel.Visibility = Visibility.Hidden;
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

            configurationCreatingPanel.Visibility = Visibility.Hidden;
            // открываем окно создания подвески и тормозов
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Visible;
        }

        // отмена создания подвески и тормозов
        private void cancelButtonSuspensionAndBrakes_Click(object sender, RoutedEventArgs e)
        {
            // закрываем окно создания комплектации
            suspensionAndBrakesCreatingPanel.Visibility = Visibility.Hidden;
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


            // если ни один двигатель еще не был добавлен (длина комбобокса = 1, в нем только кнопка)
            if (suspensionAndBrakesComboBox.Items.Count == 1)
            {
                // тогда меняем содержимое кнопки и добавляем в комбобокс подвеску
                createSuspensionAndBrakesButton.Content = "Изменить";
                suspensionAndBrakesComboBox.Items.Add(createInfo(currentSuspensionAndBrakes));
            }
            else
            {
                // иначе изменяем последний элемент комбобокса
                suspensionAndBrakesComboBox.Items[suspensionAndBrakesComboBox.Items.Count - 1] = createInfo(currentSuspensionAndBrakes);
            };

            // в качестве выбранного элемента задаем последний
            suspensionAndBrakesComboBox.SelectedIndex = suspensionAndBrakesComboBox.Items.Count - 1;
            // закрываем окно
            cancelButtonSuspensionAndBrakes_Click(sender, e);
        }
    }
}
